(function () {
    'use strict';

    angular
        .module('app.approval.chain.form')
        .controller('ApprovalChainFormController', ['$state', '$rootScope', '$scope', '$mdSidenav', '$mdDialog', ApprovalChainFormController]);

    /** @ngInject */
    function ApprovalChainFormController($state, $rootScope, $scope, $mdSidenav, $mdDialog) {
        var vm = this;

        //helpers
        var folderController = new mdBusinessLogic.dataAccess.controllers.folderController();
        var userController = new mdBusinessLogic.dataAccess.controllers.userController();
        var approvalChainController = new mdBusinessLogic.dataAccess.controllers.approvalChainController();

        vm.approvalchain = {};
        var addOrEdit;
        $state.params.approvalChainId ? addOrEdit = 'edit' : addOrEdit = 'add';
        var dialog = new mdBusinessLogic.helpers.dialog($mdDialog, $state);
        var dialogInfo = { };
        var stateInfo = { };

        //methods
        vm.sendForm = sendForm;
        vm.Back = Back;

        if (addOrEdit === 'add') {
            //automaticaly assign folder id when new approval chain is being created
            vm.approvalchain.FolderId = $state.params.folderId;
            //defaults
            vm.approvalchain.IsActive = false;
            //vm.approvalchain.steps = [];
        }
        else {
            approvalChainController.getById($state.params.approvalChainId, function (data) {
                $scope.$apply(function () {
                    vm.approvalchain = data;
                });
            }, function (error) {

            });
        }

        function Back() {
            var backtoFolder = $state.params.path;
            $state.go('app.approvalchain_list', { folderPath: backtoFolder, currentView: $state.params.currentView }, { reload: false });
        }

        function sendForm(ev) {
            //covert to necessary type
            vm.approvalChain = new mdBusinessLogic.dataAccess.entites(vm.approvalchain);
            approvalChainController.save(vm.approvalchain, function (data) {
                var dialogInfoText = addOrEdit === 'add' ? $rootScope.globals.resources.Labels.AddedText : $rootScope.globals.resources.Labels.EditedText;
                dialogInfo = {
                    title: $rootScope.globals.resources.Titles.ActionCompleted,
                    text: dialogInfoText
                }
                stateInfo = {
                    changeState: true,
                    stateToGo: 'app.approval_chain.list',
                    stateParams: {
                        folderPath: vm.folder.FolderPath
                    }
                }
                dialog.showSimpleDialogO(dialogInfo, stateInfo);
            }, function (error) {
                $mdDialog.show(
                                 $mdDialog.alert()
                                   .clickOutsideToClose(true)
                                   .title($rootScope.globals.resources.Titles.ActionCompleted)
                                   .textContent(error.message)
                                   .ok($rootScope.globals.resources.Labels.GotIt, $state.go('app.approval_chain.list', { folderPath: vm.folder.FolderPath }, { reload: false }))
                               );
            });
        }
    }
})();