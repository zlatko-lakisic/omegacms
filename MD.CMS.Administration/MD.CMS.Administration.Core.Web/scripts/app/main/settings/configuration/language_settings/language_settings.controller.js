(function () {
    'use strict';

    angular
        .module('app.settings.configuration.language_settings')
        .controller('LanguageSettingsController', ['$state', '$rootScope', '$mdSidenav', '$mdDialog', 'allCultures', LanguageSettingsController]);

    /** @ngInject */
    function LanguageSettingsController($state, $rootScope, $mdSidenav, $mdDialog, allCultures) {

        var vm = this;
        var cultureController = new mdBusinessLogic.dataAccess.controllers.cultureController();
        vm.cultures = [];
        vm.selected = {};
        vm.index;
        vm.cultures = allCultures;
        vm.dataTableOptions = {
            dom: '<"top"f>rt<"bottom"<"left"<"length"l>><"right"<"info"i><"pagination"p>>>',
            pagingType: 'simple',
            pageLength: 20,
            lengthMenu: [10, 20, 50, 100],
            autoWidth: false,
            responsive: true
        };
      
        function showDialog(title, text, redirect) {
            var parentElement = angular.element(document.querySelector('.' + $state.current.bodyClass));
            $mdDialog.show(
                            $mdDialog.alert()                              
                              .parent(parentElement)
                              .clickOutsideToClose(true)
                              .parent(parentElement)
                              .title(title)
                              .textContent(text)
                              .ariaLabel(title)
                              .ok($rootScope.globals.resources.Labels.GotIt)
                          );
            if (redirect) {
                $state.go('app.content_list', { folderPath: vm.folder.FolderPath }, { reload: true });
            }
        }

       

        vm.languages = {
            en: {
                'title': 'English (United Kingdom)',
                'translation': 'TOOLBAR.ENGLISH',
                'code': 'en-GB',
                'flag': 'United Kingdom',
                'lcid': 2057
            }
        };

        // Methods
        //vm.sort = function (keyname) {
        //    vm.sortKey = keyname;   //set the sortKey to the param passed
        //    vm.reverse = !vm.reverse; //if true make it false and vice versa
        //}

        vm.Approve = function (culture, $index) {
            if (culture.IsApproved) {
                cultureController.save(culture,
                    function (data) {
                        showDialog(
                            $rootScope.globals.resources.Titles.ActionCompleted,
                            $rootScope.globals.resources.Labels.CultureSaved,
                            false);
                    },
                    function (error) {
                        showDialog(error.errorData.statusText, error.errorData.responseText, false)
                    }
                    );
            }
            else {
                cultureController.del(culture, function (data) {
                    showDialog(
                        $rootScope.globals.resources.Titles.ActionCompleted,
                        $rootScope.globals.resources.Labels.CultureRemoved,
                        false);
                 
                   
                }, function (error) {
                    showDialog(error.errorData.statusText, error.errorData.responseText, false)
                });
            }
            vm.selected = culture;
            vm.index = $index;
        };



        vm.select = select;

        function select(culture, $index) {
            vm.selected = culture;
            vm.index = $index;
        }

        vm.deleteItem = function () {
            var alertInfo = {
                title: '',
                content: '',
                redirect: false
            };

            alertInfo.title = $rootScope.globals.resources.Titles.ActionCompleted;
            alertInfo.content = $rootScope.globals.resources.Labels.CultureRemoved;
            alertInfo.redirect = true;
            $mdDialog.show(
              $mdDialog.alert()
                .clickOutsideToClose(true)
                .title(alertInfo.title)
                .textContent(alertInfo.content)
                .ariaLabel(alertInfo.title)
                .ok($rootScope.globals.resources.Labels.GotIt)
            );

            vm.cultures.splice(vm.index, 1);

        }
        //////////
    }
})();
