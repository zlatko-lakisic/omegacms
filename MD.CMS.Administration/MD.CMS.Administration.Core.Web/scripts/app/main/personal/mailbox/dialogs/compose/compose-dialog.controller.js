(function () {
    'use strict';

    angular
        .module('app.personal.mailbox')
        .controller('ComposeDialogController', ['$mdDialog', 'mdFeedbackService', 'mdFieldService', ComposeDialogController]);

    /** @ngInject */
    function ComposeDialogController($mdDialog, $mdFeedbackService, mdFieldService) {
        //Private Attributes
        var vm = this;
        var messageController = new mdBusinessLogic.dataAccess.controllers.messageController();
        var userController = new mdBusinessLogic.dataAccess.controllers.userController();


        //Public Attributes
        vm.message = new mdBusinessLogic.dataAccess.entities.message();
        vm.loggedOnUser = mdBusinessLogic.globals.loggedOnUser;
        vm.message.FromUser = vm.loggedOnUser;
        vm.toUser = mdFieldService.transformOther('', true, '', ';', 0, '', '', '', {}, '', false, '');
        vm.text = mdFieldService.transformOther('', true, '', ';', 0, '', '', '', {}, '', false, '');


        //Public Methods
        vm.closeDialog = closeDialog;
        vm.sendMessage = sendMessage;
        vm.querySearch = querySearch;


        //Private Methods
        function sendMessage() {
            vm.message.ToUser = vm.selectedUser;
            vm.message.ToUserId = vm.toUser.value;
            vm.message.FromUserId = vm.loggedOnUser.Id;
            vm.message.MessageContent = vm.text.value;
            messageController.save(vm.message, function (data) {
                vm.message.MainThread = data.MainThread;
                $mdDialog.hide(vm.message);
            }, function (error) {
                $mdDialog.cancel(error);               
            })
        }

        function loadUsersForAutocomplete() {
            userController.getAll(function (data) {
                vm.allUsers = data.filter(function (user) {
                    return user.Id != mdBusinessLogic.globals.loggedOnUser.Id;
                });
            }, function (error) {

            })
        }

        function closeDialog() {
            $mdDialog.cancel();
        }

        function querySearch(query) {
            var results = query ? vm.allUsers.filter(createFilterFor(query)) : vm.allUsers,
                deferred;
            return results;
        }

        function createFilterFor(query) {
            var lowercaseQuery = query.toLowerCase();

            return function filterFn(user) {
                return (user.Username.indexOf(lowercaseQuery) === 0);
            };
        }

        loadUsersForAutocomplete();
    }
})();
