define(function (require) {
    var http = require("plugins/http");
    var app = require("durandal/app");

    var List = function() {
        this.boards = ko.observableArray();
        this.menu = "viewmodels/list/menu";
    }

    List.prototype.activate = function() {
        var self = this;

        return http.get("/api/boards")
            .then(function(boards) {
                self.boards(boards);
            })
            .then(function() {
                app.on("update:BoardList", function (boards) {
                    ko.object.update(self, { boards: boards });
                });
            });
    };

    return new List();

});