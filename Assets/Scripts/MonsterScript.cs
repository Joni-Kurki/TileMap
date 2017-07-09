using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour {

    public int spawnX;
    public int spawnY;
    public string mType;
    public Sprite [] monsterArts;
    int monsterID;

	// Use this for initialization
	void Start () {
        
        //Debug.Log(monsterArts.);
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
       
	}
}
