using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public int time;
    bool isDead;

	// Use this for initialization
	void Awake() {
        isDead = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (time <= 0 && !isDead) {
            Debug.Log("Time's up mofo! You Dead!");
            isDead = true;
        }
	}

    public void TakeDamage(int value){
        time -= value;
        Debug.Log("Taking damage! -"+value+" time! Left: "+time);
    }
}
