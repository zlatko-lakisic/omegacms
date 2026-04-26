(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsFields', [mdCmsFields]);
    /** @ngInject */
    function mdCmsFields() {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-cms-fields/md-cms-fields.template.html',
            transclude: true,
            scope: {
                mdParentId: "=?",
                mdFields: "=",
                mdFolderPath: "=",
                mdFormName: "@",
                mdTextAreaOptions: "=?",
                registerUploadEvents: "&",
                onSave: "&",
                mdDisabled: "=",
                reinitEventName: '@?',
                mdNestedDepth: '@?',
                mdCurrentDepth: '@?',
                layout: '@?'
            },
            controller: 'mdCmsFieldsController as vm'
        };
    }
})();
