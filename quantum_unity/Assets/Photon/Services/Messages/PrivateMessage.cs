namespace Quantum.Services
{
	using Photon.Chat;

	public static partial class PrivateMessages
	{
		public sealed class Text : PrivateMessage
		{
			public string Message { get; private set; }

			public Text(string message)
			{
				Message = message;
			}

			private Text()
			{
			}

			protected override object Serialize()
			{
				return Message;
			}

			protected override void Deserialize(object data)
			{
				Message = (string)data;
			}
		}
	}

	public abstract class PrivateMessage : Message
	{
		protected override sealed string GetChannel(ChatClient client, string receiver)
		{
			return client.GetPrivateChannelNameByUser(receiver);
		}

		protected override sealed void Send(ChatClient client, string receiver, object data)
		{
			client.SendPrivateMessage(receiver, data);
		}
	}
}
