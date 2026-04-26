/**
 * This module defines a custom jsDoc tag.
 * It allows you to document header parameters of a directive.
 */

'use strict';

exports.name = 'directive';
exports.options = {
    canHaveType: true,
    canHaveName: true,
    onTagged: function (doclet, tag) {
        doclet.directive = {
            'name': tag.value.name,
            'type': tag.value.type ? (tag.value.type.names.length === 1 ? tag.value.type.names[0] : tag.value.type.names) : '',
        };
    },
}
exports.newDocletHandler = function (e) {
    var directive = e.doclet.directive;
    if (directive) {
        e.doclet.kind = 'class';
        e.doclet.scope = 'directive';
        e.doclet.description = '';
    }
}