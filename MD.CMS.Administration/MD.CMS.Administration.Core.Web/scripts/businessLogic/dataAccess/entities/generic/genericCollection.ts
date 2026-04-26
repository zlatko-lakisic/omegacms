/// <reference path="./genericKeyValuePair.ts" />

namespace mdBusinessLogic.dataAccess.entities.generic {
    export interface iGenericCollection<T> {
        Collection?: Array<genericKeyValuePair<T>>;
    }

    export class genericCollection<T> implements base.IBaseEntity<genericCollection<T>> {
        private Collection: Array<genericKeyValuePair<T>>;

        constructor(obj?: iGenericCollection<T>) {
            this.Collection = new Array<genericKeyValuePair<T>>();
            if (obj && obj.Collection) {
                this.Collection = obj.Collection;
            }
        }

        public getCollection(): Array<genericKeyValuePair<T>> {
            return this.Collection;
        }

        public remove(key: string): void {
            for (let i = this.Collection.length - 1; i >= 0; i--) {
                if (this.Collection[i].Key == key) {
                    this.Collection.splice(i, 1);
                    break;
                }
            }
        }

        public add(key: string, value: T): void {
            let constraint: generic.genericKeyValuePair<T> = this.getKeyValuePair(key);
            if (constraint) {
                constraint.Value = value;
            } else {
                this.Collection.push(new generic.genericKeyValuePair({
                    Key: key,
                    Value: value
                }))
            }
        }

        public get(key: string): T {
            let constraint: generic.genericKeyValuePair<T> = this.getKeyValuePair(key);
            if (constraint) {
                return constraint.Value
            }
            return null;
        }

        public getKeyValuePair(key: string): generic.genericKeyValuePair<T> {
            let constraint: generic.genericKeyValuePair<T> = this.Collection.filter((constraint) => { return constraint.Key == key; })[0];
            if (constraint) {
                return constraint
            }
            return null;
        }

        public construct(data: any): void {
            this.Collection = helpers.entityHelper.getValue<Array<genericKeyValuePair<T>>>(data, "Collection", new Array<genericKeyValuePair<T>>());
        }

        public clone(): genericCollection<T> {
            return new genericCollection({
                Collection: this.getCollection()
            });
        }
    }
}
