using System;
using System.Collections.Generic;

namespace Domain.Projections
{
    public interface IBoardDetail
    {
        Board Get(Guid id);
    }

    public class Board
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<Lane> Lanes { get; set; }
    }

    public class Lane   
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<CardSummary> Cards { get; set; }
    }

    public class CardSummary
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Excerpt { get; set; }
    }
}