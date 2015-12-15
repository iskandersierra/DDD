using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModelSoft.Messaging.MessageHandlers
{
    public class DefaultHeadersMessageHandler : MessageSendingHandler
    {
        private IDictionary<string, object> headers;
        private bool overwrite;

        public DefaultHeadersMessageHandler(HandleNextMessage next) : base(next)
        {
        }

        public IDictionary<string, object> Headers
        {
            get { return headers ?? (headers = new Dictionary<string, object>()); }
            set { headers = value; }
        }

        public bool Overwrite
        {
            get { return overwrite; }
            set { overwrite = value; }
        }

        public override async Task SendAsync(SendMessageContext context)
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            if (headers != null)
            {
                foreach (var pair in headers)
                {
                    if (Overwrite || !context.Message.Headers.ContainsKey(pair.Key))
                    {
                        context.Message.Headers[pair.Key] = pair.Value;
                    }
                }
            }

            await Next(context);

            context.CancellationToken.ThrowIfCancellationRequested();
        }
    }
}
