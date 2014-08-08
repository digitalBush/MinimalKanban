using System;
using Domain.Aggregates;
using Machine.Specifications;

namespace Specs.Domain
{
    [Subject(typeof(Card))]
    public class when_creating_a_card
    {
        static Card Card;
        static CardCreated Expected= new CardCreated() {
            BoardId = Guid.NewGuid(),
            Title = "Test",
            Description = "Some text",
            LaneId = 0
        };

        Because of = () => Card = new Card(
            title:Expected.Title, 
            description:Expected.Description, 
            boardId:Expected.BoardId, 
            laneId:Expected.LaneId);

        It should_raise_correct_event = () => Card.ShouldRaiseEvents(Expected);
    }

    
}