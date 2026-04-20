using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Talkable.Data.Models;

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

            if (userId == null || roomId == null)
            {
                            return "User Id or Room Id is null";
            }

            var room = _rooms.FirstOrDefault(r => r.Id == roomId);
            if (room == null)
            {
                
                return "Room Id is incorrect";
            }

            if (room.SecondUserId != null)
            {
                return "Room is full";
            }

            room.SecondUserId = userId;
            room.SecondUserConnectionId = Context.ConnectionId;

            

            await Clients.Client(room.FirstUserConnectionId).SendAsync("JoinRoomNotification", userId);

           
            return roomId;
        }

        public async Task SendFeatures(string roomId, float[] features)
        {
            var room = _rooms.FirstOrDefault(r => r.Id == roomId);

            if (room == null)
                throw new Exception("Room not found");

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync(
                    "http://127.0.0.1:8000/predict",
                    new { features = features }
                );

                if (!response.IsSuccessStatusCode)
                    throw new Exception("AI API failed");

                var result = await response.Content
                    .ReadFromJsonAsync<PredictionResponse>();

                var connId = Context.ConnectionId;

                string? targetConnId = null;

                if (room.FirstUserConnectionId == connId)
                    targetConnId = room.SecondUserConnectionId;
                else
                    targetConnId = room.FirstUserConnectionId;

                if (string.IsNullOrEmpty(targetConnId))
                    return;

                // 🔥 send to other user
                await Clients.Client(targetConnId)
                    .SendAsync("ReceivePrediction", result.text, result.confidence);

                // ✅ optional (debug)
                await Clients.Caller
                    .SendAsync("ReceivePrediction", result.text, result.confidence);
            }
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
