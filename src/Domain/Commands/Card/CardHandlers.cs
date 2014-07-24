using System;
using System.Threading.Tasks;
using Domain.Commands.Board;

namespace Domain.Commands.Card
{
    public class CardHandlers :
        IHandleCommand<CreateCard,Guid>
        
    {
        readonly IRepository<Aggregates.Card> _repo;

        public CardHandlers(IRepository<Aggregates.Card> repo)
        {
            _repo = repo;
        }


        public async Task<Guid> Handle(CreateCard cmd)
        {
            //TODO: verify lane is valid for a given board

            var card = new Aggregates.Card(cmd.Title, cmd.Description, cmd.BoardId, cmd.LaneId);
            await _repo.Save(card);
            return card.Id;
        }
    }
}