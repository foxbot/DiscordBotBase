using Discord;
using Discord.Commands;
using DiscordBot.Data;
using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class FeedbackHandlingModule : ModuleBase<SocketCommandContext>
    { 
        public LiteDatabase db { get; set; }

        [Command("GiveFeedback")]
        public Task GiveFeedback(string feedback) => CreateFeedbackAsynch(feedback);

        [Command("GetFeedback")]
        public Task GetFeedback(int id) => GetFeedbackAsynch(id);

        [Command("ReplyToFeedback")]
        public Task ReplyToFeedback(params string[] inputs) => ReplyToFeedbackAsynch(inputs);

        private async Task CreateFeedbackAsynch(string feedback)
        {
            int created;
            try
            {
                var feedbacks = db.GetCollection<Feedback>();
                created = feedbacks.Insert(new Feedback() {FeedbackMessage = feedback, FeedbackProvider = Context.User.Id, IsOpen = true });
            } catch (Exception E)
            {
                await ReplyAsync("Something went wrong giving the feedback :( Exception is: " + E.Message);
                return;
            }

            await ReplyAsync("Feedback with ID of " + created + " was created");
            return;
        }

        private async Task GetFeedbackAsynch(int id)
        {
            try
            {
                var feedbacks = db.GetCollection<Feedback>();
                var feedback = feedbacks.FindById(id);
                if (feedback != null)
                {
                    await ReplyAsync("Found it! Feedback was " + feedback.FeedbackMessage);
                }
                else
                {
                    await ReplyAsync("I couldn't find feedback with that ID");
                    return;
                }

            }
            catch (Exception E)
            {
                await ReplyAsync("Something went wrong getting the feedback :( Exception is: " + E.Message);
                return;
            }
        }

        private async Task ReplyToFeedbackAsynch(params string [] input)
        {
            try
            {
                var feedbackId = int.Parse(input[0]);
                var feedbacks = db.GetCollection<Feedback>();
                var feedback = feedbacks.FindOne(x => x.Id == feedbackId);
                if (feedback != null)
                {
                    var submitterId = feedback.FeedbackProvider;
                    var targetUser = Context.Guild.GetUser(submitterId);
                    if(targetUser == null)
                    {
                        await ReplyAsync("That user wasnt found on this server");
                    }
                    var response = input[1];
                    await targetUser.SendMessageAsync(response);
                }
                else
                {
                    await ReplyAsync("I couldn't find feedback with that ID");
                    return;
                }
            }
            catch (Exception E)
            {
                await ReplyAsync("Something went wrong replying to the feedback :( Exception is " + E.Message);
                return;
            }

        }
    }
}
