using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(G_TileMap))]
public class Graphics_TileMap_Inspector : Editor {
	public override void OnInspectorGUI(){
		DrawDefaultInspector ();

		if (GUILayout.Button ("Randomize!")) {
			G_TileMap tileMap = (G_TileMap)target;
			tileMap.BuildMesh ();
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
