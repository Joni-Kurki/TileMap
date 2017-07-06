using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public int start_x = 0;
	public int start_y = 0;
	public int moveDistance = 1;

	public G_TileMap G_tilemap;

	// Use this for initialization
	void Start () {
		
		//transform.position = G_tilemap.GetStartingLocation ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.W)) {
			if(G_tilemap.GetTileAt((int)transform.position.x, (int)transform.position.z + 1) == 1){
				transform.position = new Vector3 ((int)transform.position.x, (int)transform.position.y, (int)transform.position.z + moveDistance);
			}
		} else if (Input.GetKeyDown (KeyCode.S)) {
			if (G_tilemap.GetTileAt ((int)transform.position.x, (int)transform.position.z - 1) == 1) {
				transform.position = new Vector3 ((int)transform.position.x, (int)transform.position.y, (int)transform.position.z - moveDistance);
			}
		} else if (Input.GetKeyDown (KeyCode.A)) {
			if (G_tilemap.GetTileAt ((int)transform.position.x - 1, (int)transform.position.z) == 1) {
				transform.position = new Vector3 ((int)transform.position.x - moveDistance, (int)transform.position.y, (int)transform.position.z);
			}
		} else if (Input.GetKeyDown (KeyCode.D)) {
			if (G_tilemap.GetTileAt ((int)transform.position.x + 1, (int)transform.position.z) == 1) {
				transform.position = new Vector3 ((int)transform.position.x + moveDistance, (int)transform.position.y, (int)transform.position.z);
			}
		} else if (Input.GetKeyDown (KeyCode.F1)) { // tää pitää saaha toimimaan ilman napin painallusta
			transform.position = G_tilemap.GetStartingLocation ();
		}
	}
}
