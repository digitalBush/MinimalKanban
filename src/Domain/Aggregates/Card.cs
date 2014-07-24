using System;

namespace Domain.Aggregates
{
    public class Card : AggregateRoot
    {
        void Apply(CardCreated e)
        {
            this.Id = e.AggregateId;
        }

        public Card(string title, string description, Guid boardId, int laneId)
        {
            RaiseEvent(new CardCreated() {
                Title=title,
                Description=description,
                BoardId=boardId,
                LaneId=laneId
            });
        }
    }

    public class CardCreated : Event
    {
        //TODO: Likely need metadata about Board and Lane to come with this event.
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid BoardId { get; set; }
        public int LaneId { get; set; }
    }

    public class CardArchived : Event
    {

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