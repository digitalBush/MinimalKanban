define(function (require) {
    var app = require("durandal/app");
    var system = require("durandal/system");

    var connection = $.connection("/events");
    connection.start().done(function () {
        system.log("Connected to event aggregator");
    });

    connection.received(function (e) {
        system.log("Received Event", e.name, e.data);
        app.trigger("update:" + e.name, e.data);
    });

});