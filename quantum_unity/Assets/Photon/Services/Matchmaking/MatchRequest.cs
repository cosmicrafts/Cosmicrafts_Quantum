namespace Quantum.Services
{
	using System;
	using System.Collections.Generic;
	using Photon.Realtime;

	using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

	public enum EMatchRequestType
	{
		None               = 0,
		Join               = 1,
		Create             = 2,
		JoinOrCreate       = 3,
		JoinRandom         = 4,
		CreateRandom       = 5,
		JoinOrCreateRandom = 6,
	}

	public sealed partial class MatchRequest
	{
		//========== PUBLIC MEMBERS ===================================================================================

		public EMatchRequestType Type;
		public string            Room;
		public string            Plugin;
		public bool              IsOpen                 = true;
		public bool              IsVisible              = true;
		public bool              IsSpectator            = false;
		public bool              AutoStart              = true;
		public int               MinStartPlayers        = 0;
		public int               ExpectedPlayers        = 0;
		public int               ExtraSlots             = 0;
		public int               MatchTTL               = 0;
		public int               PlayerTTL              = 0;
		public int               FillTimeout            = 0;
		public MatchConfig       Config                 = new MatchConfig();
		public PhotonHashtable   PlayerProperties       = new PhotonHashtable();
		public PhotonHashtable   ExpectedRoomProperties = new PhotonHashtable();
		public PhotonHashtable   RoomProperties         = new PhotonHashtable();
		public List<string>      RoomPropertiesForLobby = new List<string>();
		public List<string>      ExpectedUserIDs        = new List<string>();
		public MatchmakingMode   MatchmakingMode        = MatchmakingMode.FillRoom;
		public TypedLobby        TypedLobby             = TypedLobby.Default;
		public string            SQLLobbyFilter         = null;

		//========== PUBLIC METHODS ===================================================================================

		public bool IsValid()
		{
			if (IsVisible == true && ExtraSlots > 0)
			{
				throw new NotSupportedException("Extra slots are allowed only in private matches");
			}

			if (MinStartPlayers < 0) { throw new ArgumentException(nameof(MinStartPlayers)); }
			if (ExpectedPlayers < 0) { throw new ArgumentException(nameof(ExpectedPlayers)); }
			if (ExtraSlots      < 0) { throw new ArgumentException(nameof(ExtraSlots));      }
			if (MatchTTL        < 0) { throw new ArgumentException(nameof(MatchTTL));        }
			if (PlayerTTL       < 0) { throw new ArgumentException(nameof(PlayerTTL));       }
			if (FillTimeout     < 0) { throw new ArgumentException(nameof(FillTimeout));     }

			if (Type != EMatchRequestType.Join && Type != EMatchRequestType.JoinRandom)
			{
				if (ExpectedPlayers <= 0) { throw new ArgumentException(nameof(ExpectedPlayers)); }
			}

			switch (Type)
			{
				case EMatchRequestType.Join:
				case EMatchRequestType.Create:
				case EMatchRequestType.JoinOrCreate:
					if (Room.HasValue() == false) { throw new ArgumentException(nameof(Room)); }
					break;
				case EMatchRequestType.JoinRandom:
				case EMatchRequestType.CreateRandom:
				case EMatchRequestType.JoinOrCreateRandom:
					if (IsSpectator == true) { throw new NotSupportedException(nameof(IsSpectator)); }
					break;
				default:
					throw new NotSupportedException(Type.ToString());
			}

			return true;
		}
	}
}
