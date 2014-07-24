using System.Collections.Generic;
using System.Web.Http;
using Domain.Commands.Board;
using Domain.Projections;

namespace Kanban.Controllers.api
{
    public class BoardsController : ApiController
    {
        readonly IBoardList _boards;

        public BoardsController(IBoardList boards)
        {
            _boards = boards;
        }

        [HttpGet, Route("api/boards")]
        public IList<BoardSummary> Boards(CreateBoard cmd)
        {
            return _boards.GetList();
        }
       
    }
}