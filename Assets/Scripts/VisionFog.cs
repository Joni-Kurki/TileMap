﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class VisionFog : MonoBehaviour {

	int fog_x;
	int fog_y;
	public Texture2D FogTexture;
	public bool isFogOn = false;
	public int fog_width;
	public int fog_heigth;
	public int tileSize;
	int tileResolution = 16;

	int[,] fogData;

	// Use this for initialization
	void Start () {
		fog_x = (int)transform.position.x;
		fog_y = (int)transform.position.z;
		fogData = new int [fog_width, fog_heigth];

		for (int x = 0; x < fog_width; x++) {
			for (int y = 0; y < fog_heigth; y++) {
				fogData[x,y] = 0;
			}
		}
		Fog ();
		BuildMesh ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (isFogOn) {
			Transform p = GameObject.FindGameObjectWithTag ("Player").transform;
			transform.position = new Vector3((int)p.transform.position.x - ( fog_width / 2), 0.05f, (int)p.transform.position.z - (fog_heigth /2 ));
		}
	}

	void Fog(){
		/*
		fogData [4, 4] = 1;
		fogData [4, 5] = 1;
		fogData [5, 4] = 1;
		fogData [5, 5] = 1;
		fogData [5, 6] = 1;
		*/

		for (int x = 10; x < fog_width-10; x++) {
			for (int y = 10; y < fog_heigth-10; y++) {
				fogData [x, y] = 1;
			}
		}

	}

	public void BuildMesh(){
		int numberOfTiles = fog_width * fog_heigth;
		int numberOfTris = numberOfTiles * 2; // koska joka tilessa 2x tris

		int vert_size_x = fog_width + 1;
		int vert_size_y = fog_heigth + 1;
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
				uv [y * vert_size_x + x] = new Vector2 ((float)x / fog_width, (float)y / fog_heigth);
			}
		}
		Debug.Log ("FOG::Verts done!");
		for (y = 0; y < fog_heigth; y++) {
			for (x = 0; x < fog_width; x++) {
				int squareIndex = y * fog_width + x;
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

		int textureWidth = fog_width * tileResolution;
		int textureHeigth = fog_heigth * tileResolution;
		Texture2D texture = new Texture2D (textureWidth, textureHeigth);

		Color[][] tiles = ChopUpTiles ();

		for (int y = 0; y < fog_heigth; y++) {
			for (int x = 0; x < fog_width; x++) {
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
