(function () {
    'use strict';

    angular
        .module('app.reporting.report_scheduler.form')
        .controller('ReportinSchedulerFormController', ['$mdDialog', '$rootScope', '$state', '$scope', '$mdMedia', '$filter', 'mdFeedbackService', 'reportScheduler', ReportinSchedulerFormController]);

    /** @ngInject */
    function ReportinSchedulerFormController($mdDialog, $rootScope, $state, $scope, $mdMedia, $filter, $mdFeedbackService, reportScheduler) {
        var vm = this;
        //helpers
        var reportSchedulerController = new mdBusinessLogic.dataAccess.controllers.reportSchedulerController();
        var reportDefinitionController = new mdBusinessLogic.dataAccess.controllers.reportDefinitionController();
        //variables
        vm.reportDefinitions = {};
        vm.reportScheduler = reportScheduler;
        vm.Start = new Date(vm.reportScheduler.Start || new Date());
        vm.End = new Date(vm.reportScheduler.End || new Date().setDate(new Date().getDate() + 1));
        vm.isNew = $state.params.action != 'edit';
        var addOrEdit = $state.params.action;
        vm.reportSchedulerId = $state.params.id;
        vm.formTitle = addOrEdit === 'add' ? $rootScope.globals.resources.Titles.AddReportScheduler : $rootScope.globals.resources.Titles.EditReportScheduler;
        var dialogInfoText = addOrEdit === 'add' ? $rootScope.globals.resources.Labels.AddedText : $rootScope.globals.resources.Labels.EditedText;

        //methods
        vm.openCreateActionModal = openCreateActionModal;
        vm.deleteAction = deleteAction;
        vm.sendForm = sendForm;
        vm.validateActionType = validateActionType;
        vm.validateIntervalToTimeSpan = validateIntervalToTimeSpan;
        vm.checkMinDate = checkMinDate;


        //DATETIME VALIDATIONS

        vm.minDate = new Date();
        vm.maxDate = new Date(new Date().setFullYear(new Date().getFullYear() + 100))
        vm.isSmallerThanCurrent = false;

        function checkMinDate() {
            if (vm.isNew) {
                if (vm.Start.setHours(0, 0, 0, 0) < new Date().setHours(0, 0, 0, 0)) {
                    vm.isSmallerThanCurrent = true;
                }
                else {
                    vm.isSmallerThanCurrent = false;
                }
            }
            else {
                if (vm.Start.setHours(0, 0, 0, 0) != new Date(vm.reportScheduler.Start).setHours(0, 0, 0, 0)) {
                    if (vm.Start.setHours(0, 0, 0, 0) < new Date().setHours(0, 0, 0, 0)) {
                        vm.isSmallerThanCurrent = true;
                    }
                    else {
                        vm.isSmallerThanCurrent = false;
                    }
                }
                else {
                    vm.isSmallerThanCurrent = false;
                }
            }
        }
        //END DATETIME VALIDATIONS


        //get all report definition for drop down list 
        reportDefinitionController.getAll({sort: "Name ASC"}, function (data) {
            $scope.$apply(function () {
                vm.reportDefinitions = data;
            });
        }, function (error) {
                $mdFeedbackService.reportError("load", error)
        });

        function validateIntervalToTimeSpan() {
            //This format Web Api accept for TimeSpan
            vm.reportScheduler.Interval = vm.reportScheduler.IsRecurring ? vm.reportScheduler.Interval.days + "." + vm.reportScheduler.Interval.hours + ":" + vm.reportScheduler.Interval.minutes + ":" + vm.reportScheduler.Interval.secunds : null;
        }
        function validateActionType() {
            for (var i in vm.reportScheduler.Actions) {
                switch (vm.reportScheduler.Actions[i].ActionType) {
                    case "E-mail":
                        vm.reportScheduler.Actions[i].ActionType = 2
                        break;
                    case "Save to disk":
                        vm.reportScheduler.Actions[i].ActionType = 1
                        break;
                    default:
                        break;
                }
            }
        }

        vm.convertDateToString = convertDateToString;
        function convertDateToString(Date) {
            return $filter('date')(Date, "yyyy-MM-dd HH:mm:ss ");
        }

        function sendForm(ev) {
            validateActionType();
            validateIntervalToTimeSpan();
            //This format Web Api accept
            vm.reportScheduler.Start = convertDateToString(vm.Start);
            vm.reportScheduler.End = convertDateToString(vm.End);
            reportSchedulerController.save(vm.reportScheduler, function (data) {
                $mdFeedbackService.reportInfo("save");
                $state.go('app.report_scheduler_list');
            }, function (error) {
                $mdFeedbackService.reportError("save", error);
            });
        }
        function deleteAction(obj) {

            for (var i in vm.reportScheduler.Actions) {
                if (obj === vm.reportScheduler.Actions[i]) {
                    vm.reportScheduler.Actions.splice(i, 1);
                }
            }
        }
        vm.validateInterval = validateInterval;
        function validateInterval() {
            vm.reportScheduler.Interval = vm.reportScheduler.Interval.split(/[ :(.\)]+/).map(Number);
            switch (vm.reportScheduler.Interval.length) {
                case 1:
                    vm.reportScheduler.Interval = {
                        days: 0,
                        hours: 0,
                        minutes: 0,
                        secunds: vm.reportScheduler.Interval[0]
                    }
                    break;
                case 2:
                    vm.reportScheduler.Interval = {
                        days: 0,
                        hours: 0,
                        minutes: vm.reportScheduler.Interval[0],
                        secunds: vm.reportScheduler.Interval[1]
                    }
                    break;
                case 3:
                    vm.reportScheduler.Interval = {
                        days: 0,
                        hours: vm.reportScheduler.Interval[0],
                        minutes: vm.reportScheduler.Interval[1],
                        secunds: vm.reportScheduler.Interval[2]
                    }
                    break;
                case 4:
                    vm.reportScheduler.Interval = {
                        days: vm.reportScheduler.Interval[0],
                        hours: vm.reportScheduler.Interval[1],
                        minutes: vm.reportScheduler.Interval[2],
                        secunds: vm.reportScheduler.Interval[3]
                    }
                    break;

                default:
                    vm.reportScheduler.Interval = {
                        days: 0,
                        hours: 0,
                        minutes: 0,
                        secunds: 0
                    }
                    break;
            }
        }
        vm.validateDateFromString = validateDateFromString;
        function validateDateFromString() {
            //Check if it is null ,when is null in database we got this 
            if (vm.reportScheduler.End === "0001-01-01T00:00:00") {
                vm.End = null;
            }
            else {
                vm.End = new Date(vm.reportScheduler.End);
            }
            vm.Start = new Date(vm.reportScheduler.Start);
        }
        //executing
        if (!vm.isNew) {
            validateInterval();
            validateDateFromString();
        }


        function openCreateActionModal(ev) {
            var useFullScreen = ($mdMedia('sm') || $mdMedia('xs')) && $scope.customFullscreen;
            $mdDialog.show({
                controller: 'ActionFormController',
                templateUrl: 'scripts/app/main/reporting/report-scheduler/form/actions/action-form.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose: true,
                fullscreen: useFullScreen
            })
            .then(function (action) {
                //todo: check this code - every property value! - is it logical, is it compatible with web api
                //todo: make user can choose name and isActive properties - currently hardcoded
                //todo: enum converter
                var actionToAdd = {};
                actionToAdd.Id = 0;
                actionToAdd.SchedulerId = vm.reportScheduler.Id;
                actionToAdd.Name = action.name || "No name";
                actionToAdd.AuthorId = vm.reportScheduler.Id;
                if (vm.isNew) {
                    actionToAdd.DateCreated = $filter('date')(new Date(), "yyyy-MM-dd HH:mm:ss ");
                    actionToAdd.DateEdited = $filter('date')(new Date(), "yyyy-MM-dd HH:mm:ss ");
                }
                else {
                    actionToAdd.DateCreated = vm.reportScheduler.DateCreated;
                    actionToAdd.DateEdited = vm.reportScheduler.DateEdited;
                }

                actionToAdd.ActionType = action.type;
                actionToAdd.Options = action.value;
                actionToAdd.IsActive = action.isActive;
                actionToAdd = new mdBusinessLogic.dataAccess.entities.reportSchedulerAction(actionToAdd);
                vm.reportScheduler.Actions.push(actionToAdd);
            }, function () {
                $scope.status = $rootScope.globals.resources.Labels.DialogCanceled;
            });
            $scope.$watch(function () {
                return $mdMedia('xs') || $mdMedia('sm');
            }, function (wantsFullScreen) {
                $scope.customFullscreen = (wantsFullScreen === true);
            });
        };
    }
})();
