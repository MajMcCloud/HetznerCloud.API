using HetznerCloudApi.Object.Action;
using HetznerCloudApi.Object.Action.Get;
using HetznerCloudApi.Object.Server;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HetznerCloudApi.Client
{
    public class PrimaryIPActionClient
    {
        private readonly string _token;

        public PrimaryIPActionClient(string token)
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
                Response response = JsonConvert.DeserializeObject<Response>(await Core.SendGetRequest(_token, $"/primary_ips/actions?page={page}&per_page={Core.PerPage}")) ?? new Response();

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
            return (await Core.SendGetRequest<SimpleActionResponse>(_token, $"/primary_ips/actions/{actionId}")).Action;
        }

        /// <summary>
        /// Assigns a primary IP address to a specified resource.
        /// </summary>
        /// <remarks>This method sends an asynchronous request to assign the specified primary IP address
        /// to the given resource. Ensure that the <paramref name="Assignee_Type"/> and <paramref name="Assignee_Id"/>
        /// correspond to a valid resource.</remarks>
        /// <param name="primaryIPId">The unique identifier of the primary IP address to be assigned.</param>
        /// <param name="Assignee_Type">The type of the resource to which the primary IP address will be assigned (e.g., "server", "load_balancer").</param>
        /// <param name="Assignee_Id">The unique identifier of the resource to which the primary IP address will be assigned.</param>
        /// <returns>An <see cref="Action"/> object representing the result of the assignment operation.</returns>
        public async Task<Action> AssignToResource(long primaryIPId, string Assignee_Type, long Assignee_Id)
        {
            var tmp = new
            {
                assignee_type = Assignee_Type,
                assignee_id = Assignee_Id
            };


            return (await Core.SendPostRequest<SimpleActionResponse>(_token, $"/primary_ips/{primaryIPId}/actions/assign", tmp)).Action;

        }

        /// <summary>
        /// Assigns the specified primary IP address to a server.
        /// </summary>
        /// <remarks>This method assigns a primary IP address to a server resource. Ensure that the
        /// provided identifiers are valid and that the primary IP address is not already assigned to another
        /// resource.</remarks>
        /// <param name="primaryIPId">The unique identifier of the primary IP address to be assigned.</param>
        /// <param name="serverId">The unique identifier of the server to which the primary IP address will be assigned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="Action"/>
        /// indicating the outcome of the assignment.</returns>
        public async Task<Action> AssignToServer(long primaryIPId, long serverId)
        {
            return await AssignToResource(primaryIPId, "server", serverId);
        }


        /// <summary>
        /// Assigns the specified primary IP address to the given server.
        /// </summary>
        /// <remarks>This method performs the assignment asynchronously and relies on the server's unique
        /// identifier to complete the operation. Ensure that the <paramref name="primaryIPId"/> corresponds to a valid
        /// primary IP address.</remarks>
        /// <param name="primaryIPId">The unique identifier of the primary IP address to be assigned.</param>
        /// <param name="server">The server to which the primary IP address will be assigned. The server's <see cref="Server.Id"/> property
        /// is used to identify the target server.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="Action"/>
        /// indicating the outcome of the assignment.</returns>
        public async Task<Action> AssignToServer(long primaryIPId, Server server)
        {
            return await AssignToResource(primaryIPId, "server", server.Id);
        }

        /// <summary>
        /// Unassigns the specified primary IP address from its associated resource.
        /// </summary>
        /// <remarks>This method sends an asynchronous request to unassign the primary IP address. Ensure
        /// that the provided <paramref name="primaryIPId"/> corresponds to a valid and existing primary IP
        /// address.</remarks>
        /// <param name="primaryIPId">The unique identifier of the primary IP address to unassign.</param>
        /// <returns>An <see cref="Action"/> object representing the unassignment operation.</returns>
        public async Task<Action> UnassignFromResource(long primaryIPId)
        {
            return (await Core.SendPostRequest<SimpleActionResponse>(_token, $"/primary_ips/{primaryIPId}/actions/unassign")).Action;

        }

        /// <summary>
        /// Changes the reverse DNS pointer (PTR record) for the specified primary IP address.
        /// </summary>
        /// <remarks>This method sends an asynchronous request to update the reverse DNS pointer for the
        /// specified primary IP address. Ensure that the provided <paramref name="DNSPtr"/> value is valid and meets
        /// the requirements of the DNS system.</remarks>
        /// <param name="primaryIPId">The unique identifier of the primary IP address for which the DNS PTR record is to be changed.</param>
        /// <param name="ip">The IP address associated with the primary IP for which the DNS PTR record is being updated.</param>
        /// <param name="DNSPtr">The new reverse DNS pointer (PTR record) to be set.</param>
        /// <returns>An <see cref="Action"/> object representing the result of the DNS PTR change operation.</returns>
        public async Task<Action> ChangeDNSPtr(long primaryIPId, string ip, string DNSPtr)
        {
            var tmp = new
            {
                dns_ptr = DNSPtr,
                ip = ip
            };

            return (await Core.SendPostRequest<SimpleActionResponse>(_token, $"/primary_ips/{primaryIPId}/actions/change_dns_ptr", tmp)).Action;

        }


        /// <summary>
        /// Enables or disables protection for the specified primary IP address.
        /// </summary>
        /// <remarks>This method sends an asynchronous request to modify the protection settings of the
        /// specified primary IP address. Ensure that the provided <paramref name="primaryIPId"/> corresponds to a valid
        /// resource.</remarks>
        /// <param name="primaryIPId">The unique identifier of the primary IP address for which protection is to be modified.</param>
        /// <param name="delete">A boolean value indicating whether delete protection should be enabled or disabled. <see langword="true"/>
        /// enables delete protection; <see langword="false"/> disables it.</param>
        /// <returns>An <see cref="Action"/> object representing the result of the protection change operation.</returns>
        public async Task<Action> EnableProtection(long primaryIPId, bool delete)
        {
            var tmp = new
            {
                delete = delete
            };
            return (await Core.SendPostRequest<SimpleActionResponse>(_token, $"/primary_ips/{primaryIPId}/actions/change_protection", tmp)).Action;
        }







    }
}
