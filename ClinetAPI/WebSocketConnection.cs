using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionAPI
{
    public abstract class WebSocketConnection
    {
        public virtual Action<string,Guid>? OnMessage { set; protected get; } = (x, y) => { };
        public virtual Action<Guid>? OnClose { set; protected get; } = (x) => { };
        public virtual Action<Guid>? OnError { set; protected get; } = (x) => { };

        public virtual Guid connectionID {protected set; get; }

        public async Task SendAsync(string message)
        {
            await SendTask(message);
        }

        public abstract Task DisconnectAsync();

        protected abstract Task SendTask(string message);
    }
}
