using System.Threading.Tasks;

namespace ModelSoft.Messaging
{
    public delegate Task HandleNextMessage(SendMessageContext context);
}