# CSharp Discord Webhooks Library

This repository contains a C# library for interacting with Discord webhooks. It provides classes and structures for creating and handling webhook messages.

If you find this repository helpful, please consider giving it a star. Your support is greatly appreciated!

## Table of Contents

## Capabilities

- Send text messages and rich embeds.
- Customize usernames and avatars.
- Add images and thumbnails.
- Embed files in messages.
- Set colors for visual emphasis.
- Include timestamps for context.
- Input validation for compliance.

## Design Patterns

This library incorporates several design patterns to ensure clean and modular code:

- **Builder Pattern**: The `HookEmbedBuilder` and `HookObjectBuilder` classes implement the builder pattern to provide a fluent interface for creating complex `HookObject` and `HookEmbed` objects with ease.

## Dependencies

- [Newtonsoft.Json](https://www.newtonsoft.com/json): A popular high-performance JSON framework for .NET. It provides functionality for serializing, deserializing, querying, and manipulating JSON data.

## Usage

For a practical implementation example, you can refer to the included `WebhookExample.cs` file in this repository.

## Additional Classes and Interfaces

- `IWebhookService`: Represents an interface for a webhook sending service.
- `IWebhookClient`: Represents a contract for a webhook client.
- `IWebhookRequest`: Represents an interface for a webhook request.
- `HookObjectValidator`: This class provides validation methods for a HookObject.
- `HookObject`: Represents a structure for creating and handling hook objects.
- `HookObjectBuilder`: A builder class for creating Discord webhook messages.
- `HookEmbedField`: Represents an embed field for use in a hook.
- `HookEmbed`: Represents an embedded message hook container.
- `HookEmbedBuilder`: A builder class for creating Discord webhook embeds.

## Notes

- For info on how to create webhooks, see [Creating Webhooks](https://support.discord.com/hc/en-us/articles/228383668-Intro-to-Webhooks)
- For discord webhook documentation, see [Discord Webhooks Documentation](https://discord.com/developers/docs/resources/webhook)
- For easy-to-use tool for building Discord messages/embeds, see [discohook.org](https://discohook.org/)

## License

This library is provided as-is under the terms of the [MIT License](LICENSE.md). Feel free to modify and adapt it to suit your needs.
