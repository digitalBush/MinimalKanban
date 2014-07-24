using System.Collections.Generic;
using Domain;
using Domain.Aggregates;
using Domain.Projections;

namespace Denormalizers
{
    public class BoardList:
        Denormalizer<List<BoardSummary>>,
        IHandle<BoardCreated>,
        IHandle<BoardArchived>,
        IBoardList
    {
        public void Handle(BoardCreated cmd)
        {   
            WithState(list =>{
                list.Add(new BoardSummary() {
                    Id = cmd.AggregateId,
                    Name = cmd.Name
                });
                return list;
            });
        }

        public void Handle(BoardArchived cmd)
        {
            WithState(list =>{
                list.RemoveAll(x => x.Id == cmd.AggregateId);
                return list;
            });
        }

        public IList<BoardSummary> GetList()
        {
            return Query(x => x);
        }
    }
}
