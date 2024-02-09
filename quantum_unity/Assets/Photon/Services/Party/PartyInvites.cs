namespace Quantum.Services
{
	using System;
	using System.Collections.Generic;
	using UnityEngine;

	public sealed class PartyInvite
	{
		public int    Slot;
		public string UserID;
		public float  Timeout;
	}

	//=================================================================================================================

	public sealed class PartyInvites
	{
		//========== PUBLIC MEMBERS ===================================================================================

		public List<PartyInvite> All { get { return _invites; } }

		//========== PRIVATE MEMBERS ==================================================================================

		private List<PartyInvite> _invites = new List<PartyInvite>();

		//========== PUBLIC METHODS ===================================================================================

		public bool Contains(string userID)
		{
			return Get(userID) != null;
		}

		public bool Contains(int slot)
		{
			return Get(slot) != null;
		}

		public PartyInvite Get(string userID)
		{
			if (userID.HasValue() == false)
			{
				throw new ArgumentNullException();
			}

			foreach (PartyInvite invite in _invites)
			{
				if (invite.UserID == userID)
				{
					return invite;
				}
			}

			return null;
		}

		public PartyInvite Get(int slot)
		{
			foreach (PartyInvite invite in _invites)
			{
				if (invite.Slot == slot)
				{
					return invite;
				}
			}

			return null;
		}

		public PartyInvite Add(string userID, int slot, float timeout)
		{
			if (userID.HasValue() == false)
			{
				throw new ArgumentNullException();
			}

			PartyInvite invite = new PartyInvite();
			invite.UserID  = userID;
			invite.Slot    = slot;
			invite.Timeout = Time.realtimeSinceStartup + timeout;

			_invites.Add(invite);

			return invite;
		}

		public bool Remove(string userID)
		{
			for (int i = 0; i < _invites.Count; ++i)
			{
				if (_invites[i].UserID == userID)
				{
					_invites.RemoveAt(i);
					return true;
				}
			}

			return false;
		}

		public void Clear()
		{
			_invites.Clear();
		}

		public void CopyTo(List<PartyInvite> invites)
		{
			invites.Clear();
			invites.AddRange(_invites);
		}
	}
}
