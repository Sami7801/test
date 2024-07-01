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
        .SendAsync(method:"ReceiveMessage", "Lets Program Bot", $"{userConnection.User} has Joined the Group");
    }
}