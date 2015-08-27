#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public static class OutlinesMenu{
	[MenuItem("Impostor/Create Outlines")]
	public static void CreateOutlines() {
		GameObject[] itemObjects = GameObject.FindGameObjectsWithTag("Item");
		
		GameObject[] outlineObjects = HardFindGameObjectsWithTag("Outline");
		foreach(var outlineObject in outlineObjects)
			GameObject.DestroyImmediate(outlineObject);

		foreach (var itemObject in itemObjects) {
			Transform cachedTransform = itemObject.GetComponent<Transform>();
			Mesh mesh = itemObject.GetComponent<MeshFilter>().sharedMesh;

			GameObject outline = new GameObject("Outline");
			outline.tag = "Outline";
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
				outlineVertices[i] = vertices[i] + normals[i] * 0.025f / maxScale;

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

	static private GameObject[] HardFindGameObjectsWithTag(string tag) {
		List<GameObject> result = new List<GameObject>();
		foreach (GameObject gameObject in GetAllObjectsInScene(false)) {
			if (gameObject.tag == tag)
				result.Add(gameObject);
		}
		return result.ToArray();
	}

	public static List<GameObject> GetAllObjectsInScene(bool onlyRoot) {
		GameObject[] allObjects = (GameObject[]) Resources.FindObjectsOfTypeAll(typeof(GameObject));
		List<GameObject> result = new List<GameObject>();
		foreach (GameObject obj in allObjects) {
			if (onlyRoot && obj.transform.parent != null) continue;

			if (obj.hideFlags == HideFlags.NotEditable || obj.hideFlags == HideFlags.HideAndDontSave) continue; 

			if (Application.isEditor) {
				string sAssetPath = AssetDatabase.GetAssetPath(obj.transform.root.gameObject);
				if (!string.IsNullOrEmpty(sAssetPath)) continue;
			}

			result.Add(obj);
		}
		
		return result;
	}
}

#endif
