using System;
using System.Collections.Generic;
using System.Text;

namespace Falconic.Messaging.DTOs
{
    public interface IQueueInnerMessage
    {
        string Type { get; }
    }
}
