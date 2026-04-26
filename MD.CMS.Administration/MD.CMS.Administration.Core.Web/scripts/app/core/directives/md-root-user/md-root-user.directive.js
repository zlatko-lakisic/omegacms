(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdRootUser', ['$rootScope', 'mdResourceService', mdResourceTextDirective]);
    /** @ngInject */
    function mdResourceTextDirective($rootScope, mdResourceService) {
        return {
            restrict: 'EA',
            template: 'Currently working as Root user!',
            link: function (scope, iElement) {
                if (mdBusinessLogic.globals.loggedOnUser !== undefined && mdBusinessLogic.globals.loggedOnUser != null && !mdBusinessLogic.globals.loggedOnUser.IsRoot) {
                    iElement.remove();
                }
            }
        };
    }
})();
