namespace Quantum.Services
{
	using System;
	using System.Collections.Generic;
	using Photon.Chat;

	public interface IMessage
	{
		string Sender  { get; }
		string Channel { get; }

		void Send(ChatClient client, string receiver);
	}

	public abstract class Message : IMessage
	{
		//========== PUBLIC MEMBERS ===================================================================================

		public string Sender  { get; private set; }
		public string Channel { get; private set; }

		//========== PRIVATE MEMBERS ==================================================================================

		private static readonly Dictionary<byte, Type> MessageTypeByID = new Dictionary<byte, Type>();
		private static readonly Dictionary<Type, byte> MessageIDByType = new Dictionary<Type, byte>();

		//========== CONSTRUCTORS =====================================================================================

		static Message()
		{
			List<Type> messageTypes = ReflectionUtility.GetInheritedTypes(typeof(Message), false);
			for (byte i = 0; i < messageTypes.Count; ++i)
			{
				Type messageType = messageTypes[i];

				MessageTypeByID[i]           = messageType;
				MessageIDByType[messageType] = i;
			}
		}

		protected Message()
		{
		}

		//========== PUBLIC METHODS ===================================================================================

		public static Message Get(string sender, string channel, object data)
		{
			object[] arrayData   = (object[])data;
			byte     id          = (byte)arrayData[0];
			object   messageData = arrayData[1];

			Type messageType = MessageTypeByID[id];

			Message message = Activator.CreateInstance(messageType, true) as Message;
			message.Sender  = sender;
			message.Channel = channel;
			message.Deserialize(messageData);
			return message;
		}

		//========== Message INTERFACE ================================================================================

		protected abstract string GetChannel(ChatClient client, string receiver);
		protected abstract void   Send(ChatClient client, string receiver, object data);

		protected virtual object Serialize()              { return null; }
		protected virtual void   Deserialize(object data) {}

		//========== IMessage INTERFACE ===============================================================================

		string IMessage.Sender  => Sender;
		string IMessage.Channel => Channel;

		void IMessage.Send(ChatClient client, string receiver)
		{
			Sender  = client.UserId;
			Channel = GetChannel(client, receiver);

			object[] data = new object[2];
			data[0] = MessageIDByType[GetType()];
			data[1] = Serialize();

			Send(client, receiver, data);
		}
	}
}
