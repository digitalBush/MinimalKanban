using Domain.Aggregates;
using Domain.Commands.Board;

namespace Domain.EventHandlers
{
    public class NewCard:IHandle<CardCreated>
    {
        readonly ICommandProcessor _processor;

        public NewCard(ICommandProcessor processor)
        {
            _processor = processor;
        }

        public void Handle(CardCreated e)
        {
            _processor.Process(new AddCard() {
                BoardId = e.BoardId,
                CardId = e.AggregateId,
                LaneId = e.LaneId
            });
        }
    }
}