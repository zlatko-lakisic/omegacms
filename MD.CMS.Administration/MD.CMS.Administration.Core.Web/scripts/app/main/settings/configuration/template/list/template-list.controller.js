(function () {
    'use strict';

    angular
        .module('app.settings.configuration.template-list')
        .controller('TemplateListController', ['$state', '$rootScope', '$scope', '$mdSidenav', '$mdDialog', 'mdFeedbackService', 'templates', TemplateListController]);

    /** @ngInject */
    function TemplateListController($state, $rootScope, $scope, $mdSidenav, $mdDialog, $mdFeedbackService, templates) {
        var vm = this;

        // Controller
        var templateController = new mdBusinessLogic.dataAccess.controllers.templateController();
        var dialog = new mdBusinessLogic.helpers.dialog($mdDialog, $state);

        // Variables
        vm.templates = templates.Items;
        vm.selected;
        vm.index;
        vm.disablebtn = true;
        vm.numberOfSearchResults = 0;
        vm.currentPage = 0;
        vm.totalItems = templates.TotalCount;
        vm.pageSize = 10;
        vm.pagesBorder = 1;
        vm.sortingString = "";
        vm.searchGotResponse = true;
        vm.showSearch = false;
        vm.searchTerm = "";
        vm.searchColumn = "All";

        // Methods
        vm.select = select;
        vm.deleteTemplate = deleteTemplate;
        vm.toggleView = toggleView;
        vm.updatePager = updatePager;
        vm.toggleSearch = toggleSearch;
        vm.search = search;
        vm.cancelSearch = cancelSearch;
        vm.sort = sort;
        vm.openTemplate = openTemplate;
        vm.toggleTemplateDetails = toggleTemplateDetails;

        function updatePager(currentPage, pageSize, pagesBorder) {
            vm.currentPage = currentPage;
            vm.pageSize = pageSize;
            vm.pagesBorder = pagesBorder;
            getTemplates();
        }

        function search(searchTerm) {
            vm.searchColumn = 'All'
            vm.searchTerm = searchTerm;
            vm.searchGotResponse = false;
            getTemplates();
            return true;
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
            getTemplates();
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

        function toggleTemplateDetails(item, event) {
            event.stopPropagation();
            select(item)
            toggleSidenav('details-sidenav');
        }

        function toggleSidenav(sidenavId) {
            $mdSidenav(sidenavId).toggle();
        }

        function select(template, $index) {
            vm.selected = template;
            vm.index = $index;
            vm.disablebtn = false;
        }

        function openTemplate(template) {
            $state.go("app.template-form", {id: template.Id});
        }

        function deleteTemplate() {
            dialog.showConfirmDialog(
                $rootScope.globals.resources.Titles.RemoveQuestion,
                $rootScope.globals.resources.Labels.RemoveAnswer,
                $rootScope.globals.resources.Labels.Yes,
                $rootScope.globals.resources.Labels.No,
                function () {
                    templateController.del(vm.selected.Id,
              function (data) {
                  $mdFeedbackService.reportInfo('delete');
                  $state.go($state.current, {}, { reload: true });
              }, function (error) {
                  $mdFeedbackService.reportError('delete', error);
              })
            },
            function () {
            });
        }

        function getNumberOfTemplates() {
            templateController.getAllCount(
                {
                    searchTerm: decodeURI(vm.searchTerm),
                    searchColumn: decodeURI(vm.searchColumn)
                },
                function (data) {
                    $scope.$apply(function () {
                        vm.templatesCount = data;
                        vm.searchGotResponse = true;
                        vm.totalItems = vm.templatesCount;
                    })
                }, function (error) {
                    $mdFeedbackService.reportError('load', error);
                });
        }

        function getTemplates() {
            templateController.getAllWithPagination(
                {
                    sort: vm.sortingString,
                    pageIndex: vm.currentPage,
                    pageSize: vm.pageSize,
                    searchTerm: decodeURI(vm.searchTerm),
                    searchColumn: decodeURI(vm.searchColumn)
                },
                function (data) {
                    $scope.$apply(function () {
                      vm.templates = data.Items;
                      vm.totalItems = data.TotalCount;
                    })
                }, function (error) {
                    $mdFeedbackService.reportError('load', error);
                });
        }

    }
})();
