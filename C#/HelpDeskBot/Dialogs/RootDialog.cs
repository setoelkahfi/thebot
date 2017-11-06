using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HelpDeskBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {

        private string category;
        private string severity;
        private string description;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            await context.PostAsync("Hi!, I'm the help desk bot and I can help you create a ticket.");
            PromptDialog.Text(context, this.DescriptionMessageReceivedAsync, "First, please briefly describe your problem to me.");
        }

        public async Task DescriptionMessageReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            this.description = await argument;
            var severities = new string[] { "high", "normal", "low"};
            PromptDialog.Choice(context, this.SeverityMessageReceivedAsync, severities, "Which is the severity of this problem?");
        }

        public async Task SeverityMessageReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            this.severity = await argument;
            PromptDialog.Text(context, this.CategoryMessageReceivedAsync, "Which would be the category for this ticket (software, hardware, networking, security or other)?");
        }

        private async Task CategoryMessageReceivedAsync(IDialogContext context, IAwaitable<string> argument)
        {
            this.category = await argument;
            var text = $"Great! I'm going to create a \"{this.severity}\" severity ticket in the \"{this.category}\" category. " +
                        $"The description I will use is \"{this.description}\". Can you please confirm that this information is correct?";
            PromptDialog.Confirm(context, this.IssueConfirmedMessageReceivedAsync, text);
        }

        private async Task IssueConfirmedMessageReceivedAsync(IDialogContext context, IAwaitable<bool> argument)
        {
            var confirmed = await argument;
            if (confirmed)
            {
                await context.PostAsync("Awesome! Your ticket has been created.");
            }
            else
            {
                await context.PostAsync("OK. The ticket was not created. You can start again if you want.");
            }
            context.Done<object>(null);
        }
    }
}