using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Aggregates
{
    public class Board : AggregateRoot
    {
        List<int> _lanes = new List<int>();
        
        void Apply(BoardCreated e)
        {
            this.Id = e.AggregateId;
            _lanes.AddRange(Enumerable.Range(0, e.LaneNames.Count));
        }

        void Apply(BoardArchived e)
        {
        }

        void Apply(CardMoved e)
        {
        }

        public Board(string name, IList<string> laneNames )
        {   
            RaiseEvent(new BoardCreated() {
                Name = name,
                LaneNames = laneNames
            });
        }

        public void MoveCard(Guid cardId, int laneId, int position)
        {
            AssertLane(laneId);

            RaiseEvent(new CardMoved()
            {
                CardId = cardId,
                LaneId = laneId,
                Position = position
            });
        }

        void AssertLane(int laneId)
        {
            if (!_lanes.Contains(laneId))
                throw new DomainException("Lane {0} is not on this board", laneId);
        }
    }

    public class BoardCreated : Event
    {
        public string Name { get; set; }
        public IList<string> LaneNames { get; set; }
    }

    public class BoardArchived : Event
    {
        
    }

    public class CardMoved : Event
    {
        public Guid CardId { get; set; }
        public int LaneId { get; set; }
        public int Position { get; set; }
    }
}