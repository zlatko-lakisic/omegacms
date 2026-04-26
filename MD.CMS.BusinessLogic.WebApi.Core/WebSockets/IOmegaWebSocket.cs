using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MD.CMS.BusinessLogic.WebApi.Core.WebSockets
{
    public interface IOmegaWebSocket
    {
        IEnumerable<string> UrlsToBindTo { get; }
        int MilisecondDelay { get; }
        Task OnConnectedAsync(OmegaSocketContext context);
        Task OnCloseAsync(OmegaSocketContext context);
        Task OnReceiveAsync(OmegaSocketContext context);
        Task<Stream> OnSendAsync(OmegaSocketContext context);
        Task<Stream> OnBroadcastAsync(OmegaSocketContext context);
        IOmegaWebSocket Clone();
    }
}
