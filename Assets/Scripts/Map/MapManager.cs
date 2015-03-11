using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(AMapGenerationAlgorithm))]
public class MapManager : MonoBehaviour
{


	/// <summary>
	/// Array of game objects to use as treasure
	/// </summary>
	public GameObject[] Treasures;

	/// <summary>
	/// Array of game objects to use as obstacles
	/// </summary>
	public GameObject[] Obstacles;

	/// <summary>
	/// Array of game objects to use as enemies
	/// </summary>
	public GameObject[] Enemies;
	
	/// <summary>
	/// List of random positions to place additional objects on the map
	/// </summary>
	private List<Vector3> additionalObjectPositions;

	/// <summary>
	/// The total weight of all obstacles, enemies, treasure, and blank space
	/// Set by difficulty settings
	/// </summary>
	private float totalWeight;

	/// <summary>
	/// The total number of additional objects positions
	/// </summary>
	private float totalObjectSlotCount;


	/// <summary>
	/// Add game start listener on enable
	/// </summary>
	void OnEnable()
	{
		GameEventManager.GameStart += OnGameStart;
	}

	/// <summary>
	/// Remove game start listener on disable
	/// </summary>
	void OnDisable()
	{
		GameEventManager.GameStart -= OnGameStart;
	}

	/// <summary>
	/// Initialize the map on game start
	/// </summary>
	/// <param name="args">Game Start Arguments.</param>
	void OnGameStart (GameStartArgs args)
	{
		Initialize(args.Difficulty);
	}

	/// <summary>
	/// Destroys all child object. This method is a jerk.
	/// </summary>
	void ClearChildren ()
	{
		// kill all children from previous game
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
	}

	/// <summary>
	/// Returns a random additional object position and removes it from the list
	/// </summary>
	/// <returns>The random object position.</returns>
	Vector3 PopRandomObjectPosition ()
	{
		int randomIndex = Random.Range (0, additionalObjectPositions.Count);
		Vector3 randomPosition = additionalObjectPositions [randomIndex];
		additionalObjectPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	/// <summary>
	/// Layouts the objects at random.
	/// </summary>
	/// <returns>Count of number of object added.</returns>
	/// <param name="objectArray">Available objects to instantiate.</param>
	/// <param name="weight">Weight of this object type.</param>
	int LayoutObjectsAtRandom (GameObject[] objectArray, float weight)
	{
		// choose how many obects to create
		int objectCount = Mathf.FloorToInt( (weight / totalWeight) * totalObjectSlotCount );
		
		// create each object
		for (int i = 0; i < objectCount; i++) {

			// choose a random position to instantiate the object at
			Vector3 randomPosition = PopRandomObjectPosition ();
			randomPosition = Vector3.Scale(randomPosition, transform.localScale);
			
			// choose a random object to instantiate
			GameObject objectChoice = objectArray [Random.Range (0, objectArray.Length)];

			randomPosition.y = objectChoice.transform.position.y;
			
			// instantiate new object 
			GameObject instantied = Instantiate (objectChoice, randomPosition, Quaternion.identity) as GameObject;

			// give it a random rotation
			instantied.transform.Rotate(instantied.transform.rotation.x, Random.value * 360.0f, instantied.transform.rotation.z);

			// attach to map game object
			instantied.transform.parent = transform;
		}

		return objectCount;
	}

	/// <summary>
	/// Draws the map surface mesh.
	/// </summary>
	/// <param name="mapMeshes">Array of map meshes.</param>
	void DrawSurface (Mesh[] mapMeshes)
	{
		Mesh thisMesh = GetComponent<MeshFilter> ().mesh;
		MeshCollider meshCollider = GetComponent<MeshCollider> ();

		// create map meshes
		CombineInstance[] combine = new CombineInstance[mapMeshes.Length];
		
		// combine map meshes
		for (int i = 0; i < mapMeshes.Length; i++) {
			combine [i].mesh = mapMeshes [i];
			combine [i].transform = transform.localToWorldMatrix;
		}

		// combine meshes & optimize 
		thisMesh.Clear ();
		thisMesh.CombineMeshes (combine, true, false); 
		thisMesh.Optimize ();
		thisMesh.RecalculateNormals ();
		thisMesh.RecalculateBounds ();
		
		// update physics collider
		meshCollider.sharedMesh = null;
		meshCollider.sharedMesh = thisMesh;
	}

	/// <summary>
	/// Initialize map with the specified difficulty.
	/// </summary>
	/// <param name="difficulty">Difficulty.</param>
	void Initialize (DifficultySetting difficulty)
	{
		ClearChildren();
		
		var map = GetComponent<AMapGenerationAlgorithm> () as AMapGenerationAlgorithm;
		
		// set difficulty
		map.SetDifficulty(difficulty);
		
		// create map meshes
		Mesh[] mapMeshes = map.Draw ();
		DrawSurface (mapMeshes);
		
		// add addtional objects
		additionalObjectPositions = map.GetObjectPositions ();
		
		totalObjectSlotCount = additionalObjectPositions.Count;
		totalWeight = difficulty.enemyWeight + difficulty.emptyCellWeight + difficulty.treasureWeight + difficulty.obstacleWeight;
		
		int treasureCount = LayoutObjectsAtRandom(Treasures, difficulty.treasureWeight);
		LayoutObjectsAtRandom(Obstacles, difficulty.obstacleWeight);
		
		LayoutObjectsAtRandom(Enemies, difficulty.enemyWeight);
		
		// Trigger map generation complete event
		MapGeneratedArgs mapGeneratedArgs = new MapGeneratedArgs();
		mapGeneratedArgs.TreasureCount = treasureCount;
		GameEventManager.TriggerMapGenerated(mapGeneratedArgs);
		
		// Reset player score
		ItemCollectedArgs itemCollectedArgs = new ItemCollectedArgs();
		itemCollectedArgs.Count = 0;
		GameEventManager.TriggerItemCollected(itemCollectedArgs);
	}

}
