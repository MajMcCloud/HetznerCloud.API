using HetznerCloudApi.Object.Action;
using HetznerCloudApi.Object.Action.Get;
using HetznerCloudApi.Object.Firewall;
using HetznerCloudApi.Object.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HetznerCloudApi.Client
{
    public class FirewallActionClient
    {
        private readonly string _token;

        public FirewallActionClient(string token)
        {
            _token = token;
        }

        /// <summary>
        /// Apply to a single server by id
        /// </summary>
        /// <param name="firewallId"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public async Task<List<Action>> ApplyToResources(long firewallId, long serverId)
        {
            return await ApplyToResources(firewallId, new List<AppliedTo>() { AppliedTo.FromServerId(serverId) });
        }

        /// <summary>
        /// Apply to a single server by id
        /// </summary>
        /// <param name="firewall"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public async Task<List<Action>> ApplyToServer(Firewall firewall, long serverId)
        {
            return await ApplyToResources(firewall.Id, serverId);
        }

        /// <summary>
        /// Apply to a single server
        /// </summary>
        /// <param name="firewall"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public async Task<List<Action>> ApplyToServer(Firewall firewall, Object.Server.Server server)
        {
            return await ApplyToResources(firewall.Id, server.Id);
        }


        /// <summary>
        /// Apply to multiple servers   
        /// </summary>
        /// <param name="firewall"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public async Task<List<Action>> ApplyToServers(Firewall firewall, IEnumerable<Object.Server.Server> servers)
        {
            return await ApplyToResources(firewall.Id, servers.Select(a => AppliedTo.FromServer(a)));
        }

        /// <summary>
        /// Apply to multiple servers by id
        /// </summary>
        /// <param name="firewall"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public async Task<List<Action>> ApplyToServers(Firewall firewall, IEnumerable<long> serverIds)
        {
            return await ApplyToResources(firewall.Id, serverIds.Select(a => AppliedTo.FromServerId(a)));
        }

        /// <summary>
        /// Apply to Resources
        /// </summary>
        /// <param name="firewallId"></param>
        /// <param name="appliedTo"
        /// <returns></returns>
        public async Task<List<Action>> ApplyToResources(Firewall firewall, IEnumerable<AppliedTo> appliedTo)
        {
            return await ApplyToResources(firewall.Id, appliedTo);
        }


        /// <summary>
        /// Apply to Resources
        /// </summary>
        /// <param name="firewallId"></param>
        /// <param name="appliedTo"
        /// <returns></returns>
        public async Task<List<Action>> ApplyToResources(long firewallId, IEnumerable<AppliedTo> appliedTo)
        {
            // Preparing raw
            var temp = new
            {
                apply_to = appliedTo
            };

            return await Core.SendPostRequest<ResponseBucket<string>>(_token, $"/firewalls/{firewallId}/actions/apply_to_resources", temp);
        }

        /// <summary>
        /// Removes one Firewall from multiple resources.
        /// </summary>
        /// <param name="firewallId"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public async Task<List<Action>> RemoveFromResources(long firewallId, long serverId)
        {
            return await RemoveFromResources(firewallId, new List<AppliedTo>() { AppliedTo.FromServerId(serverId) });
        }

        /// <summary>
        /// Removes one Firewall from multiple resources.
        /// </summary>
        /// <param name="firewallId"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public async Task<List<Action>> RemoveFromResources(long firewallId, IEnumerable<AppliedTo> removeFrom)
        {
            // Preparing raw
            var temp = new
            {
                remove_from = removeFrom
            };

            return await Core.SendPostRequest<ResponseBucket<string>>(_token, $"/firewalls/{firewallId}/actions/remove_from_resources", temp);

        }

        /// <summary>
        /// Removes one Firewall from multiple resources.
        /// </summary>
        /// <param name="firewall"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public async Task<List<Action>> RemoveFromResources(Firewall firewall, Object.Server.Server server)
        {
            return await RemoveFromResources(firewall.Id, new List<AppliedTo>() { AppliedTo.FromServerId(server.Id) });
        }

        /// <summary>
        /// Remove from multiple servers   
        /// </summary>
        /// <param name="firewall"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public async Task<List<Action>> RemoveFromResources(Firewall firewall, IEnumerable<Object.Server.Server> servers)
        {
            return await RemoveFromResources(firewall.Id, servers.Select(a => AppliedTo.FromServer(a)));
        }

        /// <summary>
        /// Remove from multiple servers by id
        /// </summary>
        /// <param name="firewall"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public async Task<List<Action>> RemoveFromResources(Firewall firewall, IEnumerable<long> serverIds)
        {
            return await RemoveFromResources(firewall.Id, serverIds.Select(a => AppliedTo.FromServerId(a)));
        }

        /// <summary>
        /// Sets the rules of a Firewall.
        /// </summary>
        /// <param name="firewall"></param>
        /// <param name="rules"></param>
        /// <returns></returns>
        public async Task<List<Action>> SetRules(Firewall firewall, List<Rule> rules)
        {
            var temp = new
            {
                rules = rules
            };

            // Preparing raw
            string raw = JsonConvert.SerializeObject(temp, Formatting.Indented);

            // Send update
            string jsonResponse = await Core.SendPostRequest(_token, $"/firewalls/{firewall.Id}/actions/set_rules", raw);

            // Return
            JObject result = JObject.Parse(jsonResponse);
            return JsonConvert.DeserializeObject<List<Action>>($"{result["actions"]}") ?? new List<Action>();
        }

        /// <summary>
        /// Returns all Action objects for a Firewall.
        /// </summary>
        /// <param name="firewallId">ID of the Resource</param>
        /// <returns></returns>
        public async Task<List<Action>> GetAllActions(long firewallId)
        {
            List<Action> list = new List<Action>();
            long page = 0;
            while (true)
            {
                // Nex
                page++;

                // Get list
                Response response = JsonConvert.DeserializeObject<Response>(await Core.SendGetRequest(_token, $"/firewalls/{firewallId}/actions?page={page}&per_page={Core.PerPage}")) ?? new Response();

                // Run
                foreach (Action row in response.Actions)
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
        /// Returns a specific Action for a Firewall.
        /// </summary>
        /// <param name="firewallId">ID of the Firewall</param>
        /// <param name="actionId">ID of the Action</param>
        /// <returns></returns>
        public async Task<Action> GetAction(long firewallId, long actionId)
        {
            // Get list
            string json = await Core.SendGetRequest(_token, $"/firewalls/{firewallId}/actions/{actionId}");

            // Set
            JObject result = JObject.Parse(json);
            Action action = JsonConvert.DeserializeObject<Action>($"{result["action"]}") ?? new Action();

            // Return
            return action;
        }

        /// <summary>
        /// Returns all Action objects
        /// </summary>
        /// <returns></returns>
        public async Task<List<Action>> GetAllActions()
        {
            List<Action> list = new List<Action>();
            long page = 0;
            while (true)
            {
                // Nex
                page++;

                // Get list
                Response response = JsonConvert.DeserializeObject<Response>(await Core.SendGetRequest(_token, $"/firewalls/actions?page={page}&per_page={Core.PerPage}")) ?? new Response();

                // Run
                foreach (Action row in response.Actions)
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
        /// Returns a specific Action object
        /// </summary>
        /// <param name="actionId">ID of the Resource</param>
        /// <returns></returns>
        public async Task<Action> GetAction(long actionId)
        {
            // Get list
            string json = await Core.SendGetRequest(_token, $"/firewalls/actions/{actionId}");

            // Set
            JObject result = JObject.Parse(json);
            Action action = JsonConvert.DeserializeObject<Action>($"{result["action"]}") ?? new Action();

            // Return
            return action;
        }
    }
}
