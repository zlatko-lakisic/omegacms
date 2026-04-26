(function () {
    'use strict';

    angular
        .module('app.reporting.report_definitions.list')
        .controller('ReportinDefinitionsListController', ['$rootScope', '$state', '$scope', '$mdSidenav', '$mdDialog', 'mdFeedbackService', 'reportDefinitions', ReportinDefinitionsListController]);

    /** @ngInject */
    function ReportinDefinitionsListController($rootScope, $state, $scope, $mdSidenav, $mdDialog, $mdFeedbackService, reportDefinitions) {
        var vm = this;

        // Controllers
        var reportDefinitionController = new mdBusinessLogic.dataAccess.controllers.reportDefinitionController();
        
        // Variables
        var initialLoad = true;
        vm.currentView = {
            Name: 'list',
            Label: 'Labels.ListView',
            Icon: 'icon-view-module'
        };
        vm.reportDefinitions = reportDefinitions.Items;
        vm.numberOfSearchResults = 0;
        vm.searchColumn = 'All';
        vm.searchGotResponse = true;
        vm.searchTerm = "";
        vm.currentPage = 0;
        vm.totalItems = reportDefinitions.TotalCount;
        vm.pageSize = 10;
        vm.sortingString = "";

        // Methods
        vm.select = select;
        vm.toggleView = toggleView;
        vm.search = search;
        vm.toggleSearch = toggleSearch;
        vm.openMenu = openMenu;
        vm.updatePager = updatePager;
        vm.sort = sort;
        vm.openReportingDefinition = openReportingDefinition;
        vm.toggleDetails = toggleDetails;
        vm.deleteReportingDefinition = deleteReportingDefinition;

        function updatePager(currentPage, pageSize, pagesBorder) {
            vm.currentPage = currentPage;
            vm.pageSize = pageSize;
            vm.pagesBorder = pagesBorder;
            getAllReportDefinitions();

        }

        function search(searchColumn) {
            if (searchColumn) {
                vm.searchColumn = searchColumn;
            }
            vm.searchGotResponse = false;
            getAllReportDefinitions();
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
            getAllReportDefinitions();
        }

        function openMenu($mdOpenMenu, ev) {
            ev.stopImmediatePropagation();
            $mdOpenMenu(ev);
        };

        function select(item) {
            vm.selected = item;
        }

        function openReportingDefinition(reportingDefinition) {
            $state.go('app.report_definitions_designer', { action: 'edit', id: reportingDefinition.Id });
        }

        function deleteReportingDefinition(id) {
            $mdDialog.show($mdDialog.confirm()
                                    .clickOutsideToClose(true)
                                    .title($rootScope.globals.resources.Titles.RemoveQuestion)
                                    .textContent($rootScope.globals.resources.Labels.RemoveAnswer)
                                    .ok($rootScope.globals.resources.Labels.Yes)
                                    .cancel($rootScope.globals.resources.Labels.No)).then(function () {
                                        reportDefinitionController.del(
                                            id,
                                            function (data) {
                                                getAllReportDefinitions();
                                                $mdFeedbackService.reportInfo('delete');
                                            },
                                            function (error) {
                                                $mdFeedbackService.reportError('delete', error);
                                            });
                                    });
            
        }

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

        function toggleDetails(item, event) {
            event.stopPropagation();
            select(item);
            toggleSidenav('details-sidenav');
        }

        function toggleSidenav(sidenavId) {
            $mdSidenav(sidenavId).toggle();
        }

        function getNumberOfReportDefinitions() {
            reportDefinitionController.getAllCount(
               {
                   searchTerm: encodeURI(vm.searchTerm),
                   searchColumn: encodeURI(vm.searchColumn)
               },
               function (data) {
                   $scope.$apply(function () {
                       vm.totalNumberOfReportDefinitions = data;
                       vm.searchGotResponse = true;
                       vm.totalItems = vm.totalNumberOfReportDefinitions;
                   });
               }, function (error) {
                   vm.searchGotResponse = true;
                   $mdFeedbackService.reportError('load', error);
               });
        }

        function getAllReportDefinitions() {
            reportDefinitionController.getAllWithPagination(
                {
                    sort: vm.sortingString,
                    searchTerm: encodeURI(vm.searchTerm),
                    searchColumn: encodeURI(vm.searchColumn),
                    pageIndex: vm.currentPage,
                    pageSize: vm.pageSize
                },
                function (data) {
                    $scope.$apply(function () {
                      vm.reportDefinitions = data.Items;
                      vm.totalCount = data.TotalCount;
                    });            
                }, function (error) {
                    $mdFeedbackService.reportError('load', error);
                });
        }

    }
})();
