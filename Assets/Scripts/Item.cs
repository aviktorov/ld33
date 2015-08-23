using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	[Header("Logic")]
	public bool selected = false;

	[Header("Description")]
	public string type = "TV";
	public bool mimic = false;
	public string label = "Label";
	public string price = "100₽";
	public string description = "Test description";

	private Transform cachedTransform;

	private GameObject outline;
	private Renderer cachedOutlineRenderer;

	private void Awake() {
		cachedTransform = GetComponent<Transform>();

		CreateOutline();
	}

	public void Seleted() {
		if (!selected) {
			selected = true;
			cachedOutlineRenderer.sharedMaterial = OutlineSettings.instance.selectedMaterial;
		}
		else {
			Unselected();
		}
	}

	public void Unselected() {
		selected = false;
		cachedOutlineRenderer.sharedMaterial = OutlineSettings.instance.highlightedMaterial;
	}

	public void Show() {
		outline.SetActive(true);
	}

	public void Hide() {
		if (!selected)
			outline.SetActive(false);
	}

	private void CreateOutline() {
		Mesh mesh = GetComponent<MeshFilter>().mesh;

		outline = new GameObject("Outliner");
		outline.transform.SetParent(cachedTransform, false);

		outline.AddComponent<MeshRenderer>();
		Mesh outlineMesh = outline.AddComponent<MeshFilter>().mesh;

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

		cachedOutlineRenderer = outline.GetComponent<Renderer>();
		cachedOutlineRenderer.sharedMaterial = OutlineSettings.instance.highlightedMaterial;
		cachedOutlineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

		outline.SetActive(false);
	}
}
