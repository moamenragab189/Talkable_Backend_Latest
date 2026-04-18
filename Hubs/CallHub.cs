using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Talkable.Data;

namespace Talkable.Hubs
{
    public class CallHub : Hub
    {
        private static List<Room> _rooms = new List<Room>();
        override public async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            await Clients.Caller.SendAsync("ReceiveConnectionId", connectionId);

        }
        public string CreateRoom(string userId)
        {
            if (userId == null)
            {
                return "User Id is null";
            }
            var room = new Room
            {
                FirstUserId = userId,
                Id = Guid.NewGuid().ToString().Substring(0,3),
                FirstUserConnectionId = Context.ConnectionId
            };

            _rooms.Add(room);
            return room.Id;
        }
        public async Task<string> JoinRoom(string userId, string roomId)
        {
            Console.WriteLine($"🟦 JoinRoom called: userId={userId}, roomId={roomId}");

            if (userId == null || roomId == null)
            {
                Console.WriteLine("❌ JoinRoom: userId or roomId is null");
                return "User Id or Room Id is null";
            }

            var room = _rooms.FirstOrDefault(r => r.Id == roomId);
            if (room == null)
            {
                Console.WriteLine($"❌ JoinRoom: Room {roomId} not found");
                Console.WriteLine($"   Available rooms: {string.Join(", ", _rooms.Select(r => r.Id))}");
                return "Room Id is incorrect";
            }

            if (room.SecondUserId != null)
            {
                Console.WriteLine($"❌ JoinRoom: Room {roomId} is full");
                return "Room is full";
            }

            room.SecondUserId = userId;
            room.SecondUserConnectionId = Context.ConnectionId;

            Console.WriteLine($"✅ JoinRoom: User {userId} joined room {roomId}");
            Console.WriteLine($"   FirstUserConnId: {room.FirstUserConnectionId}");
            Console.WriteLine($"   SecondUserConnId: {room.SecondUserConnectionId}");
            Console.WriteLine($"   Sending notification to: {room.FirstUserConnectionId}");

            await Clients.Client(room.FirstUserConnectionId).SendAsync("JoinRoomNotification", userId);

            Console.WriteLine($"✅ JoinRoomNotification sent!");
            return roomId;
        }
        public async Task<string> SendOffer(string offer)
        {
            if (offer == null)
            {
                return "Offer is null";
            }
            var connId = Context.ConnectionId;
            var room = _rooms.FirstOrDefault(r => r.FirstUserConnectionId == connId || r.SecondUserConnectionId == connId);
            if (room == null)
            {
                return "Room not found";
            }
            var targetedUserConnId = "";
            if (room.FirstUserConnectionId == connId)
            {
                targetedUserConnId = room.SecondUserConnectionId;
            }
            else
            {
                targetedUserConnId = room.FirstUserConnectionId;
            }
            if (targetedUserConnId == null)
            {
                return "Targeted user connection ID is null";
            }
            await Clients.Client(targetedUserConnId).SendAsync("ReceiveOffer", offer);
            return "Offer sent";
        }
        public async Task<string> SendAnswer(string answer)
        {
            if (answer == null)
            {
                return "Answer is null";
            }
            var connId = Context.ConnectionId;
            var room = _rooms.FirstOrDefault(r => r.FirstUserConnectionId == connId || r.SecondUserConnectionId == connId);
            if (room == null)
            {
                return "Room not found";
            }
            var targetedUserConnId = "";
            if (room.FirstUserConnectionId == connId)
            {
                targetedUserConnId = room.SecondUserConnectionId;
            }
            else
            {
                targetedUserConnId = room.FirstUserConnectionId;
            }
            if (targetedUserConnId == null)
            {
                return "Targeted user connection ID is null";
            }
            await Clients.Client(targetedUserConnId).SendAsync("ReceiveAnswer", answer);
            return "Answer sent";
        }
        public async Task<string> SendIceCandidate(string candidate)
        {
            if (candidate == null)
            {
                return "Candidate is null";
            }
            var connId = Context.ConnectionId;
            var room = _rooms.FirstOrDefault(r => r.FirstUserConnectionId == connId || r.SecondUserConnectionId == connId);
            if (room == null)
            {
                return "Room not found";
            }
            var targetedUserConnId = "";
            if (room.FirstUserConnectionId == connId)
            {
                targetedUserConnId = room.SecondUserConnectionId;
            }
            else
            {
                targetedUserConnId = room.FirstUserConnectionId;
            }
            if (targetedUserConnId == null)
                {
                return "Targeted user connection ID is null";
            }
            await Clients.Client(targetedUserConnId).SendAsync("ReceiveIceCandidate", candidate);
            return "Candidate sent";
        }
        public async Task<string> LeaveRoom()
        {
            var connId = Context.ConnectionId;
            var room = _rooms.FirstOrDefault(r => r.FirstUserConnectionId == connId || r.SecondUserConnectionId == connId);
            if (room == null)
            {
                return "Room not found";
            }
            if (room.FirstUserConnectionId == null || room.SecondUserConnectionId == null)
            {
                _rooms.Remove(room);

            }
            else
            {
                var targetedUserConnId = "";
                if (room.FirstUserConnectionId == connId)
                {
                    targetedUserConnId = room.SecondUserConnectionId;
                    room.FirstUserConnectionId = null;
                    room.FirstUserId = null;
                }
                else
                {
                    targetedUserConnId = room.FirstUserConnectionId;
                    room.SecondUserConnectionId = null;
                    room.SecondUserId = null;
                }

                await Clients.Client(targetedUserConnId).SendAsync("LeaveRoomNotification");
            }
            return "Left the room";

        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
          await LeaveRoom();
            
        }


    }
}
