namespace EasyNetQ.Exceptionless
{
    using System;
    using EasyNetQ;
    using EasyNetQ.Consumer;

    using global::Exceptionless;

    class ExceptionlessConsumerErrorStrategy : DefaultConsumerErrorStrategy {
        readonly ExceptionlessClient _client;
        readonly Action<EventBuilder, ConsumerExecutionContext> _intercept;

        public ExceptionlessConsumerErrorStrategy(IConnectionFactory connectionFactory, ISerializer serializer, IEasyNetQLogger logger, IConventions conventions, ITypeNameSerializer typeNameSerializer, IErrorMessageSerializer errorMessageSerializer, ExceptionlessClient client = null, Action<EventBuilder, ConsumerExecutionContext> intercept = null)
            : base(connectionFactory, serializer, logger, conventions, typeNameSerializer, errorMessageSerializer) {
            _client = client ?? ExceptionlessClient.Default;
            _intercept = intercept;
        }

        public override AckStrategy HandleConsumerError(ConsumerExecutionContext context, Exception exception) {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            try {
                return base.HandleConsumerError(context, exception);
            }
            finally {
                var eventBuilder = exception.ToExceptionless();

                _intercept?.Invoke(eventBuilder, context);

                eventBuilder.Submit();
            }
        }
    }
}
