(function () {
    'use strict';

    angular
        .module('app.core')
        .directive('mdGenerictypeDesignerElement', ['$mdSidenav', '$mdDialog', '$timeout', '$http', 'FriendlyNameMakerService', 'mdFeedbackService', mdGenerictypeDesignerElement]);

    /** @ngInject */
    function mdGenerictypeDesignerElement($mdSidenav, $mdDialog, $timeout, $http, FriendlyNameMakerService, mdFeedbackService) {
        return {
            restrict: 'E',
            scope: {
                uniqueId: '@',
                mdField: '=',
                mdIndex: '=',
                mdStartIndex: '@?',
                mdEditMode: '=',
                mdShowEditDialog: '&',
                mdOnTileEvent: '&?',
                mdOnRegisterEditEvent: '&?',
                mdFields: '=?',
            },
            templateUrl: 'scripts/app/core/directives/md-generictype-designer/element/md-generictype-designer-element.html',
            controller: 'mdGenerictypeDesignerElementController as vm',
            link: function (scope, element, attrs) {
                if (scope.uniqueId === undefined) {
                    scope.uniqueId = mdBusinessLogic.helpers.Guid.create().value;
                }
                angular.element(element).attr('id', scope.uniqueId);

                if (scope.mdFields === undefined) {
                    scope.mdFields = [];
                }

                if (scope.mdStartIndex === undefined) {
                    scope.mdStartIndex = 0;
                }

                scope.mdBaseStartIndex = parseInt(scope.mdStartIndex);

                // Add class
                element.addClass('md-generictype-designer-element flex-100');
            }
        };
    }
})();
