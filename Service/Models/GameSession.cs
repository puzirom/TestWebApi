using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using TestWebApi.Service.Enums;
using TestWebApi.Service.Helper;

namespace TestWebApi.Service.Models
{
    [DataContract(Namespace = "http://serialize")]
    public class GameSession
    {
        [DataMember(EmitDefaultValue = false)] public Guid Id { get; private set; }
        [DataMember(EmitDefaultValue = false)] public string CustomerId { get; private set; }
        [DataMember(EmitDefaultValue = false)] public Guid GameId { get; private set; }
        [DataMember(EmitDefaultValue = false)] public DateTime Started { get; private set; }
        [DataMember(EmitDefaultValue = false)] public DateTime? Stopped { get; private set; }

        public static GameSession StartGameSession(string customerId, Guid gameId)
        {
            return new GameSession
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                GameId = gameId,
                Started = DateTime.UtcNow
            };
        }

        public void StopGameSession()
        {
            Stopped = DateTime.UtcNow;
        }
    }

    public static class GameSessionList
    {
        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GameSessions.xml");
        public static readonly List<GameSession> Items;

        static GameSessionList()
        {
            Items = DataStorageHelper.Deserialize<List<GameSession>>(FilePath);
        }

        public static void Update()
        {
            var xml = DataStorageHelper.Serialize(Items.GetType(), Items);
            xml.Save(FilePath);
        }
    }

    public class GameSessionRequest
    {
        public string CustomerId { get; set; }
        public Guid GameId { get; set; }
    }

    public class GameSessionResponse
    {
        public string GameUrl { get; private set; }
        public Guid? SessionId { get; private set; }
        public GameSessionStatus Status { get; private set; }

        public static GameSessionResponse GameSessionCreated(Guid sessionId, string gameUrl)
        {
            return new GameSessionResponse
            {
                GameUrl = gameUrl,
                SessionId = sessionId,
                Status = GameSessionStatus.Started
            };
        }

        public static GameSessionResponse GameSessionActiveAlready()
        {
            return new GameSessionResponse {Status = GameSessionStatus.IsActiveAlready};
        }

        public static GameSessionResponse CustomerUnknown()
        {
            return new GameSessionResponse { Status = GameSessionStatus.CustomerUnknown };
        }

        public static GameSessionResponse GameUnknown()
        {
            return new GameSessionResponse { Status = GameSessionStatus.GameUnknown };
        }
    }
}