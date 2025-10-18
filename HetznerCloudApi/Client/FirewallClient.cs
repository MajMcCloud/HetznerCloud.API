using HetznerCloudApi.Object.Action;
using HetznerCloudApi.Object.Firewall;
using HetznerCloudApi.Object.Firewall.Get;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HetznerCloudApi.Client
{
    public class FirewallClient
    {
        private readonly string _token;

        public FirewallClient(string token)
        {
            _token = token;
        }

        /// <summary>
        /// Returns all Firewall objects.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Firewall>> Get(string name = null)
        {
            List<Firewall> list = new List<Firewall>();
            long page = 0;
            while (true)
            {
                // Next
                page++;

                // Get list
                Response response = JsonConvert.DeserializeObject<Response>(await Core.SendGetRequest(_token, $"/firewalls?page={page}&per_page={Core.PerPage}&name={name}")) ?? new Response();

                // Run
                foreach (Firewall row in response.Firewalls)
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
        /// Gets a specific Firewall object.
        /// </summary>
        /// <param name="id">ID of the resource</param>
        /// <returns></returns>
        public async Task<Firewall> Get(long id)
        {
            return (await Core.SendGetRequest<Object.Action.Get.ResponseBucket<Firewall>>(_token, $"/firewalls/{id}")).Response;
        }

        /// <summary>
        /// Creates a new Firewall.
        /// </summary>
        /// <param name="name">Name of the Firewall</param>
        /// <returns></returns>
        public async Task<Firewall> Create(string name)
        {
            return await Create(new Firewall { Name = name });
        }

        /// <summary>
        /// Creates a new Firewall.
        /// </summary>
        /// <param name="name">Name of the Firewall</param>
        /// <returns></returns>
        public async Task<Firewall> Create(Firewall new_firewall)
        {
            return (await Core.SendPostRequest<Object.Action.Get.ResponseBucket<Firewall>>(_token, "/firewalls", new_firewall)).Response;
        }


        /// <summary>
        /// Updates the Firewall.
        /// </summary>
        /// <param name="firewall"></param>
        /// <returns></returns>
        public async Task<Firewall> Update(Firewall firewall)
        {
            return (await Core.SendPutRequest<Object.Action.Get.ResponseBucket<Firewall>>(_token, $"/firewalls/{firewall.Id}", firewall)).Response;
        }

        /// <summary>
        /// Deletes a Firewall.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(long id)
        {
            await Core.SendDeleteRequest(_token, $"/firewalls/{id}");
        }

        /// <summary>
        /// Deletes a Firewall.
        /// </summary>
        /// <param name="firewall"></param>
        /// <returns></returns>
        public async Task Delete(Firewall firewall)
        {
            await Delete(firewall.Id);
        }
    }
}
