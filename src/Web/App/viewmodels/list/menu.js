define(function (require) {
    var app = require("durandal/app");
    return {
        addNew: function() {
            return app.showDialog("viewmodels/list/NewBoardDialog")
                .then(function(result) {
                    console.log(result);
                });
        }
    };
});