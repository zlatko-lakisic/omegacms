(function () {
    'use strict';

    angular
        .module('app.core')
        .factory('mdContentService', ['$q', mdContentService]);

    /** @ngInject */
    function mdContentService($q) {

        var contentController = new mdBusinessLogic.dataAccess.controllers.contentController();

        function parseExpression(contentRequest, expression, defaultValue) {
            if (defaultValue === undefined) {
                defaultValue = '';
            }
            return $q(function (resolve, reject) {
                contentController.get(contentRequest, function (data) {
                        var result = expression;
                        try {
                            var regex = /(?<=\[)(.*?)(?=\])/g;
                            var matches = expression.match(regex);
                            for (var i = 0; i < matches.length; i++) {
                                switch (matches[i]) {
                                    case 'Title':
                                    case 'Id':
                                    case 'LCID':
                                    case 'DateCreated':
                                    case 'Html':
                                        if (data[0][matches[i]] === undefined || data[0][matches[i]] == null) {
                                            throw 'Expression ' + expression + ' could not be parsed!';
                                        }
                                        result = result.replace('[' + matches[i] + ']', data[0][matches[i]]);
                                        continue;
                                }
                                if (data[0].ContentType.getFieldValue(matches[i]) === undefined ||  data[0].ContentType.getFieldValue(matches[i]) == null) {
                                    throw 'Expression ' + expression + ' could not be parsed!';
                                }
                                result = result.replace('[' + matches[i] + ']', data[0].ContentType.getFieldValue(matches[i]));
                            }
                        } catch (e) {
                            console.warn(e);
                            result = defaultValue;
                        } finally {
                            resolve(result);
                        }
                }, function (error) {
                    reject(error);
                });
            });
        }

        return {
            parse: parseExpression
        };

    }
}());
