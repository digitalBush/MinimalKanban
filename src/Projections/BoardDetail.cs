using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Domain;
using Domain.Aggregates;
using Domain.Projections;
using Board = Domain.Projections.Board;

namespace Denormalizers
{
    public class BoardDetail :
        Denormalizer<Guid, Board>,
        IHandle<BoardCreated>,
        IHandle<CardCreatedv2>,
        IHandle<CardMoved>,
        IHandle<CardArchived>,
    IBoardDetail
    {
        public void Handle(BoardCreated e)
        {
            WithState(e.AggregateId, x =>
                new Board() {
                    Id = e.AggregateId,
                    Name = e.Name,
                    Lanes = e.LaneNames.Select((lane, index) =>
                        new Lane() {
                            Id = index,
                            Name = lane,
                            Cards = new List<CardSummary>()
                        }).ToList()
                });
        }

        public void Handle(CardCreatedv2 e)
        {
            WithState(e.BoardId, board => {
                board.Lanes[e.LaneId].Cards.Add(new CardSummary() {
                    Id = e.AggregateId,
                    Title = e.Title,
                    Excerpt = ExcerptOf(e.HtmlDescription)
                });
                return board;
            });
        }

        public void Handle(CardMoved e)
        {
            WithState(e.AggregateId, board =>{
                var summary = board.Lanes[e.FromLaneId].Cards.SingleOrDefault(x => x.Id == e.CardId);
                board.Lanes[e.FromLaneId].Cards.Remove(summary);
                board.Lanes[e.LaneId].Cards.Insert(e.Position,summary);
                return board;
            });
        }

        public void Handle(CardArchived e)
        {
            WithState(e.AggregateId, board =>{
                foreach (var lane in board.Lanes)
                {
                    lane.Cards.RemoveAll(x => x.Id == e.CardId);
                }
                return board;
            });
        }


        public Board Get(Guid id)
        {
            return Query(id);
        }

        string ExcerptOf(string description)
        {
            if (description == null)
                return "";
            var parsed = new HtmlAgilityPack.HtmlDocument();
            parsed.LoadHtml(description);
            description = parsed.DocumentNode.InnerText;

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