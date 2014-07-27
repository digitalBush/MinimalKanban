define(function (require) {
    var dialog = require('plugins/dialog');
    var http = require('plugins/http');

    var CardDetail = function (boardId, id) {
        this.boardId = boardId;
        this.id = id;
    };

    $.extend(CardDetail.prototype, {
        activate:function() {
            var self = this;
            return http.get('/api/card/' + self.id)
                .then(function(response) {
                    return ko.object.update(self, response);
                });
        },
        close:function () {
            dialog.close(this);
        },
        archive: function () {
            var self = this;
            return http.remove("/api/board/" + self.boardId + '/' + self.id)
                .then(function (result) {
                    return dialog.close(self, result);
                });
        }
    });
        
    return CardDetail;
});
