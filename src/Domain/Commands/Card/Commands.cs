using System;

namespace Domain.Commands.Card
{
    public class CreateCard : ICommand
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid BoardId { get; set; }
        public int LaneId { get; set; }
    }
}