(function () {
    'use strict';

    angular
        .module('app.support.javascript_documentation')
        .controller('JavascriptDocumentationController', ['$scope', '$mdSidenav', JavascriptDocumentationController]);


    /** @ngInject */
    function JavascriptDocumentationController($scope, $mdSidenav) {
        var vm = this;

        vm.iframeSrc = mdBusinessLogic.settings.appBase + 'scripts/documentation/javascript/index.html';
    }
})();