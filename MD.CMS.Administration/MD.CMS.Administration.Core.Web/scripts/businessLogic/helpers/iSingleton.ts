namespace mdBusinessLogic.helpers {
    export interface iSingleton<T> {
        getInstance(): T;
    }
}
