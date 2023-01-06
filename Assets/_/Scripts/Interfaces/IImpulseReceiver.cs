using UnityEngine;

namespace Prototype
{
    public interface IMessageReceiver
    {
        public static void SendMessage<T>(Object recipient, string messageName, T parameter)
        {
            foreach (var messageReceiver in recipient.GetComponents<IMessageReceiver>())
                messageReceiver.ReceiveMessage(messageName, parameter);
        }

        public void ReceiveMessage<T>(string messageName, T parameter);
    }
}