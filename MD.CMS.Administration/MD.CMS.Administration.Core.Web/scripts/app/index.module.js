(function ()
{
    'use strict';

    /**
     * Main module of the Fuse
     */
    angular
        .module('omega', [
            'app.core',
            'app.navigation',
            'app.toolbar',
            'app.quick-panel',
            'app.settings',
            'app.content',
            'app.taxonomy',
            'app.menu',         
            'app.mediacontent',
            'app.login',
            'app.dashboards',
            'app.reporting',
            'app.support',
            'app.services',
            'app.addons',
            'app.filters',
            'app.personal',
            'app.errors',
            'uiGmapgoogle-maps',
            'ui.tinymce',      
            'app.forgetPassword',
            'ngMessages',
            'ui.grid'
        ])
        .config(['msNavigationServiceProvider', 'mdSavedDataProvider', config]);

    /** @ngInject */
    function config(msNavigationServiceProvider, mdSavedDataProvider) {
        msNavigationServiceProvider.saveItem('main', {
            title: 'Menus.MainContent',
            group: true,
            weight: 2
        });

        mdSavedDataProvider.storeData('layoutStyle', "verticalNavigation", true);
    }
})();
