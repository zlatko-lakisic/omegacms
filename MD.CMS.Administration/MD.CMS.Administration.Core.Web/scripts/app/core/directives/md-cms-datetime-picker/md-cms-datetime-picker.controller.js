(function () {
    'use strict';

    angular
        .module('app.core')
        .controller('mdCmsDatetimePickerController', ['$scope', '$timeout', '$mdDialog', mdCmsDatetimePickerController]);
    /** @ngInject */
    function mdCmsDatetimePickerController($scope, $timeout, $mdDialog) {

        //Private Attributes
        var vm = this;
        var initialLoad;

        //Public Attributes
        vm.uniqueId = mdBusinessLogic.helpers.Guid.create().value;
        vm.mdModel = null;
        vm.ngPattern;
        vm.ngDisabled;
        vm.mdInputName;
        vm.mdFloatingLabel;
        vm.momentValue;
        vm.mdFormat;
        vm.mdStartView;
        vm.timezone;
        vm.selectedTimezone;
        vm.mdCompactMode;
        vm.mdCompactModeDisplay;
        vm.mdMinDate;
        vm.mdMaxDate;
        vm.mdSelectedRanges;
        vm.dateChanged;
        
        //Public Methods
        vm.onDateChange = onDateChange;
        vm.selectedTimezoneQuerySearch = selectedTimezoneQuerySearch;
        vm.openDialog = openDialog;
        vm.closeDialog = closeDialog;
        vm.isSelectable = isSelectable;
        vm.mdMinDate = undefined;
        vm.mdMaxDate = undefined;
        vm.mdSelectedRanges = [];

        //Private Methods
        function openDialog(ev) {
            $timeout(function () {
                $mdDialog.show({
                    contentElement: angular.element('#' + vm.uniqueId + ' .dialog_box'),
                    parent: angular.element(document.body),
                    targetEvent: ev,
                    multiple: true,
                    fullscreen: true,
                    clickOutsideToClose: true
                });
            });
        }
        function closeDialog() {
            $mdDialog.hide();
        }

        function momentToZimeZoneName(dateString) {
            return moment.tz.names().filter(function (zone) {
                var matches = dateString.match(/[+|-]\d\d:\d\d/g);
                var timeZoneName = moment.tz(zone).format('Z');
                return matches != null && matches.indexOf(timeZoneName) >= 0;
            })[0];
        }

        function selectedTimezoneQuerySearch(searchText) {
            var result = moment.tz.names().filter(function (zone) {
                return zone.indexOf(searchText) >= 0;
            });
            return result.length > 10 ? result.slice(0, 10) : result;
        }

        function onDateChange(newValue, oldValue) {
            if (!vm.dateChanged) {
                $scope.mdModel = mdBusinessLogic.helpers.entityHelper.parseDateAndTimezoneToString(newValue, vm.selectedTimezone);
                vm.mdCompactModeDisplay = newValue.format(vm.mdFormat);
                vm.dateChanged = true;
                $timeout(function () { vm.dateChanged = false; }, 1000);
            }
        }

        function isSelectable(date, type) {
            for (var i = 0; i < vm.mdSelectedRanges.length; i++) {
                if (moment(parseMdDateValue(vm.mdSelectedRanges[i][0])).toDate().getTime() <= date.toDate().getTime() &&
                    moment(parseMdDateValue(vm.mdSelectedRanges[i][1])).toDate().getTime() >= date.toDate().getTime()) {
                    return false;
                }
            }
            return true;
        }

        function parseMdDateValue(input) {
            var result = input;
            if (Object.prototype.toString.call(result) === '[object Date]') {
                result = moment(result).format().replace('Z', '');
            }
            return mdBusinessLogic.helpers.entityHelper.parseDateValue(result);
        }

        function init() {
            vm.selectedTimezone = moment.tz.guess();
            vm.timezone = false;
            vm.mdStartView = 'day';
            vm.momentValue = moment();
            initialLoad = false;
            vm.mdCompactMode = false;
            vm.dateChanged = false;

            $scope.$watch(function () { return $scope.mdTimezone }, function (mdTimezone) {
                if (mdTimezone !== undefined && mdTimezone != null) {
                    vm.timezone = mdTimezone;
                }
            });
            $scope.$watch(function () { return $scope.mdModel }, function (mdModel) {
                if (mdModel !== undefined && mdModel != null && !initialLoad) {
                    vm.mdModel = parseMdDateValue(mdModel);

                    vm.selectedTimezone = mdBusinessLogic.helpers.entityHelper.parseTimeZoneValue(mdModel);

                    vm.momentValue = moment(vm.mdModel);

                    if (vm.timezone) {
                        if (!moment.tz(vm.selectedTimezone)) {
                            vm.selectedTimezone = momentToZimeZoneName(mdModel);
                            if (!vm.selectedTimezone) {
                                vm.selectedTimezone = moment.tz.guess();
                            }
                        }
                    }
                    initialLoad = true;
                }
            });
            $scope.$watch(function () { return $scope.ngPattern }, function (ngPattern) {
                vm.ngPattern = ngPattern;
            });
            $scope.$watch(function () { return $scope.ngDisabled }, function (ngDisabled) {
                if (ngDisabled !== undefined && ngDisabled != null) {
                    vm.ngDisabled = ngDisabled;
                }
            });
            $scope.$watch(function () { return $scope.mdInputName }, function (mdInputName) {
                vm.mdInputName = mdInputName;
            });
            $scope.$watch(function () { return $scope.mdFloatingLabel }, function (mdFloatingLabel) {
                vm.mdFloatingLabel = mdFloatingLabel;
            });
            $scope.$watch(function () { return $scope.mdFormat }, function (mdFormat) {
                vm.mdFormat = mdFormat;
            });
            $scope.$watch(function () { return $scope.mdStartView }, function (mdStartView) {
                vm.mdStartView = mdStartView;
            });
            $scope.$watch(function () { return $scope.mdMinDate }, function (mdMinDate) {
                if (mdMinDate !== undefined && mdMinDate != null) {
                    vm.mdMinDate = moment(parseMdDateValue(mdMinDate));
                }
            });
            $scope.$watch(function () { return $scope.mdMaxDate }, function (mdMaxDate) {
                if (mdMaxDate !== undefined && mdMaxDate != null) {
                    vm.mdMaxDate = moment(parseMdDateValue(mdMaxDate));
                }
            });
            $scope.$watch(function () { return $scope.mdSelectedRanges }, function (mdSelectedRanges) {
                vm.mdSelectedRanges = mdSelectedRanges;
            });
            $scope.$watch(function () { return vm.selectedTimezone }, function (selectedTimezone) {
                if (vm.timezone) {
                    //onDateChange(moment($scope.mdModel.split(';')[0]), null);
                }
            });
            $scope.$watch(function () { return $scope.mdCompactMode }, function (mdCompactMode) {
                if (mdCompactMode !== undefined && mdCompactMode != null) {
                    vm.mdCompactMode = mdCompactMode;
                }
            });
            $timeout(function () {
                vm.mdCompactMode = angular.element('#' + vm.uniqueId).width() < 500;
            });
        } 

        init();
    }
})();
