namespace mdBusinessLogic.helpers {
    export module typeConversion {
        /**
         * Convert the passed value to an integer value
         * @param value The value to be converted
         * @param defaultValue The default value if parsing fails, defaults to 0
         * @param stripNonNumbers Strip non numbers from passed value, defaults to true
        */
        export function toInt(value: number | string, defaultValue: number = 0, stripNonNumbers: boolean = true): number {
            let returnValue: number = defaultValue;

            try {
                if (stripNonNumbers) {
                    value = value.toString().replace(/\D/g, '');
                }

                returnValue = parseInt(value.toString());
            } catch {
                returnValue = defaultValue;
            }

            return returnValue;
        }

        /**
         * Convert the passed value to a float value
         * @param value The value to be converted
         * @param defaultValue The default value if parsing fails, defaults to 0
         * @param stripNonNumbers Strip non numbers from passed value, defaults to true
         */
        export function toFloat(value: number | string, defaultValue: number = 0, stripNonNumbers: boolean = true): number {
            let returnValue: number = defaultValue;

            try {
                if (stripNonNumbers) {
                    value = value.toString().replace(/\D/g, '');
                }

                returnValue = parseFloat(value.toString());
            } catch {
                returnValue = defaultValue;
            }

            return returnValue;
        }
    }
}