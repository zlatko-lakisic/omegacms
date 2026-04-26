(function () {
    'use strict';

    angular
        .module('app.reporting.report_scheduler.form')
        .controller('ActionFormController', ['$mdDialog', '$state', '$scope', '$mdMedia', ActionFormController]);

    /** @ngInject */
    function ActionFormController($mdDialog, $state, $scope, $mdMedia) {
        var vm = this;

        //helpers
        var reportSchedulerController = new mdBusinessLogic.dataAccess.controllers.reportSchedulerController();       
        var reportDirectoryController = new mdBusinessLogic.dataAccess.controllers.reportDirectoryController();

        //variables
        vm.actionTypes = [];
        vm.actionType;
        vm.action = {};
        vm.action.isActive = true;

        //methods
        vm.discard = discard;
        vm.cancel = cancel;
        vm.save = save;
        vm.change = change;
        vm.chooseDirectory = chooseDirectory;
        vm.disableBtn = true;
        
        reportSchedulerController.getReportSchedulerActionTypes(function (data) {
            vm.actionTypes = data;
        }, function (error) {

        });

        function chooseDirectory(path) {
            reportDirectoryController.getReportDirectoryByPath(path ? path : "", function (data) {
                $scope.$apply(function () {
                    vm.reportData = data;
                    showBackRefresh(vm.reportData.Path);
                });
            }, function (error) { })
        }       

        function discard() {           
            $mdDialog.hide();
        };
        function cancel () {
            $mdDialog.cancel();
        };
        function save(actionType) {
            switch (actionType) {
                case "SaveToDisk":
                    vm.action.value = vm.reportData.Path;
                    break;
            }
            $mdDialog.hide(vm.action);
        };

        function change(actionType) {
            vm.disableBtn = false;
            switch (actionType) {
                case "SaveToDisk":
                    vm.action.type = 1;
                    chooseDirectory();
                    break;
                case "Email":
                    vm.action.type = 2;
                    break;
                default:
                    vm.action.type = 0;
                    break;
            }          
        }

     
        vm.goBack = goBack;      
        vm.showBack = false;      

        function goBack(path) {
            path = path.substring(0, path.lastIndexOf("\\"));
            chooseDirectory(path);           
            showBackRefresh(path);
        }

        function showBackRefresh(path) {
            if (path) {
                vm.showBack = path.length > 0;
            } else {
                vm.showBack = false;
            }
          
        }
    }
})();