# CSharp Discord Webhooks Library

This repository contains a C# library for interacting with Discord webhooks. It provides classes and structures for creating and handling webhook messages.

If you find this repository helpful, please consider giving it a star. Your support is greatly appreciated!

## Table of Contents
- [Capabilities](#capabilities)
- [Dependencies](#dependencies)
- [Usage](#usage)
- [Notes](#notes)
- [License](#license)

## Capabilities

- Send text messages and rich embeds.
- Customize usernames and avatars.
- Add images and thumbnails.
- Embed files in messages.
- Set colors for visual emphasis.
- Include timestamps for context.
- Input validation for compliance.

## Dependencies

- [Newtonsoft.Json](https://www.newtonsoft.com/json): A popular high-performance JSON framework for .NET. It provides functionality for serializing, deserializing, querying, and manipulating JSON data.

## Usage

The `HookObjectBuilder` class is responsible for creating a `HookObject` used in a Discord webhook. It provides methods for setting content, adding embeds, and configuring the username and avatar.
The `WebhookHelper` class provides methods for sending webhook messages asynchronously. 

For a practical implementation example, you can refer to the included  [`WebhookExample.cs`](WebhookExample.cs) file in this repository.

## Notes
- For info on how to create webhooks, see [Creating Webhooks](https://support.discord.com/hc/en-us/articles/228383668-Intro-to-Webhooks)
- For discord webhook documentation, see [Discord Webhooks Documentation](https://discord.com/developers/docs/resources/webhook)
- For easy-to-use tool for building and sending Discord messages and embeds using webhook, see [discohook.org](https://discohook.org/)

## License

This library is provided as-is under the terms of the [MIT License](LICENSE.md). Feel free to modify and adapt it to suit your needs.
