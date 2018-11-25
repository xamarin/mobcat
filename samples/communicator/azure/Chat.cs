using Microsoft.AspNetCore.SignalR;

public class Chat : Hub
{
    public void BroadcastMessage(string name, string message)
    {
        Clients.All.SendAsync("broadcastMessage", name, message);
    }

    public void Echo(string name, string message)
    {
        Clients.Client(Context.ConnectionId).SendAsync("echo", name, message + " (echo from server)");
    }
}