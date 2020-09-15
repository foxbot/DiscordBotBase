using Discord;
using Discord.Commands;
using DiscordBot.Data;
using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class FeedbackHandlingModule : ModuleBase<SocketCommandContext>
    { 
        public LiteDatabase db { get; set; }

        [Command("GiveFeedback")]
        public Task GiveFeedback(params string[] feedback) => CreateFeedbackAsynch(feedback);

        [Command("GetFeedback")]
        public Task GetFeedback(int id) => GetFeedbackAsynch(id);

        [Command("ReplyToFeedback")]
        public Task ReplyToFeedback(params string[] inputs) => ReplyToFeedbackAsynch(inputs);

        [Command("GetAllOpenFeedback")]
        public Task GetAllOpenFeedback() => GetAllOpenFeedbackAsynch();

        [Command("DeleteAllFeedback")]
        public Task DeleteAllFeedback() => DeleteAllFeedbackAsynch(); 

        private async Task CreateFeedbackAsynch(params string[] feedback)
        {
            int created;
            if (Context.Guild != null)
            {
                await Context.Message.Author.SendMessageAsync("Please DM me to leave feedback! You can reply with GiveFeedback <your feedback message here> and I will get it to the right people :)");
                await Context.Message.DeleteAsync();
                return;
            }
            try
            {
                var feedbacks = db.GetCollection<Feedback>();
                created = feedbacks.Insert(new Feedback() {FeedbackMessage = feedback[0], FeedbackProvider = Context.User.Id, IsOpen = true });
            } catch (Exception E)
            {
                await ReplyAsync("Something went wrong giving the feedback! Exception is: " + E.Message);
                return;
            }
            await ReplyAsync("Your Feedback with ID of " + created + " was created");
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
                await ReplyAsync("Something went wrong getting the feedback! Exception is: " + E.Message);
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
                    feedback.IsOpen = false;
                    feedbacks.Update(feedback);
                }
                else
                {
                    await ReplyAsync("I couldn't find feedback with that ID");
                    return;
                }
            }
            catch (Exception E)
            {
                await ReplyAsync("Something went wrong replying to the feedback! Exception is " + E.Message);
                return;
            }

        }

        private async Task GetAllOpenFeedbackAsynch()
        {
            try
            {
                var feedbacks = db.GetCollection<Feedback>();
                var openFeedback = feedbacks.Find(db => db.IsOpen).ToList(); 

                if(!openFeedback.Any())
                {
                    await ReplyAsync("There is no open feedback right now");
                    return;
                }

                openFeedback.ForEach(async fb =>
                {
                    await ReplyAsync("ID: " + fb.Id + " Message: " + fb.FeedbackMessage);
                    return;
                });
            } catch(Exception e)
            {
                await ReplyAsync("Something went wrong getting the open feedback! Exception is " + e.Message);
            }
        }

        /// <summary>
        /// Testing only
        /// </summary>
        /// <returns></returns>
        private async Task DeleteAllFeedbackAsynch()
        {
            var feedbacks = db.GetCollection<Feedback>();
            feedbacks.Delete(_ => true);
            return;
        }
    }
}
