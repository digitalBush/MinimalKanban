define(function (require) {
    var dialog = require('plugins/dialog');
    var http = require('plugins/http');

    var CardDetail = function (id) {
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
        }
    });
        
    return CardDetail;
});
