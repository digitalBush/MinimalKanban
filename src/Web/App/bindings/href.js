ko.bindingHandlers.href = {
    update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
        var path = valueAccessor();
        var replaced = path.replace(/:([A-Za-z_]+)/g, function (_, token) {
            return ko.unwrap(viewModel[token]);
        });
        element.href = replaced;
    }
};