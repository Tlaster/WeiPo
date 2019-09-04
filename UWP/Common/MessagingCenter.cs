using System;
using System.Collections.Concurrent;
using System.Linq;

namespace WeiPo.Common
{
    internal class MessagingCenter
    {
        private readonly ConcurrentDictionary<Guid, (string message, Action<object, object> action)> _listeners =
            new ConcurrentDictionary<Guid, (string message, Action<object, object> action)>();

        public void Send(object sender, string message, object args = null)
        {
            foreach (var item in _listeners.Where(it => it.Value.message == message))
            {
                item.Value.action.Invoke(sender, args);
            }
        }

        public Guid Subscribe(string message, Action<object, object> action)
        {
            var id = Guid.NewGuid();
            _listeners.TryAdd(id, (message, action));
            return id;
        }

        public void Unsubscribe(Guid id)
        {
            _listeners.TryRemove(id, out _);
        }
    }
}