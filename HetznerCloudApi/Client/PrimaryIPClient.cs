using HetznerCloudApi.Object.Action;
using HetznerCloudApi.Object.Datacenter;
using HetznerCloudApi.Object.PrimaryIPs;
using HetznerCloudApi.Object.PrimaryIPs.Get;
using HetznerCloudApi.Object.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HetznerCloudApi.Client
{
    public class PrimaryIPClient
    {
        private readonly string _token;

        public PrimaryIPClient(string token)
        {
            _token = token;
        }

        /// <summary>
        /// List multiple Primary IPs.
        /// Use the provided URI parameters to modify the result.
        /// </summary>
        /// <returns></returns>
        public async Task<List<PrimaryIP>> Get()
        {
            List<PrimaryIP> list = new List<PrimaryIP>();
            long page = 0;
            while (true)
            {
                // Nex
                page++;

                // Get list
                Response response = JsonConvert.DeserializeObject<Response>(await Core.SendGetRequest(_token, $"/primary_ips?page={page}&per_page={Core.PerPage}")) ?? new Response();

                // Run
                foreach (PrimaryIP row in response.Primary_IPs)
                {
                    list.Add(row);
                }

                // Finish?
                if (response.Meta.Pagination.NextPage == 0)
                {
                    // Yes, finish
                    return list;
                }
            }
        }

        /// <summary>
        /// Returns a Primary IP.
        /// </summary>
        /// <param name="id">ID of the Primary IP.</param>
        /// <returns></returns>
        public async Task<PrimaryIP> Get(long id)
        {
            // Get list
            string json = await Core.SendGetRequest(_token, $"/primary_ips/{id}");

            // Set
            JObject result = JObject.Parse(json);
            PrimaryIP primip = JsonConvert.DeserializeObject<PrimaryIP>($"{result["primary_ip"]}") ?? null;

            // Return
            return primip;
        }


        /// <summary>
        /// Create a new Primary IP.
        ///
        /// <para>Can optionally be assigned to a resource by providing an assignee_id and assignee_type.</para>
        ///
        /// <para>If not assigned to a resource the datacenter key needs to be provided.This can be either the ID or the name of the Datacenter this Primary IP shall be created in.</para>
        ///
        /// <para>A Primary IP can only be assigned to resource in the same Datacenter later on.</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PrimaryIPCreatedResponse> Create(PrimaryIPCreateRequest request)
        {
            if(request.Datacenter!=null && request.AssigneeId != 0)
            {
                throw new InvalidOperationException("Please choose Datacenter OR AssigneeId as parameter.");
            }


            return await Core.SendPostRequest<PrimaryIPCreatedResponse>(_token, $"/primary_ips", request);
        }

        public async Task<PrimaryIPCreatedResponse> Create(Server server, string name = null, ePrimaryIPType type = ePrimaryIPType.ipv4, bool auto_delete = true, Dictionary<string, string> labels = null)
        {
            var request = new PrimaryIPCreateRequest()
            {
                AssigneeId = server.Id,
                AssigneeType = ePrimaryIPAssigneeType.server,
                AutoDelete = auto_delete,
                Name = name ?? server.Name,
                Type = type,
                Labels = labels
            };

            return await Core.SendPostRequest<PrimaryIPCreatedResponse>(_token, $"/primary_ips", request);
        }

        /// <summary>
        /// Update a Primary IP.
        /// If another change is concurrently performed on this Primary IP, a error response with code conflict will be returned.
        /// </summary>
        /// <param name="id">ID of the Primary IP.</param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<(Object.Action.Action, PrimaryIP)> Update(long id, PrimaryIPCreateRequest request)
        {
            var response = await Core.SendPutRequest<Object.Action.Get.ResponseBucket<PrimaryIP>>(_token, $"/primary_ips/{id}", request);
            
            return (response.Action, response.Response);    
        }

        /// <summary>
        /// If assigned to a Server the Primary IP will be unassigned automatically. The Server must be powered off (status off) in order for this operation to succeed.
        /// </summary>
        /// <param name="id">ID of the Primary IP.</param>
        /// <returns></returns>
        public async Task Delete(long id)
        {
            // Get list
            await Core.SendDeleteRequest(_token, $"/primary_ips/{id}");
        }
    }
}
