define(function (require) {
    var app = require('durandal/app');
    var http = require('plugins/http');
    var Dialog = require("viewmodels/board/NewCardDialog");

    var Lane = function () {
        this.cards = ko.observableArray();
    };


    var Board = function () {
        this.lanes = ko.observableArray().extend({ ctor: Lane });
    };

    $.extend(Board.prototype, {
        activate: function (id) {
            var self = this;
            return http.get("/api/board/" + id)
                .then(function (response) {
                    return ko.object.update(self, response);
                })
                .then(function() {
                    app.on("update:BoardDetail:"+id, function (e) {
                        ko.object.update(self, e);
                    });
                });
        },
        addCard: function (lane) {
            app.showDialog(new Dialog(this.id, this.lanes(), lane))
                .then(function(result) {
                    console.log(result);
                });
        },
        moveCard: function (item, position, dest) {
            var self = this;
            http.put('/api/board/' + self.id + '/lane/' + dest.id + '/' + item.id, position);
        }
    });

    return Board;
});