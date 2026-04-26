(function () {
    'use strict';

    angular
        .module('app.personal.mailbox')
        .controller('AddNewFolderController', ['$state', '$scope', '$mdDialog', 'msApi', 'Icons', AddNewFolderController]);


    /** @ngInject */
    function AddNewFolderController($state, $scope, $mdDialog, msApi, Icons) {
        var vm = this;

        //services
        vm.messageFolder = new mdBusinessLogic.dataAccess.entities.messageFolder();
        var messageFolderController = new mdBusinessLogic.dataAccess.controllers.messageFolderController();

        //variables
        vm.Icons = Icons;
        vm.loggedOnUser = mdBusinessLogic.globals.loggedOnUser;

        //methods
        vm.submitForm = submitForm;


        vm.cancel = function () {
            $mdDialog.cancel();
        };

        function submitForm() {
            vm.messageFolder.Icon = vm.selectedIcon.icon.tags[0];
            vm.messageFolder.AuthorId = vm.loggedOnUser.Id;
            messageFolderController.save(vm.messageFolder,
                function (savedFolder) {
                    $mdDialog.hide(savedFolder);                  
            }, function (error) {
                console.log(error);
            });
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