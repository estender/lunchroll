using UnityEngine;
using System.Collections;

public class MapRegionSquare : AMapRegionShape {

	public MapRegionSquare () : base () {

	}

	/// <summary>
	/// Generates an array of vertices for a square map region
	/// </summary>
	public override void CalculateVertices() {

		float radiusX = _size.x / 2.0f;
		float radiusY = _size.z / 2.0f;

		Vector2[] corners = new Vector2[4];
		corners[0] = new Vector2(_center.x - radiusX, _center.z - radiusY);
		corners[1] = new Vector2(_center.x + radiusX, _center.z - radiusY);
		corners[2] = new Vector2(_center.x + radiusX, _center.z + radiusY);
		corners[3] = new Vector2(_center.x - radiusX, _center.z + radiusY);

		Vertices = corners;
	}

	/// <summary>
	/// Gets random position on this mesh to place an object
	/// </summary>
	/// <param name="additionalObjectRange">Percentage of region to place random object</param>
	/// <returns>The random object position.</returns>
	public override Vector3 GetRandomObjectPosition(float additionalObjectRange)
	{
		// choose a random location in the rectangle
		Vector3 offset = new Vector3((0.5f - Random.value), 0.0f, (0.5f - Random.value));
		Vector3 scaledOffset = Vector3.Scale(_size, offset) * additionalObjectRange;

		// return postion relative to the center
		Vector3 randomPosition = _center + scaledOffset;

		return randomPosition;
	}

}
