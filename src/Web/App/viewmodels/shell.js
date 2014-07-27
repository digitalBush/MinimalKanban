define(['plugins/router', 'durandal/app','services/events'], function (router, app, events) {
    return {
        router: router,
        childMenu: ko.computed(function(){
            var child = router.activeItem();
            return child.menu;
        },this,{deferEvaluation:true}),
        activate: function () {
            router.map([
                { route: '', title: 'Boards', moduleId: 'viewmodels/list' },
                { route: 'board/:id(/:cardId)', moduleId: 'viewmodels/board'}
            ]).buildNavigationModel();
            
            return router.activate({ pushState: true});
        }
    };
});