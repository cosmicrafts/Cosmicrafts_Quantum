namespace Quantum.Services
{
	public interface IMatchmakingMessage : IMessage
	{
	}

	public static partial class MatchmakingMessages
	{
		//=============================================================================================================

		public sealed class Join : PrivateMessage, IMatchmakingMessage
		{
			public string Region  { get; private set; }
			public string Room    { get; private set; }
			public bool   Started { get; private set; }

			public Join(string region, string room, bool started)
			{
				Region  = region;
				Room    = room;
				Started = started;
			}

			private Join()
			{
			}

			protected override object Serialize()
			{
				return new object[] { Region, Room, Started };
			}

			protected override void Deserialize(object data)
			{
				Region  = (string)((object[])data)[0];
				Room    = (string)((object[])data)[1];
				Started =   (bool)((object[])data)[2];
			}
		}

		//=============================================================================================================

		public sealed class Invite : PrivateMessage, IMatchmakingMessage
		{
			public string Region { get; private set; }
			public string Room   { get; private set; }

			public Invite(string region, string room)
			{
				Region = region;
				Room   = room;
			}

			private Invite()
			{
			}

			protected override object Serialize()
			{
				return new object[] { Region, Room };
			}

			protected override void Deserialize(object data)
			{
				Region = (string)((object[])data)[0];
				Room   = (string)((object[])data)[1];
			}
		}

		//=============================================================================================================

		public sealed class Decline : PrivateMessage, IMatchmakingMessage
		{
			public string Room { get; private set; }

			public Decline(string room)
			{
				Room = room;
			}

			private Decline()
			{
			}

			protected override object Serialize()
			{
				return Room;
			}

			protected override void Deserialize(object data)
			{
				Room = (string)data;
			}
		}

		//=============================================================================================================
	}
}
