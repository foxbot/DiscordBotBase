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

Forked from foxbot/DiscordBotBase - https://github.com/foxbot/DiscordBotBase
