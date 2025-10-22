using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using HetznerCloudApi.Object.Network;
using HetznerCloudApi.Object.Network.Get;
using HetznerCloudApi.Object.SshKey;

namespace HetznerCloudApi.Client
{
    public class NetworkClient
    {
        private readonly string _token;

        public NetworkClient(string token)
        {
            _token = token;
        }

        /// <summary>
        /// Gets all existing networks that you have available.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Network>> Get()
        {
            List<Network> list = new List<Network>();
            long page = 0;
            while (true)
            {
                // Nex
                page++;

                // Get list
                Response response = JsonConvert.DeserializeObject<Response>(await Core.SendGetRequest(_token, $"/networks?page={page}&per_page={Core.PerPage}")) ?? new Response();

                // Run
                foreach (Network row in response.Networks)
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
        /// Gets a specific network object.
        /// </summary>
        /// <param name="id"></param>
        /// <returns><see cref="Network"/></returns>
        public async Task<Network> Get(long id)
        {
            // Return
            return await Core.SendGetRequest<Object.Action.Get.ResponseBucket<Network>>(_token, $"/networks/{id}");
        }

        /// <summary>
        /// Creates a network with the specified ip_range.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ipRange"></param>
        /// <returns><see cref="Network"/></returns>
        public async Task<Network> Create(string name, string ipRange)
        {
            return await Create(new Network { Name = name, IpRange = ipRange });
        }

        public async Task<Network> Create(Network network)
        {
            return await Core.SendPostRequest<Object.Action.Get.ResponseBucket<Network>>(_token, "/networks", network);
        }

        /// <summary>
        /// Update the network name.
        /// </summary>
        /// <param name="network"></param>
        /// <returns><see cref="Network"/></returns>
        public async Task<Network> Update(Network network)
        {
            return await Core.SendPutRequest<Object.Action.Get.ResponseBucket<Network>>(_token, $"/networks/{network.Id}", network);
        }

        /// <summary>
        /// Delete a Network
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(long id)
        {
            await Core.SendDeleteRequest(_token, $"/networks/{id}");
        }

        /// <summary>
        /// Delete a Network
        /// </summary>
        /// <param name="network"></param>
        /// <returns></returns>
        public async Task Delete(Network network)
        {
            await Delete(network.Id);
        }
    }
}
