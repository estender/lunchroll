using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	/// <summary>
	/// Singleton implementation
	/// </summary>
	private static GameManager _instance;
	public static GameManager instance 
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GameManager>();
			}
			return _instance;
		}
	}

	/// <summary>
	/// Current difficulty selected
	/// </summary>
	private int difficulty;

	/// <summary>
	/// The current score.
	/// </summary>
	private int score;

	/// <summary>
	/// The score needed to win.
	/// </summary>
	private int scoreMax;

	/// <summary>
	/// Array of all difficuly settings.
	/// </summary>
	public DifficultySetting[] difficulySettings;

	/// <summary>
	/// Register listeners for game events on enable
	/// </summary>
	void OnEnable()
	{
		GameEventManager.GameStart += GameStart;
		GameEventManager.OnItemCollected += ItemCollected;
		GameEventManager.OnMapGenerated += MapGenerated;
	}

	/// <summary>
	/// Remove listeners for game events on disable
	/// </summary>
	void OnDisable()
	{
		GameEventManager.GameStart -= GameStart;
		GameEventManager.OnItemCollected -= ItemCollected;
		GameEventManager.OnMapGenerated -= MapGenerated;
	}


	/// <summary>
	/// Triggers the game start event
	/// </summary>
	public void TriggerGameStart()
	{
		GameStartArgs args = new GameStartArgs();
		args.Difficulty = difficulySettings[difficulty];
		
		GameEventManager.TriggerGameStart(args);
	}

	/// <summary>
	/// Resets score when the game starts.
	/// </summary>
	/// <param name="args">Game Start Arguments</param>
	public void GameStart(GameStartArgs args)
	{
		score = 0;
		scoreMax = 0;
	}

	/// <summary>
	/// Counts the score and checks for a win on item collected
	/// </summary>
	/// <param name="args">Item Collected Arguments.</param>
	void ItemCollected(ItemCollectedArgs args)
	{
		score = args.Count;

		// if at the max score, trigger win
		if (score >= scoreMax)
		{
			audio.Play();

			GameOverArgs gameOver = new GameOverArgs();
			gameOver.IsWin = true;
			GameEventManager.TriggerGameOver(gameOver);
		}
	}

	/// <summary>
	/// Store the total number of treasures when map is generated
	/// </summary>
	/// <param name="args">Map Generated Arguments.</param>
	void MapGenerated(MapGeneratedArgs args)
	{
		scoreMax = args.TreasureCount;
	}

	/// <summary>
	/// Sets the difficulty as an int (index in difficultySettings array)
	/// </summary>
	/// <param name="value">Difficulty</param>
	public void SetDifficulty(float value)
	{
		difficulty = (int)value;
	}

	/// <summary>
	/// Returns all difficulty settings
	/// </summary>
	/// <returns>The difficulty settings.</returns>
	public DifficultySetting[] GetDifficultySettings() 
	{
		return difficulySettings;
	}

}
