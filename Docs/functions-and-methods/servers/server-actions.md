# Server Actions
	
With Server Actions, you can perform various actions on your servers, such as starting, stopping, rebooting, attaching networks, and much more. The methods are provided via the `ServerActionClient` class.

---

## Initialization

```csharp
using HetznerCloudApi.Client;

HetznerCloudClient hetznerCloudClient = new HetznerCloudClient("apiKey");
```

---

## Start server

```csharp
long serverId = 123456;	
var action = await hetznerCloudClient.ServerActions.PowerOn(serverId);
```

## Stop server (Power Off)

```csharp
long serverId = 123456;
var action = await hetznerCloudClient.ServerActions.PowerOff(serverId);
```

## Soft reboot server		

```csharp
long serverId = 123456;
var action = await hetznerCloudClient.ServerActions.SoftReboot(serverId);
```

## Hard reboot server (Reset)

```csharp
long serverId = 123456;
var action = await hetznerCloudClient.ServerActions.Reset(serverId);
```

## Shutdown server

```csharp
long serverId = 123456;
var action = await hetznerCloudClient.ServerActions.Shutdown(serverId);
```

## Change server type

```csharp
long serverId = 123456;
bool upgradeDisk = true;
string serverType = "cx21";
var action = await hetznerCloudClient.ServerActions.ChangeType(serverId, upgradeDisk, serverType);
```

## Attach server to network

```csharp
long serverId = 123456;
long networkId = 654321;
var action = await hetznerCloudClient.ServerActions.AttachToNetwork(serverId, networkId);
```

## Detach server from network

```csharp
long serverId = 123456;
long networkId = 654321;
var action = await hetznerCloudClient.ServerActions.DetachFromNetwork(serverId, networkId);
```

## Enable backup

```csharp
long serverId = 123456;
var action = await hetznerCloudClient.ServerActions.EnableBackup(serverId);
```

## Disable backup

```csharp
long serverId = 123456;
var action = await hetznerCloudClient.ServerActions.DisableBackup(serverId);
```

## Enable rescue mode

```csharp
long serverId = 123456;
string type = "linux64";
var action = await hetznerCloudClient.ServerActions.EnableRescueMode(serverId, type);
```

## Disable rescue mode

```csharp
long serverId = 123456;
var action = await hetznerCloudClient.ServerActions.DisableRescueMode(serverId);
```

## Reset root password

```csharp
long serverId = 123456;
var response = await hetznerCloudClient.ServerActions.ResetRootPassword(serverId);
string newPassword = response.RootPassword;
```

## Request console (VNC)

```csharp
long serverId = 123456;
var response = await hetznerCloudClient.ServerActions.RequestConsole(serverId);
string wssUrl = response.Wss_Url;
string password = response.Password;
```

## Attach ISO

```csharp
long serverId = 123456;
string isoName = "ubuntu-22.04";
var action = await hetznerCloudClient.ServerActions.AttachISO(serverId, isoName);
```

## Detach ISO

```csharp
long serverId = 123456;
var action = await hetznerCloudClient.ServerActions.DetachISO(serverId);
```

## Create snapshot/image

```csharp
long serverId = 123456;
string description = "Backup before update";
string type = "snapshot";
var labels = new Dictionary<string, string> { { "env", "prod" } };
var action = await hetznerCloudClient.ServerActions.CreateImage(serverId, description, type, labels);
```

## Rebuild server from image

```csharp
long serverId = 123456;
string image = "ubuntu-22.04";
var response = await hetznerCloudClient.ServerActions.RebuildFromImage(serverId, image);
string newRootPassword = response.RootPassword;
```

---

You can find more methods and details in the API documentation and in the source code of the `ServerActionClient` class.

