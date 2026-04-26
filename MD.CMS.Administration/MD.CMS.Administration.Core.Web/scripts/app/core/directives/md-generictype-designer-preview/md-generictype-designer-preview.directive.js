(function ()
{
    'use strict';

    angular
        .module('app.core')
        .directive('mdGenerictypeDesignerPreview', mdGenerictypeDesignerPreviewDirective);

    /** @ngInject */
    function mdGenerictypeDesignerPreviewDirective()
    {
        return {
            require : 'ngModel',
            restrict: 'E',
            templateUrl: 'scripts/app/core/directives/md-generictype-designer-preview/md-generictype-designer-preview.html',
            link: function ($scope, element, attrs, ngModel) {
                // Add class
                element.addClass('md-generictype-designer-preview');
                $scope.$watch(function () {
                    return ngModel.$modelValue;
                }, function (newValue) {
                    $scope.genericTypeObj = newValue;
                });
            }
        };
    }
})();
