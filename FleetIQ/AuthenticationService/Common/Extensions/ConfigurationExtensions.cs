using Microsoft.Extensions.Configuration;

namespace AuthenticationService.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Gets a strongly typed configuration section from IConfiguration.
        /// </summary>
        /// <typeparam name="T">The type of configuration section.</typeparam>
        /// <param name="configuration">The IConfiguration instance.</param>
        /// <param name="sectionName">The name of the section.</param>
        /// <returns>A strongly typed configuration object.</returns>
        public static T GetConfig<T>(this IConfiguration configuration, string sectionName) where T : new()
        {
            var configInstance = new T();
            configuration.GetSection(sectionName).Bind(configInstance);
            return configInstance;
        }
    }
}
