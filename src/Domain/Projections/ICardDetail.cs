using System;

namespace Domain.Projections
{
    public interface ICardDetail
    {
        Card Get(Guid id);
    }

    public class Card
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}