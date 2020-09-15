using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Data
{
    public class Feedback
    {
        public int Id { get; set; }

        public ulong FeedbackProvider { get; set; }

        public bool IsOpen { get; set; }

        public string FeedbackMessage { get; set; }

    }
}
