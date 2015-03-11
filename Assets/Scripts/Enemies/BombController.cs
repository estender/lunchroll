using UnityEngine;
using System.Collections;

public class BombController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void StartExplosion() {
		ParticleEmitter[] emitters = gameObject.GetComponentsInChildren<ParticleEmitter>();

		foreach (ParticleEmitter emitter in emitters) {
			emitter.Emit();
		}

	}
}
