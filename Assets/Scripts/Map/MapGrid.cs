using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class to handle display of a grid-shaped map
/// </summary>
public class MapGrid : MonoBehaviour {

	/// <summary>
	/// Number of map regions along the x axis
	/// </summary>
	public int columnCount;

	/// <summary>
	/// Number of map regions along the z axis.
	/// </summary>
	public int rowCount;
	
	/// <summary>
	/// Three dimensional size of each region.
	/// </summary>
	public Vector3 regionSize;

	/// <summary>
	/// Fraction on region area to be used for random object placement.
	/// When value is zero, objects are only placed at the square region center or curve's foci
	/// When value is one, any point on the map shape is fair game
	/// </summary>
	[Range(0.0f, 1.0f)]
	public float additionalObjectRange;

	/// <summary>
	/// Potential positions for additional objects on the board
	/// </summary>
	private List<Vector3> objectPositions;
	

	/// <summary>
	/// Procedurally generates a new maze and generates map region objects to represent it
	/// </summary>
	public Mesh[] Draw(bool[,] grid) {
	
		// loop through the maze and create the map regions
		Vector3 currentLocation;
		MapRegion currentRegion;
		Mesh currentMesh;
		bool north, south, east, west;


		objectPositions = new List<Vector3>();
		List<Mesh> meshes = new List<Mesh>();

		// build out map region objects
		for (int i = 0; i < columnCount; i++) {
			for (int j = 0; j < rowCount; j++) {
				if (grid[i,j] == true) {
					// create a new map region at current location
					currentLocation = new Vector3(i * regionSize.x, 0.0f, j * regionSize.z);
					currentRegion = new MapRegion(currentLocation, regionSize);

					// see if this region's neighbors are in the maze or not
					north = (i+1 < columnCount) && grid[i+1, j];
					south = (i-1 >= 0) && grid[i-1, j];
					east = (j+1 < rowCount) && grid[i, j+1];
					west = (j-1 >= 0) && grid[i, j-1];

					// edge case hack: force starting cell to be a square
					if (i + j == 0)
					{
						north = true;
						south = true;
					}

					// determine shape of tile based on neighbors
					currentRegion.DetermineShapeAndDirection(north, south, east, west);

					// add to list of regions
					currentMesh = currentRegion.draw();
					meshes.Add(currentMesh);

					// if not the starting cell, add a random point for additional objects like loot and enemies
					if (i + j > 0) {
						Vector3 objectLocation = currentRegion.GetRandomObjectPosition(additionalObjectRange); 
						objectPositions.Add(objectLocation);
					}
				}
			}
		}

		return meshes.ToArray();
	}

	/// <summary>
	/// Gets a list of available object positions.
	/// </summary>
	/// <returns>The object positions.</returns>
	public List<Vector3> GetObjectPositions() {
		return objectPositions;
	}

	/// <summary>
	/// Sets the difficulty settings
	/// </summary>
	/// <param name="difficulty">Difficulty settings.</param>
	public void SetDifficulty (DifficultySetting difficulty)
	{
		columnCount = difficulty.columns;
		rowCount = difficulty.rows;
		
		regionSize = new Vector3(difficulty.regionSize, regionSize.y, difficulty.regionSize);

	}

}
