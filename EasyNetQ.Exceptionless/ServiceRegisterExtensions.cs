namespace EasyNetQ.Exceptionless
{
    using System;
    using EasyNetQ;
    using EasyNetQ.Consumer;

    using global::Exceptionless;

    /// <summary>
    /// Extension methods for the <see cref="IServiceRegister"/> interface.
    /// </summary>
    public static class ServiceRegisterExtensions {
        /// <summary>
        /// Enable reporting message consumer errors to Exceptionless.
        /// </summary>
        /// <param name="serviceRegister">Extension instance.</param>
        /// <param name="client">Optional Exceptionless client. Defaults to ExceptionlessClient.Default.</param>
        /// <param name="intercept">Optional interceptor hook, allowing you to customize the exceptionless event.</param>
        /// <returns>Extension instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the value passed to the <paramref name="serviceRegister"/> parameter is <c>null</c>.
        /// </exception>
        public static IServiceRegister EnableExceptionless(this IServiceRegister serviceRegister, ExceptionlessClient client = null, Action<EventBuilder, ConsumerExecutionContext> intercept = null) {
            if (serviceRegister == null)
                throw new ArgumentNullException(nameof(serviceRegister));

            serviceRegister.Register<IConsumerErrorStrategy>(services => new ExceptionlessConsumerErrorStrategy(
                services.Resolve<IConnectionFactory>(),
                services.Resolve<ISerializer>(),
                services.Resolve<IEasyNetQLogger>(),
                services.Resolve<IConventions>(),
                services.Resolve<ITypeNameSerializer>(),
                services.Resolve<IErrorMessageSerializer>(),
                client,
                intercept
            ));

            return serviceRegister;
        }
    }
}
