using UnityEngine;
using System.Collections;

public class FallTriggerController : MonoBehaviour {

	/// <summary>
	/// Triggers game over when colliding with object tagged as "Player"
	/// </summary>
	/// <param name="other">Colliding game object</param>
	void OnTriggerEnter (Collider other) {
		// if player falls off,
		if (other.gameObject.tag == "Player") {

			// play game over audio
			audio.Play();

			// trigger loss
			GameOverArgs gameOver = new GameOverArgs();
			gameOver.IsWin = false;
			GameEventManager.TriggerGameOver(gameOver);
		}
	}
}
