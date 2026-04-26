(function () {
    'use strict';

    angular
        .module('app.settings.configuration.meta-data-field', [
            'app.settings.configuration.meta-data-field-list',
            'app.settings.configuration.meta-data-field-form'
        ])
        .config(['msNavigationServiceProvider', config]);

    /** @ngInject */
    function config(msNavigationServiceProvider) {

        // Navigation
        //msNavigationServiceProvider.saveItem('meta-data-field', {
        //    title: 'Meta data fields',
        //    icon: 'icon-bookmark',
        //    state: 'app.meta-data-field.list',
        //    weight: 4
        //});
    }
})();