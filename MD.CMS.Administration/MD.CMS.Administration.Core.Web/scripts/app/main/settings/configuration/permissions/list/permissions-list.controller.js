(function () {
    'use strict';

    angular
        .module('app.settings.configuration.permissions-list')
        .controller('PermissionsListController', ['$scope', '$state', '$rootScope', '$mdSidenav', '$mdDialog', 'mdPermissionEntitiesService', PermissionsListController]);

    /** @ngInject */
    function PermissionsListController($scope, $state, $rootScope, $mdSidenav, $mdDialog, mdPermissionEntitiesService) {

        var vm = this;

        //Private Attributes
        var sortDirection = 'ASC';

        //Public Attributes
        vm.entityGroups = [];
        vm.selected = undefined;

        //Public Methods
        vm.sort = sort;
        vm.select = select;
        vm.showAlert = showAlert;
        vm.open = open;

        //Private Methods
        function open(group) {
            $state.go('app.permissions-form', {id:group.id});
        }

        function sort(sortString) {
            var sortAttribute = sortString.split(' ')[0];
            sortDirection = sortString.split(' ')[1];
            $scope.$apply(function () {
                vm.entityGroups.sort(function (a, b) {
                    if (a[sortAttribute] < b[sortAttribute]) { return sortDirection == 'ASC' ? -1 : 1; }
                    if (a[sortAttribute] > b[sortAttribute]) { return sortDirection == 'ASC' ? 1 : -1; }
                    return 0;
                });
            });
        }

        function select(group, $index) {
            vm.selected = group;
        }

        function showAlert(ev, title, content) {
            $mdDialog.show(
                $mdDialog.alert()
                    .parent(angular.element(document.querySelector('body')))
                    .clickOutsideToClose(true)
                    .title(title)
                    .textContent(content)
                    .ariaLabel(title)
                    .ok($rootScope.globals.resources.Labels.Close)
                    .targetEvent(ev)
            );
        }

        function init() {
            vm.entityGroups = mdPermissionEntitiesService.groups();
        }

        init();
    }
})();
