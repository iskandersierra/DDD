using System;
using System.Threading.Tasks;

namespace ModelSoft.Messaging.MessageHandlers
{
    public abstract class MessageSendingHandler
    {
        protected HandleNextMessage Next { get; private set; }

        protected MessageSendingHandler(HandleNextMessage next)
        {
            if (next == null) throw new ArgumentNullException(nameof(next));

            Next = next;
        }

        public abstract Task SendAsync(SendMessageContext context);
    }
}
