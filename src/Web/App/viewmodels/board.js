define(function (require) {
    var app = require('durandal/app');
    var http = require('plugins/http');
    var router = require('plugins/router');

    var Lane = function () {
        this.cards = ko.observableArray();
    }

    var Board = function () {
        this.lanes = ko.observableArray().extend({ ctor: Lane });
    };

    $.extend(Board.prototype, {
        canReuseForRoute: function (id) {
            return this.id == id;
        },
        activate: function (id,cardId) {
            var self = this;

            if (cardId) {
                var Dialog = require("viewmodels/card");
               
                app.showDialog(new Dialog(id,cardId))
                    .then(function() {
                        router.navigate('/board/' + id);
                    });
            }

            //TODO: better manage api calls when reusing instance
            return http.get("/api/board/" + id)
                .then(function(response) {
                    return ko.object.update(self, response);
                })
                .then(function() {
                    app.on("update:BoardDetail:" + id, function(e) {
                        ko.object.update(self, e);
                    });
                });
            
        },
        detached: function () {
            app.off("update:BoardDetail:" + this.id);
        },
        addCard: function (lane) {
            var Dialog = require("viewmodels/board/NewCardDialog");
            app.showDialog(new Dialog(this.id, this.lanes(), lane))
                .then(function (result) {
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