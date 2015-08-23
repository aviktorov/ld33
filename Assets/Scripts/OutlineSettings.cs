using UnityEngine;
using System.Collections;

public class OutlineSettings : MonoSingleton<OutlineSettings> {
	public float width = 0.05f;
	public Material highlightedMaterial;
	public Material selectedMaterial;
	public Material selectedDebugMaterial;
}
