using EdjCase.ICP.Candid.Mapping;
using CanisterPK.NFTsICHub.Models;
using EdjCase.ICP.Candid.Models;
using System;
using AccountIdentifier = System.String;

namespace CanisterPK.NFTsICHub.Models
{
	[Variant]
	public class User1
	{
		[VariantTagProperty]
		public User1Tag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public User1(User1Tag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected User1()
		{
		}

		public static User1 Address(AccountIdentifier info)
		{
			return new User1(User1Tag.Address, info);
		}

		public static User1 Principal(Principal info)
		{
			return new User1(User1Tag.Principal, info);
		}

		public AccountIdentifier AsAddress()
		{
			this.ValidateTag(User1Tag.Address);
			return (AccountIdentifier)this.Value!;
		}

		public Principal AsPrincipal()
		{
			this.ValidateTag(User1Tag.Principal);
			return (Principal)this.Value!;
		}

		private void ValidateTag(User1Tag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum User1Tag
	{
		[CandidName("address")]
		Address,
		[CandidName("principal")]
		Principal
	}
}