using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Monolith.Core.Attributes;

namespace Monolith.Core.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T Load<T>(this IConfiguration configuration, string name)
        {
            var source = typeof(T);
            var options = configuration.GetSection(name).Get<T>();

            if (options == null)
            {
                throw new Exception($"Options from '{source.FullName}', not found for '{name}'");
            }

            SetEnvironmentValues(options);

            Validate(options);

            return options;
        }

        public static T Load<T>(this IConfiguration configuration)
        {
            var source = typeof(T);
            var target = typeof(OptionAttribute);

            if (source.GetCustomAttributes(target, true).FirstOrDefault() is not OptionAttribute attribute)
            {
                throw new Exception($"Missing option attribute for '{source.FullName}'");
            }

            return Load<T>(configuration, attribute.Name);
        }

        #region Private Methods

        private static void Validate<T>(T options)
        {
            var source = typeof(T);
            var validations = new List<ValidationResult>();
            var context = new ValidationContext(options, null, null);

            Validator.TryValidateObject(options, context, validations, true);

            if (!validations.Any())
            {
                return;
            }

            var errors = validations.Select(entry => entry.ErrorMessage);
            var message = string.Join('\n', errors);

            throw new Exception($"One or more properties from '{source.FullName}' are invalid:\n{message}");
        }

        private static void SetEnvironmentValues<T>(T options)
        {
            var source = typeof(T);
            var target = typeof(EnvironmentAttribute);
            var properties = source.GetProperties();

            foreach (var property in properties)
            {
                if (property.GetCustomAttributes(target, true).FirstOrDefault() is not EnvironmentAttribute attribute)
                {
                    continue;
                }

                var optValue = property.GetValue(options) as string;
                var envValue = Environment.GetEnvironmentVariable(attribute.Name);

                if (attribute.Priority)
                {
                    property.SetValue(options, envValue);
                }
                else if (string.IsNullOrWhiteSpace(optValue))
                {
                    property.SetValue(options, envValue);
                }
            }
        }

        #endregion
    }
}
