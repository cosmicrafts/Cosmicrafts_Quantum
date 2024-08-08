using EdjCase.ICP.Candid.Mapping;
using Cosmicrafts.MainCanister.Models;
using System;

namespace Cosmicrafts.MainCanister.Models
{
	[Variant]
	public class Category
	{
		[VariantTagProperty]
		public CategoryTag Tag { get; set; }

		[VariantValueProperty]
		public object? Value { get; set; }

		public Category(CategoryTag tag, object? value)
		{
			this.Tag = tag;
			this.Value = value;
		}

		protected Category()
		{
		}

		public static Category Avatar(AvatarMetadata info)
		{
			return new Category(CategoryTag.Avatar, info);
		}

		public static Category Character(CharacterMetadata info)
		{
			return new Category(CategoryTag.Character, info);
		}

		public static Category Chest(ChestMetadata info)
		{
			return new Category(CategoryTag.Chest, info);
		}

		public static Category Trophy(TrophyMetadata info)
		{
			return new Category(CategoryTag.Trophy, info);
		}

		public static Category Unit(Unit info)
		{
			return new Category(CategoryTag.Unit, info);
		}

		public AvatarMetadata AsAvatar()
		{
			this.ValidateTag(CategoryTag.Avatar);
			return (AvatarMetadata)this.Value!;
		}

		public CharacterMetadata AsCharacter()
		{
			this.ValidateTag(CategoryTag.Character);
			return (CharacterMetadata)this.Value!;
		}

		public ChestMetadata AsChest()
		{
			this.ValidateTag(CategoryTag.Chest);
			return (ChestMetadata)this.Value!;
		}

		public TrophyMetadata AsTrophy()
		{
			this.ValidateTag(CategoryTag.Trophy);
			return (TrophyMetadata)this.Value!;
		}

		public Unit AsUnit()
		{
			this.ValidateTag(CategoryTag.Unit);
			return (Unit)this.Value!;
		}

		private void ValidateTag(CategoryTag tag)
		{
			if (!this.Tag.Equals(tag))
			{
				throw new InvalidOperationException($"Cannot cast '{this.Tag}' to type '{tag}'");
			}
		}
	}

	public enum CategoryTag
	{
		[CandidName("avatar")]
		Avatar,
		[CandidName("character")]
		Character,
		[CandidName("chest")]
		Chest,
		[CandidName("trophy")]
		Trophy,
		[CandidName("unit")]
		Unit
	}
}