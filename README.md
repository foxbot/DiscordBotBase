# Feedback Loop - A Friendly lightweight bot for providing an anonymous feedback loop

This is a lightweight bot for anonymous feedback - most importantly it provides a mechanism for
a feedback "loop". Anonymous feedback is a great tool for leadership to use - the problem becomes 
how to close the loop by responding to the feedback while preserving the anonymity of the submitter. 

This simple little bot solves for that. Users can submit feedback by DMing the bot. Feedback is stored in 
a simple DBLite database along with the ID of the submitter. Users can then query the bot to get open feedback 
(seeing only the feedback's ID, not the ID of the submitter) and the message and respond to individual feedback items 
to the bot - which will take care of conveying the message to the submitter behind the scenes. 

Very lightweight for now, I built this for my World of Warcraft guild as my fellow officers and I were brainstorming ways to
get anonymous feedback working in a way we were happy with. 

More features are planning including role-based permissions, configurable server settings (including a "broadcast to channel" option 
allowing you to have feedback conveyed as soon as its submitted instead of relying on querying the bot), intelligent recognition of which server feedback is for (in
case a user is in multiple servers using the bot), and other QOL enhancements. 

Usage: 

GiveFeedback "<Your Feedback Here>" - Works only in DM's to the bot (FeedbackLoop will helpfully tell you to DM him if you try to give feedback in a server and delete your message to preserve your anonymity). Make sure you wrap your feedback in quotes - GiveFeedback "I sure wish this documentation was prettier". Feedback loop will confirm your submission and give you your feedback ID. 

GetFeedback <ID> - Give FeedbackLoop a feedback ID and he will return the feedback message stored for you. EZ
  
ReplyToFeedback <ID> "<Feedback Response>" - The cool part. Give the bot an ID and a message responding to that feedback and he will send a DM to the orignal submitter with that response. At no point in this process is the original submitter's ID exposed - allowing server owners/officers/DM's/whoever to address feedback without compromising anonymity. 
  
GetAllOpenFeedback - Gets all the open feedback. Boring obligatory index option. 

Forked from foxbot/DiscordBotBase - https://github.com/foxbot/DiscordBotBase
