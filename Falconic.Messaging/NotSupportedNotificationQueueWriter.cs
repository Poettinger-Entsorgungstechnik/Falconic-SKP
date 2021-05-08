using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Falconic.Messaging.Contracts;
using Falconic.Messaging.DTOs;

namespace Falconic.Messaging
{
    /// <summary>
    /// Fake implementation to satisfy DI container.
    /// </summary>
    public class NotSupportedNotificationQueueWriter : INotificationQueueWriter
    {
        public Task WriteMessagesToQueueAsync(IEnumerable<IQueueInnerMessage> message)
        {
            throw new NotSupportedException("Writing to the notification queue is not supported in this implementation.");
        }

        public Task WriteMessageToQueueAsync(IQueueInnerMessage message)
        {
            throw new NotSupportedException("Writing to the notification queue is not supported in this implementation.");
        }
    }
}
