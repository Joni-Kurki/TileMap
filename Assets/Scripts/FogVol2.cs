﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class FogVol2 : MonoBehaviour {

	public int size_x;
	public int size_y;
	public int vision_width;
	public int vision_height;
	public Texture2D FogTexture;
	public int tileSize;
	int tileResolution = 16;
	GameObject mapdata;
	G_TileMap g;
	public Vector3 lastPosition;
	int[,] fogData;
	Transform player;
	bool FogInit = false;

	public int fog_x_min;
	public int fog_y_min;
	public int fog_x_max;
	public int fog_y_max;

	// Use this for initialization
	void Awake () {
		fogData = new int [size_x, size_y];
	
		for (int x = 0; x < size_x; x++) {
			for (int y = 0; y < size_y; y++) {
				fogData[x,y] = 0;
			}
		}

		BuildMesh ();
	}

	void Start(){
		mapdata = GameObject.FindGameObjectWithTag ("GameWorld");
		g = mapdata.GetComponent<G_TileMap> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		lastPosition = player.transform.position;
	}

	
	// Update is called once per frame
	void LateUpdate () {
		if (!FogInit) {
			SeeAllCorridors ();
			FogInit = true;
		}
		if (lastPosition != player.transform.position) { // katotaan onko pelaaja liikkunut, jolloin fogia muutetaan
			Debug.Log ("Me have moved! Build Texture again!");
			lastPosition = player.transform.position;
			PlayerFog ();
		}
	}

	void Update(){
		
	}

	void PlayerFog(){
		fog_x_min = 0;
		fog_y_min = 0;
		fog_x_max = 0;
		fog_y_max = 0;

		if (player.transform.position.x - (vision_width / 2) > 0) {
			fog_x_min = (int)(player.transform.position.x - (vision_width / 2));
		} else {
			fog_x_min = 0;
		}
		if (player.transform.position.z - (vision_height / 2) > 0) {
			fog_y_min = (int)(player.transform.position.z - (vision_height / 2));
		} else{
			fog_y_min = 0;
		}
		if (player.transform.position.x + (vision_width / 2) < size_x) {
			fog_x_max = (int)(player.transform.position.x + (vision_width / 2));
		} else {
			fog_x_max = size_x;
		}
		if (player.transform.position.z + (vision_height / 2) < size_y) {
			fog_y_max = (int)(player.transform.position.z + (vision_height / 2));
		} else {
			fog_y_max = size_y;
		}
		for (int x = 0; x < size_x; x++) {
			for (int y = 0; y < size_y; y++) {
				if (x >= fog_x_min && x <= fog_x_max && y >= fog_y_min && y <= fog_y_max && (g.GetTileAt(x,y) == 1 || g.GetTileAt(x,y) == 2)) {
					fogData [x, y] = 1;
				} else {
					fogData [x, y] = 0;
				}
			}
		}
		BuildTexture ();
	}

	void SeeAllCorridors(){
		for (int x = 0; x < size_x; x++) {
			for (int y = 0; y < size_y; y++) {
				if (g.GetTileAt (x, y) == 1 || g.GetTileAt (x, y) == 2) {
					fogData [x, y] = 1;
				} else {
					fogData [x, y] = 0;
				}
			}
		}
		BuildTexture ();
	}

	public void BuildMesh(){
		int numberOfTiles = size_x * size_y;
		int numberOfTris = numberOfTiles * 2; // koska joka tilessa 2x tris

		int vert_size_x = size_x + 1;
		int vert_size_y = size_y + 1;
		int numberOfVerts = vert_size_x * vert_size_y;

		Vector3[] verticles = new Vector3[numberOfVerts];
		Vector3[] normals = new Vector3[numberOfVerts];
		Vector2[] uv = new Vector2[numberOfVerts];

		int[] triangles = new int[numberOfTris * 3];

		int x, y;
		for (y = 0; y < vert_size_y; y++) {
			for (x = 0; x < vert_size_x; x++) {
				verticles [y * vert_size_x + x] = new Vector3 (x * tileSize, 0, y * tileSize);
				normals [y * vert_size_x + x] = Vector3.up; // pinta suoraan ylöspäin
				uv [y * vert_size_x + x] = new Vector2 ((float)x / size_x, (float)y / size_y);
			}
		}
		Debug.Log ("FOG::Verts done!");
		for (y = 0; y < size_y; y++) {
			for (x = 0; x < size_x; x++) {
				int squareIndex = y * size_x + x;
				int trisOffset = squareIndex * 6;

				triangles [trisOffset + 0] = y * vert_size_x + x + 					0;
				triangles [trisOffset + 1] = y * vert_size_x + x + vert_size_x + 	0;
				triangles [trisOffset + 2] = y * vert_size_x + x + vert_size_x +	1;

				triangles [trisOffset + 3] = y * vert_size_x + x + 					0;
				triangles [trisOffset + 4] = y * vert_size_x + x + vert_size_x +	1;
				triangles [trisOffset + 5] = y * vert_size_x + x + 					1;
			}
		}
		Debug.Log ("FOG::Tris done!");
		// luodaan mesh aiemmin luodulla arvoilla
		Mesh mesh = new Mesh ();
		mesh.vertices = verticles;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;
		//
		MeshFilter mesh_filter = GetComponent<MeshFilter> ();
		MeshCollider mesh_collider = GetComponent<MeshCollider> ();

		mesh_filter.mesh = mesh;
		mesh_collider.sharedMesh = mesh;
		Debug.Log ("FOG::Mesh done!");

		BuildTexture ();
	}

	void BuildTexture(){
		//dTileMap =  new Data_TileMap (size_x, size_y);

		int textureWidth = size_x * tileResolution;
		int textureHeigth = size_y * tileResolution;
		Texture2D texture = new Texture2D (textureWidth, textureHeigth);

		Color[][] tiles = ChopUpTiles ();

		for (int y = 0; y < size_y; y++) {
			for (int x = 0; x < size_x; x++) {
				Color[] c = tiles [fogData[x, y]];
				texture.SetPixels (x * tileResolution, y * tileResolution, tileResolution, tileResolution, c);
			}
		}

		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply ();

		MeshRenderer mesh_renderer = GetComponent<MeshRenderer> ();
		mesh_renderer.sharedMaterials [0].mainTexture = texture;
		Debug.Log("FOG::Textures done!");
	}

	Color [][] ChopUpTiles(){
		int numberOfTilesPerRow = 2;//FogTexture.width / tileResolution;
		int numberOfRowsInTexture = 1;//FogTexture.height / tileResolution;

		Color[][] tiles = new Color[numberOfTilesPerRow * numberOfRowsInTexture][];

		for (int y = 0; y < numberOfRowsInTexture; y++) {
			for (int x = 0; x < numberOfTilesPerRow; x++) {
				tiles [y * numberOfTilesPerRow + x] = FogTexture.GetPixels (x * tileResolution, y * tileResolution, tileResolution, tileResolution);
			}
		}
		return tiles;
	}
}
