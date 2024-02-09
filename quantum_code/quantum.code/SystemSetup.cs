namespace Quantum
{
	public static class SystemSetup
	{
		public static SystemBase[] CreateSystems(RuntimeConfig gameConfig, SimulationConfig simulationConfig)
		{
			return new SystemBase[] 
			{
				// pre-defined core systems
				new Core.CullingSystem2D(),

				new Core.PhysicsSystem2D(),

				new Core.NavigationSystem(),
				new Core.EntityPrototypeSystem(),

				// user systems go here
				new GameplaySystem(),
				new UnitStatsSystem(),
				new CardManagerSystem(),
				new UnitSystem(),
				new UnitAISystem(),
				new BuffSystem(),
				new WeaponSystem(),
				new ProjectileSystem(),
				new EffectAreaSystem(),
				new MovementSystem(),

				// Signals Only
			};
		}
	}
}
