using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Aggregates;
using Domain.Projections;
using Card = Domain.Projections.Card;

namespace Denormalizers
{
    public class CardDetail :
        Denormalizer<Guid, Card>,
        IHandle<CardCreated>,
        ICardDetail
    {   

        public void Handle(CardCreated e)
        {
            WithState(e.AggregateId, card => new Card() {
                    Id = e.AggregateId,
                    Title = e.Title,
                    Description = e.Description
                });
        }


        public Card Get(Guid id)
        {
            return Query(id);
        }
    }
}