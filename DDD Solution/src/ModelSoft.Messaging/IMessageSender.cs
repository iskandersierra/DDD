using System.Threading;
using System.Threading.Tasks;

namespace ModelSoft.Messaging
{
    /// <summary>
    /// This class represents an asynchronous message sending point for messages like commands, 
    /// events and the like
    /// </summary>
    public interface IMessageSender
    {
        Task SendMessageAsync(Message message, CancellationToken cancellationToken);
    }
}
