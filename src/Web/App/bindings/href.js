(function () {
    var absolutePath = /^(.+?\:\/\/|mailto\:|\/)/i;

    ko.bindingHandlers.href = {
        update: function(element, valueAccessor, allBindings, viewModel, bindingContext) {
            var path = valueAccessor();

            if (!absolutePath.test(path)) {
                //TODO: Need to remove possible "//" when concatenating
                path = window.location.pathname + '/' + path;
            }

            var replaced = path.replace(/:([A-Za-z_]+)/g, function(_, token) {
                return ko.unwrap(viewModel[token]);
            });
            element.href = replaced;
        }
    };
})();