using System.Collections.Generic;
using System.Threading.Tasks;
using Falconic.Messaging.DTOs;

namespace Falconic.Messaging.Contracts
{
    public interface INotificationQueueWriter
    {
        Task WriteMessageToQueueAsync(IQueueInnerMessage message);
        Task WriteMessagesToQueueAsync(IEnumerable<IQueueInnerMessage> message);
    }
}
