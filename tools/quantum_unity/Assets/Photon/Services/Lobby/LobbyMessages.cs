namespace Quantum.Services
{
	public interface ILobbyMessage : IMessage
	{
	}

	public static partial class LobbyMessages
	{
		//=============================================================================================================

		public sealed class Text : ChannelMessage, ILobbyMessage
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

		//=============================================================================================================
	}
}
