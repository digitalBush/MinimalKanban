(function () {
	function create(ctor, source) {
		return update(new ctor(), source);
	};

	function update(dest, source) {
		return _.merge(dest, source, function (a, b) {
			if (ko.isWriteableObservable(a)) {
				if (a.ctor && a.isObservableArray) {
					b = _.map(b, create.bind(this, a.ctor));
				} else if (a.ctor) {
					b = create(a.ctor, b);
				}
				a(b);
				return a;
			}
			return b;
		});
	};

	ko.observableArray.fn.isObservableArray = true;

	ko.extenders.ctor = function (target, ctor) {
		target.ctor = ctor;
		return target;
	};

	ko.object = {
		create: create,
		update: update
	};
})();