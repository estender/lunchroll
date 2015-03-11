using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InstructionsPanel : MonoBehaviour {

	public float hideDelay;


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
	/// Show the panel on the game start event.
	/// </summary>
	/// <param name="args">Game Start Arguments.</param>
	void OnGameStart(GameStartArgs args)
	{
		gameObject.SetActive(true);

		StartCoroutine(DelayedDisable());
	}

	/// <summary>
	/// Waits a specified number of seconds then disables the window
	/// </summary>
	IEnumerator DelayedDisable ()
	{
		yield return new WaitForSeconds(hideDelay);
		
		gameObject.SetActive(false);
	}

	/// <summary>
	/// Hide the panel on game over (in case game ends before DelayedDisable() triggers)
	/// </summary>
	/// <param name="args">Game Over Arguments.</param>
	void OnGameOver(GameOverArgs args)
	{
		gameObject.SetActive(false);
	}
}
