using System;
using System.Web.Http;
using TestWebApi.Service.Models;
using TestWebApi.Service.Services;

namespace TestWebApi.Service.Controllers
{
    public class GameController : ApiController
    {
        private readonly GameService _gameService = new GameService();

        #region Game methods

        /// <summary>
        /// Get count of available games
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        public IHttpActionResult GetGamesCount()
        {
            return Ok(_gameService.GetGamesCount());
        }

        /// <summary>
        /// Get list of available games
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        [HttpGet]
        public IHttpActionResult GetGames(int pageNumber = 0, int pageSize = 0)
        {
            return Ok(_gameService.GetGames(pageNumber, pageSize));
        }

        /// <summary>
        /// Get specific game
        /// </summary>
        /// <param name="id">Game identifier</param>
        /// <returns>IHttpActionResult</returns>
        [HttpGet]
        public IHttpActionResult GetGame(Guid id)
        {
            var game = _gameService.GetGame(id);
            if (game == null) return NotFound();
            return Ok(game);
        }

        #endregion

        #region Collections methods

        /// <summary>
        /// Get count of available collections
        /// </summary>
        /// <returns>IHttpActionResult</returns>
        public IHttpActionResult GetCollectionsCount()
        {
            return Ok(_gameService.GetCollectionsCount());
        }

        /// <summary>
        /// Get list of available collections
        /// </summary>
        /// <param name="pageNumber">page number - not required</param>
        /// <param name="pageSize">page size - not required</param>
        /// <returns>IHttpActionResult</returns>
        [HttpGet]
        public IHttpActionResult GetCollections(int pageNumber = 0, int pageSize = 0)
        {
            return Ok(_gameService.GetCollections(pageNumber, pageSize));
        }

        /// <summary>
        /// Get specific collection
        /// </summary>
        /// <param name="id">Collection identifier</param>
        /// <returns>IHttpActionResult</returns>
        [HttpGet]
        public IHttpActionResult GetCollection(Guid id)
        {
            var collection = _gameService.GetCollection(id);
            if (collection == null) return NotFound();
            return Ok(collection);
        }

        #endregion

        #region Game Sessions methods

        /// <summary>
        /// Starts a new game session
        /// </summary>
        /// <param name="request">instance of GameSessionRequest</param>
        /// <returns>IHttpActionResult</returns>
        [HttpPost]
        [Authorize]
        public IHttpActionResult StartGameSession([FromBody]GameSessionRequest request)
        {
            var response = _gameService.StartGameSession(request);
            return Ok(response);
        }

        /// <summary>
        /// Stops existing and active game session
        /// </summary>
        /// <param name="sessionId">Session identifier</param>
        /// <returns>IHttpActionResult</returns>
        [HttpGet]
        [Authorize]
        public IHttpActionResult StopGameSession(Guid sessionId)
        {
            var response = _gameService.StopGameSession(sessionId);
            return Ok(response);
        }

        #endregion
    }
}
