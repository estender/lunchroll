using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	/// <summary>
	/// Player movement multiplier
	/// </summary>
	public float Speed;

	/// <summary>
	/// Amount player should grow in scale from collecting a treat.
	/// Values over 1 grow the player bigger, under 1 smaller
	/// 1.0f disables growth
	/// </summary>
	public float GrowthMultiplier;

	public AudioClip pickupSound;

	/// <summary>
	/// Number of treats picked up by the player
	/// </summary>
	private int score = 0;

	/// <summary>
	/// Flag to disable player movement
	/// </summary>
	private bool movementEnabled = false;



	/// <summary>
	/// Add game start/game over listeners on enable
	/// </summary>
	void OnEnable()
	{
		GameEventManager.GameStart += OnGameStart;
		GameEventManager.GameOver += OnGameOver;
	}

	/// <summary>
	/// Remove listeners on disable
	/// </summary>
	void OnDisable()
	{
		GameEventManager.GameStart -= OnGameStart;
		GameEventManager.GameOver -= OnGameOver;
	}

	/// <summary>
	/// Resets player position and score on game start
	/// </summary>
	/// <param name="args">Game start arguments</param>
	void OnGameStart(GameStartArgs args)
	{
		score = 0;
		movementEnabled = true;

		transform.position = new Vector3(0, 1.0f, 0);
		rigidbody.constantForce.force = Vector3.zero;
		rigidbody.useGravity = true;
	}

	/// <summary>
	/// Stops player movement when the game over event triggers
	/// </summary>
	/// <param name="args">Game over arguments</param>
	void OnGameOver(GameOverArgs args)
	{
		movementEnabled = false;
		rigidbody.constantForce.force = Vector3.zero;
		rigidbody.useGravity = false;
	}


	/// <summary>
	/// Add a constant force to the player when moved
	/// </summary>
	void FixedUpdate () {

		if (movementEnabled)
		{
			float horizontal = Input.GetAxis ("Horizontal");
			float vertical = Input.GetAxis ("Vertical");

			Vector3 movement = new Vector3 (horizontal, 0.0f, vertical);

			// add constant force to player based on input
			rigidbody.constantForce.force += movement * Speed;
		}
	}

	/// <summary>
	/// Raises the trigger enter event.
	/// </summary>
	/// <param name="other">Other object colliding with.</param>
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "Treasure") {
			// hide pickup
			other.gameObject.SetActive (false);

			// increment score
			score++;

			// play collect audio
			audio.Play();

			// apply player growth multiplier
			transform.localScale = transform.localScale * GrowthMultiplier;

			// trigger item pickup event
			ItemCollectedArgs eventArgs = new ItemCollectedArgs();
			eventArgs.Count = score;
			GameEventManager.TriggerItemCollected(eventArgs);
		}
	}
}
