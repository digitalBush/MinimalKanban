using System;
using Domain.Aggregates;
using Machine.Specifications;

namespace Specs.Domain
{
    [Subject(typeof(Board))]
    public class when_creating_a_board
    {
        static Board Board;
        static BoardCreated Expected = new BoardCreated(){
            Name = "Test Board",
            LaneNames = new []{"Todo","Doing","Done"}
        };
        

        Because of = () => Board = new Board(
            name: Expected.Name,
            laneNames: Expected.LaneNames
        );

        It should_raise_correct_event = () => Board.ShouldRaiseEvents(Expected);
    }

    [Subject(typeof(Board))]
    public class when_creating_a_board_with_no_title
    {
        static Board Board;
        static ArgumentException Expected;

        Because of = () => Expected = Catch.Only<ArgumentException>( ()=> Board = new Board(
            name: "",
            laneNames: new[] { "Todo", "Doing", "Done" }
        ));

        It should_not_create_object = () => Board.ShouldBeNull();
        It should_throw_exception = () => Expected.ParamName.ShouldEqual("name");
    }

    [Subject(typeof(Board))]
    public class when_creating_a_board_with_no_lanes
    {
        static Board Board;
        static ArgumentException Expected;

        Because of = () => Expected = Catch.Only<ArgumentException>(() => Board = new Board(
            name: "LOL",
            laneNames: null
        ));

        It should_not_create_object = () => Board.ShouldBeNull();
        It should_throw_exception = () => Expected.ParamName.ShouldEqual("laneNames");
    }
    
}