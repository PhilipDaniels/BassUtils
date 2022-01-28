using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dawn;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BassUtils.NetCore
{
    /// <summary>
    /// Extensions for <c>IServiceCollection</c>.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Validates an object by applying its <c>System.ComponentModel.DataAnnotations</c>.
        /// If validation fails, all the validation problems are gathered into a single message, which is
        /// prefixed by messagePrefix, and a <c>ConfigurationException</c> is thrown.
        /// </summary>
        public static void ValidateConfigurationAndThrow(object obj, string messagePrefix)
        {
            var ctx = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(obj, ctx, validationResults, true))
            {
                var msg = messagePrefix + ":" + string.Join(";", validationResults.Select(r => r.ToString()));
                throw new ConfigurationException(msg);
            }
        }

        /// <summary>
        /// Adds an instance of a strongly-typed configuration model to the <paramref name="services"/> as
        /// a singleton. If the model has data annotations they will be checked an an exception thrown
        /// if the model is invalid. There is expected to be a node in the configuration whose name
        /// matches that of the model type, <typeparamref name="T"/>, e.g. "ConnectionStrings",
        /// unless you specify an explicit section name using the parameter.
        /// </summary>
        /// <typeparam name="T">Type of the configuration model.</typeparam>
        /// <param name="services">The service collection to add a model instance to.</param>
        /// <param name="configuration">Configuration object.</param>
        /// <param name="sectionName">Explicit section name to load.</param>
        /// <returns>Service collection, for further configuration.</returns>
        [Obsolete("Use AddConfigurationModelAsSingleton instead, its name is more explicit. This method will be removed in a future version.")]
        public static IServiceCollection AddConfigurationModel<T>(this IServiceCollection services, IConfiguration configuration, string sectionName = null)
            where T : class, new()
        {
            return AddConfigurationModelAsSingleton<T>(services, configuration, sectionName);
        }

        /// <summary>
        /// Adds an instance of a strongly-typed configuration model to the <paramref name="services"/> as
        /// a singleton. If the model has data annotations they will be checked an an exception thrown
        /// if the model is invalid.
        /// </summary>
        /// <typeparam name="T">Type of the configuration model.</typeparam>
        /// <param name="services">The service collection to add a model instance to.</param>
        /// <param name="section">Configuration section object.</param>
        /// <returns>Service collection, for further configuration.</returns>
        [Obsolete("Use AddConfigurationModelAsSingleton instead, its name is more explicit. This method will be removed in a future version.")]
        public static IServiceCollection AddConfigurationModel<T>(this IServiceCollection services, IConfigurationSection section)
            where T : class, new()
        {
            return AddConfigurationModelAsSingleton<T>(services, section);
        }

        /// <summary>
        /// Adds an instance of a strongly-typed configuration model to the <paramref name="services"/> as
        /// a singleton. If the model has data annotations they will be checked an an exception thrown
        /// if the model is invalid. There is expected to be a node in the configuration whose name
        /// matches that of the model type, <typeparamref name="T"/>, e.g. "ConnectionStrings",
        /// unless you specify an explicit section name using the parameter.
        /// </summary>
        /// <typeparam name="T">Type of the configuration model.</typeparam>
        /// <param name="services">The service collection to add a model instance to.</param>
        /// <param name="configuration">Configuration object.</param>
        /// <param name="sectionName">Explicit section name to load.</param>
        /// <returns>Service collection, for further configuration.</returns>
        public static IServiceCollection AddConfigurationModelAsSingleton<T>(this IServiceCollection services, IConfiguration configuration, string sectionName = null)
            where T : class, new()
        {
            Guard.Argument(services, nameof(services)).NotNull();
            Guard.Argument(configuration, nameof(configuration)).NotNull();

            sectionName = sectionName ?? typeof(T).Name;
            var section = configuration.GetSection(sectionName);

            var model = new T();

            new ConfigureFromConfigurationOptions<T>(section)
                .Configure(model);

            ValidateConfigurationAndThrow(model, sectionName);

            services.AddSingleton(model);

            return services;
        }

        /// <summary>
        /// Adds an instance of a strongly-typed configuration model to the <paramref name="services"/> as
        /// a singleton. If the model has data annotations they will be checked an an exception thrown
        /// if the model is invalid.
        /// </summary>
        /// <typeparam name="T">Type of the configuration model.</typeparam>
        /// <param name="services">The service collection to add a model instance to.</param>
        /// <param name="section">Configuration section object.</param>
        /// <returns>Service collection, for further configuration.</returns>
        public static IServiceCollection AddConfigurationModelAsSingleton<T>(this IServiceCollection services, IConfigurationSection section)
            where T : class, new()
        {
            Guard.Argument(services, nameof(services)).NotNull();
            Guard.Argument(section, nameof(section)).NotNull();

            var model = new T();

            new ConfigureFromConfigurationOptions<T>(section)
                .Configure(model);

            ValidateConfigurationAndThrow(model, typeof(T).Name);

            services.AddSingleton(model);

            return services;
        }

        /// <summary>
        /// Adds a strongly-typed configuration model to the <paramref name="services"/>
        /// collection. The application will respond to changes in the configuration value at runtime, for
        /// example caused by editing appSettings.json. This method will validate the app setting
        /// object using data annotations and throw a <c>ConfigurationException</c> if the object is invalid.
        /// If this happens at runtime, your application will probably stop.
        /// To have your application actually pick up the changes, you should have your classes take
        /// an <c>IOptionsMonitor></c> or <c>IOptionsSnapshot</c> by dependency injection.
        /// Use IOptions&lt;T&gt; or AddConfigurationModelAsSingleton when you are not expecting your
        /// config values to change. Use IOptionsSnaphot&lt;T&gt; when you are expecting your values to change but want it
        /// to be consistent for the entirety of a request. Use IOptionsMonitor&lt;T&gt; when you need real time values (or
        /// you have a long running service which is only constructed once but you still want to respond to configuration
        /// changes).
        /// </summary>
        /// <remarks>
        /// See https://stackoverflow.com/questions/50788988/difference-between-ioptionsmonitor-vs-ioptionssnapshot
        /// and https://andrewlock.net/reloading-strongly-typed-options-in-asp-net-core-1-1-0/
        /// </remarks>
        /// <typeparam name="T">Type of the configuration model.</typeparam>
        /// <param name="services">The service collection to add a model instance to.</param>
        /// <param name="configuration">Configuration object.</param>
        /// <param name="sectionName">Explicit section name to load.</param>
        /// <returns>Service collection, for further configuration.</returns>
        public static IServiceCollection AddConfigurationModelWithMonitoring<T>(this IServiceCollection services, IConfiguration configuration, string sectionName = null)
            where T : class
        {
            Guard.Argument(services, nameof(services)).NotNull();
            Guard.Argument(configuration, nameof(configuration)).NotNull();

            sectionName = sectionName ?? typeof(T).Name;

            // Registration is a two-stage order-dependent process. The second call is what performs
            // the validation. If validation fails, the application will stop.
            services.Configure<T>(configuration.GetSection(sectionName));

            services.Configure<T>(configurationObject => {
                ValidateConfigurationAndThrow(configurationObject, sectionName);
            });

            return services;
        }
    }
}
