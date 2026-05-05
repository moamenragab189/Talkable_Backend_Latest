using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Talkable.Data.Models;

namespace Talkable.Hubs
{
    public class CallHub : Hub
    {
        private static List<Room> _rooms = new List<Room>();

        private readonly IHttpClientFactory _httpClientFactory;
        public CallHub(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
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

        public async Task SendFeatures(float[] features)
        {
            if (features == null || features.Length != 225)
                return;

            var connId = Context.ConnectionId;

            // هات الروم من الكونكشن
            var room = _rooms.FirstOrDefault(r =>
                r.FirstUserConnectionId == connId || r.SecondUserConnectionId == connId);

            if (room == null)
                return;

            // حدّد الطرف التاني (اللي هنبعتله الترجمة)
            string? targetConnId =
                room.FirstUserConnectionId == connId ? room.SecondUserConnectionId : room.FirstUserConnectionId;

            if (string.IsNullOrWhiteSpace(targetConnId))
                return;

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromSeconds(3);

            var response = await httpClient.PostAsJsonAsync(
                "https://lair-budget-sureness.ngrok-free.dev/predict",
                new { features = features }
            );

            if (!response.IsSuccessStatusCode)
                return;

            var result = await response.Content.ReadFromJsonAsync<PredictionResponse>();
            if (result == null)
                return;

            // ✅ ابعت الترجمة للطرف الآخر (الشخص الطبيعي – meeting-2)
            await Clients.Client(targetConnId).SendAsync("ReceivePrediction", result.predicted_text, result.confidence);

            // (اختياري) ابعتها للمرسل كـ Debug
            // await Clients.Caller.SendAsync("ReceivePrediction", result.text, result.confidence);
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
