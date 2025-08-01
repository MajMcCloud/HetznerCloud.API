using HetznerCloudApi.Object.Action;
using HetznerCloudApi.Object.Action.Get;
using HetznerCloudApi.Object.ServerAction;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HetznerCloudApi.Client
{
    public class ServerActionClient
    {
        private readonly string _token;

        public ServerActionClient(string token)
        {
            _token = token;
        }

        /// <summary>
        /// Returns all Action objects. You can sort the results by using the sort URI parameter, and filter them with the status and id parameter.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Action>> GetActions()
        {
            List<Action> list = new List<Action>();
            long page = 0;
            while (true)
            {
                // Nex
                page++;

                // Get list
                Response response = JsonConvert.DeserializeObject<Response>(await Core.SendGetRequest(_token, $"/server/actions?page={page}&per_page={Core.PerPage}")) ?? new Response();

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
        /// Returns a specific Action object.
        /// </summary>
        /// <param name="actionId">ID of the Action</param>
        /// <returns></returns>
        public async Task<Action> Get(long actionId)
        {
            // Get list
            string json = await Core.SendGetRequest(_token, $"/servers/actions/{actionId}");

            // Set
            JObject result = JObject.Parse(json);
            Action action = JsonConvert.DeserializeObject<Action>($"{result["action"]}") ?? new Action();

            // Return
            return action;
        }

        /// <summary>
        /// Returns all Action objects for a Server. You can sort the results by using the sort URI parameter, and filter them with the status parameter.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Action>> GetServerActions(long Id)
        {
            List<Action> list = new List<Action>();
            long page = 0;
            while (true)
            {
                // Nex
                page++;

                // Get list
                Response response = JsonConvert.DeserializeObject<Response>(await Core.SendGetRequest(_token, $"/server/{Id}/actions?page={page}&per_page={Core.PerPage}")) ?? new Response();

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
        /// Returns a specific Action object.
        /// </summary>
        /// <param name="Id">ID of the Server</param>
        /// <param name="actionId">ID of the Action</param>
        /// <returns></returns>
        public async Task<Action> Get(long Id, long actionId)
        {
            // Get list
            string json = await Core.SendGetRequest(_token, $"/servers/{Id}/actions/{actionId}");

            // Set
            JObject result = JObject.Parse(json);
            Action action = JsonConvert.DeserializeObject<Action>($"{result["action"]}") ?? new Action();

            // Return
            return action;
        }

        /// <summary>
        /// Starts a Server by turning its power on.
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <returns></returns>
        public async Task PowerOn(long id)
        {
            await Core.SendPostRequest(_token, $"/servers/{id}/actions/poweron");
        }

        /// <summary>
        /// Cuts power to the Server. This forcefully stops it without giving the Server operating system time to gracefully stop. May lead to data loss, equivalent to pulling the power cord. Power off should only be used when shutdown does not work.
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <returns></returns>
        public async Task PowerOff(long id)
        {
            await Core.SendPostRequest(_token, $"/servers/{id}/actions/poweroff");
        }

        /// <summary>
        /// Reboots a Server gracefully by sending an ACPI request. The Server operating system must support ACPI and react to the request, otherwise the Server will not reboot.
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <returns></returns>
        public async Task SoftReboot(long id)
        {
            await Core.SendPostRequest(_token, $"/servers/{id}/actions/reboot");
        }

        /// <summary>
        /// Cuts power to a Server and starts it again. This forcefully stops it without giving the Server operating system time to gracefully stop. This may lead to data loss, it’s equivalent to pulling the power cord and plugging it in again. Reset should only be used when reboot does not work.
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <returns></returns>
        public async Task Reset(long id)
        {
            await Core.SendPostRequest(_token, $"/servers/{id}/actions/reset");
        }

        /// <summary>
        /// Shuts down a Server gracefully by sending an ACPI shutdown request. The Server operating system must support ACPI and react to the request, otherwise the Server will not shut down. Please note that the action status in this case only reflects whether the action was sent to the server. It does not mean that the server actually shut down successfully. If you need to ensure that the server is off, use the poweroff action.
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <returns></returns>
        public async Task Shutdown(long id)
        {
            await Core.SendPostRequest(_token, $"/servers/{id}/actions/shutdown");
        }

        /// <summary>
        /// Changes the type (Cores, RAM and disk sizes) of a Server.
        /// 
        /// Server must be powered off for this command to succeed.
        /// 
        /// This copies the content of its disk, and starts it again.
        /// 
        /// You can only migrate to Server types with the same storage_type and equal or bigger disks.Shrinking disks is not possible as it might destroy data.
        /// 
        /// 
        /// If the disk gets upgraded, the Server type can not be downgraded any more.If you plan to downgrade the Server type, set upgrade_disk to false.
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <param name="Upgrade_Disk">If false, do not upgrade the disk (this allows downgrading the Server type later).</param>
        /// <param name="Server_Type">ID or name of Server type the Server should migrate to.</param>
        /// <returns></returns>
        public async Task<Action> ChangeType(long id, bool Upgrade_Disk, string Server_Type)
        {
            var request = new ChangeTypeRequest
            {
                Upgrade_Disk = Upgrade_Disk,
                Server_Type = Server_Type
            };

            return await Core.SendPostRequest<Action>(_token, $"/servers/{id}/actions/change_type", request);
        }


        /// <summary>
        /// Changes the protection configuration of the Server.
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <param name="Delete">If true, prevents the Server from being deleted (currently delete and rebuild attribute needs to have the same value).</param>
        /// <param name="Rebuild">If true, prevents the Server from being rebuilt (currently delete and rebuild attribute needs to have the same value).</param>
        /// <returns></returns>
        public async Task<Action> ChangeProtection(long id, bool? Delete, bool? Rebuild)
        {
            var request = new ChangeProtectionRequest
            {
                Delete = Delete,
                Rebuild = Rebuild
            };


            return await Core.SendPostRequest<Action>(_token, $"/servers/{id}/actions/change_type", request);
        }

        /// <summary>
        /// Attaches a Server to a network. This will complement the fixed public Server interface by adding an additional ethernet interface to the Server which is connected to the specified network.
        ///
        ///The Server will get an IP auto assigned from a subnet of type server in the same network_zone.
        ///
        ///Using the alias_ips attribute you can also define one or more additional IPs to the Servers.Please note that you will have to configure these IPs by hand on your Server since only the primary IP will be given out by DHCP.
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <param name="network">ID of an existing network to attach the Server to.</param>
        /// <param name="ip">IP to request to be assigned to this Server; if you do not provide this then you will be auto assigned an IP address.</param>
        /// <param name="alias_ips">Additional IPs to be assigned to this Server.</param>
        /// <returns></returns>
        public async Task<Action> AttachToNetwork(long id, long network, string ip = null, string[] alias_ips = null)
        {
            var request = new AttachToNetworkRequest
            {
                Network = network,
                IP = ip,
                Alias_IPs = alias_ips
            };

            return await Core.SendPostRequest<Action>(_token, $"/servers/{id}/actions/attach_to_network", request);
        }

        /// <summary>
        /// Detaches a Server from a network. The interface for this network will vanish.
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <param name="network">ID of an existing network to detach the Server from.</param>
        /// <returns></returns>
        public async Task<Action> DetachFromNetwork(long id, long network)
        {
            var request = new DetachFromNetworkRequest
            {
                Network = network,
            };

            return await Core.SendPostRequest<Action>(_token, $"/servers/{id}/actions/detach_from_network", request);
        }
    }
}
