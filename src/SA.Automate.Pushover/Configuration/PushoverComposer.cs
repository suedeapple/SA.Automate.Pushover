using Microsoft.Extensions.DependencyInjection;
using SA.Automate.Pushover.Actions;
using SA.Automate.Pushover.Connection;
using Umbraco.Automate.Core.Actions;
using Umbraco.Automate.Core.Connections;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace SA.Automate.Pushover.Configuration;

/// <summary>
/// Registers all Pushover Automate services with the Umbraco dependency injection container.
/// This composer wires up the global settings, connection type, and available actions.
/// </summary>
public class PushoverComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        // Bind the Pushover settings section from appsettings.json (e.g. ApiToken, Retry, Expire)
        builder.Services.AddOptions<PushoverSettings>()
            .BindConfiguration(PushoverSettings.SectionName);

        // Register the Pushover connection type so it appears in Umbraco Automate connections
        builder.WithCollectionBuilder<ConnectionTypeCollectionBuilder>()
            .Add<PushoverConnectionType>();

        // Register the Send Notification action so it is available in Umbraco Automate workflows
        builder.WithCollectionBuilder<ActionCollectionBuilder>()
            .Add<SendNotificationAction>();
    }
}