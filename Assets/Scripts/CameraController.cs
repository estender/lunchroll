using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	/// <summary>
	/// The player.
	/// </summary>
	public GameObject player;

	/// <summary>
	/// Configurable modifier for the rotation speed of camera
	/// </summary>
	public float tiltSpeed;

	/// <summary>
	/// Vector to keep constant distance between the player and camera
	/// </summary>
	private Vector3 offset;

	/// <summary>
	/// Quaternion to store initial rotation of camera
	/// </summary>
	private Quaternion initialRotation;

	void OnEnable()
	{
		GameEventManager.GameStart += OnGameStart;
	}
	
	void OnDisable()
	{
		GameEventManager.GameStart -= OnGameStart;
	}
	
	void OnGameStart(GameStartArgs args)
	{
		transform.rotation = initialRotation;
		transform.position = player.transform.position + offset;
	}

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Awake () {

		// store initial distance between player and camera
		offset = transform.position;
		initialRotation = transform.rotation;

		// target player for use with Transform.RotateAround()
		transform.LookAt(player.transform);
	}
	
	// LateUpdate is called once per frame after Update()
	void LateUpdate () {

		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		// follow the player
		transform.position = player.transform.position + offset;

		// rotate the camera around the player to give the appearance of the whole map rotating
		transform.RotateAround(player.transform.position, Vector3.forward, horizontal * tiltSpeed * Time.deltaTime);
		transform.RotateAround(player.transform.position, Vector3.left, vertical * tiltSpeed * Time.deltaTime);

	}
}
