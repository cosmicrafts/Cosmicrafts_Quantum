using TowerRush;
using TowerRush.Core;
using Quantum;
using UnityEngine;

public unsafe partial class EntityComponentEffectArea
{
	[SerializeField] Transform m_Visual;

	protected override void OnUpdate(SceneContext context)
	{
		base.OnUpdate(context);

		var frame       = QuantumRunner.Default.Game.Frames.Predicted;
		var qEffectArea = frame.Unsafe.GetPointer<EffectArea>(Entity.EntityRef);

		var scale = qEffectArea->Radius.AsFloat * 2f;
		m_Visual.localScale = new Vector3(scale, 0.1f, scale);
	}
}
