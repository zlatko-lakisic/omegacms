(function () {
    'use strict';

    angular
        .module('app.personal.mailbox')
        .controller('EditFolderController', ['$state', '$scope', '$mdDialog', 'msApi', 'Icons', 'selectedFolder', EditFolderController]);


    /** @ngInject */
    function EditFolderController($state, $scope, $mdDialog, msApi, Icons, selectedFolder) {
        var vm = this;

        //services      
        var messageFolderController = new mdBusinessLogic.dataAccess.controllers.messageFolderController();

        //variables
        vm.Icons = Icons;
        vm.messageFolder = selectedFolder;
        vm.selectedIcon = vm.messageFolder.Icon;
        vm.loggedOnUser = mdBusinessLogic.globals.loggedOnUser;

        //methods
        vm.submitForm = submitForm;
        vm.deleteFolder = deleteFolder;

        vm.cancel = function () {
            $mdDialog.cancel();
        };

        function submitForm() {
            if (vm.selectedIcon && vm.selectedIcon.icon && vm.selectedIcon.icon.tags) {
                vm.messageFolder.Icon = vm.selectedIcon.icon.tags[0];
            }
            vm.messageFolder.AuthorId = vm.loggedOnUser.Id;
            messageFolderController.save(vm.messageFolder,
                function (savedFolder) {
                    $mdDialog.hide(savedFolder);
                }, function (error) {
                    console.log(error);
                });
        }

        function deleteFolder() {
            $mdDialog.hide();
        }

        //icons
        vm.selectedIcon;
        vm.querySearch = querySearch;
        vm.createFilterFor = createFilterFor;

        function querySearch(query) {
            return query ? vm.Icons.filter(createFilterFor(query)) : vm.Icons;
        }
        function createFilterFor(query) {
            var lowercaseQuery = query.toLowerCase();

            return function filterFn(Icon) {
                return (Icon.properties.name.indexOf(lowercaseQuery) === 0);
            };
        }
        //end icons
    }
})();