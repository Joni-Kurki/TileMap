using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
	
    bool isDead;
    Text timeText;
    Text infoText;
    Text expText;
	Text gameOverText;
    private float lastTime;
    private bool spawnerFound;
    
    G_TileMap map;
    MonsterSpawnerScript spawner;
    MonsterScript mScript;
	GameManagerScript gmS;

	public int [] levels;

	// Visible stats
	public int time;
	public int damage;
	public float actionInterval;
	public int experience;
	public int level;
	public const int MAX_LEVEL = 50;

    // Use this for initialization
	void Awake() {
		isDead = false;
		time = 22;
        damage = 2;
        experience = 0;
		level = 1;
		SetExperienceLevels ();
		SetPlayerLevel ();
	}

    void Start() {
		// pelaajan "action" viive
		actionInterval = 0.5f;
        lastTime = Time.fixedTime;
		// UI elementit
        timeText = GameObject.FindGameObjectWithTag("UI_Time").GetComponent<Text>();
        infoText = GameObject.FindGameObjectWithTag("UI_Info").GetComponent<Text>();
        expText = GameObject.FindGameObjectWithTag("UI_Exp").GetComponent<Text>();
		gameOverText = GameObject.FindGameObjectWithTag ("UI_GameOver").GetComponent<Text> ();
        timeText.text = "Time: " + time;
        infoText.text = "Common test";
		expText.text = "Exp: " + experience + " (Level: "+level+")";
		// Tarvittavat muut skriptit
        map = GameObject.FindGameObjectWithTag("GameWorld").GetComponent<G_TileMap>();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<MonsterSpawnerScript>();
		gmS = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManagerScript> ();
    }
	// aluestetaan expa levelit, joku kokeilu kaava vaan tässä atm.
	void SetExperienceLevels(){
		levels = new int[MAX_LEVEL];
		for (int i = 0; i < MAX_LEVEL; i++) {
			if (i == 0) {
				levels [i] = 8;
			} else {
				levels [i] = 2*levels [i - 1] + (levels [i - 1] / 3);
			}
		}
	}
	// päivitellään pelaajan taso mikäli mennään aina rajan yli
	void SetPlayerLevel(){
		if (experience > levels [level-1]) {
			level++;
			damage++;
		}
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
		if (Time.fixedTime > lastTime + actionInterval) {
            timeText.text = "Time: " + time;
			expText.text = "Exp: " + experience + " (Level: "+level+")";
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
		if (Input.GetKeyDown (KeyCode.E) && (map.CheckIfStandingOnSpecial((int)transform.position.x, (int)transform.position.z) == "Stairs Down")) {
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
		SetPlayerLevel ();
    }
}
