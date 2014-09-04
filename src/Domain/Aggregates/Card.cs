using System;

namespace Domain.Aggregates
{
    public class Card : AggregateRoot
    {
        void Apply(CardCreatedv2 e)
        {
            this.Id = e.AggregateId;
        }

        public Card(string title, string description, Guid boardId, int laneId)
        {
            RaiseEvent(new CardCreatedv2() {
                Title=title,
                HtmlDescription=description,
                BoardId=boardId,
                LaneId=laneId
            });
        }
    }

    public class CardCreated : Event
    {   
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid BoardId { get; set; }
        public int LaneId { get; set; }
    }

    public class CardCreatedv2 : Event
    {   
        public string Title { get; set; }
        public string HtmlDescription { get; set; }
        public Guid BoardId { get; set; }
        public int LaneId { get; set; }
    }

    public class UserAssigned : Event
    {

    }

    public class UserUnAssigned : Event
    {

    }

    public class ContentChanged : Event
    {

    }

    public class TitleChanged : Event
    {

    }

    public class CardCompleted : Event
    {

    }
}