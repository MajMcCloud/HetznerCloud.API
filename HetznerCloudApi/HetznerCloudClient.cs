using HetznerCloudApi.Client;
using HetznerCloudApi.Object.Server;

namespace HetznerCloudApi
{
    public class HetznerCloudClient
    {
        public string Token { get; private set; }

        public HetznerCloudClient(string token)
        {
            Token = token;

            Action = new ActionClient(token);
            Datacenter = new DatacenterClient(token);
            Firewall = new FirewallClient(token);
            FirewallAction = new FirewallActionClient(token);
            Image = new ImageClient(token);
            ISO = new ISOClient(token);
            Location = new LocationClient(token);
            Network = new NetworkClient(token);
            NetworkAction = new NetworkActionClient(token);
            Network = new NetworkClient(token);
            PrimaryIP = new PrimaryIPClient(token);
            PrimaryIPAction = new PrimaryIPActionClient(token);
            Server = new ServerClient(token);
            ServerAction = new ServerActionClient(token);
            ServerType = new ServerTypeClient(token);
            SshKey = new SshKeyClient(token);
            Volume = new VolumeClient(token);
            VolumeAction = new VolumeActionClient(token);
        }

        public ActionClient Action { get; private set; }
        public DatacenterClient Datacenter { get; private set; }
        public FirewallClient Firewall { get; private set; }
        public FirewallActionClient FirewallAction { get; private set; }
        public ImageClient Image { get; private set; }
        public ISOClient ISO { get; private set; }
        public LocationClient Location { get; private set; }
        public NetworkClient Network { get; private set; }
        public NetworkActionClient NetworkAction { get; private set; }
        public PrimaryIPClient PrimaryIP { get; private set; }
        public PrimaryIPActionClient PrimaryIPAction { get; private set; }
        public ServerClient Server { get; private set; }
        public ServerActionClient ServerAction { get; private set; }
        public ServerTypeClient ServerType { get; private set; }
        public SshKeyClient SshKey { get; private set; }
        public VolumeClient Volume { get; private set; }
        public VolumeActionClient VolumeAction { get; private set; }
    }
}
