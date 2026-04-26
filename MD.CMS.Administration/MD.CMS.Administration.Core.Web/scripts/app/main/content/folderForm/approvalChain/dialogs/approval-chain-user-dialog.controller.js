(function () {
    'use strict';

    angular
        .module('app.folder.forms')
        .controller('approvalChainUserFormController', ['$scope', '$mdDialog', 'step', 'users', 'otherSteps', approvalChainUserFormController]);

    /** @ngInject */
    function approvalChainUserFormController($scope, $mdDialog, step, users, otherSteps) {
        $scope.step = step;
        $scope.Users = [];
        $scope.nextUser = $scope.step.UserIds.length;
        $scope.searchText = '';
        $scope.querySearch = querySearch;
        $scope.selectedItemChange = selectedItemChange;
        $scope.beforeSteps = [];
        if (step.Id) //if step exist actions must too
        {
            $scope.step.Actions[0].Id *= -1; // if id is negativ number server will update existing step actions, if id is not set server will create new record
            $scope.step.Actions[1].Id *= -1;
        }
        //Get all users from system
        for (var userId in $scope.step.UserIds) {
            for (var user in users) {
                if (users[user].Id === $scope.step.UserIds[userId]) {
                    $scope.Users.push(users[user]);
                    break;
                }
            }
        }

        //Get steps before current one to enable user to redirect to some step when content on this one is rejected
        for (var i = 0; i < otherSteps.length; i++) {
            if (step.Order > otherSteps[i].Order) {
                $scope.beforeSteps.push(otherSteps[i]);
            }
        }

        $scope.hide = function () {
            $mdDialog.hide();
        };
        $scope.cancel = function () {
            $mdDialog.cancel();
        };
        $scope.answer = function () {
            $mdDialog.hide($scope.Users);
        };
        $scope.isValid = function () {
            if ($scope.Users.length < 1)
            {
                return false;
            }

            if ($scope.step.Actions[1].Action === '1' && 
                (typeof $scope.step.Actions[1].RedirectTo === 'undefined' || $scope.step.Actions[1].RedirectTo === ''
                || $scope.step.Actions[1].RedirectTo === null))
            {
                return false;
            }

            return true;
        }

        function querySearch(query) {
            var results = query ? users.filter(createQueryFilter(query)) : users, deferred;
            return results;
        }

        function selectedItemChange(item) {
            $scope.user = item;
        }
        /**
         * Create filter function for a query string
         */
        function createQueryFilter(query) {
            return function fn(user) {
                return user.Username.toLowerCase().indexOf(query.toLowerCase()) >= 0;
            }
        }
    }
}());
