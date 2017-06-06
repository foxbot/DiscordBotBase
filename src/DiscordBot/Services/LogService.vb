Imports Discord
Imports Discord.Commands
Imports Discord.WebSocket
Imports Microsoft.Extensions.Logging

Namespace DiscordBot.Services
    Public Class LogService
        Private ReadOnly _discord As DiscordSocketClient
        Private ReadOnly _commands As CommandService
        Private ReadOnly _loggerFactory As ILoggerFactory
        Private ReadOnly _discordLogger As ILogger, _commandsLogger As ILogger

        Public Sub New(discord As DiscordSocketClient, commands As CommandService, loggerFactory As ILoggerFactory)
            _discord = discord
            _commands = commands

            _loggerFactory = ConfigureLogging(loggerFactory)
            _discordLogger = _loggerFactory.CreateLogger("discord")
            _commandsLogger = _loggerFactory.CreateLogger("commands")

            AddHandler _discord.Log, AddressOf LogDiscord
            AddHandler _commands.Log, AddressOf LogCommand
        End Sub

        Private Function ConfigureLogging(factory As ILoggerFactory) As ILoggerFactory
            factory.AddConsole()
            Return factory
        End Function

        Private Function LogDiscord(message As LogMessage) As Task
            _discordLogger.Log(
                LogLevelFromSeverity(message.Severity),
                0,
                message,
                message.Exception,
                Function(_1, _2) message.ToString(prependTimestamp:=False))
            Return Task.CompletedTask
        End Function

        Private Function LogCommand(message As LogMessage) As Task
            ' Return an error for async commands
            Dim command = TryCast(message.Exception, CommandException)
            If Not IsNothing(command) Then
                ' Don't risk blocking the logging task by awaiting a message send; ratelimits!?
                Dim m = command.Context.Channel.SendMessageAsync($"Error: {command.Message}")
            End If

            _discordLogger.Log(
                LogLevelFromSeverity(message.Severity),
                0,
                message,
                message.Exception,
                Function(_1, _2) message.ToString(prependTimestamp:=False))
            Return Task.CompletedTask
        End Function

        Private Shared Function LogLevelFromSeverity(severity As LogSeverity)
            Return DirectCast(Math.Abs(severity - 5), LogLevel)
        End Function

    End Class
End Namespace

