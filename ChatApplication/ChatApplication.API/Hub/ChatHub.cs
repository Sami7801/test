using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.API.Hub;

public class ChatHub : Microsoft.AspNetCore.SignalR.Hub {

    private readonly IDictionary<string, UserRoomConnection> _connection;

    public ChatHub(IDictionary<string, UserRoomConnection> connection)
    {
        _connection = connection;
    }

    public async Task JoinRoom(UserRoomConnection userConnection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room!);
        _connection[Context.ConnectionId] = userConnection;
        await Clients.Group(userConnection.Room!)
        .SendAsync(method:"ReceiveMessage", arg1: "Lets Program Bot", arg2: $"{userConnection.User} has Joined the Group");
    }

    public async Task SendMessage(string message) {
        if(_connection.TryGetValue(Context.ConnectionId, out UserRoomConnection userRoomConnection)){
            await Clients.Group(userRoomConnection.Room!)
            .SendAsync(method: "ReceiveMessage", arg1: userRoomConnection.User, arg2: message, arg3: DateTime.Now);
        }
    }

    public Task SendConnectedUser(string room)
    {
        var users = _connection.Values
        .Where(u => u.Room == room)
        .Select(s => s.User);
        return Clients.Group(room).SendAsync(method:"ConnectedUser", users); 
    }
}