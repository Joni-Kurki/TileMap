using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    public int time;
    bool isDead;
    Text timeText;
    Text infoText;

    G_TileMap map;
	// Use this for initialization
	void Awake() {
        isDead = false;
	}

    void Start() {
        timeText = GameObject.FindGameObjectWithTag("UI_Time").GetComponent<Text>();
        infoText = GameObject.FindGameObjectWithTag("UI_Info").GetComponent<Text>();
        timeText.text = "Time: " + time;
        infoText.text = "Common test";
        map = GameObject.FindGameObjectWithTag("GameWorld").GetComponent<G_TileMap>();
    }
	// Update is called once per frame
	void Update () {
        if (time <= 0 && !isDead) {
            Debug.Log("Time's up mofo! You Dead!");
            isDead = true;
        }
        timeText.text = "Time: " + time;
        if (map.CheckIfStandingOnSpecial((int)transform.position.x, (int)transform.position.z) != "nothing") {
            infoText.text = "" + map.CheckIfStandingOnSpecial((int)transform.position.x, (int)transform.position.z);
        } else {
            infoText.text = "";
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
