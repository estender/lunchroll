using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {

	/// <summary>
	/// Text object to diplay "you win" / "game over"
	/// </summary>
	public Text winText;

	/// <summary>
	/// Canvas containing ingame UI.
	/// </summary>
	public Canvas ingameUI;
	
	/// <summary>
	/// Register listeners on awake
	/// </summary>
	void Awake()
	{
		GameEventManager.GameStart += OnGameStart;
		GameEventManager.GameOver += OnGameOver;
	}

	/// <summary>
	/// Remove listeners on destroy
	/// </summary>
	void OnDestroy()
	{
		GameEventManager.GameStart -= OnGameStart;
		GameEventManager.GameOver -= OnGameOver;
	}

	/// <summary>
	/// Hide the window on the game start event.
	/// </summary>
	/// <param name="args">Game Start Arguments.</param>
	void OnGameStart(GameStartArgs args)
	{
		// disable menu & enable GUI
		gameObject.SetActive(false);
		ingameUI.gameObject.SetActive(true);
	}

	/// <summary>
	/// Set win text and show menu on game over
	/// </summary>
	/// <param name="args">Game Over Arguments.</param>
	void OnGameOver(GameOverArgs args)
	{
		// set win text
		if (args.IsWin)
		{
			winText.text = "You Win!";
			winText.color = Color.green;
		}
		else
		{
			winText.text = "GAME OVER";
			winText.color = Color.red;
		}

		// enable menu & disable GUI
		gameObject.SetActive(true);
		ingameUI.gameObject.SetActive(false);
	}

}
