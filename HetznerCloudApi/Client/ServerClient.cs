﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HetznerCloudApi.Object.Server;
using HetznerCloudApi.Object.Server.Get;
using HetznerCloudApi.Object.Universal;
using HetznerCloudApi.Object.Network;
using HetznerCloudApi.Object.Action;

namespace HetznerCloudApi.Client
{
    public class ServerClient
    {
        private readonly string _token;

        public ServerClient(string token)
        {
            _token = token;
        }

        public async Task<List<Server>> Get()
        {
            List<Server> list = new List<Server>();
            long page = 0;
            while (true)
            {
                // Nex
                page++;

                // Get list
                Response response = JsonConvert.DeserializeObject<Response>(await Core.SendGetRequest(_token, $"/servers?page={page}&per_page={Core.PerPage}")) ?? new Response();

                // Run
                foreach (Server row in response.Servers)
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

        public async Task<Server> Get(long id)
        {
            // Return
            return await Core.SendGetRequest<Object.Action.Get.ResponseBucket<Server>>(_token, $"/servers/{id}");
        }

        /// <summary>
        /// Creates a new Server. Returns preliminary information about the Server as well as an Action that covers progress of creation.
        /// </summary>
        /// <param name="dataCenter">Data center where the resource will be created</param>
        /// <param name="imageId">ID or name of the Image the Server is created from</param>
        /// <param name="name">Name of the Server to create (must be unique per Project and a valid hostname as per RFC 1123)</param>
        /// <param name="serverType">ID or name of the Server type this Server should be created with</param>
        /// <param name="ipv4">Attach an IPv4 on the public NIC. If false, no IPv4 address will be attached. Defaults to true.</param>
        /// <param name="ipv6">Attach an IPv6 on the public NIC. If false, no IPv6 address will be attached. Defaults to true.</param>
        /// <param name="privateNetoworksIds">Network IDs which should be attached to the Server private network interface at the creation time</param>
        /// <param name="sshKeysIds">SSH key IDs which should be injected into the Server at creation time</param>
        /// <param name="volumesIds">Volume IDs which should be attached to the Server at the creation time. Volumes must be in the same Location.</param>
        /// <param name="placementGroupId">ID of the Placement Group the server should be in</param>
        /// <param name="userData">Cloud-Init user data to use during Server creation. This field is limited to 32KiB.</param>
        /// <returns></returns>
        public async Task<Object.Action.Get.ResponseBucket<Server>> Create(
            eDataCenter dataCenter,
            long imageId,
            string name,
            long serverType,
            bool ipv4 = true,
            bool ipv6 = true,
            List<long> privateNetoworksIds = default,
            List<long> sshKeysIds = default,
            List<long> volumesIds = default,
            long placementGroupId = 0,
            string userData = "")
        {

            // Location
            long datacenterId = 0;
            switch (dataCenter)
            {
                case eDataCenter.fsn1:
                    datacenterId = 4;
                    break;

                case eDataCenter.nbg1:
                    datacenterId = 2;
                    break;

                case eDataCenter.hel1:
                    datacenterId = 3;
                    break;

                case eDataCenter.ash:
                    datacenterId = 5;
                    break;

                case eDataCenter.hil:
                    datacenterId = 6;
                    break;
            }

            CreateServerRequest post = new CreateServerRequest
            {
                Datacenter = datacenterId,
                Image = imageId,
                Name = name,
                ServerType = serverType,
                PublicNet = new PublicNet
                {
                    EnableIpv4 = ipv4,
                    EnableIpv6 = ipv6
                },
                Networks = privateNetoworksIds,
                SshKeys = sshKeysIds,
                UserData = userData
            };

            if (volumesIds != null && volumesIds.Count > 0)
            {
                post.Volumes = volumesIds;
                post.Automount = true;
            }
            else
            {
                post.Volumes = null;
                post.Automount = null;
            }

            if (placementGroupId != 0)
            {
                post.PlacementGroup = placementGroupId;
            }
            else
            {
                post.PlacementGroup = null;
            }

            return await Create(post);
        }

        /// <summary>
        /// Creates a new Server. Returns preliminary information about the Server as well as an Action that covers progress of creation.
        /// </summary>
        /// <param name="datacenterId">ID of the Datacenter</param>
        /// <param name="imageId">ID or name of the Image the Server is created from</param>
        /// <param name="name">Name of the Server to create (must be unique per Project and a valid hostname as per RFC 1123)</param>
        /// <param name="serverType">ID or name of the Server type this Server should be created with</param>
        /// <param name="ipv4">Attach an IPv4 on the public NIC. If false, no IPv4 address will be attached. Defaults to true.</param>
        /// <param name="ipv6">Attach an IPv6 on the public NIC. If false, no IPv6 address will be attached. Defaults to true.</param>
        /// <param name="privateNetworkIds">Network IDs which should be attached to the Server private network interface at the creation time</param>
        /// <param name="sshKeysIds">SSH key IDs which should be injected into the Server at creation time</param>
        /// <param name="volumesIds">Volume IDs which should be attached to the Server at the creation time. Volumes must be in the same Location.</param>
        /// <param name="placementGroupId">ID of the Placement Group the server should be in</param>
        /// <param name="userData">Cloud-Init user data to use during Server creation. This field is limited to 32KiB.</param>
        /// <returns></returns>
        public async Task<Object.Action.Get.ResponseBucket<Server>> Create(
            long datacenterId,
            long imageId,
            string name,
            long serverType,
            bool ipv4 = true,
            bool ipv6 = true,
            List<long> privateNetworkIds = default,
            List<long> sshKeysIds = default,
            List<long> volumesIds = default,
            long placementGroupId = 0,
            string userData = "")
        {

            CreateServerRequest post = new CreateServerRequest
            {
                Datacenter = datacenterId,
                Image = imageId,
                Name = name,
                ServerType = serverType,
                PublicNet = new PublicNet
                {
                    EnableIpv4 = ipv4,
                    EnableIpv6 = ipv6,
                },
                Networks = privateNetworkIds,
                SshKeys = sshKeysIds,
                UserData = userData
            };

            if (volumesIds != null && volumesIds.Count > 0)
            {
                post.Volumes = volumesIds;
                post.Automount = true;
            }
            else
            {
                post.Volumes = null;
                post.Automount = null;
            }

            if (placementGroupId != 0)
            {
                post.PlacementGroup = placementGroupId;
            }
            else
            {
                post.PlacementGroup = null;
            }

            return await Create(post);
        }

        public async Task<Object.Action.Get.ResponseBucket<Server>> Create(CreateServerRequest post)
        {
            // Send post
            var bucket = await Core.SendPostRequest<Object.Action.Get.ResponseBucket<Server>>(_token, "/servers", post);

            bucket.Response.RootPassword = bucket.GetObject<string>("root_password");

            return bucket;
        }



        /// <summary>
        /// Updates a Server. You can update a Server’s name.
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public async Task<Object.Action.Get.ResponseBucket<Server>> Update(Server server)
        {
            return await Core.SendPutRequest<Object.Action.Get.ResponseBucket<Server>>(_token, $"/servers/{server.Id}", server);
        }

        /// <summary>
        /// Delete a Server
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Object.Action.Action> Delete(long id)
        {
            return await Core.SendDeleteRequest<SimpleActionResponse>(_token, $"/servers/{id}");
        }

        /// <summary>
        /// Delete a Server
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public async Task<Action> Delete(Server server)
        {
            return await Delete(server.Id);
        }

        /// <summary>
        /// Class used to create the server
        /// </summary>
        public class CreateServerRequest
        {
            [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
            public string Name { get; set; }

            [JsonProperty("ssh_keys", NullValueHandling = NullValueHandling.Ignore)]
            public List<long> SshKeys { get; set; }

            [JsonProperty("datacenter", NullValueHandling = NullValueHandling.Ignore)]
            public long Datacenter { get; set; }

            [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
            public long Image { get; set; }

            [JsonProperty("server_type", NullValueHandling = NullValueHandling.Ignore)]
            public long ServerType { get; set; }

            [JsonProperty("automount", NullValueHandling = NullValueHandling.Ignore)]
            public bool? Automount { get; set; }

            //[JsonProperty("firewalls", NullValueHandling = NullValueHandling.Ignore)]
            //public List<Firewall> Firewalls { get; set; }

            [JsonProperty("public_net", NullValueHandling = NullValueHandling.Ignore)]
            public PublicNet PublicNet { get; set; }

            [JsonProperty("networks", NullValueHandling = NullValueHandling.Ignore)]
            public List<long> Networks { get; set; }

            [JsonProperty("volumes", NullValueHandling = NullValueHandling.Ignore)]
            public List<long> Volumes { get; set; }

            [JsonProperty("user_data", NullValueHandling = NullValueHandling.Ignore)]
            public string UserData { get; set; }

            [JsonProperty("placement_group", NullValueHandling = NullValueHandling.Ignore)]
            public long? PlacementGroup { get; set; }
        }

        public class PublicNet
        {
            [JsonProperty("enable_ipv4", NullValueHandling = NullValueHandling.Ignore)]
            public bool EnableIpv4 { get; set; }

            [JsonProperty("enable_ipv6", NullValueHandling = NullValueHandling.Ignore)]
            public bool EnableIpv6 { get; set; }


            [JsonProperty("ipv4", NullValueHandling = NullValueHandling.Ignore)]
            public int? IPv4 { get; set; }

            [JsonProperty("ipv6", NullValueHandling = NullValueHandling.Ignore)]
            public int? IPv6 { get; set; }
        }
    }
}
