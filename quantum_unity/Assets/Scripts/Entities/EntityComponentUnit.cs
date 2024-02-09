using Quantum;
using UnityEngine;
using TowerRush;
using TowerRush.Core;
using Photon.Deterministic;

public unsafe partial class EntityComponentUnit
{
	// CONSTANTS

	private static readonly int HASH_DEAD   = Animator.StringToHash("Dead");
	private static readonly int HASH_MOVE   = Animator.StringToHash("Move");
	private static readonly int HASH_ATTACK = Animator.StringToHash("Attack");

	// PUBLIC MEMBERS

	public Vector3 HUDPivotPosition { get { return m_HUDPosition != null ? m_HUDPosition.position : transform.position; } }
	public float   HUDScale         { get { return m_HUDScale; } }
	public bool    IsAlive          { get; set; }

	// CONFIGURATION

	[SerializeField] Transform      m_HUDPosition;
	[SerializeField] float          m_HUDScale;

	[SerializeField] MaterialInfo[] m_TeamMaterials;
	[SerializeField] Material       m_FriendlyMaterial;
	[SerializeField] Material       m_EnemyMaterial;

	[SerializeField] GameObject     m_AliveGroup;
	[SerializeField] GameObject     m_DeadGroup;

	// PRIVATE MEMBERS

	private Animator  m_Animator;
	private FPVector2 m_LastPosition;

	// EntityComponent INTERFACE

	protected override void OnInitialize()
	{
		base.OnInitialize();

		Entity.OnEntityInstantiated.AddListener(OnQuantumEntityInstantiated);
		Entity.OnEntityDestroyed.AddListener(OnQuantumEntityDestroyed);

		m_Animator = GetComponentInChildren<Animator>(true);
	}

	protected override void OnDeinitialize()
	{
		Entity.OnEntityInstantiated.RemoveListener(OnQuantumEntityInstantiated);
		Entity.OnEntityDestroyed.RemoveListener(OnQuantumEntityDestroyed);

		base.OnDeinitialize();
	}

	protected override void OnUpdate(SceneContext context)
	{
		if (m_Animator != null)
		{
			var transform2D = context.Frame.Unsafe.GetPointer<Transform2D>(Entity.EntityRef);
			var moving      = FPVector2.DistanceSquared(transform2D->Position, m_LastPosition) > FP._0;

			m_Animator.SetBool(HASH_MOVE, moving);

			m_LastPosition = transform2D->Position;
		}
	}

	// HANDLERS

	private void OnQuantumEntityInstantiated(QuantumGame game)
	{
		var frame = QuantumRunner.Default.Game.Frames.Predicted;

		QuantumEvent.Subscribe<EventDeath>(this, OnDeathEvent);
		QuantumEvent.Subscribe<EventAttackStart>(this, OnAttackStartEvent);
		Signals.LocalPlayerChanged.Connect(OnLocalPlayerChanged);

		m_LastPosition = frame.Unsafe.GetPointer<Transform2D>(Entity.EntityRef)->Position;

		SetVisual();
		SetDeathParams(false);
	}

	private void OnQuantumEntityDestroyed(QuantumGame game)
	{
		SetDeathParams(true);

		QuantumEvent.UnsubscribeListener<EventDeath>(this);
		QuantumEvent.UnsubscribeListener<EventAttackStart>(this);
		Signals.LocalPlayerChanged.Disconnect(OnLocalPlayerChanged);
	}

	private void OnDeathEvent(EventDeath death)
	{
		if (death.Entity != Entity.EntityRef)
			return;

		SetDeathParams(true);
	}

	private void OnLocalPlayerChanged(int playerRef, EntityRef playerEntity)
	{
		SetVisual();
	}

	private void OnAttackStartEvent(EventAttackStart attackStart)
	{
		if (m_Animator == null)
			return;
		if (Entity.EntityRef != attackStart.Entity)
			return;

		m_Animator.SetTrigger(HASH_ATTACK);
	}

	// PRIVATE METHODS

	private void SetDeathParams(bool death)
	{
		if (m_AliveGroup != null)
		{
			m_AliveGroup.SetActive(death == false);
		}

		if (m_DeadGroup != null)
		{
			m_DeadGroup.SetActive(death == true);
		}

		if (m_Animator  != null)
		{
			m_Animator.SetBool(HASH_DEAD, death);
		}

		IsAlive = death == false;
	}
	
	private void SetVisual()
	{
		if (Entities.LocalPlayer < 0)
			return;

		var frame = QuantumRunner.Default.Game.Frames.Predicted;
		var unit  = frame.Unsafe.GetPointer<Unit>(Entity.EntityRef);

		var material = Entities.LocalPlayer == unit->Owner ? m_FriendlyMaterial : m_EnemyMaterial;

		for (int idx = 0, count = m_TeamMaterials.Length; idx < count; idx++)
		{
			var teamMaterial = m_TeamMaterials[idx];

			var materials = teamMaterial.Renderer.sharedMaterials;

			for (int idy = 0, countY = teamMaterial.MaterialIndex.Length; idy < countY; idy++)
			{
				materials[teamMaterial.MaterialIndex[idy]] = material;
			}

			teamMaterial.Renderer.sharedMaterials = materials;
		}
	}

	[System.Serializable]
	private struct MaterialInfo
	{
		public MeshRenderer Renderer;
		public int[]        MaterialIndex;
	}
}
