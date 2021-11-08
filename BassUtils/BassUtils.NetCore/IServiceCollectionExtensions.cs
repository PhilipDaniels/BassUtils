using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        public static IServiceCollection AddConfigurationModel<T>(this IServiceCollection services, IConfiguration configuration, string sectionName = null)
            where T : class, new()
        {
            sectionName = sectionName ?? typeof(T).Name;
            var section = configuration.GetSection(sectionName);
            var model = new T();

            new ConfigureFromConfigurationOptions<T>(section)
                .Configure(model);

            Validate(model, sectionName);

            services.AddSingleton(model);

            return services;
        }

        static void Validate(object obj, string message)
        {
            var ctx = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(obj, ctx, validationResults, true))
            {
                var msg = message + ":" + string.Join(";", validationResults.Select(r => r.ToString()));
                throw new ConfigurationException(msg);
            }
        }
    }
}
