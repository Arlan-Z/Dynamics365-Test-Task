using System;

namespace AwaraIT.Training.Plugins.PluginExtensions.Builder
{
    public class ExceptionHandlerSubscription
    {
        private Func<Exception, bool> _handler;
        private Type _exceptionType;

        public static ExceptionHandlerSubscription Create<TException>(
            Func<TException, bool> handler)
            where TException : Exception
        {
            return new ExceptionHandlerSubscription()
            {
                _handler = (Func<Exception, bool>)(exception => handler(exception as TException)),
                _exceptionType = typeof(TException)
            };
        }

        public bool IsSuitble(Exception exception)
        {
            return exception.GetType() == this._exceptionType;
        }

        public bool Handle(Exception exception)
        {
            return this._handler(exception);
        }
    }
}
