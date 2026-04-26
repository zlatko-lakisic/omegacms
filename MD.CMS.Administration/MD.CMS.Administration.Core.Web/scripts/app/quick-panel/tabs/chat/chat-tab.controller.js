(function ()
{
    'use strict';

    angular
        .module('app.quick-panel')
        .controller('ChatTabController', ['msApi', '$timeout', '$scope', '$q', '$mdDialog', ChatTabController]);

    /** @ngInject */
    function ChatTabController(msApi, $timeout, $scope, $q, $mdDialog)
    {
        //Private Properties
        var vm = this;
        var messageFolderController = new mdBusinessLogic.dataAccess.controllers.messageFolderController();
        var messageController = new mdBusinessLogic.dataAccess.controllers.messageController();
        var userController = new mdBusinessLogic.dataAccess.controllers.userController();
        var chatMessageFolder = null;
        var chatThreadId = null;

        //Public Properties
        vm.messages = [];
        vm.users = [];
        vm.currentThread = [];
        vm.message = null;
        vm.chat = {};
        vm.chatActive = false;
        vm.replyMessage = '';

        //Public Methods
        vm.toggleChat = toggleChat;
        vm.reply = reply;

        //Private Methods
        function loadMessages() {
            loadFolder().then(function (chatMessageFolder) {
                messageController.getAllChats(function (data) {
                    $scope.$apply(function () {
                        vm.messages = data.Items;
                    });
                }, function (error) { });
            });
        }
        function loadFolder() {
            return $q(function (resolve, reject) {
                if (chatMessageFolder == null) {
                    messageFolderController.getAll(function (data) {
                        chatMessageFolder = data.filter(function (f) { return f.Name == 'Chat'; })[0];
                        resolve(chatMessageFolder);
                    }, function (data) {
                        reject();
                    });
                } else {
                    resolve(chatMessageFolder);
                }
            });
        }
        function loadUsers() {
            userController.getAll(function (data) {
                $scope.$apply(function () {
                    vm.users = data.filter(function (user) { return user.Id != mdBusinessLogic.globals.loggedOnUser.Id; });
                });
            }, function (error) { });
        }
        function loadThread(threadId) {
            messageController.getByMainThread(threadId, function (data) {
                $scope.$apply(function () {
                    vm.currentThread = data;
                    scrollToBottomOfChat(0);
                });
            }, function (error) {
            });
        }
        function toggleChat(user, threadId) {
            vm.chatActive = !vm.chatActive;

            if (vm.chatActive) {
                loadMessageAndChatData(user, threadId);
            }
        }
        function loadMessageAndChatData(user, threadId) {
            vm.chat.contact = user;
            vm.currentThread = [];
            vm.message = new mdBusinessLogic.dataAccess.entities.message();
            vm.message.ToUser = user;
            vm.message.ToUserId = user.Id;
            vm.message.FromUserId = mdBusinessLogic.globals.loggedOnUser.Id;
            vm.message.MessageFolderId = chatMessageFolder.Id;
            if (threadId !== undefined) {
                chatThreadId = threadId;
                vm.message.MainThread = threadId;
                loadThread(chatThreadId);
            }
        }
        function reply() {
            if (vm.message.MessageContent === '') {
                return;
            }

            messageController.save(vm.message, function (data) {
                if (chatThreadId == null) {
                    chatThreadId = vm.message.MainThread;
                }
                loadMessageAndChatData(vm.message.ToUser, chatThreadId);
            }, function (error) {
                $mdDialog.cancel(error);
            });

            vm.replyMessage = '';

            scrollToBottomOfChat(400);
        }
        function scrollToBottomOfChat(speed) {
            var chatDialog = angular.element('#chat-dialog');

            $timeout(function () {
                chatDialog.animate({
                    scrollTop: chatDialog[0].scrollHeight
                }, speed);
            }, 0);

        }
        function init() {
            loadMessages();
            loadUsers();
        }

        init();
    }

})();