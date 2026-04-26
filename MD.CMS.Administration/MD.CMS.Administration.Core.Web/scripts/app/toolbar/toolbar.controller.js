(function () {
    'use strict';

    angular
        .module('app.toolbar')
        .controller('ToolbarController', ['$rootScope', '$mdSidenav', '$translate', '$location', 'mdSavedDataService', '$scope', '$state', 'mdFeedbackService', ToolbarController]);

    /** @ngInject */
    function ToolbarController($rootScope, $mdSidenav, $translate, $location, mdSavedDataService, $scope, $state, $mdFeedbackService) {
        var cultureController = new mdBusinessLogic.dataAccess.controllers.cultureController();
        var messagesController = new mdBusinessLogic.dataAccess.controllers.messageController();
        var userController = new mdBusinessLogic.dataAccess.controllers.userController();
        var vm = this;
        vm.selectedLanguage;
        // Data
        $rootScope.global = {
            search: ''
        };

        vm.loggedOnUser = mdBusinessLogic.globals.loggedOnUser;
        
        //mdBusinessLogic.globals is outside of angular context and will sometimes be updated after this controller finishes
        //causeing vm.loggedOnUser to stay null - below funciton should fix this by making angular check for update on
        //global varibles by runing function - by watching only variable it can still cause cacheing and not updating vm.loggedOnUser
        $scope.$watch(function () {
            return mdBusinessLogic.globals.loggedOnUser;
        }, function () {
            vm.loggedOnUser = mdBusinessLogic.globals.loggedOnUser;
        });

        vm.bodyEl = angular.element('body');
        vm.userStatusOptions = [
            {
                'title': 'Online',
                'icon': 'icon-checkbox-marked-circle',
                'color': '#4CAF50'
            },
            {
                'title': 'Away',
                'icon': 'icon-clock',
                'color': '#FFC107'
            },
            {
                'title': 'Do not Disturb',
                'icon': 'icon-minus-circle',
                'color': '#F44336'
            },
            {
                'title': 'Invisible',
                'icon': 'icon-checkbox-blank-circle-outline',
                'color': '#BDBDBD'
            },
            {
                'title': 'Offline',
                'icon': 'icon-checkbox-blank-circle-outline',
                'color': '#616161'
            }
        ];

        vm.languages = {
            "en-GB": {
                'title': 'English (United Kingdom)',
                'translation': 'TOOLBAR.ENGLISH',
                'code': 'en-GB',
                'flag': 'United Kingdom',
                'lcid': 2057
            }
        };


        // Methods
        vm.toggleSidenav = toggleSidenav;
        vm.logout = logout;
        vm.changeLanguage = changeLanguage;
        vm.changeUiLanguage = changeUiLanguage;
        vm.setUserStatus = setUserStatus;
        vm.toggleHorizontalMobileMenu = toggleHorizontalMobileMenu;
        //vm.change = vm.change;

        this.open = function openMenu() {
            vm.languages = {
            };
            cultureController.getApproved(function (data) {
                for (var i = 0; i < data.length; i++) {
                    var code = data[i].Code;
                    vm.languages[code] = {};
                    vm.languages[code].code = code;
                    vm.languages[code].flag = data[i].IsoCode;
                    vm.languages[code].title = data[i].Name;
                    vm.languages[code].translation = 'TOOLBAR.' + data[i].Name.toUpperCase();
                    vm.languages[code].LCID = data[i].LCID;
                }
                //In case $coockieStore language is not approved set one from the list above
                vm.selectedLanguage = vm.languages[mdSavedDataService.getData('globals.selectedLanguage') || "en-GB"] || vm.languages["en-GB"];
                if (!vm.selectedLanguage.LCID) {
                    vm.selectedLanguage.LCID = 2057;
                }
                mdBusinessLogic.settings.lcid = vm.selectedLanguage.LCID;
            }, function (error) {
                $mdFeedbackService.reportError("load", error);
            });
        };

        // Expose a open function to the child scope for html to use
        $scope.$change = this.open;

        init();

        /**
         * Initialize
         */
        function init() {
            // Select the first status as a default
            cultureController.selectCulture(function (data) {
            }, function (error) {
                $mdFeedbackService.reportError("load", error);
            });
            vm.userStatus = vm.userStatusOptions[0];

            vm.selectedLanguage = vm.languages[(mdSavedDataService.getData('globals.selectedLanguage') || "en-GB")] || vm.languages["en-GB"];
            if (mdSavedDataService.getData('settings.lcid') != 0) {
                mdBusinessLogic.settings.lcid = mdSavedDataService.getData('settings.lcid');
            }

            $scope.$change();
        }


        /**
         * Toggle sidenav
         *
         * @param sidenavId
         */
        function toggleSidenav(sidenavId) {
            $mdSidenav(sidenavId).toggle();
        }

        /**
         * Sets User Status
         * @param status
         */
        function setUserStatus(status) {
            vm.userStatus = status;
        }

        /**
         * Logout Function
         */
        function logout() {
            $state.go("app.login", {
                sessionTimeout: false,
                returnUrl: encodeURI($location.path())
            });
        }

        /**
         * Change Language
         */
        function changeLanguage(lang) {
            vm.selectedLanguage = lang;

            mdBusinessLogic.globals.selectedLanguage = lang.code;
            mdBusinessLogic.settings.lcid = lang.LCID;
            mdSavedDataService.storeData('globals.selectedLanguage', mdBusinessLogic.globals.selectedLanguage.toString(), true);
            mdSavedDataService.storeData('settings.lcid', mdBusinessLogic.settings.lcid.toString(), true);
            if (lang.code !== 'en') {
                location.reload();
                return;
            }

            // Change the language
            $translate.use(lang.code);
            location.reload();
            return;
        }

        /*
         * Change UI Language
         */
        function changeUiLanguage(language) {
            $rootScope.globals.selectedLanguage = language;
        }


        /**
         * Toggle horizontal mobile menu
         */
        function toggleHorizontalMobileMenu() {
            vm.bodyEl.toggleClass('ms-navigation-horizontal-mobile-menu-active');
        }

        $scope.$watch('pluginJobsInfo', function () {
            vm.currentPluginStatus = $rootScope.pluginJobsInfo.currentPluginStatus;
            vm.pluginJobs = $rootScope.pluginJobsInfo.pluginJobs;
        });

        /* Get all plugin jobs */
        vm.pluginJobs = [];
        vm.currentPluginStatus = 'idle';
        vm.pluginStatuses = {
            working: {
                'title': 'Working',
                'icon': 'icon-refresh',
                'color': '#4CAF50'
            },
            idle: {
                'title': 'All Plugins Idle',
                'icon': 'icon-close',
                'color': '#BDBDBD'
            }
        };

        vm.unreadMessagesNumber = 0;
        var allMessages = [];
        var oldMessages = [];
        var newMessages = [];

        function findDifference(large, small) {            
            var lengthDiff = large.length - small.length;
            var difference = large.splice(large.length - lengthDiff, lengthDiff);
            return difference;
        }
    }
})();
