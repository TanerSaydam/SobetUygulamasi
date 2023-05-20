using Microsoft.AspNetCore.SignalR;
using Server.Context;

namespace Server.Hubs;

public sealed class ChatHub : Hub
{
    public async Task JoinGroup(int chatId)
    {
        if(chatId > 0)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }
    }

    public async Task LeaveGroup(int chatId)
    {
        if(chatId > 0)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
        }
    }
}
