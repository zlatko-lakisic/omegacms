(function ()
{
    'use strict';

    angular
        .module('app.core',
            [
                'ngAnimate',
                'ngCookies',
                'ngMessages',
                'ngResource',
                'ngSanitize',
                'ngMaterial',
                'pascalprecht.translate',
                'ui.router',
                'angular-loading-bar',
                'nvd3',
                'datatables',
                'app.searchResults.module',
                'ngMaterialDatePicker',
                'md.time.picker',
                'jsonFormatter',
                'moment-picker',
                'sticky',
                'omegaCmsLfNgMdFileInput',
                'ngStorage'
            ]);
})();
