using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	[Header("Outline")]
	public float outlineWidth = 0.05f;

	private Transform cachedTransform;
	private GameObject outline;

	private void Awake () {
		cachedTransform = GetComponent<Transform>();

		outline = new GameObject("Outliner");
		outline.transform.SetParent(cachedTransform, false);
		outline.AddComponent<MeshRenderer>();

		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Mesh outlineMesh = outline.AddComponent<MeshFilter>().mesh;

		Vector3[] normals = mesh.normals;
		Vector3[] outlineNormals = new Vector3[normals.Length];
		for (int i = 0; i < outlineNormals.Length; i++)
			outlineNormals[i] = -normals[i];

		Vector3[] vertices = mesh.vertices;
		Vector3[] outlineVertices = new Vector3[vertices.Length];
		float maxScale = Mathf.Max(Mathf.Abs(cachedTransform.localScale.x), Mathf.Abs(cachedTransform.localScale.y), Mathf.Abs(cachedTransform.localScale.z));
		for (int i = 0; i < outlineVertices.Length; i++) 
			outlineVertices[i] = vertices[i] + normals[i] * outlineWidth / maxScale;

		Vector2[] uv = mesh.uv;
		Vector2[] outlineUV = new Vector2[uv.Length];
		for (int i = 0; i < outlineUV.Length; i++)
			outlineUV[i] = uv[i];

		int[] triangles  = mesh.triangles ;
		int[] outlineTriangles  = new int[triangles .Length];
		for (int i = 0; i < outlineTriangles.Length; i += 3) {
			outlineTriangles[i + 1] = triangles[i];
			outlineTriangles[i] = triangles[i + 1];
			outlineTriangles[i + 2] = triangles[i + 2];
		}

		outlineMesh.vertices = outlineVertices;
		outlineMesh.uv = outlineUV;
		outlineMesh.triangles  = outlineTriangles;
		outlineMesh.normals = outlineNormals;

		outline.GetComponent<Renderer>().sharedMaterial = Player.instance.outlineMaterial;

		outline.SetActive(false);
	}
	
	private void Update () {
	
	}

	public void Show() {
		outline.SetActive(true);
	}

	public void Hide() {
		outline.SetActive(false);
	}
}
