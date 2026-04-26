(function () {
    'use strict';

    angular
        .module('app.support.assembly_documentation')
        .controller('AssemblyDocumentationController', ['$scope', '$sce', 'documentationData', AssemblyDocumentationController]);


    /** @ngInject */
    function AssemblyDocumentationController($scope, $sce, documentationData) {
        var vm = this;

        vm.documentationData = documentationData.menus;
        vm.selectedDocumentation = vm.documentationData[0];

        vm.trustSrc = function (src) {
            return mdBusinessLogic.settings.appBase + $sce.trustAsResourceUrl(src);
        }
    }
})();