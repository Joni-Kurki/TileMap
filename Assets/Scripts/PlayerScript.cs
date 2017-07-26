using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    public int time;
    bool isDead;
    Text timeText;
    Text infoText;
    private float movementInterval;
    private float lastTime;
    private bool spawnerFound;

    G_TileMap map;
    MonsterSpawnerScript spawner;
    MonsterScript mScript;
    // Use this for initialization
	void Awake() {
        isDead = false; 
	}

    void Start() {
        movementInterval = 0.5f;
        lastTime = Time.fixedTime;
        timeText = GameObject.FindGameObjectWithTag("UI_Time").GetComponent<Text>();
        infoText = GameObject.FindGameObjectWithTag("UI_Info").GetComponent<Text>();
        timeText.text = "Time: " + time;
        infoText.text = "Common test";
        map = GameObject.FindGameObjectWithTag("GameWorld").GetComponent<G_TileMap>();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<MonsterSpawnerScript>();
    }
	// Update is called once per frame
	void Update () {
        if (time <= 0 && !isDead) {
            Debug.Log("Time's up mofo! You Dead!");
            isDead = true;
        }
        if (Time.fixedTime > lastTime + movementInterval) {
            timeText.text = "Time: " + time;
            if (map.CheckIfStandingOnSpecial((int)transform.position.x, (int)transform.position.z) != "nothing") {
                infoText.text = "" + map.CheckIfStandingOnSpecial((int)transform.position.x, (int)transform.position.z);
            } else {
                infoText.text = "";
            }
            CheckIfMonstersNearby();
            lastTime = Time.fixedTime;
        }
	}

    void CheckIfMonstersNearby() {
        GameObject [] goList = GameObject.FindGameObjectsWithTag("Monster");
        for (int i = 0; i < goList.Length; i++) {
            mScript = goList[i].GetComponent<MonsterScript>();
            int tempIndex = mScript.GetListIndex();
            Monster tempM;
            if ((int)transform.position.x + 1 == mScript.GetMLocation().x && (int)transform.position.z == mScript.GetMLocation().y) {
                tempM = spawner.GetMonsterData(tempIndex);
                tempM.TakeDamage(1);
                Debug.Log("Player x+1" + tempM.GetMType());
            }
            if ((int)transform.position.x -1 == mScript.GetMLocation().x && (int)transform.position.z == mScript.GetMLocation().y) {
                tempM = spawner.GetMonsterData(tempIndex);
                tempM.TakeDamage(1);
                Debug.Log("Player x-1" + tempM.GetMType());
            }
            if ((int)transform.position.x == mScript.GetMLocation().x  && (int)transform.position.z +1 == mScript.GetMLocation().y) {
                tempM = spawner.GetMonsterData(tempIndex);
                tempM.TakeDamage(1);
                Debug.Log("Player y+1" + tempM.GetMType());
            }
            if ((int)transform.position.x == mScript.GetMLocation().x && (int)transform.position.z -1 == mScript.GetMLocation().y) {
                tempM = spawner.GetMonsterData(tempIndex);
                tempM.TakeDamage(1);
                Debug.Log("Player y-1" + tempM.GetMType());
            }
        }
    }

    public void TakeDamage(int value){
        if (time > 0) {
            time -= value;
            Debug.Log("Taking damage! -" + value + " time! Left: " + time);
        } else {
            Debug.Log("Dead cannot take damage, fool!");
        }
    }
}
