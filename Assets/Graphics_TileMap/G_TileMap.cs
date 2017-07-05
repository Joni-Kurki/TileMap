using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class G_TileMap : MonoBehaviour {

	public int size_x = 50;
	public int size_y = 50;
	public float tileSize = 1.0f;

	public Texture2D mapTileGraphics; // tähän spritet
	public int tileResolution; // esim 8x8px tile -> 8px

	// Use this for initialization
	void Start () {
		BuildMesh ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	Color [][] ChopUpTiles(){
		int numberOfTilesPerRow = mapTileGraphics.width / tileResolution;
		int numberOfRowsInTexture = mapTileGraphics.height / tileResolution;

		Color[][] tiles = new Color[numberOfTilesPerRow * numberOfRowsInTexture][];

		for (int y = 0; y < numberOfRowsInTexture; y++) {
			for (int x = 0; x < numberOfTilesPerRow; x++) {
				tiles [y * numberOfTilesPerRow + x] = mapTileGraphics.GetPixels (x * tileResolution, y * tileResolution, tileResolution, tileResolution);
			}
		}

		return tiles;
	}

	void BuildTexture(){
		Data_TileMap dTileMap =  new Data_TileMap (size_x, size_y);

		int textureWidth = size_x * tileResolution;
		int textureHeigth = size_y * tileResolution;
		Texture2D texture = new Texture2D (textureWidth, textureHeigth);

		Color[][] tiles = ChopUpTiles ();

		for (int y = 0; y < size_y; y++) {
			for (int x = 0; x < size_x; x++) {
				Color[] c = tiles [dTileMap.GetTileAt(x,y)/*Random.Range(0,4)*/];
				texture.SetPixels (x * tileResolution, y * tileResolution, tileResolution, tileResolution, c);
			}
		}

		texture.filterMode = FilterMode.Bilinear;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply ();

		MeshRenderer mesh_renderer = GetComponent<MeshRenderer> ();
		mesh_renderer.sharedMaterials [0].mainTexture = texture;

		Debug.Log("Textures done!");
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
		Debug.Log ("Verts done!");
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
		Debug.Log ("Tris done!");
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
		Debug.Log ("Mesh done!");

		BuildTexture ();
	}
}
