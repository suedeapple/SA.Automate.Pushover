# SA.Automate.Pushover

[![Downloads](https://img.shields.io/nuget/dt/SA.Automate.Pushover?color=cc9900)](https://www.nuget.org/packages/SA.Automate.Pushover/)
[![NuGet](https://img.shields.io/nuget/vpre/SA.Automate.Pushover?color=0273B3)](https://www.nuget.org/packages/SA.Automate.Pushover)
[![GitHub license](https://img.shields.io/github/license/suedeapple/SA.Automate.Pushover?color=8AB803)](https://github.com/suedeapple/SA.Automate.Pushover/blob/main/LICENSE)

A Pushover connection type and action for [Umbraco Automate](https://github.com/umbraco/Umbraco.Automate). Send Push notifications to Pushover as part of an automation workflow.

## What is Pushover?

[Pushover](https://pushover.net/) is a simple notification service that lets apps send real-time alerts to mobile and desktop devices.

## What can this be used for?

This package is useful when you want instant operational alerts from Umbraco Automate workflows, for example:

- **Umbraco Commerce new orders**: play a "kerching" sound on your mobile device when a new order is placed.
- **Backoffice moderation tasks**: use a distinct alert sound on your mobile device when content is submitted for approval.
- **General team notifications**: route different events to different users or groups with different sounds and priorities.

## Installation

```bash
dotnet add package SA.Automate.Pushover
```

No further setup required. The composer registers itself automatically via Umbraco's `IComposer` discovery.

## Setup

### 1. Generate a Pushover access token

In your Pushover account go to **Your Applications → Create an API Token**. Copy the access token.

### 2. Add the token to appsettings

Access tokens are stored in configuration, not the backoffice. Add the following to your `appsettings.json` (or `appsettings.Production.json`):

```json
{
  "Umbraco": {
    "Automate": {
      "Providers": {
        "SA.Automate.Pushover": {
           "ApiToken": "your-access-token-here"
        }
      }
    }
  }
}
```

#### Full Setup

For advanced configuration, you can specify additional parameters for **Priority 2** notifications, which will retry at a specified interval and expire after a certain time:

```json
{
  "Umbraco": {
    "Automate": {
      "Providers": {
        "SA.Automate.Pushover": {
          "ApiToken": "your-access-token-here"
          "Retry": "60", // Retry interval in seconds for Priority 2 notifications
          "Expires": "1800" // Expiration time in seconds for Priority 2 notifications
        }
      }
    }
  }
}
```

### 3. Create the connection in the backoffice

1. Go to **Automate → Connections** and create a new **Pushover** connection.
2. Give the connection a name.
2. Enter your **User Key or Group Key**
3. Click **Test connection** to verify.

**Tip:** You can create multiple connections, with different User Keys or Group Keys, to send notifications to different users or groups.

## Usage

Add the **Send Pushover Notification** action to any automation and select your Pushover connection. Available fields:

| Field | Description |
|---|---|
| Title | The notification title. Supports `${ binding }` expressions. |
| Message | The notification message. Supports `${ binding }` expressions. |
| Sound | The sound to play when the notification is received. https://pushover.net/api#sounds Supports `${ binding }` expressions. |
| URL | Optional URL appended to the notification. Supports `${ binding }` expressions. |
| URL Title | Optional title for the URL. Supports `${ binding }` expressions. |
| Priority | The priority of the notification. Defaults to `0`. |

## Compatibility

| Package version | Umbraco Automate | Umbraco CMS |
|---|---|---|
| 1.x | 17.x | 17.x |

## Links

- [Source code](https://github.com/suedeapple/SA.Automate.Pushover)
- [Report an issue](https://github.com/suedeapple/SA.Automate.Pushover/issues)
