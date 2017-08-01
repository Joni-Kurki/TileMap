using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    public int time;
    private int damage;
    bool isDead;
    Text timeText;
    Text infoText;
    Text expText;
	Text gameOverText;
    private float movementInterval;
    private float lastTime;
    private bool spawnerFound;
    private int experience;

    G_TileMap map;
    MonsterSpawnerScript spawner;
    MonsterScript mScript;
	GameManagerScript gmS;
    // Use this for initialization
	void Awake() {
        isDead = false;
        damage = 2;
        experience = 0;
	}

    void Start() {
		// pelaajan "action" viive
        movementInterval = 0.5f;
        lastTime = Time.fixedTime;
		// UI elementit
        timeText = GameObject.FindGameObjectWithTag("UI_Time").GetComponent<Text>();
        infoText = GameObject.FindGameObjectWithTag("UI_Info").GetComponent<Text>();
        expText = GameObject.FindGameObjectWithTag("UI_Exp").GetComponent<Text>();
		gameOverText = GameObject.FindGameObjectWithTag ("UI_GameOver").GetComponent<Text> ();
        timeText.text = "Time: " + time;
        infoText.text = "Common test";
        expText.text = "Exp: " + experience;
		// Tarvittavat muut skriptit
        map = GameObject.FindGameObjectWithTag("GameWorld").GetComponent<G_TileMap>();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<MonsterSpawnerScript>();
		gmS = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManagerScript> ();
    }
	// Update is called once per frame
	void Update () {
		if (isDead) {
			//gameOverText.enabled = true;
			gameOverText.text = "You died! Game Over!";
		}
        if (time <= 0 && !isDead) {
            Debug.Log("Time's up mofo! You Dead!");
            isDead = true;
        }
        if (Time.fixedTime > lastTime + movementInterval) {
            timeText.text = "Time: " + time;
            expText.text = "Exp: " + experience;
            if (map.CheckIfStandingOnSpecial((int)transform.position.x, (int)transform.position.z) != "nothing") {
                infoText.text = "" + map.CheckIfStandingOnSpecial((int)transform.position.x, (int)transform.position.z);

            } else {
                infoText.text = "";
            }
            CheckIfMonstersNearby();
			CheckIfSomethingOnTile ();
            lastTime = Time.fixedTime;
        }
		// use key
		if (Input.GetKeyDown (KeyCode.E) && (map.CheckIfStandingOnSpecial((int)transform.position.x, (int)transform.position.z) != "nothing")) {
			Debug.Log ("E");
			gmS.NextLevel ();
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
                tempM.TakeDamage(damage);
                //Debug.Log("Player x+1" + tempM.GetMType());
            }
            if ((int)transform.position.x -1 == mScript.GetMLocation().x && (int)transform.position.z == mScript.GetMLocation().y) {
                tempM = spawner.GetMonsterData(tempIndex);
                tempM.TakeDamage(damage);
                //Debug.Log("Player x-1" + tempM.GetMType());
            }
            if ((int)transform.position.x == mScript.GetMLocation().x  && (int)transform.position.z +1 == mScript.GetMLocation().y) {
                tempM = spawner.GetMonsterData(tempIndex);
                tempM.TakeDamage(damage);
                //Debug.Log("Player y+1" + tempM.GetMType());
            }
            if ((int)transform.position.x == mScript.GetMLocation().x && (int)transform.position.z -1 == mScript.GetMLocation().y) {
                tempM = spawner.GetMonsterData(tempIndex);
                tempM.TakeDamage(damage);
                //Debug.Log("Player y-1" + tempM.GetMType());
            }
        }
        
    }

	void CheckIfSomethingOnTile(){
		GameObject [] goList = GameObject.FindGameObjectsWithTag("Loot_Time");
		for (int i = 0; i < goList.Length; i++) {
			if ((int)transform.position.x == (int)goList[i].transform.position.x && (int)transform.position.z == (int)goList[i].transform.position.z) {
				//Debug.Log("Time looted!");
				TimePrefabScript tps = goList[i].GetComponent<TimePrefabScript>();
				time += tps.PickUp();
			}
		}
	}

    public void TakeDamage(int value){
        if (time > 0) {
            time -= value;
            //Debug.Log("Taking damage! -" + value + " time! Left: " + time);
        } else {
            Debug.Log("Dead cannot take damage, fool!");
        }
    }

    public bool GetIsDead() {
        return isDead;
    }

    public void AddToExp(int value) {
		experience += (int)(value * gmS.GetDifficultyMultiplier());
    }
}
