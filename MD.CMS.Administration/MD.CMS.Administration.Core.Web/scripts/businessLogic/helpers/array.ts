/// <reference path="../globalVariables.ts" />
/// <reference path="../settings.ts" />
namespace mdBusinessLogic.helpers {
    export module array {
        Array.prototype['move'] = function (pos1, pos2) {
            var i, tmp;
            pos1 = parseInt(pos1, 10);
            pos2 = parseInt(pos2, 10);
            if (pos1 !== pos2 && 0 <= pos1 && pos1 <= this.length && 0 <= pos2 && pos2 <= this.length) {
                tmp = this[pos1];
                if (pos1 < pos2) {
                    for (i = pos1; i < pos2; i++) {
                        this[i] = this[i + 1];
                    }
                }
                else {
                    for (i = pos1; i > pos2; i--) {
                        this[i] = this[i - 1];
                    }
                }
                this[pos2] = tmp;
            }
        };
    }
}
