(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdPermissions', ['$q', 'mdFeedbackService', 'mdPermissionEntitiesService', mdPermissions]);
    /** @ngInject */
    function mdPermissions($q, $mdFeedbackService, mdPermissionEntitiesService) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-permissions/md-permissions.template.html',
            scope: {
                group: '='
            },
            controller: 'mdPermissionsController as vm'
        };
    }
})();
