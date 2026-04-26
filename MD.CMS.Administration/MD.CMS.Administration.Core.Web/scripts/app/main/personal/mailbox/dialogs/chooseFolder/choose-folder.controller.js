(function () {
    'use strict';

    angular
        .module('app.personal.mailbox')
        .controller('ChooseFolderController', ['$state', '$scope', '$mdDialog', 'selectedFolder', ChooseFolderController]);


    /** @ngInject */
    function ChooseFolderController($state, $scope, $mdDialog, selectedFolder) {
        var vm = this;
        var messageFolderController = new mdBusinessLogic.dataAccess.controllers.messageFolderController();
       
        //variables      
        vm.toDelete = selectedFolder;
        vm.folders = [];

        //methods
        vm.chooseFolder = chooseFolder;

        function getFoldersForDropdown() {
            messageFolderController.getByAuthorId(function (data) {
                for (var i = 0, length = data.length; i < length; i++) {
                    if (data[i].Id != vm.toDelete.Id) {
                        vm.folders.push(data[i]);
                    }
                }
                messageFolderController.getById(3, function (data) {
                    vm.folders.unshift(data);
                }, function (error) { 
                })
            }, function (error) {

            });
        }

        function chooseFolder() {                  
            $mdDialog.hide(JSON.parse(vm.choosenFolder));
        }

        //executing
        getFoldersForDropdown();
    }
})();