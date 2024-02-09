namespace Quantum.Services
{
	using System.Collections.Generic;

	public static partial class PartyExtensions
	{
		//========== PUBLIC METHODS ===================================================================================

		public static string[] GetPartyUserIDs(this Party party)
		{
			if (party.IsValid == false)
				return null;

			List<string> userIDs = new List<string>();
			foreach (PartyPlayer player in party.Players.All)
			{
				userIDs.Add(player.UserID);
			}

			return userIDs.ToArray();
		}
	}
}
