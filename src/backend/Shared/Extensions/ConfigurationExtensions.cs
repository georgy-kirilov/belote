using Shared.Exceptions;
using Microsoft.Extensions.Configuration;
using Shared.Constants;

namespace Shared.Extensions;

public static class ConfigurationExtensions
{
    public static T GetRequiredValue<T>(this IConfiguration configuration, string section)
    {
        var sectionData = configuration.GetSection(section);

        if (sectionData is null || !sectionData.Exists())
        {
            throw new FailedToLoadConfigurationValueException(section);
        }

        return sectionData.Get<T>() ?? throw new FailedToLoadConfigurationValueException(section);
    }

    public static string GetDefaultConnectionString(this IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString(ConfigurationSections.DefaultConnectionString);
        return connection ?? throw new FailedToLoadConfigurationValueException(ConfigurationSections.DefaultConnectionString);
    }
}
