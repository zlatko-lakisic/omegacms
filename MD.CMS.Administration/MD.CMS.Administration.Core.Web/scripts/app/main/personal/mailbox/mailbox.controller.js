(function () {
    'use strict';

    angular
        .module('app.personal.mailbox')
      .controller('MailboxController', ['$state', '$rootScope', '$scope', '$filter', '$mdSidenav', '$mdDialog', '$document', '$mdMedia', 'Icons', '$timeout', 'mdFeedbackService', '$UnreadMessagesService', 'systemFolders', 'userFolders', MailboxController]);


    /** @ngInject */
    function MailboxController($state, $rootScope, $scope, $filter, $mdSidenav, $mdDialog, $document, $mdMedia, Icons, $timeout, $mdFeedbackService, $UnreadMessagesService, systemFolders, userFolders) {
        var vm = this;
        var unreadMessages = $UnreadMessagesService.unreadMessages

        // Controllers
        var messageFolderController = new mdBusinessLogic.dataAccess.controllers.messageFolderController();
        var messageController = new mdBusinessLogic.dataAccess.controllers.messageController();
        var contentController = new mdBusinessLogic.dataAccess.controllers.contentController();
        var approvalChainController = new mdBusinessLogic.dataAccess.controllers.approvalChainController();

        // Variables
        var inboxFolderId = 1, sentFolderId = 2, trashFolderId = 3, approvalsFolderId = 4;
        vm.loggedOnUser = mdBusinessLogic.globals.loggedOnUser;
        vm.userFolders = userFolders;
        vm.systemFolders = systemFolders;
        vm.allFolders = vm.systemFolders.concat(vm.userFolders);
        selectFolder(vm.allFolders[0], 0);
        vm.messages = [];
        vm.activeMailPaneIndex = 0;
        vm.checked = [];
        vm.approval = {};
        vm.didGet = true;
        vm.noMessages;
        vm.colors = ['blue-bg', 'blue-grey-bg', 'orange-bg', 'pink-bg', 'purple-bg'];
        vm.responsiveReadPane = undefined;
        vm.activeMailPaneIndex = 0;
        vm.dynamicHeight = false;
        vm.scrollPos = 0;
        vm.scrollEl = angular.element('#content');
        vm.selectedMailShowDetails = false;
        vm.currentPage = 0;
        vm.pageSize = 10;
        vm.totalItems = 0;
        vm.pagesBorder = 1;
        vm.search = search;
        vm.searchTerm = "";
        vm.searchGotResponse = true;
        vm.indetermined = false;
        vm.listLoading = false;
        vm.messageLoading = false;

        // Methods     
        vm.selectFolder = selectFolder;
        vm.selectMessage = selectMessage;      
        vm.checkAll = checkAll;
        vm.toggleCheck = toggleCheck;
        vm.isChecked = isChecked;      
        vm.moveMessages = moveMessages;      
        vm.closeReadPane = closeReadPane;
        vm.updatePager = updatePager;
        vm.sendApproval = sendApproval;
        vm.goToContent = goToContent;
        vm.convertApproval = convertApproval;
        vm.toggleSidenav = toggleSidenav;
        vm.addNewMessageFolderDialog = addNewMessageFolderDialog;
        vm.deleteMessagesDialog = deleteMessagesDialog;
        vm.composeDialog = composeDialog;
        vm.editFolderDialog = editFolderDialog;
        vm.replyDialog = replyDialog;

        function init() {
            var notFound = true;
            if ($state.params.folder) {
                for (var i = 0; i < vm.allFolders.length; i++){
                    if (vm.allFolders[i].Name.toLowerCase() === $state.params.folder.toLowerCase()) {
                        selectFolder(vm.allFolders[i], i);
                        notFound = false;
                        break;
                    }
                }
                if (notFound) {
                    $state.go("app.mailbox", { folder: "inbox"});
                }
            }
        }
        init();

        function openShowDialog(title) {
            $mdDialog.show(
                   $mdDialog.alert()
                  .clickOutsideToClose(true)
                  .title(title)
                  .ariaLabel(title + " alert")
                  .ok('Got it!')
                  .targetEvent()
                  );
        }

        function updatePager(currentPage, pageSize, pagesBorder) {
            vm.currentPage = currentPage;
            vm.pageSize = pageSize;
            vm.pagesBorder = pagesBorder;
            getMessagesByFolderAndUser();
        }

        function getMessages() {
            vm.listLoading = true;
            messageController.search({
                searchTerm: vm.searchTerm,
                currentPageIndex: vm.currentPage,
                maxNumberOfRows: vm.pageSize,
            }, function (data) {
                $scope.$apply(function () {
                    vm.searchGotResponse = true;
                    assignMessages(data);
                    getMessageCount();
                })
            }, function (error) {
                $scope.$apply(function () {
                    vm.searchGotResponse = true;
                    $mdFeedbackService.reportError('load', error);
                })
            })
        }
        
        function getMessageCount() {
            messageController.searchCount(
                    vm.searchTerm,
                    function (data) {
                        $scope.$apply(function () {
                            vm.totalItems = data;
                        });
                    }, function (error) {
                        $scope.$apply(function () {
                            $mdFeedbackService.reportError('load', error);
                        })
                    });
        }

        function search() {
            vm.searchMode = vm.searchTerm.length > 0;
            vm.selectedFolder = null;
            vm.searchGotResponse = false;
            if (vm.searchMode) {
                getMessages();
            } else {
                selectFolder(vm.allFolders[0], 0);
            }
        }

        function loadIcons() {
            vm.Icons = Icons.icons.map(function (state) {
                return {
                    name: state.properties.name
                };
            });
            vm.Icons = Icons.icons;
            return vm.Icons;
        }

        function addNewMessageFolderDialog(event) {
            loadIcons();
            $mdDialog.show({
                controller: 'AddNewFolderController',
                controllerAs: 'vm',
                templateUrl: 'scripts/app/main/personal/mailbox/dialogs/addNewFolder/addNewFolder.html',
                parent: angular.element($document.body),
                locals: {
                    Icons: loadIcons()
                },
                targetEvent: event,
                clickOutsideToClose: true,
                fullscreen: true,
            }).then(function (savedFolder) {
                vm.userFolders.push(savedFolder);
                vm.allFolders.push(savedFolder);
            }, function () {
                $scope.status = $rootScope.globals.resources.Labels.DialogCanceled;
            });
        }

        function editFolderDialog(event, folder) {
            loadIcons();
            $mdDialog.show({
                controller: 'EditFolderController',
                controllerAs: 'vm',
                templateUrl: 'scripts/app/main/personal/mailbox/dialogs/editFolder/editFolder.html',
                parent: angular.element($document.body),
                locals: {
                    Icons: loadIcons(),
                    selectedFolder: vm.selectedFolder
                },
                targetEvent: event,
                clickOutsideToClose: true,
                fullscreen: true,
            }).then(function (editedFolder) {
                if (editedFolder) {
                    vm.allFolders[vm.selectedFolderIndex] = editedFolder;
                } else {
                    var confirm = $mdDialog.confirm()
                                     .title($rootScope.globals.resources.Titles.RemoveQuestion)
                                     .targetEvent(event)
                                     .clickOutsideToClose(true)
                                     .parent(angular.element(document.body))
                                     .ok($rootScope.globals.resources.Labels.Yes)
                                     .cancel($rootScope.globals.resources.Labels.No);
                    $mdDialog.show(confirm).then(function () {
                        if (vm.selectedFolder.MessagesCount < 1) {
                            deleteFolder(vm.selectedFolder, vm.selectedFolderIndex);
                            return;
                        }
                        var confirm2 = $mdDialog.confirm()
                                          .title($rootScope.globals.resources.Titles.SaveQuestion)
                                          .targetEvent(event)
                                          .clickOutsideToClose(false)
                                          .parent(angular.element(document.body))
                                          .ok($rootScope.globals.resources.Labels.Yes)
                                          .cancel($rootScope.globals.resources.Labels.No);
                        $mdDialog.show(confirm2).then(function () {
                            chooseFolderForMessagesDialog();
                        }, function () {
                            deleteFolder(vm.selectedFolder, vm.selectedFolderIndex);
                        });
                    }, function () {
                    });
                }
            }, function canceled() {

            });
        }

        function chooseFolderForMessagesDialog() {
            $scope.showAdvanced = function (ev) {
                $mdDialog.show({
                    controller: "ChooseFolderController",
                    controllerAs: 'vm',
                    templateUrl: 'scripts/app/main/personal/mailbox/dialogs/chooseFolder/choose-folder.html',
                    parent: angular.element(document.body),
                    targetEvent: ev,
                    clickOutsideToClose: true,
                    fullscreen: true,
                    locals: {
                        selectedFolder: vm.selectedFolder
                    },
                })
                .then(function (choosenFolder) {
                    choosenFolder = getFolderById(choosenFolder.Id);
                    getMessagesByFolderAndUser(vm.selectedFolder, 0, 1000, function (data) {
                        moveMessages(choosenFolder, vm.messages, function () {
                            deleteFolder(vm.selectedFolder, vm.selectedFolderIndex);
                        });
                    });
                }, function () {
                    $scope.status = $rootScope.globals.resources.Labels.DialogCanceled;
                });
            };
            $scope.showAdvanced(event);
        }

        function deleteFolder(folderToDelete, index) {
            messageFolderController['delete'](folderToDelete.Id, function (data) {
                vm.allFolders.splice(index, 1);
                selectFolder(vm.allFolders[0], 0)
                $mdFeedbackService.reportInfo('delete');
            }, function (error) {
                $mdFeedbackService.reportError('delete', error);
            })
        }

        function alreadyReplied() {
            if (!vm.threadMessages || vm.threadMessages.length < 1) {
                return false;
            }

            var myReply = vm.threadMessages.find(function (message) {
                return message.FromUserId == mdBusinessLogic.globals.loggedOnUser.Id
            })
            return myReply
        }

        function afterSendMessage(sentMessage, incrementSent) {
            if (sentMessage) {
                openShowDialog($rootScope.globals.resources.Labels.MessageSent);

                if (!alreadyReplied() || incrementSent) {
                    vm.allFolders[1].MessagesCount++;
                    vm.totalItems = vm.selectedFolder.MessagesCount;
                    var unbind = $scope.$watch('vm.currentPage', function () {
                        if (vm.selectedFolder.Id == vm.allFolders[1].Id && vm.currentPage == 0) {
                            vm.messages.unshift(sentMessage);
                        }
                        unbind();
                    });
                }
            }
        }

        function composeDialog(event) {
            vm.threadMessages = [];
            $mdDialog.show({
                controller: 'ComposeDialogController',
                controllerAs: 'vm',
                templateUrl: 'scripts/app/main/personal/mailbox/dialogs/compose/compose-dialog.html',
                parent: angular.element($document.body),
                locals: {

                },
                targetEvent: event,
                clickOutsideToClose: false,
                fullscreen: true,
            }).then(function (sentMessage) {
                afterSendMessage(sentMessage, true);
            }, function (cancelData) {
            });
        }

        function replyDialog(event) {
            $mdDialog.show({
                controller: 'ReplyController',
                controllerAs: 'vm',
                templateUrl: 'scripts/app/main/personal/mailbox/dialogs/reply/reply.html',
                parent: angular.element($document.body),
                locals: {
                    message: vm.selectedMessage,
                    threadMessages: vm.threadMessages
                },
                targetEvent: event,
                clickOutsideToClose: true,
                fullscreen: true,
            }).then(function (message) {
                afterSendMessage(message);
                if (message.MainThread == vm.threadMessages[0].MainThread) {
                    vm.threadMessages.push(message);
                }
            }, function () {
            });
        }

        function deleteMessages(messages) {
            for (var i = 0, length = messages.length; i < length; i++) {
                messageOut(vm.allFolders[2], messages[i]);
            }
            messageController.deleteMultiple({ ValueName: '', ValueArray: vm.checked }, function (data) {
                selectFolder(vm.selectedFolder, vm.selectedFolderIndex);
                openShowDialog($rootScope.globals.resources.Labels.MessagesRemoved);
            }, function (error) {
                $mdFeedbackService.reportError('delete', error);
            });
        }

        function getFolderById(id) {
            var folder = vm.allFolders.find(function (f) {
                return f.Id === id;
            });
            return folder;
        }

        function moveMessages(toFolder, messages, afterMoveCallback) {
            vm.searchMode = false;
            if (messages.length < 1) {
                openShowDialog($rootScope.globals.resources.Labels.NoMessagesSelected);
                return;
            }

            for (var i = 0, length = messages.length; i < length; i++) {
                messages[i].MessageFolderId = toFolder.Id;
            }
            messageController.replaceMultiple({ ValueName: '', ValueArray: messages }, function (data) {
                openShowDialog($rootScope.globals.resources.Labels.MessagesReplaced);
                var inboxMessage = messages.find(function (m) {
                    return m.CurrentFolder.Id == inboxFolderId;
                });
                var sentMessage = messages.find(function (m) {
                    return m.CurrentFolder.Id == sentFolderId;
                });
                if (inboxMessage) {
                    messageFolderController.getByIdAndAuthorId(vm.allFolders[sentFolderId - 1].Id,
                       function (data) {
                           $scope.$apply(function () {
                               vm.allFolders[sentFolderId - 1] = data;
                           });                        
                       },
                       function onError(error) {
                           $mdFeedbackService.reportError('load', error);

                       });                   
                }
                if (sentMessage) {
                    messageFolderController.getByIdAndAuthorId(vm.allFolders[inboxFolderId - 1].Id,
                        function (data) {
                            $scope.$apply(function () {
                                vm.allFolders[inboxFolderId - 1] = data;
                            });
                        }, function onError(error) {
                            $mdFeedbackService.reportError('load', error);
                        })
                }
                
                for (var i = 0, length = messages.length; i < length; i++) {
                    messageOut(messages[i].CurrentFolder, messages[i]);
                    messageIn(toFolder, messages[i]);
                }

                if (afterMoveCallback) {
                    afterMoveCallback();
                } else {
                    if (vm.selectedFolder) {
                        selectFolder(vm.selectedFolder);
                    }
                }
            }, function (error) {
                $mdFeedbackService.reportError('load', error);
            })
        }

        function messageOut(folder, message) {
            folder.MessagesCount--;
        }

        function messageIn(folder, message) {
            folder.MessagesCount++;
            message.MessageFolderId = folder.Id;
        }

        function deleteMessagesDialog(event) {
            if (vm.checked.length < 1) {
                openShowDialog($rootScope.globals.resources.Labels.NoMessagesSelected);
            } else {
                var isInTrash = false;
                if (vm.selectedFolder && vm.selectedFolder.Id == trashFolderId) {
                    isInTrash = true;
                }
                var dialogTextContent = isInTrash ? $rootScope.globals.resources.Labels.SelectedMessagesWillBeRemoved : $rootScope.globals.resources.Labels.SelectedMessagesWillBeMoved;
                var confirm = $mdDialog.confirm()
                                       .title($rootScope.globals.resources.Titles.RemoveQuestion)
                                       .textContent(dialogTextContent)
                                       .targetEvent(event)
                                       .clickOutsideToClose(true)
                                       .parent(angular.element(document.body))
                                       .ok($rootScope.globals.resources.Labels.Yes)
                                       .cancel($rootScope.globals.resources.Labels.No);
                $mdDialog.show(confirm).then(function () {
                    if (isInTrash) {
                        deleteMessages(vm.checked);
                    } else {
                        moveMessages(vm.allFolders[2], vm.checked, function () {
                            if (!vm.searchMode) {
                                selectFolder(vm.selectedFolder, vm.selectedFolderIndex);
                            } else {
                                setDefaultValues();
                            }
                        });
                    }

                }, function () {

                });
            }
        }

        function getAllFoldersToDisplay() {
            messageFolderController.getAllSystemFolders(
                function (data) {
                    vm.systemFolders = data;
                    messageFolderController.getByAuthorId(function (data) {
                        vm.userFolders = data;
                        vm.allFolders = vm.systemFolders.concat(vm.userFolders);
                        selectFolder(vm.allFolders[0], 0);
                    }, function (error) {
                        $mdFeedbackService.reportError('load', error);
                    });
                },
                function (error) {
                    $mdFeedbackService.reportError('load', error);
                });
        }

        function setDefaultValues() {
            vm.allChecked = false;
            vm.checked = [];
            vm.messages = [];
            vm.selectedMessage = null;
            vm.listLoading = false;
        }
          
        function selectFolder(messageFolder, $index) {
            if (!messageFolder) {
                return;
            }
            messageFolderController.getByIdAndAuthorId(messageFolder.Id,
                function (renewedFolder) {
                    $scope.$apply(function () {
                        vm.selectedFolder = renewedFolder;
                        vm.selectedFolderIndex = $index;
                        vm.allFolders[$index] = renewedFolder;
                        vm.currentPage = 0;
                        getMessagesByFolderAndUser();
                    });
                },
                function onError(error) {
                    $mdFeedbackService.reportError('load', error);
                });
        }

        function assignCurrentFolderToMessages(messages) {
            for (var i = 0, length = messages.length; i < length; i++) {
                messages[i].CurrentFolder = getFolderById(messages[i].MessageFolderId);
            }
        }

        function getMessagesByFolderAndUser() {
            vm.didGet = false;
            vm.listLoading = true;
            messageController.getByMessageFolderAndUser({
                folderId: vm.selectedFolder.Id,
                searchTerm: encodeURIComponent(vm.searchTerm),
                currentPageIndex: vm.currentPage,
                maxNumberOfRows: vm.pageSize
            }, function (data) {
                $scope.$apply(function () {
                    assignMessages(data.Items);
                    vm.totalItems = data.TotalCount
                });
            }, function (error) {
                vm.didGet = true;
                $mdFeedbackService.reportError('load', error);
            })
        }

        function assignMessages(data) {
            vm.messages = data;
            vm.listLoading = false;
            vm.checked = []
            vm.allChecked = false;
            vm.selectedMessage = null;
            assignCurrentFolderToMessages(vm.messages);
            vm.didGet = true;
            if (vm.messages.length > 0) {
                vm.selectMessage(vm.messages[0], 0);
            }
        }

        function markAsRead(message) {
            if (!message.IsRead) {
                message.IsRead = true;
                messageController.messageRead(message, function (data) {
                }, function (error) {
                    $mdFeedbackService.reportError('update', error);
                })
            }
        }

        function getMessagesByThread(message) {
            vm.messageLoading = true;
            messageController.getByMainThread(message.MainThread, function (data) {
                $scope.$apply(function () {
                    vm.threadMessages = data;
                    vm.messageLoading = false;
                });
            }, function (error) {
                $mdFeedbackService.reportError('load', error);
                vm.messageLoading = false;
            })
        }

        function selectMessage(message, index) {
            if (vm.messageIndex != index) {
                vm.messageIndex = index;
                vm.selectedMessage = message;
                markAsRead(message);
                getMessagesByThread(message);
                $timeout(function () {
                    // If responsive read pane is
                    // active, navigate to it
                    if (angular.isDefined(vm.responsiveReadPane) && vm.responsiveReadPane) {
                        vm.activeMailPaneIndex = 1;
                    }
                    // Store the current scrollPos
                    vm.scrollPos = vm.scrollEl.scrollTop();
                    // Scroll to the top
                    vm.scrollEl.scrollTop(0);
                });
            }
        }

        function closeReadPane() {
            if (angular.isDefined(vm.responsiveReadPane) && vm.responsiveReadPane) {
                vm.activeMailPaneIndex = 0;

                $timeout(function () {
                    vm.scrollEl.scrollTop(vm.scrollPos);
                }, 650);
            }
        }

        function toggleSidenav(sidenavId) {
            $mdSidenav(sidenavId).toggle();
        }

        function toggleCheck(message, event) {
            if (event) {
                event.stopPropagation();
            }
            var index = vm.checked.indexOf(message);
            if (index > -1) {
                vm.checked.splice(index, 1);
            }
            else {
                vm.checked.push(message);
            }
            
            if (vm.allChecked) {
                vm.allChecked = false;
            }
            if (vm.checked.length == vm.messages.length) {
                vm.allChecked = true;
            }
            vm.indetermined = vm.checked.length != vm.messages.length && vm.checked.length > 0;
        }

        function isChecked(mail) {
            return vm.checked.indexOf(mail) > -1;
        }

        function checkAll() {
            if (vm.allChecked) {
                vm.checked = [];
                vm.allChecked = false;
            }
            else {
                angular.forEach(vm.messages, function (message) {
                    if (!isChecked(message)) {
                        toggleCheck(message);
                    }
                });
                vm.allChecked = true;
            }
        }

        //approval
        function sendApproval(type) {
            var data = new mdBusinessLogic.dataAccess.entities.approvalChainApproval({
                ApprovalType: type,
                Step: vm.selectedMessage.MessageContent.Step,
                Content: vm.selectedMessage.MessageContent.Content,
                User: vm.selectedMessage.MessageContent.User,
                Comment: vm.selectedMessage.MessageContent.comment
            });
            approvalChainController.addApproval(data, function (data) {
                messageController.deleteMultiple({ ValueName: '', ValueArray: [vm.selectedMessage] }, function (data) {
                    vm.selectFolder(vm.allFolders[3], 3);
                }, function (error) {
                    vm.selectFolder(vm.allFolders[3], 3);
                    $mdFeedbackService.reportError('save', error);
                });
            }, function (error) {
                vm.selectFolder(vm.allFolders[3], 3);
                $mdFeedbackService.reportError('save', error);
            });
        }

        function goToContent() {
            var content = vm.selectedMessage.MessageContent.Content;
            if (content) {
                contentController.getById(content.Id, true, content.lcid, true, content.IsDataBound, content.ContentType.Id, function (data) {
                    $state.go('app.content_form', {
                        currentView: vm.currentView,
                        action: 'edit',
                        path: data.Path,
                        folderId: content.FolderId,
                        id: content.Id,
                        lcid: content.LCID
                    });
                }, function (error) {
                    $mdFeedbackService.reportError('load', error);
                });
            }
        }

        function convertApproval(message) {
            if (vm.selectedFolder.Id === approvalsFolderId && typeof message.MessageContent === "string") {
                try {
                    var messageJSON = JSON.parse(message.MessageContent);
                    message.MessageContent = {
                      Content: new mdBusinessLogic.dataAccess.entities.content(messageJSON),
                      User: mdBusinessLogic.globals.loggedOnUser,
                      Step: new mdBusinessLogic.dataAccess.entities.approvalChainStep({ Id: messageJSON.stepId }),
                      Reason: messageJSON.Reason,
                      Reject: messageJSON.Rejected,
                      Edit: messageJSON.Edit
                    };
                    if (messageJSON.Edit) {
                        message.MessageContent.Preview = $rootScope.globals.resources.Labels.ContentChanges;
                    } else if (messageJSON.Rejected) {
                        message.MessageContent.Preview = $rootScope.globals.resources.Labels.ContentRejected;
                    } else {
                        message.MessageContent.Preview = $rootScope.globals.resources.Labels.NewContent;
                    }
                    return message.MessageContent.Preview;
                } catch (e) {
                    if (e instanceof SyntaxError){
                        message.NotContentApproval = true;
                        return $filter('htmlToPlaintext')(message.MessageContent);
                    }
                    return '';
                }
            }
        }

        $scope.$on('newMessages', function (event, newMessages) {           
            for (var i = 0, length = newMessages.length; i < length; i++) {
                angular.element(document.querySelector('#mf' + newMessages[i].MessageFolderId)).addClass('has-unread');
            }
            getAllFoldersToDisplay();
        });

        // Watch screen size to activate responsive read pane
        $scope.$watch(function () {
            return $mdMedia('gt-md');
        }, function (current) {
            vm.responsiveReadPane = !current;
        });

        // Watch screen size to activate dynamic height on tabs
        $scope.$watch(function () {
            return $mdMedia('xs');
        }, function (current) {
            vm.dynamicHeight = current;
        });
    }
})();
