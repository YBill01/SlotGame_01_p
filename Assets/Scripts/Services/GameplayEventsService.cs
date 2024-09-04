using System;

public class GameplayEventsService : IService
{
	public Action<Gameplay> Initialized;

	public Action Home;
	public Action Game;

	public Action<int, bool> StatsEnergyUpdate;


}