using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Semicircle map region shape
/// </summary>
public class MapRegionCap : AMapRegionShape {

	public MapRegionCap () : base () {

	}
	
	/// <summary>
	/// Gets the angle offset to start drawing the curve in radians
	/// </summary>
	/// <returns>The rotation angle offset in radians</returns>
	private float getRotationAngle() {
		float rotationAngle = 0.0f;
		
		switch (_direction) {
		case Direction.North:

			rotationAngle = Mathf.PI / 2.0f;
			break;
			
		case Direction.East:
			rotationAngle = Mathf.PI;
			break;
			
		case Direction.South:
			rotationAngle = 1.5f * Mathf.PI;
			break;
			
		case Direction.West:
			rotationAngle = 0.0f;
			break;
		}
		
		return rotationAngle;
	}
	
	/// <summary>
	/// Gets a directional vector to locate the curve's focal point relative to the center
	/// </summary>
	/// <returns>The focal offset.</returns>
	private Vector2 getFocalOffset () {
		Vector2 focalOffset = new Vector2();
		
		switch (_direction) {
		case Direction.North:
			focalOffset = new Vector2(1, 0);
			break;
			
		case Direction.East:
			focalOffset = new Vector2(0, 1);
			break;
			
		case Direction.South:
			focalOffset = new Vector2(-1, 0);
			break;
			
		case Direction.West:
			focalOffset = new Vector2(0, -1);
			break;
		}
		
		return focalOffset;
	}

	/// <summary>
	/// Generates an array of vertices for a semicircle map region
	/// </summary>
	public override void CalculateVertices() {

		float x, y, angleInRadians;

		List<Vector2> vertices = new List<Vector2>();

		int curveCount = CurveQuality();

		float radiusX = _size.x / 2;
		float radiusY = _size.z / 2;

		float rotationAngle = getRotationAngle();
		Vector2 focalOffset = getFocalOffset();

		// determine focal point of the curve (center of circle)
		Vector2 focalPoint = new Vector2(_center.x + (radiusX * focalOffset.x), _center.z + (radiusY * focalOffset.y));

		vertices.Add(focalPoint);
		for (int i=0; i <= curveCount; i++) {
			angleInRadians =  Mathf.PI * i / (curveCount);
			x = focalPoint.x + radiusX * Mathf.Cos(angleInRadians + rotationAngle);
			y = focalPoint.y + radiusY * Mathf.Sin(angleInRadians + rotationAngle);
			vertices.Add(new Vector2(x, y));
		}

		Vertices = vertices.ToArray();
	}

}
