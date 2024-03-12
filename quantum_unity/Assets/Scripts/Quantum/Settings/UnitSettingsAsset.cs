using Quantum;

public partial class UnitSettingsAsset
{
	public override byte GetEnergyCost()
	{
		return Settings.EnergyCost;
	}

	public override AssetGuid GetAssetGuid()
	{
		return Settings.Identifier.Guid;
	}

	public override ERarity GetRarity()
	{
		return Settings.Rarity;
	}
}
