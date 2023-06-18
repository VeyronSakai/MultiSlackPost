# MultiSlackPost

[![Test](https://github.com/VeyronSakai/MultiSlackPost/actions/workflows/test.yml/badge.svg)](https://github.com/VeyronSakai/MultiSlackPost/actions/workflows/test.yml)

CLI tool for posting to multiple Slack channels simultaneously.

[NuGet.org](https://www.nuget.org/packages/mslack)

## Requirements

- .NET 6.0 or higher SDK is installed on your machine.
- You have a Slack account.
- You have created a Slack App for the workspace to which you want to post messages.
- The All Slack Apps have `chat:write` permission.

## Installation

```bash
dotnet tool install -g mslack
```

## Usage

### Setup

Register all channels and workspaces you wish to post to, and OAuth Tokens that can be used in those workspaces.

Both User OAuth Token and Bot User OAuth Token can be used, but when using Bot User OAuth Token, the App must be added to the channel where the message is posted.

```bash
mslack config channel add -w "workspace1" -c "channel1"  
mslack config channel add -w "workspace1" -c "channel2"
mslack config channel add -w "workspace2" -c "channel3"
mslack config channel add -w "workspace2" -c "channel4"
# It does not matter if the channel name is prefixed with #, as in #channel.

mslack config token add -w "workspace1" -t "token1"
mslack config token add -w "workspace2" -t "token2"
...
```
This setup can be done once.

The order in which the commands are executed can be reversed.

These information is stored in `~/.config/mslack/config.json`.

### Post to multiple channels

```bash
mslack post "Hello, World!"
```
