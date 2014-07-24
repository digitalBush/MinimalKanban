define(function (require) {
    var dialog = require('plugins/dialog');
    var http = require('plugins/http');

    var NewBoard = function () {
        this.name = ko.observable();
    };

    NewBoard.prototype.close = function () {
        dialog.close(this);
    };

    NewBoard.prototype.save = function () {
        var self = this;
        return http.put("/api/board", {name:self.name, lanes:["To Do","Doing","Done"]})
            .then(function(result) {
                return dialog.close(self, result);
            });
    };

    return NewBoard;
});