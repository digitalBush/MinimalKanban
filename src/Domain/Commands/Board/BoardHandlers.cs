using System;
using System.Threading.Tasks;

namespace Domain.Commands.Board
{
    public class BoardHandlers :
        IHandleCommand<CreateBoard,Guid>,
        IHandleCommand<MoveCard>
    {
        readonly IRepository<Aggregates.Board> _repo;

        public BoardHandlers(IRepository<Aggregates.Board> repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(CreateBoard cmd)
        {
            var board = new Aggregates.Board(cmd.Name,cmd.Lanes);
            await _repo.Save(board);
            return board.Id;
        }

        public async Task Handle(MoveCard cmd)
        {
            var board = await _repo.Get(cmd.BoardId);
            board.MoveCard(cmd.CardId,cmd.LaneId,cmd.Position);
            await _repo.Save(board);
        }
    }
}