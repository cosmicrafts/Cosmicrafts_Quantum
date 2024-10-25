using Quantum.Prototypes.Unity;
using UnityEngine;

public unsafe partial class EntityComponentGameplay
{
	private static readonly Color BLOCKED_COLOR = new Color(1f, 0f, 0f, 0.45f);

	// CONFIGURATION

	[Header("Unity")]
	[SerializeField] bool m_ShowAlphaBlockedArea;
	[SerializeField] bool m_ShowBetaBlockedArea;

	// EDITOR

	private void OnDrawGizmosSelected()
	{
		if (m_ShowAlphaBlockedArea == true)
		{
			DrawBlockedArea(Prototype.AlphaArea);
		}
		if (m_ShowBetaBlockedArea == true)
		{
			DrawBlockedArea(Prototype.BetaArea);
		}
	}

	private void DrawBlockedArea(PlayerArea_Prototype area)
	{
		var tmpColor = Gizmos.color;
		Gizmos.color = BLOCKED_COLOR;

		for (int idx = 0, count = area.BlockedAreas.Length; idx < count; idx++)
		{
			var block = area.BlockedAreas[idx];
			if (block.Enabled == false)
				continue;

			var center = new Vector3((block.MinX + block.MaxX) / 2f, 0f, (block.MinY + block.MaxY) / 2f);
			var size   = new Vector3((block.MaxX - block.MinX)     , 1f, (block.MaxY - block.MinY));

			Gizmos.DrawCube(center, size);
		}

		Gizmos.color = tmpColor;
	}
}
