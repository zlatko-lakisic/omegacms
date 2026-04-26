(function () {
    'use strict';

    angular
        .module('app.reporting.report_scheduler.list')
        .controller('ReportinSchedulerListController', ['$state', '$rootScope', '$scope', '$mdSidenav', '$mdDialog', 'mdFeedbackService', 'reportSchedulerData', 'reportDefinitions', ReportinSchedulerListController]);


    /** @ngInject */
    function ReportinSchedulerListController($state, $rootScope, $scope, $mdSidenav, $mdDialog, $mdFeedbackService, reportSchedulerData, reportDefinitions) {
        var vm = this;
     
        // Controllers
        var reportSchedulerController = new mdBusinessLogic.dataAccess.controllers.reportSchedulerController();
        var reportDefinitionController = new mdBusinessLogic.dataAccess.controllers.reportDefinitionController();

        // Variables
        vm.currentView = {
            Name: 'list',
            Label: 'Labels.ListView',
            Icon: 'icon-view-module'
        };
        vm.reportSchedulerData = reportSchedulerData.Items;
        vm.reportDefinitions = reportDefinitions;
        vm.numberOfSearchResults = 0;
        vm.searchColumn = 'All';
        vm.searchGotResponse = true;
        vm.searchTerm = "";
        vm.currentPage = 0;
        vm.totalItems = reportSchedulerData.TotalCount;
        vm.pageSize = 10;
        vm.sortingString = "";
        vm.actionTypes = [];
      
        // Methods
        vm.selectReportScheduler = selectReportScheduler;
        vm.select = select;
        vm.validateAddNew = validateAddNew;
        vm.toggleView = toggleView;
        vm.search = search;
        vm.toggleSearch = toggleSearch;
        vm.openMenu = openMenu;
        vm.updatePager = updatePager;
        vm.sort = sort;
        vm.openReportScheduler = openReportScheduler;
        vm.toggleDetails = toggleDetails;
        vm.deleteReportingScheduler = deleteReportingScheduler;

        function updatePager(currentPage, pageSize, pagesBorder) {
            vm.currentPage = currentPage;
            vm.pageSize = pageSize;
            vm.pagesBorder = pagesBorder;
            getAllSchedulers();

        }

        function search(searchColumn) {
            if (searchColumn) {
                vm.searchColumn = searchColumn;
            }
            vm.searchGotResponse = false;
            getAllSchedulers();
        }

        function toggleSearch() {
            vm.isSearchVisible = !vm.isSearchVisible;
            if (!vm.isSearchVisible) {
                if (!vm.searchTerm.length == 0) {
                    vm.cancelSearch();
                }
            }
        }

        function cancelSearch() {
            vm.searchGotResponse = true;
            vm.searchTerm = "";
            vm.search();
        }

        function sort(sortingString) {
            vm.sortingString = sortingString;
            getAllSchedulers();
        }

        function openMenu($mdOpenMenu, ev) {
            ev.stopImmediatePropagation();
            $mdOpenMenu(ev);
        };

        function toggleView(view) {
            if ((vm.currentView.Name == 'grid' && view != 'grid') || view == 'list') {
                vm.currentView = {
                    Name: 'list',
                    Label: 'Labels.ListView',
                    Icon: 'icon-view-module'
                };
            } else if (view == 'grid' || view == undefined) {
                vm.currentView = {
                    Name: 'grid',
                    Label: 'Labels.GridView',
                    Icon: 'icon-view-headline'
                };
            }
        }

        function validateAddNew() {
            if (vm.reportDefinitions.length === 0) {
                $mdDialog.show(
                       $mdDialog.alert()
                         .parent(angular.element(document.querySelector('#popupContainer')))
                         .clickOutsideToClose(true)
                         .title($rootScope.globals.resources.Titles.Warning)
                         .textContent($rootScope.globals.resources.Labels.ReportDefinitionsMissing)
                         .ok($rootScope.globals.resources.Labels.GotIt)
                    );
            } else {
                $state.go('app.report_scheduler_form', { action: 'add' });
            }
        }

        function getNumberOfSchedulers() {
            reportSchedulerController.getAllCount(
                {
                    searchTerm: encodeURI(vm.searchTerm),
                    searchColumn: encodeURI(vm.searchColumn)
                }, function (data) {
                    $scope.$apply(function () {
                        vm.totalNumberOfReportSchedulers = data;
                        vm.totalItems = vm.totalNumberOfReportSchedulers;
                    });
                }, function (error) {
                    $mdFeedbackService.reportError('load', error);
                });
        }

        function getAllSchedulers(){
            reportSchedulerController.getAllWithPagination(
                {
                    sort: vm.sortingString,
                    searchTerm: encodeURI(vm.searchTerm),
                    searchColumn: encodeURI(vm.searchColumn),
                    pageIndex: vm.currentPage,
                    pageSize: vm.pageSize
                },
                function (data) {
                    $scope.$apply(function () {
                      vm.reportSchedulerData = data.Items;
                      vm.totalItems = data.TotalCount
                      vm.searchGotResponse = true;
                      for (var i in vm.reportSchedulerData) {
                          if (vm.reportSchedulerData[i].DateEdited === "0001-01-01T00:00:00") {
                              vm.reportSchedulerData[i].DateEdited = null;
                          }
                      }
                        });
                }, function (error) {
                    vm.searchGotResponse = true;
                    $mdFeedbackService.reportError('load', error);
                });
        }

        function deleteReportingScheduler(id) {
            $mdDialog.show($mdDialog.confirm()
                                    .clickOutsideToClose(true)
                                    .title($rootScope.globals.resources.Titles.RemoveQuestion)
                                    .textContent($rootScope.globals.resources.Labels.RemoveAnswer)
                                    .ok($rootScope.globals.resources.Labels.Yes)
                                    .cancel($rootScope.globals.resources.Labels.No)).then(function () {
                                        reportSchedulerController.del(
                                            id,
                                            function (data) {
                                                getAllSchedulers();
                                                $mdFeedbackService.reportInfo('delete');
                                            },
                                            function (error) {
                                                $mdFeedbackService.reportError('delete', error);
                                            });
                                    });
        }

        function selectReportScheduler(item) {
            vm.selectedScheduler = item;
            select(item);
        }
        
        function select(item) {
            vm.selected = item;
        }

        function openReportScheduler(reportSchduler) {
            $state.go("app.report_scheduler_form", { action: 'edit', id: reportSchduler.Id });
        }
        
        function toggleDetails(item, event) {
            event.stopPropagation();
            select(item);
            toggleSidenav('details-sidenav');
        }

        function toggleSidenav(sidenavId) {
            $mdSidenav(sidenavId).toggle();
        }
    }
})();


