using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Domain.Commands.Board;
using Domain.Projections;
using Infrastructure;
using Kanban.Helpers;

namespace Kanban.Controllers.api
{
    [RoutePrefix("api/board")]
    public class BoardController : ApiController
    {
        readonly IBoardDetail _detail;

        public BoardController(IBoardDetail detail)
        {
            _detail = detail;
        }

        [HttpPut,Route("")]
        public async Task<Guid> Create(CreateBoard cmd)
        {
            return await Command.Process<Guid>(cmd);
        }

        [HttpGet, Route("{id:guid}")]
        public Board Get(Guid id)
        {
            return _detail.Get(id);
        }

        [HttpPut, Route("{boardId:guid}/lane/{laneId}/{cardId:guid}")]
        public async Task<StatusCodeResult> Get(Guid boardId, int laneId, Guid cardId, [FromBody]int position)
        {
            await Command.Process(new MoveCard() {
                BoardId = boardId,
                LaneId = laneId,
                CardId = cardId,
                Position = position
            });

            return StatusCode(HttpStatusCode.Accepted);
        }
    }
}