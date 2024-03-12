namespace Quantum.Services
{
	public interface IPartyMessage : IMessage
	{
	}

	public static partial class PartyMessages
	{
		//=============================================================================================================

		public sealed class Text : ChannelMessage, IPartyMessage
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

		public sealed class Join : PrivateMessage, IPartyMessage
		{
			public string PartyID { get; private set; }

			public Join(string partyID)
			{
				PartyID = partyID;
			}

			private Join()
			{
			}

			protected override object Serialize()
			{
				return PartyID;
			}

			protected override void Deserialize(object data)
			{
				PartyID = (string)data;
			}
		}

		//=============================================================================================================

		public sealed class Invite : PrivateMessage, IPartyMessage
		{
			public string PartyID    { get; private set; }
			public float  Timeout    { get; private set; }
			public int    MaxPlayers { get; private set; }

			public Invite(string partyID, float timeout, int maxPlayers)
			{
				PartyID    = partyID;
				Timeout    = timeout;
				MaxPlayers = maxPlayers;
			}

			private Invite()
			{
			}

			protected override object Serialize()
			{
				return new object[] { PartyID, Timeout, MaxPlayers };
			}

			protected override void Deserialize(object data)
			{
				PartyID    = (string)((object[])data)[0];
				Timeout    =  (float)((object[])data)[1];
				MaxPlayers =    (int)((object[])data)[2];
			}
		}

		//=============================================================================================================

		public sealed class Decline : PrivateMessage, IPartyMessage
		{
			public string PartyID { get; private set; }

			public Decline(string partyID)
			{
				PartyID = partyID;
			}

			private Decline()
			{
			}

			protected override object Serialize()
			{
				return PartyID;
			}

			protected override void Deserialize(object data)
			{
				PartyID = (string)data;
			}
		}

		//=============================================================================================================

		public sealed class Leave : ChannelMessage, IPartyMessage
		{
			public Leave()
			{
			}
		}

		//=============================================================================================================

		public sealed class Kick : ChannelMessage, IPartyMessage
		{
			public string UserID { get; private set; }
			public string Reason { get; private set; }

			public Kick(string userID, string reason)
			{
				UserID = userID;
				Reason = reason;
			}

			private Kick()
			{
			}

			protected override object Serialize()
			{
				return new object[] { UserID, Reason };
			}

			protected override void Deserialize(object data)
			{
				UserID = (string)((object[])data)[0];
				Reason = (string)((object[])data)[1];
			}
		}

		//=============================================================================================================
	}
}
