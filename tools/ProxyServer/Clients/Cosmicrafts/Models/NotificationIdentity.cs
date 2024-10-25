using EdjCase.ICP.Candid.Mapping;
using ProxyServer.Cosmicrafts.Models;
using System;
using UserID = EdjCase.ICP.Candid.Models.Principal;

namespace ProxyServer.Cosmicrafts.Models
{
	[Variant]
	public class NotificationIdentity
	{
		[VariantTagProperty]
		public NotificationIdentityTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public NotificationIdentity(NotificationIdentityTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected NotificationIdentity()
		{
		}

		public static NotificationIdentity FriendAdded(UserID info)
		{
			return new NotificationIdentity(NotificationIdentityTag.FriendAdded, info);
		}

		public static NotificationIdentity FriendRequest(UserID info)
		{
			return new NotificationIdentity(NotificationIdentityTag.FriendRequest, info);
		}

		public static NotificationIdentity UserMessage(UserID info)
		{
			return new NotificationIdentity(NotificationIdentityTag.UserMessage, info);
		}

		public UserID AsFriendAdded()
		{
			this.ValidateTag(NotificationIdentityTag.FriendAdded);
			return (UserID)this.Value!;
		}

		public UserID AsFriendRequest()
		{
			this.ValidateTag(NotificationIdentityTag.FriendRequest);
			return (UserID)this.Value!;
		}

		public UserID AsUserMessage()
		{
			this.ValidateTag(NotificationIdentityTag.UserMessage);
			return (UserID)this.Value!;
		}

		private void ValidateTag(NotificationIdentityTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum NotificationIdentityTag
	{
		FriendAdded,
		FriendRequest,
		UserMessage
	}
}