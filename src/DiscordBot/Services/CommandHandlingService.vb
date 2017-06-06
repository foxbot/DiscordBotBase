Imports System.Reflection
Imports Discord
Imports Discord.Commands
Imports Discord.WebSocket

Namespace DiscordBot.Services
    Public Class CommandHandlingService

        Private ReadOnly _discord As DiscordSocketClient
        Private ReadOnly _commands As CommandService
        Private _provider As IServiceProvider

        Sub New(provider As IServiceProvider, discord As DiscordSocketClient, commands As CommandService)
            _discord = discord
            _commands = commands

            AddHandler _discord.MessageReceived, AddressOf MessageReceived
        End Sub

        Async Function InitializeAsync(provider As IServiceProvider) As Task
            _provider = provider
            Await _commands.AddModulesAsync(Assembly.GetEntryAssembly())
            ' Add additional initialization code here...
        End Function

        Async Function MessageReceived(rawMessage As SocketMessage) As Task
            ' Ignore system messages and messages from bots
            Dim userMessage = TryCast(rawMessage, SocketUserMessage)
            If (IsNothing(userMessage) OrElse Not (rawMessage.Source = MessageSource.User)) Then
                Return
            End If

            Dim argPos = 0
            If Not (userMessage.HasMentionPrefix(_discord.CurrentUser, argPos)) Then
                Return
            End If

            Dim context = New SocketCommandContext(_discord, userMessage)
            Dim result = Await _commands.ExecuteAsync(context, argPos, _provider)

            If (TypeOf result Is ParseResult OrElse
            TypeOf result Is PreconditionResult OrElse
            TypeOf result Is ExecuteResult OrElse
            TypeOf result Is TypeReaderResult) AndAlso Not result.IsSuccess Then
                Await context.Channel.SendMessageAsync(result.ToString())
            End If
        End Function

    End Class
End Namespace
