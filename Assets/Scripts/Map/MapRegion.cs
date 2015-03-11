using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Class to handle the display of a various-shaped map regions in a grid
/// </summary>
public class MapRegion {

	/// <summary>
	/// The shape of the region
	/// </summary>
	protected AMapRegionShape _shape;
	
	/// <summary>
	/// Three dimensional size of the region
	/// </summary>
	protected Vector3 _size;

	/// <summary>
	/// Center point of the region
	/// </summary>
	/// <value>The center.</value>
	public Vector3 Center { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="MapRegion"/> class.
	/// </summary>
	/// <param name="center">Center point of region</param>
	/// <param name="size">Size of the region</param>
	public MapRegion (Vector3 center, Vector3 size) {
		Center = center;
		_size = size;
	}


	/// <summary>
	/// Determines the shape and direction of a map region based on if the region's neighbors
    /// are part of the maze or not
	/// </summary>
	/// <param name="north">If set to <c>true</c>, the region to the north is in the maze.</param>
	/// <param name="south">If set to <c>true</c>, the region to the south is in the maze.</param>
	/// <param name="east">If set to <c>true</c>, the region to the east is in the maze.</param>
	/// <param name="west">If set to <c>true</c>, the region to the west is in the maze.</param>
	public void DetermineShapeAndDirection(bool north, bool south, bool east, bool west) {

		// if opposite neighbor regions are in the maze, set the shape to a square
		// if only two catty-corner regions are in the maze, set the shape to a curve
		// if only one region is in the maze, set the shape to a semicircular cap

		if ((north && south) || (east && west)) {
		
			_shape = new MapRegionSquare();
			_shape.SetDirection(Direction.North);

		} else if (north && east) {

			_shape = new MapRegionCurve();
			_shape.SetDirection(Direction.North);

		} else if (north && west) {

			_shape = new MapRegionCurve();
			_shape.SetDirection(Direction.East);

		} else if (south && east) {
			
			_shape = new MapRegionCurve();
			_shape.SetDirection(Direction.West);
			
		} else if (south && west) {
			
			_shape = new MapRegionCurve();
			_shape.SetDirection(Direction.South);
			
		} else if (south) {
			_shape = new MapRegionCap();
			_shape.SetDirection(Direction.South);

		} else if (north) {
			_shape = new MapRegionCap();
			_shape.SetDirection(Direction.North);

		} else if (south) {
			_shape = new MapRegionCap();
			_shape.SetDirection(Direction.South);

		} else if (east) {

			_shape = new MapRegionCap();
			_shape.SetDirection(Direction.East);

		} else if (west) {

			_shape = new MapRegionCap();
			_shape.SetDirection(Direction.West);

		} else {
			_shape = new MapRegionSquare();
			_shape.SetDirection(Direction.North);
		}

		// pass along this region's shape and size to the shape object
		_shape.SetCenter(Center);
		_shape.SetSize(_size);

	}

	/// <summary>
	/// Draw this instance as an extruded polygon
	/// </summary>
	public Mesh draw () {
		_shape.CalculateVertices();
		ExtrudedPolygon poly = new ExtrudedPolygon(_shape.Vertices, _size.y);
		return poly.draw();
	}

	/// <summary>
	/// Gets random position in this region to place an object
	/// </summary>
	/// <param name="additionalObjectRange">Percentage of region to place random object</param>
	/// <returns>The random object position.</returns>
	public Vector3 GetRandomObjectPosition(float additionalObjectRange)
	{
		return _shape.GetRandomObjectPosition(additionalObjectRange);
	}

}
