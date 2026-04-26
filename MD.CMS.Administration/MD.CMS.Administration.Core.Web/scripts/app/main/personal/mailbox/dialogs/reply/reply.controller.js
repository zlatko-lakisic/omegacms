(function () {
    'use strict';

    angular
        .module('app.personal.mailbox')
        .controller('ReplyController', ['$mdDialog', 'message', 'mdFieldService', 'threadMessages', ReplyController]);

    /** @ngInject */
    function ReplyController($mdDialog, message, mdFieldService, threadMessages) {
        //Private Attributes
        var vm = this;
        var messageController = new mdBusinessLogic.dataAccess.controllers.messageController();


        //Public Attributes
        vm.reply = new mdBusinessLogic.dataAccess.entities.message();
        vm.message = message;
        vm.threadMessages = threadMessages;
        vm.text = mdFieldService.transformOther('', true, '', ';', 0, '', '', '', {}, '', false, '');


        //Public Methods
        vm.sendMessage = sendMessage;
        vm.closeDialog = closeDialog;


        //Private Methods
        function sendMessage() {
            vm.reply.Subject = vm.message.Subject;
            vm.reply.MainThread = vm.message.MainThread;
            vm.reply.MessageFolderId = vm.message.MessageFolderId;
            vm.reply.MessageContent = vm.text.value;
            if (vm.message.Type == 1) { //sent
                vm.reply.ToUserId = vm.message.ToUser.Id;
                vm.reply.ToUser = vm.message.ToUser;

                vm.reply.FromUserId = vm.message.FromUser.Id;
                vm.reply.FromUser = vm.message.FromUser;
            } else if (vm.message.Type == 2) { //recieved
                vm.reply.ToUserId = vm.message.FromUser.Id;
                vm.reply.ToUser = vm.message.FromUser;

                vm.reply.FromUserId = vm.message.ToUser.Id;
                vm.reply.FromUser = vm.message.ToUser;
            }
            messageController.save(vm.reply, function (data) {
                $mdDialog.hide(vm.reply);
            }, function (error) {
                $mdDialog.hide();
            })
        }

        function closeDialog() {
            $mdDialog.cancel();
        }
    }
})();
