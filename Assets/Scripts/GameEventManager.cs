public static class GameEventManager
{
	public delegate void GameStartEvent(GameStartArgs args);

	public delegate void GameOverEvent(GameOverArgs args);

	public delegate void ItemCollected(ItemCollectedArgs args);

	public delegate void MapGenerated(MapGeneratedArgs args);

	public static event GameStartEvent GameStart;

	public static event GameOverEvent GameOver;

	public static event ItemCollected OnItemCollected;

	public static event MapGenerated OnMapGenerated;

	public static void TriggerGameStart(GameStartArgs args)
	{
		if (GameStart != null)
		{
			GameStart(args);
		}
	}

	public static void TriggerGameOver(GameOverArgs args)
	{
		if (GameOver != null)
		{
			GameOver(args);
		}
	}

	public static void TriggerItemCollected(ItemCollectedArgs args)
	{
		if (OnItemCollected != null)
		{
			OnItemCollected(args);
		}
	}

	public static void TriggerMapGenerated(MapGeneratedArgs args)
	{
		if (OnMapGenerated != null)
		{
			OnMapGenerated(args);
		}
	}
}