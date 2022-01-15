using System;
using System.Collections.Generic;
using System.Linq;
using TestWebApi.Service.Enums;
using TestWebApi.Service.Models;

namespace TestWebApi.Service.Services
{
    public class GameService
    {
        private static readonly object Locker = new object();
        
        #region Game methods

        public int GetGamesCount()
        {
            return GameList.Items.Count;
        }

        public IEnumerable<Game> GetGames(int pageNumber = 0, int pageSize = 0)
        {
            return pageNumber > 0 && pageSize > 0
                ? GameList.Items.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                : GameList.Items;
        }

        public Game GetGame(Guid id)
        {
            return GameList.Items.SingleOrDefault(game => game.Id == id);
        }

        #endregion

        #region Collection methods

        public int GetCollectionsCount()
        {
            return CollectionList.Items.Count;
        }

        public IEnumerable<Collection> GetCollections(int pageNumber = 0, int pageSize = 0)
        {
            return pageNumber > 0 && pageSize > 0
                ? CollectionList.Items.Skip((pageNumber - 1) * pageSize).Take(pageSize)
                : CollectionList.Items;
        }

        public Collection GetCollection(Guid id)
        {
            return CollectionList.Items.SingleOrDefault(item => item.Id == id);
        }

        #endregion

        #region GameSession methods

        public GameSessionResponse StartGameSession(GameSessionRequest request)
        {
            var customer = CustomerList.Items.SingleOrDefault(item => item.Id == request.CustomerId);
            if (customer == null) return GameSessionResponse.CustomerUnknown();

            var game = GameList.Items.SingleOrDefault(item => item.Id == request.GameId);
            if (game == null) return GameSessionResponse.GameUnknown();

            var instance = GameSessionList.Items.SingleOrDefault(session =>
                session.CustomerId == request.CustomerId && session.GameId == request.GameId);
            if (instance != null) return GameSessionResponse.GameSessionActiveAlready();

            var url = GetGameUrl(request.CustomerId, request.GameId);
            instance = GameSession.StartGameSession(request.CustomerId, request.GameId);

            lock (Locker)
            {
                GameSessionList.Items.Add(instance);
                GameSessionList.Update();
                return GameSessionResponse.GameSessionCreated(instance.Id, url);
            }
        }

        public GameSessionStatus StopGameSession(Guid sessionId)
        {
            lock (Locker)
            {
                var instance = GameSessionList.Items.SingleOrDefault(session => session.Id == sessionId);
                if (instance == null) return GameSessionStatus.NotFound;
                if (instance.Stopped != null) return GameSessionStatus.IsNotActiveAlready;

                instance.StopGameSession();
                GameSessionList.Update();
                return GameSessionStatus.Stopped;
            }
        }

        private static string GetGameUrl(string customerId, Guid gameId)
        {
            return $"https://gameserver.fake.com?gameId={gameId}&customer={customerId}";
        }

        #endregion
    }
}