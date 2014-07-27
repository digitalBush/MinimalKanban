using System;
using System.Threading.Tasks;
using System.Web.Http;
using Domain.Commands.Card;
using Domain.Projections;
using Infrastructure;
using Kanban.Helpers;

namespace Kanban.Controllers.api
{
    [RoutePrefix("api/card")]
    public class CardController : ApiController
    {
        readonly ICardDetail _card;

        public CardController(ICardDetail card)
        {
            _card = card;
        }

        [HttpGet, Route("{id:guid}")]
        public Card Get(Guid id)
        {
            return _card.Get(id);
        }

        [HttpPut, Route("")]
        public async Task<Guid> Create(CreateCard cmd)
        {
            return await Command.Process<Guid>(cmd);
        }
    }
}