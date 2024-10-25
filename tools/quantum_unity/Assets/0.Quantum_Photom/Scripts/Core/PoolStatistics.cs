namespace TowerRush.Core
{
	using System.Collections.Generic;
	
	public abstract class PoolStatistics
	{
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		
		public readonly static   List<PoolStatistics>       List           = new List<PoolStatistics>(8);
		
		public                   System.Type                CollectionType { get; protected set; }
		public                   System.Type                ElementType    { get; protected set; }
		public                   uint                       GetsCount      { get; private set; }
		public                   uint                       ReturnsCount   { get; private set; }
		private                  uint                       TimeStamp      { get; set; }
		
		///////////////////////////////////////////////////////////////////////////////////////////////////////////////
		
		//-------------------------------------------------------------------------------------------------------------
		protected PoolStatistics()
		{
			List.Add(this);
		}
		
		//-------------------------------------------------------------------------------------------------------------
		public void OnGet()
		{
			++GetsCount;
			TimeStamp = (uint) System.Environment.TickCount;    // number of milliseconds elapsed since the system started
		}
		
		//-------------------------------------------------------------------------------------------------------------
		public void OnReturn()
		{
			++ReturnsCount;
			TimeStamp = (uint) System.Environment.TickCount;
		}
		
		//-------------------------------------------------------------------------------------------------------------
		public abstract int GetCollectionsCount();
		
		//-------------------------------------------------------------------------------------------------------------
		public abstract int GetCollectionCapacity(int index);
		
		//-------------------------------------------------------------------------------------------------------------
		public float GetSecondsFromLastAccess()
		{
			var ticks = (uint) System.Environment.TickCount;
			
			return (float)( ticks - TimeStamp ) / 1000f;        // only up to 49.71 days !!!
		}
	}
}
