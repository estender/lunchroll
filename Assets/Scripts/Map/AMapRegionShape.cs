using System;
using UnityEngine;

//------------------------------------------------------------------------------
// AMapRegionShape
//------------------------------------------------------------------------------

/// <summary>
/// Abstract class to handle various map region shapes
/// </summary>
abstract public class AMapRegionShape  {

	/// <summary>
	/// Center point of the shape
	/// </summary>
	protected Vector3 _center;

	/// <summary>
	/// Size of the shape
	/// </summary>
	protected Vector3 _size;

	/// <summary>
	/// Cardinal direction this shape should face
	/// </summary>
	protected Direction _direction;
	
	/// <summary>
	/// Gets or sets the mesh's vertices.
	/// </summary>
	/// <value>The mesh vertices.</value>
	public Vector2[] Vertices { get; protected set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="AMapRegionShape"/> class.
	/// </summary>
	public AMapRegionShape () {

		_center = new Vector3();
		_size = new Vector3();

	}

	/// <summary>
	/// Returns the number of vertices in the curves of curved shapes
	/// </summary>
	/// <returns>Number of vertices in curves</returns>
	protected int CurveQuality () {
		// TODO: implement game quality settings
		return 8;
	}

	/// <summary>
	/// Sets the center point of the region
	/// </summary>
	/// <param name="center">Center point</param>
	public void SetCenter (Vector3 center) {
		_center = center;
	}

	/// <summary>
	/// Sets the size of the shape.
	/// </summary>
	/// <param name="size">Size.</param>
	public void SetSize (Vector3 size) {
		_size = size;
	}

	/// <summary>
	/// Sets the direction of the shape
	/// </summary>
	/// <param name="direction">Cardinal Direction (i.e. North)</param>
	public void SetDirection (Direction direction) {
		_direction = direction;
	}

	/// <summary>
	/// Gets random position on this mesh to place an object
	/// </summary>
	/// <param name="additionalObjectRange">Percentage of region to place random object</param>
	/// <returns>The random object position.</returns>
	virtual public Vector3 GetRandomObjectPosition(float additionalObjectRange)
	{
		// choose a random vertex other than 0
		int randomVertex = (int)UnityEngine.Random.Range(1, Vertices.Length - 1);

		// return a random position between vertex 0 and the chosen vertex
		Vector2 randomPostion = Vector2.Lerp(Vertices[0], Vertices[randomVertex], UnityEngine.Random.value * additionalObjectRange);
	
		return new Vector3(randomPostion.x, 0.0f, randomPostion.y);
	}

	/// <summary>
	/// Gets the vertices for the shape's mesh
	/// </summary>
	/// <returns>Vertices for the shape's mesh</returns>
	abstract public void CalculateVertices ();
}


