using System;
using System.Collections.Generic;

namespace ModelSoft.Messaging
{
    public class Message
    {
        private Dictionary<string, object> _headers;

        public Uri Schema { get; set; }

        public Dictionary<string, object> Headers => _headers ?? (_headers = new Dictionary<string, object>());

        public dynamic Content { get; set; }
    }
}