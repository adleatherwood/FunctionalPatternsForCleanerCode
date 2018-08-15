using System;

namespace FunctionalTypes
{
    public class Result<T>
    {
        private Result(bool wasSuccessful, T value, string error) => 
            (WasSuccessful, Value, Error) = (wasSuccessful, value, error);
        
        public readonly bool WasSuccessful;
        public readonly T Value;
        public readonly string Error;

        public static Result<T> Success(T a)
        {
            return new Result<T>(true, a, null);
        }

        public static Result<T> Failure(string message)
        {
            return new Result<T>(false, default(T), message);
        }

        public static implicit operator Result<T>(T t)
        {
            return Success(t);
        }

        public static implicit operator Result<T>(string message)
        {
            return Failure(message);
        }

        public Result<TResult> Bind<TResult>(Func<T, Result<TResult>> f)
        {
            if (this.WasSuccessful)
                return f(this.Value);
            else
                return this.Error;
        }

        public Result<TResult> Bind<TResult, A>(Func<T, A, Result<TResult>> f, A a)
        {
            if (this.WasSuccessful)
                return f(this.Value, a);
            else
                return this.Error;
        }

        public Result<TResult> Bind<TResult, A, B>(Func<T, A, B, Result<TResult>> f, A a, B b)
        {
            if (this.WasSuccessful)
                return f(this.Value, a, b);
            else
                return this.Error;
        }

        public Result<TResult> Map<TResult>(Func<T, TResult> f)
        {
            if (this.WasSuccessful)
                return f(this.Value);
            else
                return this.Error;
        }
    }    
}