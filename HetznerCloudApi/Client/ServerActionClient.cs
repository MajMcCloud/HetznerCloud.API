using HetznerCloudApi.Object.Action;
using HetznerCloudApi.Object.Action.Get;
using HetznerCloudApi.Object.ISOs;
using HetznerCloudApi.Object.ServerAction;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
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
        public async Task<Action> GetAction(long actionId)
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
        public async Task<Action> GetAction(long Id, long actionId)
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
        public async Task<Action> PowerOn(long id)
        {
            return await Core.SendPostRequest<Action>(_token, $"/servers/{id}/actions/poweron");
        }

        /// <summary>
        /// Cuts power to the Server. This forcefully stops it without giving the Server operating system time to gracefully stop. May lead to data loss, equivalent to pulling the power cord. Power off should only be used when shutdown does not work.
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <returns></returns>
        public async Task<Action> PowerOff(long id)
        {
            return await Core.SendPostRequest<Action>(_token, $"/servers/{id}/actions/poweroff");
        }

        /// <summary>
        /// Reboots a Server gracefully by sending an ACPI request. The Server operating system must support ACPI and react to the request, otherwise the Server will not reboot.
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <returns></returns>
        public async Task<Action> SoftReboot(long id)
        {
            return await Core.SendPostRequest<Action>(_token, $"/servers/{id}/actions/reboot");
        }

        /// <summary>
        /// Cuts power to a Server and starts it again. This forcefully stops it without giving the Server operating system time to gracefully stop. This may lead to data loss, it’s equivalent to pulling the power cord and plugging it in again. Reset should only be used when reboot does not work.
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <returns></returns>
        public async Task<Action> Reset(long id)
        {
            return await Core.SendPostRequest<Action>(_token, $"/servers/{id}/actions/reset");
        }

        /// <summary>
        /// Shuts down a Server gracefully by sending an ACPI shutdown request. The Server operating system must support ACPI and react to the request, otherwise the Server will not shut down. Please note that the action status in this case only reflects whether the action was sent to the server. It does not mean that the server actually shut down successfully. If you need to ensure that the server is off, use the poweroff action.
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <returns></returns>
        public async Task<Action> Shutdown(long id)
        {
            return await Core.SendPostRequest<Action>(_token, $"/servers/{id}/actions/shutdown");
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

        /// <summary>
        /// Enables and configures the automatic daily backup option for the Server.  <para><c>Enabling automatic backups will increase the price of the Server by 20%. </c></para> In return, you will get seven slots where Images of type backup can be stored.
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <returns></returns>
        public async Task<Action> EnableBackup(long id)
        {
            return await Core.SendPostRequest<Action>(_token, $"/servers/{id}/actions/enable_backup");
        }

        /// <summary>
        /// Disables the automatic backup option and deletes all existing Backups for a Server. No more additional charges for backups will be made.
        /// <para><c>Caution: This immediately removes all existing backups for the Server!</c></para>
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <returns></returns>
        public async Task<Action> DisableBackup(long id)
        {
            return await Core.SendPostRequest<Action>(_token, $"/servers/{id}/actions/disable_backup");
        }

        /// <summary>
        /// Enable the Hetzner Rescue System for this Server. The next time a Server with enabled rescue mode boots it will start a special minimal Linux distribution designed for repair and reinstall.
        ///<para>In case a Server cannot boot on its own you can use this to access a Server’s disks.</para>
        ///<para>Rescue Mode is automatically disabled when you first boot into it or if you do not use it for 60 minutes</para>
        ///<para><c>Enabling rescue mode will not reboot your Server — you will have to do this yourself.</c></para>
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <param name="type">Type of rescue system to boot. (Allowed: linux64, Default: linux64)</param>
        /// <returns></returns>
        public async Task<Action> EnableRescueMode(long id, string type)
        {
            return await Core.SendPostRequest<Action>(_token, $"/servers/{id}/actions/enable_rescue");
        }

        /// <summary>
        /// Disables the Hetzner Rescue System for a Server. This makes a Server start from its disks on next reboot.
        /// <para>Rescue Mode is automatically disabled when you first boot into it or if you do not use it for 60 minutes.</para>
        /// <para><c>Disabling rescue mode will not reboot your Server — you will have to do this yourself.</c></para>
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <returns></returns>
        public async Task<Action> DisableRescueMode(long id)
        {
            return await Core.SendPostRequest<Action>(_token, $"/servers/{id}/actions/disable_rescue");
        }

        /// <summary>
        /// Adds a Server to a Placement Group.
        /// <para><c>Server must be powered off for this command to succeed.</c></para>
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <returns></returns>
        public async Task<Action> AddToPlacementGroup(long id)
        {
            // Preparing raw
            string raw = $"{{ \"placement_group\": {id} }}";

            return await Core.SendPostRequest<Action>(_token, $"/servers/{id}/actions/add_to_placement_group", raw);
        }

        /// <summary>
        /// Removes a Server from a Placement Group.
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <returns></returns>
        public async Task<Action> RemoveFromPlacementGroup(long id)
        {
            return await Core.SendPostRequest<Action>(_token, $"/servers/{id}/actions/remove_from_placement_group");
        }

        /// <summary>
        /// Resets the root password. <c>Only works for Linux systems that are running the qemu guest agent.</c> 
        /// <para>Server must be <c>powered on (status running)</c> in order for this operation to succeed.</para>
        ///
        /// <para>This will generate a new password for this Server and return it.</para>
        ///
        /// <para>If this does not succeed you can use the rescue system to netboot the Server and manually change your Server password by hand.</para>
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <returns></returns>
        public async Task<ResetRootPasswordResponse> ResetRootPassword(long id)
        {
            return await Core.SendPostRequest<ResetRootPasswordResponse>(_token, $"/servers/{id}/actions/reset_password");
        }

        /// <summary>
        /// Requests credentials for remote access via VNC over websocket to keyboard, monitor, and mouse for a Server. The provided URL is valid for 1 minute, after this period a new url needs to be created to connect to the Server. How long the connection is open after the initial connect is not subject to this timeout.
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <returns></returns>
        public async Task<RequestConsoleResponse> RequestConsole(long id)
        {
            return await Core.SendPostRequest<RequestConsoleResponse>(_token, $"/servers/{id}/actions/request_console");
        }

        /// <summary>
        /// Attaches an ISO to a Server. The Server will immediately see it as a new disk. An already attached ISO will automatically be detached before the new ISO is attached.

        /// Servers with attached ISOs have a modified boot order: They will try to boot from the ISO first before falling back to hard disk.
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <param name="iso">ID or name of ISO to attach to the Server as listed in GET /isos.</param>
        /// <returns></returns>
        public async Task<Action> AttachISO(long id, string iso)
        {
            // Preparing raw
            string raw = $"{{ \"iso\": \"{iso}\" }}";

            return await Core.SendPostRequest<Action>(_token, $"/servers/{id}/actions/attach_iso", raw);
        }

        /// <summary>
        /// Detaches an ISO from a Server. In case no ISO Image is attached to the Server, the status of the returned Action is immediately set to success.
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <returns></returns>
        public async Task<Action> DetachISO(long id)
        {
            return await Core.SendPostRequest<Action>(_token, $"/servers/{id}/actions/detach_iso");
        }

        /// <summary>
        /// Changes the alias IPs of an already attached Network. Note that the existing aliases for the specified Network will be replaced with these provided in the request body. So if you want to add an alias IP, you have to provide the existing ones from the Network plus the new alias IP in the request body.
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <param name="network">ID of an existing Network already attached to the Server.</param>
        /// <param name="alias_ips">New alias IPs to set for this Server.</param>
        /// <returns></returns>
        public async Task<Action> ChangeAliasIPs(long id, long network, string[] alias_ips)
        {
            var request = new ChangeAliasIPRequest
            {
                Network = network,
                Alias_IPs = alias_ips
            };
            return await Core.SendPostRequest<Action>(_token, $"/servers/{id}/actions/change_alias_ips", request);
        }

        /// <summary>
        /// Changes the hostname that will appear when getting the hostname belonging to the primary IPs (IPv4 and IPv6) of this Server.
        /// <para><c>Floating IPs assigned to the Server are not affected by this.</c></para>
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <param name="ip">Primary IP address for which the reverse DNS entry should be set.</param>
        /// <param name="dns_ptr">Hostname to set as a reverse DNS PTR entry, reset to original value if null.</param>
        /// <returns></returns>
        public async Task<Action> ChangeReverseDNSEntry(long id, string ip, string dns_ptr)
        {
            var request = new ChangeReverseDNSEntryRequest
            {
                IP = ip,
                DNS_Ptr = dns_ptr
            };

            return await Core.SendPostRequest<Action>(_token, $"/servers/{id}/actions/change_dns_ptr", request);
        }

        /// <summary>
        /// Creates an Image (snapshot) from a Server by copying the contents of its disks. This creates a snapshot of the current state of the disk and copies it into an Image. If the Server is currently running you must make sure that its disk content is consistent. Otherwise, the created Image may not be readable.
        /// <para>To make sure disk content is consistent, we recommend to shut down the Server prior to creating an Image.</para>
        /// <para>You can either create a backup Image that is bound to the Server and therefore will be deleted when the Server is deleted, or you can create a snapshot Image which is completely independent of the Server it was created from and will survive Server deletion. Backup Images are only available when the backup option is enabled for the Server. Snapshot Images are billed on a per GB basis.</para>
        /// </summary>
        /// <param name="id">ID of the Server.</param>
        /// <param name="description">Description of the Image, will be auto-generated if not set.</param>
        /// <param name="type">Type of Image to create. (Allowed: snapshot backup, Default: snapshot)</param>
        /// <param name="labels">User-defined labels (key/value pairs) for the Resource. For more information, see "Labels".</param>
        /// <returns></returns>
        public async Task<Action> CreateImage(long id, string description, string type, Dictionary<string, string> labels)
        {
            var request = new CreateImageRequest
            {
                Description = description,
                Type = type,
                Labels = labels
            };

            return await Core.SendPostRequest<Action>(_token, $"/servers/{id}/actions/create_image", request);
        }

        /// <summary>
        /// <c>Rebuilds a Server overwriting its disk with the content of an Image, thereby destroying all data on the target Server</c>
        /// <para>The Image can either be one you have created earlier(backup or snapshot Image) or it can be a completely fresh system Image provided by us.</para>
        /// <para>You can get a list of all available Images with GET /images.</para>
        /// <para>Your Server will automatically be powered off before the rebuild command executes.</para>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public async Task<RebuildFromImageResponse> RebuildFromImage(long id, string image)
        {
            // Preparing raw
            string raw = $"{{ \"image\": \"{image}\" }}";

            return await Core.SendPostRequest<RebuildFromImageResponse>(_token, $"/servers/{id}/actions/rebuild", raw);
        }
    }
}
