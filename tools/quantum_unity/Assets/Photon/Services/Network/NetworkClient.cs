namespace Quantum.Services
{
	using System.Collections.Generic;
	using ExitGames.Client.Photon;
	using Photon.Realtime;

	using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

	public sealed class NetworkClient : LoadBalancingClient
	{
		//========== CONSTRUCTORS =====================================================================================

		public NetworkClient(ConnectionProtocol protocol = ConnectionProtocol.Udp) : base(protocol)
		{
		}

        public NetworkClient(string masterAddress, string appId, string gameVersion, ConnectionProtocol protocol = ConnectionProtocol.Udp) : base(masterAddress, appId, gameVersion, protocol)
        {
        }

		//========== PUBLIC METHODS ===================================================================================

		public bool AddExpectedUser(string userID)
		{
			if (userID.HasValue() == false)
				return false;
			if (CurrentRoom == null)
				return false;

			string[]     oldExpectedUsers = CurrentRoom.ExpectedUsers;
			List<string> newExpectedUsers = new List<string>(oldExpectedUsers);

			if (newExpectedUsers.Contains(userID) == true)
				return true;

			newExpectedUsers.Add(userID);

			PhotonHashtable newRoomProperties = new PhotonHashtable();
			newRoomProperties[GamePropertyKey.ExpectedUsers] = newExpectedUsers.ToArray();
			PhotonHashtable oldRoomProperties = new PhotonHashtable();
			oldRoomProperties[GamePropertyKey.ExpectedUsers] = oldExpectedUsers;

			return OpSetPropertiesOfRoom(newRoomProperties, oldRoomProperties);
		}

		public bool RemoveExpectedUser(string userID)
		{
			if (userID.HasValue() == false)
				return false;
			if (CurrentRoom == null)
				return false;

			string[]     oldExpectedUsers = CurrentRoom.ExpectedUsers;
			List<string> newExpectedUsers = new List<string>(oldExpectedUsers);

			if (newExpectedUsers.Remove(userID) == true)
			{
				PhotonHashtable newRoomProperties = new PhotonHashtable();
				newRoomProperties[GamePropertyKey.ExpectedUsers] = newExpectedUsers.ToArray();
				PhotonHashtable oldRoomProperties = new PhotonHashtable();
				oldRoomProperties[GamePropertyKey.ExpectedUsers] = oldExpectedUsers;

				return OpSetPropertiesOfRoom(newRoomProperties, oldRoomProperties);
			}

			return false;
		}
	}
}
