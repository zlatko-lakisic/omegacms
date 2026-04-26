(function () {
    'use strict';

    angular
        .module('app.settings.configuration.profile-types-form')
        .controller('ProfileTypesFormController', ['$scope', '$state', '$mdDialog', 'profileType', '$timeout', ProfileTypesFormController]);

    /** @ngInject */
    function ProfileTypesFormController($scope, $state, $mdDialog, profileType, $timeout) {

        var vm = this;

        //Private Attributes
        var profileTypeController = new mdBusinessLogic.dataAccess.controllers.profileTypeController();
        var dialog = new mdBusinessLogic.helpers.dialog($mdDialog, $state);

        //Public Attributes
        vm.isEditMode = false;
        vm.isNew = false;


        //Public Methods
        vm.openEditor = openEditor;
        vm.closeEditor = closeEditor;
        vm.save = save;
        vm.registerEditEvent = registerEditEvent;

        //Private Methods
        var toggleEditMode = function () { }
        function registerEditEvent(event) {
            toggleEditMode = event;
        }
        function openEditor() {
            toggleEditMode();
            $scope.$root.navigationFolded = true;
            vm.isEditMode = true;
            angular.element(document.querySelector('#profile-types-preview')).addClass('full-screen');
        }
        function closeEditor(reinit) {
            if (reinit === undefined) {
                reinit = true;
            }
            vm.profileType = angular.copy(profileType);
            $scope.$root.navigationFolded = false;
            $state.go('app.profile-types-list');
        }
        function save() {
            profileTypeController.save(vm.profileType, function (data) {
                $scope.$apply(function () {
                    vm.closeEditor();
                });
            }, function (error) {
            });
        }
        function toggleMsNavigationFolded() {
            $scope.$root.navigationFolded = !$scope.$root.navigationFolded;
        }
        function init() {
            vm.isEditMode = false;
            vm.profileType = angular.copy(profileType);
            vm.isNew = vm.profileType.Id == 0;
            $timeout(function () {
                if (vm.isNew) {
                    openEditor();
                }
            }, 1500);

            $scope.$on('$destroy', function () {
                $scope.$root.navigationFolded = false;
            })
        }

        init();
    }
})();
