using AutoMapper;
using Domain.Aggregates;
using NEventStore.Conversion;

namespace EventConverters
{
    public class CardCreatedUp:
        IUpconvertEvents<CardCreated,CardCreatedv2>
    {
        static CardCreatedUp()
        {
            Mapper.CreateMap<CardCreated, CardCreatedv2>()
                .ForMember(x=>x.HtmlDescription,cfg=>cfg.MapFrom(x=>x.Description));
        }
        public CardCreatedv2 Convert(CardCreated sourceEvent)
        {
            return Mapper.Map<CardCreatedv2>(sourceEvent);
        }
    }
}
