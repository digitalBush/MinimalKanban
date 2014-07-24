using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Aggregates;
using Domain.Projections;
using Board = Domain.Projections.Board;

namespace Denormalizers
{
    public class BoardDetail :
        Denormalizer<Guid, Board>,
        IHandle<BoardCreated>,
        IHandle<CardCreated>,
        IBoardDetail
    {
        public void Handle(BoardCreated cmd)
        {
            WithState(cmd.AggregateId, x =>
                new Board() {
                    Id = cmd.AggregateId,
                    Name = cmd.Name,
                    Lanes = cmd.LaneNames.Select((lane, index) =>
                        new Lane() {
                            Id = index,
                            Name = lane,
                            Cards = new List<CardSummary>()
                        }).ToList()
                });
        }

        public void Handle(CardCreated cmd)
        {
            WithState(cmd.BoardId, board => {
                board.Lanes[cmd.LaneId].Cards.Add(new CardSummary() {
                    Id = cmd.AggregateId,
                    Title = cmd.Title,
                    Excerpt = ExcerptOf(cmd.Description)
                });
                return board;
            });
        }


        public Board Get(Guid id)
        {
            return Query(id);
        }

        string ExcerptOf(string description)
        {
            if (description.Length < 140)
                return description;

            var chars = description
                .Take(140)
                .Reverse()
                .SkipWhile(c=>!Char.IsWhiteSpace(c))
                .SkipWhile(Char.IsWhiteSpace)
                .Reverse()
                .Concat("...");

            return String.Concat(chars);
        }

        
    }
}