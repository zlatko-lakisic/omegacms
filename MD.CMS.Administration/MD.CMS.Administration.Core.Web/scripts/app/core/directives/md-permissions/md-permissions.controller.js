(function () {
    'use strict';

    angular
        .module('app.core')
        .controller('mdPermissionsController', ['$q', 'mdFeedbackService', '$scope', mdPermissionsController]);
    /** @ngInject */
    function mdPermissionsController($q, $mdFeedbackService, $scope) {
        var vm = this;

        //Private Attributes
        var entities = $scope.entities;


        //Public Attributes


        //Public Methods


        //Private Methods
    }
})();
