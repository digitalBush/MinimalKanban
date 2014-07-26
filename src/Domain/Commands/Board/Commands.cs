using System;
using System.Collections.Generic;

namespace Domain.Commands.Board
{
    public class CreateBoard : ICommand
    {
        public string Name { get; set; }
        public IList<string> Lanes { get; set; }
    }

    public class AddCard : ICommand
    {
        public Guid BoardId { get; set; }
        public Guid CardId { get; set; }
        public int LaneId { get; set; }
    }

    public class MoveCard : ICommand
    {
        public Guid BoardId { get; set; }
        public Guid CardId { get; set; }
        public int LaneId { get; set; }
        public int Position { get; set; }
    }
}