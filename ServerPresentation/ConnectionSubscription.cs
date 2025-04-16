using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectionAPI;
using TPUMProject.Logic.Abstract;

namespace TPUMProject.ServerPresentation
{
    internal class ConnectionSubscription : IObserver<SubscriptionEventArgs>
    {
        private Action<SubscriptionEventArgs,WebSocketConnection> _subscriptionAction;
        private WebSocketConnection _connection;
        IDisposable SubscriptionLink;

        public ConnectionSubscription(Action<SubscriptionEventArgs,WebSocketConnection> subscriptionAction,IBookService service,WebSocketConnection connection)
        {
            _subscriptionAction = subscriptionAction;
            SubscriptionLink = service.Subscribe(this);
            _connection = connection;
        }
        public void OnCompleted()
        {
            SubscriptionLink?.Dispose();
        }

        public void OnError(Exception error)
        {
           
        }

        public void OnNext(SubscriptionEventArgs value)
        {
            _subscriptionAction?.Invoke(value,_connection);
        }
    }
}
