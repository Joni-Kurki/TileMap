using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePrefabScript : MonoBehaviour {

    public int timeReward;

	// Use this for initialization
	void Awake () {
        timeReward = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeReward == 0) {
			Destroy(gameObject);
		}
	}

    public void SetTimeReward(int timeReward) {
        this.timeReward = timeReward;
    }

    public int PickUp() {
        Destroy(gameObject);
		return timeReward;
    }
}
