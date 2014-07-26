using System;
using System.Threading.Tasks;
using System.Web.Http;
using Domain.Commands.Card;
using Infrastructure;
using Kanban.Helpers;

namespace Kanban.Controllers.api
{
    [RoutePrefix("api/card")]
    public class CardController : ApiController
    {
        [HttpPut, Route("")]
        public async Task<Guid> Create(CreateCard cmd)
        {
            return await Command.Process<Guid>(cmd);
        }
    }
}