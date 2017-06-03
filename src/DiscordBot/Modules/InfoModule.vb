Imports Discord.Commands

Namespace DiscordBot.Modules
    Public Class InfoModule
        Inherits ModuleBase(Of SocketCommandContext)

        <Command("Info")>
        Public Function Info() As Task
            Return ReplyAsync($"Hello, I am a bot called {Context.Client.CurrentUser.Username} written in Discord.Net 1.0")
        End Function
    End Class
End Namespace

