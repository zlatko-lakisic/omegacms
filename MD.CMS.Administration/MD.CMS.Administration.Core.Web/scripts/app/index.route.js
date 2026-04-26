(function ()
{
    'use strict';

    angular
        .module('omega')
        .config(['$stateProvider', '$urlRouterProvider', '$locationProvider', 'mdSavedDataProvider', routeConfig]);

    /** @ngInject */
    function routeConfig($stateProvider, $urlRouterProvider, $locationProvider, mdSavedDataService)
    {
        $locationProvider.html5Mode(true);

        $urlRouterProvider.otherwise(function ($injector, $location) {
            var $state = $injector.get('$state');
            switch (mdBusinessLogic.globals.responseCode) {
                case 402:
                    $state.go('app.errors_error-402');
                    break;
                case 500:
                    $state.go('app.errors_error-500');
                    break;
                default:
                    if (window.location.href.indexOf('/login/') >= 0 && window.location.href.indexOf('/reset/') >= 0) {
                        var tokenArray = window.location.href.split('/');
                        var token = tokenArray[tokenArray.length - 1];
                        $state.go('app.login-reset', {
                            token: (token == 'token') ? '' : token
                        });
                    }
                    else {
                        $state.go('app.login');
                    }
                    break;
            }
        });

        /**
         * Layout Style Switcher
         *
         * This code is here for demonstration purposes.
         * If you don't need to switch between the layout
         * styles like in the demo, you can set one manually by
         * typing the template urls into the `State definitions`
         * area and remove this code
         */

        // Get active layout
        var layoutStyle = mdSavedDataService.getData('layoutStyle') || 'verticalNavigation';

        var layouts = {
            verticalNavigation  : {
                main: mdBusinessLogic.settings.appBase + 'scripts/app/core/layouts/vertical-navigation.html',
                toolbar: mdBusinessLogic.settings.appBase + 'scripts/app/toolbar/layouts/vertical-navigation/toolbar.html',
                navigation: mdBusinessLogic.settings.appBase + 'scripts/app/navigation/layouts/vertical-navigation/navigation.html'
            },
            horizontalNavigation: {
                main: mdBusinessLogic.settings.appBase + 'scripts/app/core/layouts/horizontal-navigation.html',
                toolbar: mdBusinessLogic.settings.appBase + 'scripts/app/toolbar/layouts/horizontal-navigation/toolbar.html',
                navigation: mdBusinessLogic.settings.appBase + 'scripts/app/navigation/layouts/horizontal-navigation/navigation.html'
            },
            contentOnly         : {
                main: mdBusinessLogic.settings.appBase + 'scripts/app/core/layouts/content-only.html',
                toolbar   : '',
                navigation: ''
            },
            contentWithToolbar  : {
                main: mdBusinessLogic.settings.appBase + 'scripts/app/core/layouts/content-with-toolbar.html',
                toolbar: mdBusinessLogic.settings.appBase + 'scripts/app/toolbar/layouts/content-with-toolbar/toolbar.html',
                navigation: ''
            }
        };
        // END - Layout Style Switcher

        // State definitions
        $stateProvider
            .state('app', {
                'abstract': true,
                views   : {
                    'main@'         : {
                        templateUrl: layouts[layoutStyle].main,
                        controller : 'MainController as vm'
                    },
                    'toolbar@app'   : {
                        templateUrl: layouts[layoutStyle].toolbar,
                        controller : 'ToolbarController as vm'
                    },
                    'navigation@app': {
                        templateUrl: layouts[layoutStyle].navigation,
                        controller : 'NavigationController as vm'
                    },
                    'quickPanel@app': {
                        templateUrl: 'scripts/app/quick-panel/quick-panel.html',
                        controller : 'QuickPanelController as vm'
                    },                   
                }
            });
    }

})();
