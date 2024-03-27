namespace Quantum.Services
{
	public interface IService
	{
		void Initialize(IServiceProvider serviceProvider);
		void Deinitialize();
		void Tick();
	}

	public interface IServiceProvider
	{
		T GetService<T>() where T : class, IService;
	}
}
