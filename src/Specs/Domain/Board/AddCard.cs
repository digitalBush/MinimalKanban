using System;
using Domain;
using Domain.Aggregates;
using Domain.Projections;
using Machine.Specifications;
using Board = Domain.Aggregates.Board;

namespace Specs.Domain{

    [Subject(typeof(Board))]
    public abstract class with_single_lane_board
    {
        protected static Board Board;

        Establish context = () =>{
            Board = new Board(
                name: "Single Lane Board",
                laneNames: new[] {"Todo"}
                );

            Board.MarkChangesAsCommitted();
        };
    }

    public class when_adding_a_card : with_single_lane_board
    {
        static CardAdded Expected = new CardAdded() {
            CardId = Guid.NewGuid(),
            LaneId = 0,
            Position = 0
        };


        Because of = () => Board.AddCard(
            cardId: Expected.CardId,
            laneId: 0);

        It should_raise_correct_event = () => Board.ShouldRaiseEvents(Expected);
    }


    public class when_adding_a_card_to_lane_containing_cards : with_single_lane_board
    {
        static CardAdded Expected = new CardAdded()
        {
            CardId = Guid.NewGuid(),
            LaneId = 0,
            Position = 2
        };

        Establish context = () => {
            Board.AddCard(
                cardId: Guid.NewGuid(), 
                laneId: 0);

            Board.AddCard(
                cardId: Guid.NewGuid(),
                laneId: 0);

            Board.MarkChangesAsCommitted();
        };


        Because of = () => Board.AddCard(
            cardId: Expected.CardId,
            laneId: 0);

        It should_raise_correct_event = () => Board.ShouldRaiseEvents(Expected);
    }

    public class when_adding_a_card_to_an_inavlid_lane : with_single_lane_board
    {
        static DomainException Expected;


        Because of = () => Expected=Catch.Only<DomainException>( 
            ()=> Board.AddCard(
                cardId: Guid.NewGuid(),
                laneId: 1));

        It should_throw_domain_exception = () => Expected.ShouldNotBeNull();
    }
}