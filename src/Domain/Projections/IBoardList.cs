using System;
using System.Collections.Generic;

namespace Domain.Projections
{
    public interface IBoardList
    {
        IList<BoardSummary> GetList();
    }

    public class BoardSummary
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}