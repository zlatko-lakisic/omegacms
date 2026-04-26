(function () {
    'use strict';

    angular
        .module('app.settings.configuration.permissions-form')
        .controller('PermissionsFormController', ['$state', '$mdDialog', '$q', 'entityGroupId', 'mdPermissionEntitiesService', PermissionsFormController]);

    /** @ngInject */
    function PermissionsFormController($state, $mdDialog, $q, entityGroupId, mdPermissionEntitiesService) {
        var vm = this;

        //Private Attributes
        var permissionSaveEvents = [];


        //Public Attributes
        vm.entityGroup = null;


        //Public Methods
        vm.registerPermissionSaveEvents = registerPermissionSaveEvents;
        vm.save = save;


        //Private Methods
        function registerPermissionSaveEvents(event) {
            permissionSaveEvents.push(event);
        }

        function init() {
            vm.entityGroup = mdPermissionEntitiesService.groups().filter(function (group) { return group.id == entityGroupId; })[0];
        }

        function save() {
            $q.all(permissionSaveEvents.map(function (event) { return event(); })).then(function () {
                $state.go('app.settings_configuration_permissions');
            });
        }

        init();
    }
})();
