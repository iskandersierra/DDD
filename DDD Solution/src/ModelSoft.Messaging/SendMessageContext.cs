using System;
using System.Threading;

namespace ModelSoft.Messaging
{
    public class SendMessageContext
    {
        public Message Message { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;
    }
}