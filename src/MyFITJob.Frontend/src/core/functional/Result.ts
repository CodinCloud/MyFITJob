export type Result<T, E = Error> = Success<T, E> | Failure<T, E>;

export class Success<T, E> {
  readonly isSuccess = true;
  readonly isFailure = false;
  
  constructor(readonly value: T) {}

  map<U>(fn: (value: T) => U): Result<U, E> {
    return new Success(fn(this.value));
  }

  mapError<U>(_fn: (error: E) => U): Result<T, U> {
    return new Success<T, U>(this.value);
  }

  fold<U>(onSuccess: (value: T) => U, _onFailure: (error: E) => U): U {
    return onSuccess(this.value);
  }
}

export class Failure<T, E> {
  readonly isSuccess = false;
  readonly isFailure = true;
  
  constructor(readonly error: E) {}

  map<U>(_fn: (value: T) => U): Result<U, E> {
    return new Failure<U, E>(this.error);
  }

  mapError<U>(fn: (error: E) => U): Result<T, U> {
    return new Failure<T, U>(fn(this.error));
  }

  fold<U>(_onSuccess: (value: T) => U, onFailure: (error: E) => U): U {
    return onFailure(this.error);
  }
}

// Fonctions utilitaires
export const Result = {
  success<T, E = Error>(value: T): Result<T, E> {
    return new Success(value);
  },

  failure<T, E = Error>(error: E): Result<T, E> {
    return new Failure(error);
  },

  // Utilitaire pour transformer une Promise en Result
  fromPromise<T>(promise: Promise<T>): Promise<Result<T, Error>> {
    return promise
      .then((value) => Result.success<T, Error>(value))
      .catch((error) => Result.failure<T, Error>(error));
  }
}; 