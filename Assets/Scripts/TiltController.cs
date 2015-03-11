using UnityEngine;
using System.Collections;

public class TiltController : MonoBehaviour {

	public float speedMultiplier;

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
		transform.rotation = Quaternion.identity;
	}

	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		Vector3 tilt = new Vector3 (vertical * speedMultiplier, 0.0f, -1 * horizontal * speedMultiplier);

		transform.Rotate (tilt * Time.deltaTime);
	}
}
