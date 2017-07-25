using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    public int time;
    bool isDead;
    Text timeText;

	// Use this for initialization
	void Awake() {
        isDead = false;
	}

    void Start() {
        timeText = GameObject.FindGameObjectWithTag("UI_Time").GetComponent<Text>();
        timeText.text = "Time: " + time;
    }
	
	// Update is called once per frame
	void Update () {
        if (time <= 0 && !isDead) {
            Debug.Log("Time's up mofo! You Dead!");
            isDead = true;
        }
        timeText.text = "Time: " + time;
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
