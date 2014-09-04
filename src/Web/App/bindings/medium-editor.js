(function() {

    ko.bindingHandlers.editor = {
        init: function (element, valueAccessor, allBindingsAccessor, context) {
            var value = valueAccessor();
            var $elm =
                $(element)
                .html(ko.unwrap(value))
                .on('input', function () {
                    value($elm.html());
                });

            var editor = new MediumEditor(element, {
                placeholder:"",
                buttons: ['bold', 'italic', 'underline', 'anchor', 'header1', 'header2', 'quote', 'strikethrough','unorderedlist','orderedlist','pre','image','indent','outdent']
            });

            ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                editor.deactivate();
            });
        }
    };
})();