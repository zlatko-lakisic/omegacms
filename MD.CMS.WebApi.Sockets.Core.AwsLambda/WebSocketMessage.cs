using System.Collections.Concurrent;

namespace MD.CMS.WebApi.Sockets.Core.AwsLambda
{
    public class WebSocketMessage
    {
        public class WebSocketMessageInner
        {
            public string address { get; set; }
            public dynamic data { get; set; }
            public ConcurrentDictionary<string, string> queryStrings { get; set; }
        }

        public string action { get; set; }
        public WebSocketMessageInner data { get; set; }
    }
}
