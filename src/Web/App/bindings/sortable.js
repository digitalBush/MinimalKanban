(function() {
    var defaults = {
        forcePlaceholderSize: true,
        placeholder: "placeholder",
        opacity: 0.8,
        revert: 300
    };

    ko.bindingHandlers.sortable = {
        init: function (element, valueAccessor, allBindingsAccessor, context) {
            var callback = valueAccessor();
            var options = allBindingsAccessor().sortableOptions || {};
            $.extend(options, {
                update: function (e, ui) {
                    if (e.target != ui.item.parent()[0])
                        return;
                    var source = ko.dataFor((ui.sender || ui.item.parent())[0]);
                    var item = ko.dataFor(ui.item[0]);
                    var dest = ko.dataFor(ui.item.parent()[0]);
                    var position = ui.item.index();

                    callback.apply(dest, [item, position, dest, source]);
                }
            });

            $(element)
                .sortable($.extend(options, defaults))
                .disableSelection();
        }
    };
})();