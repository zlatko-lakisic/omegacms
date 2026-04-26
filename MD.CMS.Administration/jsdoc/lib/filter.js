/**
 * This module defines a custom jsDoc tag.
 * It allows you to document header parameters of a filter.
 */

'use strict';

exports.name = 'filter';
exports.options = {
    canHaveType: true,
    canHaveName: true,
    onTagged: function (doclet, tag) {
        doclet.filter = {
            'name': tag.value.name,
            'type': tag.value.type ? (tag.value.type.names.length === 1 ? tag.value.type.names[0] : tag.value.type.names) : '',
        };
    },
}
exports.newDocletHandler = function (e) {
    var filter = e.doclet.filter;
    if (filter) {
        e.doclet.kind = 'class';
        e.doclet.scope = 'filter';
        e.doclet.description = '';
    }
}