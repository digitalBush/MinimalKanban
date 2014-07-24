using System;
using System.Threading.Tasks;
using System.Web.Http;
using Domain.Commands.Board;
using Domain.Projections;
using Infrastructure;

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
    }
}