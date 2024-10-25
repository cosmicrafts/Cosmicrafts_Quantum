namespace Quantum
{
	using Photon.Deterministic;

	partial class RuntimePlayer
	{
		public byte       Level;
		public CardInfo[] Cards;

		partial void SerializeUserData(BitStream stream)
		{
			stream.Serialize(ref Level);

			if (stream.Writing)
			{
				stream.WriteByte((byte)Cards.Length);

				for (int idx = 0, count = Cards.Length; idx < count; idx++)
				{
					var card = Cards[idx];
					stream.WriteByte(card.Level);
					stream.WriteLong(card.CardSettings.Id.Value);
					stream.WriteFP(card.BaseHealth);
					stream.WriteFP(card.Damage);
					stream.WriteInt(card.TokenID);
				}
			}
			else if (stream.Reading)
			{
				var count = stream.ReadByte();
				Cards     = new CardInfo[count];

				for (int idx = 0; idx < count; idx++)
				{
					Cards[idx] = new CardInfo()
					{
						Level        = stream.ReadByte(),
						CardSettings = new AssetRefCardSettings() { Id = new AssetGuid(stream.ReadLong()) },
						BaseHealth = stream.ReadFP(),
						Damage = stream.ReadFP(),
						TokenID = stream.ReadInt(),
					};
				}
			}
		}
	}
}
