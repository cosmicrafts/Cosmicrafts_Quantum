﻿using Quantum;
using UnityEngine;

public partial class CardSettingsAsset
{
	[Header("Unity")]
	public GameObject GhostPrefab;
	public GameObject prevUIPrefab;

	[Header("UI")]
	public Sprite Sprite;
	public string DisplayName;

	public abstract byte      GetEnergyCost();
	public abstract AssetGuid GetAssetGuid();
	public abstract ERarity   GetRarity();
}
