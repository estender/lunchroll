using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class to build an extruded polygon mesh based on an array of 2d vertices
/// </summary>
public class ExtrudedPolygon {

	/// <summary>
	/// Vertices of the 2d polygon to extrude
	/// </summary>
	private Vector2[] _polygon;

	/// <summary>
	/// Distance polygon should be extruded
	/// </summary>
	private float _extrusion;

	/// <summary>
	/// Initializes a new instance of the <see cref="ExtrudedPolygon"/> class.
	/// </summary>
	/// <param name="polygon">Vertices of the 2d polygon to extrude</param>
	/// <param name="extrusion">Distance polygon should be extruded</param>
	public ExtrudedPolygon (Vector2[] polygon, float extrusion) {
		_polygon = polygon;
		_extrusion = extrusion;
	}

	/// <summary>
	/// Draw this instance.
	/// </summary>
	/// <returns>Unity mesh for this extruded polygon</returns>
	public Mesh draw() {

		Mesh mesh = new Mesh(); // mesh to return

		// calculate vertices of both faces 
		Vector3[] vertices = new Vector3[_polygon.Length * 2];
		for (int i= 0; i < _polygon.Length; i++) {
			// top face
			vertices[i].x = _polygon[i].x;
			vertices[i].y = 0;
			vertices[i].z = _polygon[i].y;

			// bottom face
			vertices[i + _polygon.Length].x = _polygon[i].x;
			vertices[i + _polygon.Length].y = _extrusion * -1;
			vertices[i + _polygon.Length].z = _polygon[i].y;
		}

		// calculate triangles of top face
		Triangulator triangulator = new Triangulator(_polygon);
		int[] faceTriangles = triangulator.Triangulate();

		// calculate triangles of all sides (6 sides and two sides)
		List<int> allTriangles = new List<int>(faceTriangles);

		// bottom face triangles mirror top face (triangles array is always a multiple of 3)
		// TODO: merge with loop above using tricky modulo
		for (int i = 0; i < faceTriangles.Length; i += 3) {
			allTriangles.Add(faceTriangles[i + 2] + _polygon.Length);
			allTriangles.Add(faceTriangles[i + 1] + _polygon.Length);
			allTriangles.Add(faceTriangles[i] + _polygon.Length);
		}

		// add both triangles to each side face
		// (expects bottom face directly after top in vertices array)
		int sideTriangleCount = vertices.Length / 2;
		for( int i = 0; i < sideTriangleCount; i++) {
			int i1 = i; 
			int i2 = (i1 + 1) % sideTriangleCount;
			int i3 = i1 + sideTriangleCount;
			int i4 = i2 + sideTriangleCount;

			// add both triangles for side
			allTriangles.Add(i4);
			allTriangles.Add(i3);
			allTriangles.Add(i1);

			allTriangles.Add(i2);
			allTriangles.Add(i4);
			allTriangles.Add(i1);
		}

		// calculate texture coorinates
		Vector2[] uv = new Vector2[vertices.Length];
		for (int i = 0; i < uv.Length; i++) {
			uv[i] = new Vector2(vertices[i].x, vertices[i].z);
		}

		// calculate tangents
		calculateMeshTangents(mesh);
		
		// build mesh
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = allTriangles.ToArray();
		mesh.uv = uv;

		mesh.Optimize ();
		mesh.RecalculateNormals ();
		mesh.RecalculateBounds ();

		return mesh;

	}

	private void calculateMeshTangents(Mesh mesh)
	{
		//speed up math by copying the mesh arrays
		int[] triangles = mesh.triangles;
		Vector3[] vertices = mesh.vertices;
		Vector2[] uv = mesh.uv;
		Vector3[] normals = mesh.normals;
		
		//variable definitions
		int triangleCount = triangles.Length;
		int vertexCount = vertices.Length;
		
		Vector3[] tan1 = new Vector3[vertexCount];
		Vector3[] tan2 = new Vector3[vertexCount];
		
		Vector4[] tangents = new Vector4[vertexCount];
		
		for (long a = 0; a < triangleCount; a += 3)
		{
			long i1 = triangles[a + 0];
			long i2 = triangles[a + 1];
			long i3 = triangles[a + 2];
			
			Vector3 v1 = vertices[i1];
			Vector3 v2 = vertices[i2];
			Vector3 v3 = vertices[i3];
			
			Vector2 w1 = uv[i1];
			Vector2 w2 = uv[i2];
			Vector2 w3 = uv[i3];
			
			float x1 = v2.x - v1.x;
			float x2 = v3.x - v1.x;
			float y1 = v2.y - v1.y;
			float y2 = v3.y - v1.y;
			float z1 = v2.z - v1.z;
			float z2 = v3.z - v1.z;
			
			float s1 = w2.x - w1.x;
			float s2 = w3.x - w1.x;
			float t1 = w2.y - w1.y;
			float t2 = w3.y - w1.y;

			float divisor = (s1 * t2 - s2 * t1);
			float r;
			if (divisor > 0)
			{
				r = 1.0f / divisor;
			}
			else
			{
				r = 0.0f;
			}
			
			Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
			Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);
			
			tan1[i1] += sdir;
			tan1[i2] += sdir;
			tan1[i3] += sdir;
			
			tan2[i1] += tdir;
			tan2[i2] += tdir;
			tan2[i3] += tdir;
		}
		
		
		for (long a = 0; a < vertexCount; ++a)
		{
			Vector3 n = normals[a];
			Vector3 t = tan1[a];
			
			//Vector3 tmp = (t - n * Vector3.Dot(n, t)).normalized;
			//tangents[a] = new Vector4(tmp.x, tmp.y, tmp.z);
			Vector3.OrthoNormalize(ref n, ref t);
			tangents[a].x = t.x;
			tangents[a].y = t.y;
			tangents[a].z = t.z;
			
			tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
		}
		
		mesh.tangents = tangents;
	}
}


