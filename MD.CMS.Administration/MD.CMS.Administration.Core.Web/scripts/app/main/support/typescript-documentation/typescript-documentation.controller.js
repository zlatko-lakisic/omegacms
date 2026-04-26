(function () {
    'use strict';

    angular
        .module('app.support.typescript_documentation')
        .controller('TypescriptDocumentationController', ['$scope', '$mdSidenav', TypescriptDocumentationController]);


    /** @ngInject */
    function TypescriptDocumentationController($scope, $mdSidenav) {
        var vm = this;

        vm.iframeSrc = mdBusinessLogic.settings.appBase + 'scripts/documentation/typescript/modules/mdbusinesslogic.html';
    }
})();
