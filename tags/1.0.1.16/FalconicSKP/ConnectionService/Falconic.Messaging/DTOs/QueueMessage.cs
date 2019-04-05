using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Falconic.Messaging.DTOs
{
    public class QueueMessage
    {
        public static QueueMessage Create<TInnerMessage>(TInnerMessage innerMessage)
            where TInnerMessage : IQueueInnerMessage
        {
            return new QueueMessage(innerMessage.Type, JsonConvert.SerializeObject(innerMessage));
        }

        [JsonConstructor]
        private QueueMessage(string type, string innerMessage)
        {
            this.Type = type;
            this.InnerMessage = innerMessage;
        }

        public string Type { get; }

        public string InnerMessage { get; }
    }
}
