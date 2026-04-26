(function () {
    'use strict';

    angular
        .module('app.core')
        .factory('mdUserService', ['$q', mdUserService]);

    /** @ngInject */
    function mdUserService($q) {

        var userController = new mdBusinessLogic.dataAccess.controllers.userController();

        function parseExpression(userId, expression, defaultValue) {
            if (defaultValue === undefined) {
                defaultValue = '';
            }
            return $q(function (resolve, reject) {
                resolveUser(userId).then(function (data) {
                    var result = expression;
                    try {
                        var regex = /(?<=\[)(.*?)(?=\])/g;
                        var matches = expression.match(regex);
                        for (var i = 0; i < matches.length; i++) {
                            for (var p = 0; p < data.ProfileTypes.length; p++) {
                                switch (matches[i]) {
                                    case 'Username':
                                    case 'Id':
                                        result = result.replace('[' + matches[i] + ']', data[matches[i]]);
                                        continue;
                                }
                                result = result.replace('[' + matches[i] + ']', data.ProfileTypes[p].getFieldValue(matches[i]));
                            }
                        }
                    } catch (e) {
                        console.warn(e);
                        result = defaultValue;
                    } finally {
                        resolve(result);
                    }
                });
            });
        }

        function resolveUser(userId) {
            return $q(function (resolve, reject) {
                userController.getById(userId, function (data) {
                    resolve(data);
                }, function (error) {
                    resolve(null);
                });
            });
        }

        return {
            parse: parseExpression,
            resolve: resolveUser
        };

    }
}());
