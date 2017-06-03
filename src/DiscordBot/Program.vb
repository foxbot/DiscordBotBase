Imports System.IO
Imports Discord
Imports Discord.Commands
Imports Discord.WebSocket
Imports DiscordBot.DiscordBot.Services
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection

Module Program

    Dim _client As DiscordSocketClient
    Dim _config As IConfiguration

    Sub Main()
        MainAsync().GetAwaiter().GetResult()
    End Sub

    Async Function MainAsync() As Task
        _client = New DiscordSocketClient
        _config = BuildConfig()

        Dim services = ConfigureServices()

        services.GetRequiredService(Of LogService)
        Await services.GetRequiredService(Of CommandHandlingService).InitializeAsync(services)

        Await _client.LoginAsync(TokenType.Bot, _config("token"))
        Await _client.StartAsync()

        Await Task.Delay(-1)
    End Function

    Function ConfigureServices() As IServiceProvider
        Dim collection As New ServiceCollection

        With collection
            .AddSingleton(_client)
            .AddSingleton(Of CommandService)
            .AddSingleton(Of CommandHandlingService)
            .AddLogging()
            .AddSingleton(Of LogService)
            .AddSingleton(_config)
        End With

        Return collection.BuildServiceProvider()
    End Function

    Function BuildConfig() As IConfiguration
        Return New ConfigurationBuilder() _
            .SetBasePath(Directory.GetCurrentDirectory) _
            .AddJsonFile("config.json") _
            .Build()
    End Function
End Module
