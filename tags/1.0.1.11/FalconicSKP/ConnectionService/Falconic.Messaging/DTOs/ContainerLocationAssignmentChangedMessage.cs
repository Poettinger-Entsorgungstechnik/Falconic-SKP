using System;

namespace Falconic.Messaging.DTOs
{
    /// <summary>
    /// Message to inform SKP when container/locationFraction assignment changed.
    /// (Uses LogicalIds for <see cref="ContainerId"/> and <see cref="LocationId"/>.)
    /// </summary>
    public class ContainerLocationAssignmentChangedMessage : IQueueInnerMessage
    {
        public string Type => QueueMessageType.ContainerLocationAssignmentChangedMessage;

        public DateTime UtcTimestamp { get; set; }
        public int ContainerId { get; set; }
        public string GsmNumber { get; set; }
        public int LocationId { get; set; }
    }
}
