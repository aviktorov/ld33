#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;

public static class OutlinesMenu{
	[MenuItem("Impostor/Create Outlines")]
	public static void CreateOutlines() {
		GameObject[] itemObjects = GameObject.FindGameObjectsWithTag("Item");

		foreach (var itemObject in itemObjects) {
			Transform cachedTransform = itemObject.GetComponent<Transform>();
			Mesh mesh = itemObject.GetComponent<MeshFilter>().sharedMesh;

			GameObject outline = new GameObject("Outline");
			outline.transform.SetParent(cachedTransform, false);

			Mesh outlineMesh = new Mesh();

			Vector3[] vertices = mesh.vertices;
			Vector3[] normals = mesh.normals;
			Vector2[] uv = mesh.uv;
			int[] triangles  = mesh.triangles ;

			Vector3[] outlineVertices = new Vector3[vertices.Length];
			Vector2[] outlineUV = new Vector2[uv.Length];
			Vector3[] outlineNormals = new Vector3[normals.Length];
			int[] outlineTriangles  = new int[triangles.Length];
			
			float maxScale = Mathf.Max(Mathf.Abs(cachedTransform.localScale.x), Mathf.Abs(cachedTransform.localScale.y), Mathf.Abs(cachedTransform.localScale.z));
			for (int i = 0; i < outlineVertices.Length; i++) 
				outlineVertices[i] = vertices[i] + normals[i] * OutlineSettings.instance.width / maxScale;

			for (int i = 0; i < outlineUV.Length; i++)
				outlineUV[i] = uv[i];

			// Flip normals
			for (int i = 0; i < outlineNormals.Length; i++)
				outlineNormals[i] = -normals[i];

			for (int i = 0; i < outlineTriangles.Length; i += 3) {
				outlineTriangles[i + 1] = triangles[i];
				outlineTriangles[i] = triangles[i + 1];
				outlineTriangles[i + 2] = triangles[i + 2];
			}

			outlineMesh.vertices = outlineVertices;
			outlineMesh.uv = outlineUV;
			outlineMesh.triangles  = outlineTriangles;
			outlineMesh.normals = outlineNormals;

			MeshFilter meshFilter = outline.AddComponent<MeshFilter>();
			meshFilter.mesh = outlineMesh;
			outline.AddComponent<MeshRenderer>();

			outline.SetActive(false);
		}
	}
}

#endif
