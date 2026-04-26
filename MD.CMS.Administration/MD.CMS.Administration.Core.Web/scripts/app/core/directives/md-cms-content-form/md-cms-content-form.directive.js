(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdCmsContentForm', ['$q', mdCmsContentForm]);

    function mdCmsContentForm($q) {
        return {
            restrict: 'EA',
            templateUrl: 'scripts/app/core/directives/md-cms-content-form/md-cms-content-form.template.html',
            scope: {
                mdContentFormName: "@?",
                mdContentTypeDefinition: "=?",
                mdFolder: "=?",
                mdFolderId: "=?",
                mdContent: "=?",
                mdOnError: "&?",
                mdOnPreSave: "&?",
                mdOnPostSave: "&?",
                mdSaveEvent: "&?",
                mdShowVersions: "=?",
                mdShowLanguages: "=?"
            },
            controller: 'mdCmsContentFormController as vm',
            link: function (scope, element, attrs) {
                if (scope.mdContentFormName === undefined || scope.mdContentFormName == null || scope.mdContentFormName == '') {
                    scope.mdContentFormName = 'contentForm';
                }
                if (scope.mdContent === undefined || scope.mdContent == null) {
                    scope.mdContent = new mdBusinessLogic.dataAccess.entities.content();
                }
                if (scope.mdFolder !== undefined && scope.mdFolder != null) {
                    scope.mdContent.FolderId = scope.mdFolder.Id;
                }
                if (scope.mdFolderId !== undefined && scope.mdFolderId != null) {
                    scope.mdContent.mdFolderId = scope.mdFolderId;
                }
            }
        };
    }
})();
