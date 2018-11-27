using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    public Task SendPrivateMessage(string user, string message)
    {
        var test = Context.UserIdentifier;
        return Clients.User(user).SendAsync("ReceiveMessage", message);
    }
    public void BroadcastMessage(string name, string message)
    {
        Clients.All.SendAsync("broadcastMessage", name, message);
    }
   public void SendReadReceipt(ReadReceipt readReceipt)
    {
        Clients.All.SendAsync("MessageReceived", readReceipt);
    }

    public async Task AddToGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

        await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
    }

    public Task ImageMessage(ImageMessage file)
    {
        return Clients.All.SendAsync("ImageMessage", file);
    }

    public async Task RemoveFromGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

        await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
    }
    public void Echo(string name, string message)
    {
        Clients.Client(Context.ConnectionId).SendAsync("echo", name, message + " (echo from server)");
    }
}