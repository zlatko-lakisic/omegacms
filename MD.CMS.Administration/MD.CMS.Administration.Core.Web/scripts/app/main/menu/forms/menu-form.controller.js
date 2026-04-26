(function () {
    'use strict';

    angular
        .module('app.menu.forms')
        .controller('MenuFormController', ['$mdDialog', '$rootScope', '$state', '$scope', 'mdFeedbackService', 'menu', 'contents', MenuFormController]);

    /** @ngInject */
    function MenuFormController($mdDialog, $rootScope, $state, $scope, $mdFeedbackService, menu, contents) {
        var vm = this;
        // Controllers
        var menuController = new mdBusinessLogic.dataAccess.controllers.menuController();
        var contentController = new mdBusinessLogic.dataAccess.controllers.contentController();
        var menuContentController = new mdBusinessLogic.dataAccess.controllers.menuContentController();
        vm.currentMenuPath = $state.params.path;
        vm.currentView = $state.params.currentView;
        var parentMenu = {};
        // Variables 
        vm.menu = menu;
        vm.selectedMenu = vm.menu;
        vm.basicForm = {};
        vm.formWizard = {};
        vm.isNew = $state.params.action != 'edit';
        var addOrEdit = $state.params.action;
        vm.id = 0;
        vm.menu.ParentId = $state.params.id;
        var dialogInfoText = addOrEdit === 'add' ? $rootScope.globals.resources.Labels.AddedText : $rootScope.globals.resources.Labels.EditedText;
        vm.formTitle = addOrEdit === 'add' ? $rootScope.globals.resources.Titles.AddMenu : $rootScope.globals.resources.Titles.EditMenu;
        vm.changeState = changeState;
        vm.AddContent = [];
        vm.contents = [];
        vm.content = [];
        vm.removedContent = [];
        vm.isSaved = false

        // Methods
        vm.sendForm = sendForm;
        vm.contentSearchText;
        vm.selectedcontentItem;
        vm.queryAllContent = queryAllContent;
        vm.addContent = addContent;
        vm.changeTab = changeTab;
        vm.RemoveContent = RemoveContent;

        if (!vm.isNew) {
            for (var i in vm.menu.Contents) {
                vm.content = vm.menu.Contents;
            }
        }

        function changeTab(tab) {
            vm.tab = tab;
        }
        vm.contents = contents.map(function (content) {
            content._lowertitle = content.Title.toLowerCase();
            return content;
        });


        function queryAllContent(query) {
            var lowercaseQuery = query.toLowerCase();
            var results = query ? vm.contents.filter(function (query) {
                return function filterFn(content) {
                    return (content._lowertitle.indexOf(lowercaseQuery) === 0);
                };
            }) : [];
            var i = results.length;
            while (i--) {
                if (contentController.doesContentExist(vm.AddContent, results[i]) >= 0 ||
                    contentController.doesContentExist(vm.content, results[i]) >= 0 ||
                    results[i]._lowertitle.indexOf(lowercaseQuery) == -1) {
                    results.splice(i, 1);
                }
            }
            return results;
        }

        function addContent(content) {
            var contentExist = contentController.doesContentExist(vm.AddContent, content) >= 0 &&
                             contentController.doesContentExist(vm.content, content) >= 0;
            if (!contentExist) {
                var contentPreviouslyRemovedBeforeSaving = vm.removedContent.indexOf(content);
                if (contentPreviouslyRemovedBeforeSaving != -1) {
                    vm.removedContent.splice(contentPreviouslyRemovedBeforeSaving, 1);
                }
                else {
                    vm.AddContent.push(content);
                    vm.menu.Contents = vm.AddContent;
                }
            }
            vm.selectedcontentItem = null;
            vm.contentSearchText = '';
        }


        function RemoveContent(content) {
            var contentExist = contentController.doesContentExist(vm.removedContent, content) >= 0 &&
                           contentController.doesContentExist(vm.content, content) < 0;
            if (!contentExist) {
                var contentPreviouslyRemovedBeforeSaving = vm.removedContent.indexOf(content);
                if (contentPreviouslyRemovedBeforeSaving != -1) {
                    vm.removedContent.splice(contentPreviouslyRemovedBeforeSaving, 1);
                }
                else {
                    vm.removedContent.push(content);
                    vm.menu.Contents = vm.removedContent;
                }
            }
        }

    
        function callApi() {
            if (mdBusinessLogic.settings.lcid!=0) {
                vm.menu.lcid = mdBusinessLogic.settings.lcid;
            }
           
            menuController.save(vm.menu, function (data) {
                vm.menu = data;
                $scope.$emit('LoadNav', {
                    action: 'save',
                    type: mdBusinessLogic.dataAccess.entities.entitiesEnum.Menu,
                    value: angular.copy(vm.menu)
                });
                vm.isSaved = true;
                changeState();
                $mdFeedbackService.reportInfo('save');
            }, function (error) {
                $mdFeedbackService.reportError('save', error);
            })
        }
        function sendContent() {
            vm.menu.Contents = vm.content;
            
        }

        function changeState() {
            if ($state.params.id) {
               
                vm.currentMenuPath = $state.params.path;

                var nameOfCurrentFolderFormPath = vm.currentMenuPath.slice(vm.currentMenuPath.lastIndexOf("/") + 1, vm.currentMenuPath.length);
               
                if (vm.isSaved === true && !vm.isNew) {
                        //If Name form path is not equal to vm.menu.Name we have to put new name in vm.currentMenuPath
                    if (nameOfCurrentFolderFormPath !== vm.menu.Name && nameOfCurrentFolderFormPath!== vm.menu.Parent.Name) {
                        //replace old name with new
                            var newCurrentMenuPath = vm.currentMenuPath.replace(nameOfCurrentFolderFormPath, vm.menu.Name);
                            $state.go('app.menu_list', { menuPath: newCurrentMenuPath, currentView: vm.currentView }, { reload: true });
                        return;
                    }
                    else {
                        $state.go('app.menu_list', { menuPath: vm.currentMenuPath, currentView: vm.currentView }, { reload: true });
                        return;
                    
                }
                }
                $state.go('app.menu_list', { menuPath: vm.currentMenuPath, currentView: vm.currentView }, { reload: true })
               }
        }

        function sendForm(ev) {
            sendContent();
            callApi();
        }
        
    }
})();