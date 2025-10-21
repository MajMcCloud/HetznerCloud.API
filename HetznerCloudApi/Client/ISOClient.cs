using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using HetznerCloudApi.Object.ISOs;
using HetznerCloudApi.Object.ISOs.Get;

namespace HetznerCloudApi.Client
{
    public class ISOClient
    {
        private readonly string _token;

        public ISOClient(string token)
        {
            _token = token;
        }

        /// <summary>
        /// Get all Actions
        /// </summary>
        /// <returns></returns>
        public async Task<List<ISO>> Get()
        {
            List<ISO> list = new List<ISO>();
            long page = 0;
            while (true)
            {
                // Next
                page++;

                // Get list
                Response response = JsonConvert.DeserializeObject<Response>(await Core.SendGetRequest(_token, $"/isos?page={page}&per_page={Core.PerPage}")) ?? new Response();

                // Run
                foreach (ISO row in response.ISOs)
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
        /// Returns a specific Action object.
        /// </summary>
        /// <param name="actionId">ID of the Resource</param>
        /// <returns></returns>
        public async Task<ISO> Get(long Id)
        {
            // Return
            return await Core.SendGetRequest<Object.Action.Get.ResponseBucket<ISO>>(_token, $"/isos/{Id}");
        }
    }
}
