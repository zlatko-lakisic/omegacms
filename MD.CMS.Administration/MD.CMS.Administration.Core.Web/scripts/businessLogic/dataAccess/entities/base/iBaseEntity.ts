declare namespace mdBusinessLogic.dataAccess.entities.base {
  export interface IBaseEntity<E> {
    construct(data: any): void;

    clone(): E;

  }
}
