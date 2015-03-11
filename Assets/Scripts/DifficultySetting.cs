using UnityEngine;
using System.Collections;

[System.Serializable]
public class DifficultySetting : ScriptableObject 
{
	public string displayText;

	[Range(0, 50)]
	public int rows = 20;

	[Range(0, 50)]
	public int columns = 20;

	[Range(1.0f, 10.0f)]
	public float regionSize = 3.0f;

	[Range(0.5f, 3.0f)]
	public float regionScale = 1.0f;
	
	public float enemyWeight = 10.0f;

	public float obstacleWeight = 10.0f;

	public float emptyCellWeight = 10.0f;

	public float treasureWeight = 10.0f;

}
