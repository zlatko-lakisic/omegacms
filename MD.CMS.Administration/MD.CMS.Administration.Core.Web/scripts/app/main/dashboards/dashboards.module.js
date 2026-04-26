(function ()
{
    'use strict';

    angular
        .module('app.dashboards', []).config(['msNavigationServiceProvider', config]);

    /** @ngInject */
    function config(msNavigationServiceProvider)
    {
        msNavigationServiceProvider.saveItem('dashboards', {
            title: 'Menus.MainDashboards',
            group : true,
            weight: 1
        });
    }

})();
