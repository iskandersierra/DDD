using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelSoft.Messaging.MessageHandlers
{
    public class RetryMessageSendingHandler : MessageSendingHandler
    {
        private static readonly TimeSpan[] NoRetryPatterns = new TimeSpan[0];
        public IEnumerable<TimeSpan> RetryPatterns { get; set; }

        public Predicate<Exception> RetryOn { get; set; }

        public RetryMessageSendingHandler(HandleNextMessage next) : base(next)
        {
        }

        public override async Task SendAsync(SendMessageContext context)
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            var enumerator = (RetryPatterns ?? NoRetryPatterns).GetEnumerator();
            var exceptions = new List<Exception>();

            do
            {
                try
                {
                    await Next(context);
                    break;
                }
                catch (Exception ex)
                {
                    if (RetryOn != null && !RetryOn(ex))
                        throw;
                }

                if (enumerator.MoveNext())
                {
                    var timeSpan = enumerator.Current;
                    await Task.Delay(timeSpan);
                }
                else throw new AggregateException(exceptions.ToArray());

            } while (true);

            context.CancellationToken.ThrowIfCancellationRequested();
        }
    }
}
