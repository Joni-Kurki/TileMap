using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour {

    public int x;
    public int y;
    public string mType;
    public Sprite [] monsterArts;
    int monsterID;
	public bool hasDestination = false;
	private float movementInterval;
	private float lastTime;
	public int destX;
	public int destY;

	MonsterSpawnerScript masterSpawner;
	public Transform player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		movementInterval = 0.5f;
		lastTime = Time.fixedTime;
		x = (int)transform.position.x;
		y = (int)transform.position.z;
		masterSpawner = gameObject.GetComponentInParent<MonsterSpawnerScript> ();
        //Debug.Log(monsterArts.);
		/*
		string t = "";
		for (int i = 0; i < 30; i++) {
			for (int j = 0; j < 30; j++) {
				t += "["+tilemapData[i,j]+"]";
			}
			t += "\n";
		}
		Debug.Log (t);
		*/
	}

    public void setMonsterID(int ID) {
        this.monsterID = ID;
        InitMonster();
    }

    void InitMonster() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = monsterArts[monsterID];
    }

	// Update is called once per frame
	void Update () {
		if (hasDestination) {
			if (Time.fixedTime > lastTime + movementInterval) {
				if (canHitPlayer ()) {
					Debug.Log ("SMACK");
				} else {
					Move ();
				}
				lastTime = Time.fixedTime;
			}
		}

	}

	public bool canHitPlayer(){
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		if (player.position.x == transform.position.x && player.position.z == transform.position.z ) {
			return true;
		}
		return false;
	}

	public void SetDestination(int dx, int dy){
		destX = dx;
		destY = dy;
		hasDestination = true;
	}

	void Move(){
		//Debug.Log (masterSpawner.GetTileAt((int)transform.position.x,(int)transform.position.z));
		bool xLoop = true;
		bool yLoop = true;
		int r = 0;
		if (xLoop && yLoop) {
			r = Random.Range (0, 2);
		} else if (xLoop && !yLoop) {
			r = 0;
		}else if(!xLoop && yLoop){
			r = 1;
		}
		switch (r) {
			case 0:
			if (x < destX && (masterSpawner.GetTileAt((int)transform.position.x + 1,(int)transform.position.z) == 1)) {
					x++;
					transform.position = new Vector3 ((int)transform.position.x + 1, (int)transform.position.y, (int)transform.position.z);
				} else if (x > destX && (masterSpawner.GetTileAt((int)transform.position.x -1 ,(int)transform.position.z) == 1)) {
					x--;
					transform.position = new Vector3 ((int)transform.position.x - 1, (int)transform.position.y, (int)transform.position.z);
				} else {
					xLoop = false;
				}
				break;
			case 1:
			if (y < destY && (masterSpawner.GetTileAt((int)transform.position.x,(int)transform.position.z +1) == 1)) {
					y++;
					transform.position = new Vector3 ((int)transform.position.x, (int)transform.position.y, (int)transform.position.z + 1);
				} else if (y > destY && (masterSpawner.GetTileAt((int)transform.position.x,(int)transform.position.z -1) == 1)) {
					y--;
					transform.position = new Vector3 ((int)transform.position.x, (int)transform.position.y, (int)transform.position.z - 1);
				} else {
					yLoop = false;
				}
				break;
		}
		if (x == destX && y == destY) {
			hasDestination = false;
			Debug.Log ("@dest");
			SetDestination (Random.Range (0, 50), Random.Range (0, 50));
		}
		//Debug.DrawLine (transform.position + new Vector3(.5f, 0, .5f), new Vector3 (destX + .5f, 0.01f, destY+ .5f), Color.red, .5f);
	}
}
