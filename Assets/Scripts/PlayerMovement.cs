using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public int start_x = 0;
	public int start_y = 0;
	public int moveDistance = 1;
    public float movInterval = 0.1f;
    float lastTime;

	public G_TileMap G_tilemap;
    PlayerScript pScript;

	// Use this for initialization
	void Start () {
        lastTime = Time.fixedTime;
        pScript = GetComponent<PlayerScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W) && !pScript.GetIsDead()) {
            if(Time.fixedTime > lastTime + movInterval){
                lastTime = Time.fixedTime;
			    if(G_tilemap.GetTileAt((int)transform.position.x, (int)transform.position.z + 1) == 1){
				    transform.position = new Vector3 ((int)transform.position.x, (int)transform.position.y, (int)transform.position.z + moveDistance);
			    }
            }
        } else if (Input.GetKey(KeyCode.S) && !pScript.GetIsDead()) {
            if (Time.fixedTime > lastTime + movInterval) {
                lastTime = Time.fixedTime;
                if (G_tilemap.GetTileAt((int)transform.position.x, (int)transform.position.z - 1) == 1) {
                    transform.position = new Vector3((int)transform.position.x, (int)transform.position.y, (int)transform.position.z - moveDistance);
                }
            }
        } else if (Input.GetKey(KeyCode.A) && !pScript.GetIsDead()) {
            if (Time.fixedTime > lastTime + movInterval) {
                lastTime = Time.fixedTime;
                if (G_tilemap.GetTileAt((int)transform.position.x - 1, (int)transform.position.z) == 1) {
                    transform.position = new Vector3((int)transform.position.x - moveDistance, (int)transform.position.y, (int)transform.position.z);
                }
            }
        } else if (Input.GetKey(KeyCode.D) && !pScript.GetIsDead()) {
            if (Time.fixedTime > lastTime + movInterval) {
                lastTime = Time.fixedTime;
                if (G_tilemap.GetTileAt((int)transform.position.x + 1, (int)transform.position.z) == 1) {
                    transform.position = new Vector3((int)transform.position.x + moveDistance, (int)transform.position.y, (int)transform.position.z);
                }
            }
		} else if (Input.GetKeyDown (KeyCode.F1)) { // tää pitää saaha toimimaan ilman napin painallusta
			transform.position = G_tilemap.GetStartingLocation ();
		}
	}
}
