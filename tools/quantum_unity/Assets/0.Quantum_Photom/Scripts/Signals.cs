namespace TowerRush
{
	using Quantum;

	public static class Signals
	{
		public static Signal                 GameOptionsChanged       = new Signal();

		public static Signal<int, EntityRef> LocalPlayerChanged       = new Signal<int, EntityRef>();

		public static Signal<Entity>         EntityCreated            = new Signal<Entity>();
		public static Signal<Entity>         EntityDestroyed          = new Signal<Entity>();
	}
}