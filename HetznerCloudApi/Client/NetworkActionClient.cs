using HetznerCloudApi.Object.Action;
using HetznerCloudApi.Object.Action.Get;
using HetznerCloudApi.Object.Network;
using HetznerCloudApi.Object.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HetznerCloudApi.Client
{
    public class NetworkActionClient
    {
        private readonly string _token;

        public NetworkActionClient(string token)
        {
            _token = token;
        }

        /// <summary>
        /// Adds a new subnet object to the Network. If you do not specify an ip_range for the subnet we will automatically pick the first available /24 range for you if possible.
        /// </summary>
        /// <param name="networkId">ID of the Network</param>
        /// <param name="ipRange">Range to allocate IPs from. Must be a Subnet of the ip_range of the parent network object and must not overlap with any other subnets or with any destinations in routes. If the Subnet is of type vSwitch, it also can not overlap with any gateway in routes. Minimum Network size is /30. We suggest that you pick a bigger Network with a /24 netmask.</param>
        /// <param name="networkZone">Name of Network zone. The Location object contains the network_zone property each Location belongs to.</param>
        /// <param name="type"></param>
        /// <returns><see cref="Action"/></returns>
        public async Task<Action> AddSubnetToNetwork(long networkId, string ipRange, string networkZone, eSubnetType type = eSubnetType.cloud)
        {
            return await AddSubnetToNetwork(networkId, new Subnet
            {
                IpRange =  ipRange,
                NetworkZone = networkZone,
                Type = type
            });
        }

        /// <summary>
        /// Adds a new subnet object to the Network. If you do not specify an ip_range for the subnet we will automatically pick the first available /24 range for you if possible.
        /// </summary>
        /// <param name="networkId"></param>
        /// <param name="subnet"></param>
        /// <returns><see cref="Action"/></returns>
        public async Task<Action> AddSubnetToNetwork(long networkId, Subnet subnet)
        {
            return await Core.SendPostRequest<SimpleActionResponse>(_token, $"/networks/{networkId}/actions/add_subnet", subnet);
        }

        /// <summary>
        /// Deletes a single subnet entry from a Network.
        /// <para></para>
        /// You cannot delete subnets which still have Servers attached.
        /// <para></para>
        /// If you have Servers attached you first need to detach all Servers that use IPs from this subnet before you can delete the subnet.
        /// </summary>
        /// <param name="networkId">ID of the Network</param>
        /// <param name="ipRange">IP range of subnet to delete</param>
        /// <returns><see cref="Action"/></returns>
        public async Task<Action> DeleteSubnetFromNetwork(long networkId, string ipRange)
        {
            return await DeleteSubnetFromNetwork(networkId, new Subnet
            {
                IpRange = ipRange
            });
        }

        /// <summary>
        /// Deletes a single subnet entry from a Network.
        /// <para></para>
        /// You cannot delete subnets which still have Servers attached.
        /// <para></para>
        /// If you have Servers attached you first need to detach all Servers that use IPs from this subnet before you can delete the subnet.
        /// </summary>
        /// <param name="networkId"></param>
        /// <param name="subnet"></param>
        /// <returns><see cref="Action"/></returns>
        public async Task<Action> DeleteSubnetFromNetwork(long networkId, Subnet subnet)
        {
            return await Core.SendPostRequest<SimpleActionResponse>(_token, $"/networks/{networkId}/actions/delete_subnet", subnet);
        }

        /// <summary>
        /// Adds a route entry to a Network.
        /// If a change is currently being performed on this Network, a error response with code conflict will be returned.
        /// </summary>
        /// <param name="networkId"></param>
        /// <param name="route"></param>
        /// <returns><see cref="Action"/></returns>
        public async Task<Action> AddRouteToNetwork(long networkId, Route route)
        {
            return await Core.SendPostRequest<SimpleActionResponse>(_token, $"/networks/{networkId}/actions/add_route", route);
        }

        /// <summary>
        /// Delete a route entry from a Network.
        /// If a change is currently being performed on this Network, a error response with code conflict will be returned.
        /// </summary>
        /// <param name="networkId"></param>
        /// <param name="route"></param>
        /// <returns><see cref="Action"/></returns>
        public async Task<Action> DeleteRouteFromNetwork(long networkId, Route route)
        {
            return await Core.SendPostRequest<SimpleActionResponse>(_token, $"/networks/{networkId}/actions/delete_route", route);
        }

        /// <summary>
        /// Changes the IP range of a Network.
        /// <para></para>
        /// The following restrictions apply to changing the IP range:
        /// <para></para>
        /// IP ranges can only be extended and never shrunk.
        /// IPs can only be added to the end of the existing range, therefore only the netmask is allowed to be changed.
        /// To update the routes on the connected Servers, they need to be rebooted or the routes to be updated manually.
        /// <para></para>
        /// For example if the Network has a range of 10.0.0.0/16 to extend it the new range has to start with the IP 10.0.0.0 as well.The netmask /16 can be changed to a smaller one then 16 therefore increasing the IP range.A valid entry would be 10.0.0.0/15, 10.0.0.0/14 or 10.0.0.0/13 and so on.
        /// <para></para>
        /// If a change is currently being performed on this Network, a error response with code conflict will be returned.
        /// </summary>
        /// <param name="networkId"></param>
        /// <param name="ipRange"></param>
        /// <returns><see cref="Action"/></returns>
        public async Task<Action> ChangeIPRange(long networkId, string ipRange)
        {
            var tmp = new
            {
                ip_range = ipRange
            };
            return await Core.SendPostRequest<SimpleActionResponse>(_token, $"/networks/{networkId}/actions/change_ip_range", tmp);
        }


        /// <summary>
        /// Changes the protection configuration of a Network.
        /// </summary>
        /// <param name="network"></param>
        /// <param name="protection"></param>
        /// <returns><see cref="Action"/></returns>
        public async Task<Action> ChangeProtection(Network network, bool protection)
        {
            return await ChangeProtection(network.Id, protection);
        }

        /// <summary>
        /// Changes the protection configuration of a Network.
        /// </summary>
        /// <param name="networkId">ID of the Network</param>
        /// <param name="protection">If true, prevents the Network from being deleted</param>
        /// <returns><see cref="Action"/></returns>
        public async Task<Action> ChangeProtection(long networkId, bool protection)
        {
            var tmp = new
            {
                delete = protection
            };

            return await Core.SendPostRequest<SimpleActionResponse>(_token, $"/networks/{networkId}/actions/change_protection", tmp);
        }



        /// <summary>
        /// Get all Actions for a Network
        /// </summary>
        /// <param name="networkId"></param>
        /// <returns></returns>
        public async Task<List<Action>> GetAllActions(long networkId)
        {
            List<Action> list = new List<Action>();
            long page = 0;
            while (true)
            {
                // Nex
                page++;

                // Get list
                Response response = JsonConvert.DeserializeObject<Response>(await Core.SendGetRequest(_token, $"/networks/{networkId}/actions?page={page}&per_page={Core.PerPage}")) ?? new Response();

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
        /// Get an Action for a Network
        /// </summary>
        /// <param name="networkId"></param>
        /// <param name="actionId"></param>
        /// <returns><see cref="Action"/></returns>
        public async Task<Action> GetAction(long networkId, long actionId)
        {
            // Return
            return await Core.SendGetRequest<SimpleActionResponse>(_token, $"/networks/{networkId}/actions/{actionId}");
        }

        /// <summary>
        /// Get all Actions
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
                Response response = JsonConvert.DeserializeObject<Response>(await Core.SendGetRequest(_token, $"/networks/actions?page={page}&per_page={Core.PerPage}")) ?? new Response();

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
        /// Get an Action
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns><see cref="Action"/></returns>
        public async Task<Action> GetAction(long actionId)
        {
            // Return
            return await Core.SendGetRequest<SimpleActionResponse>(_token, $"/networks/actions/{actionId}");
        }
    }
}
