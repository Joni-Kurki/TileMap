using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePrefabScript : MonoBehaviour {

    private int timeReward;

	// Use this for initialization
	void Start () {
        timeReward = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetTimeReward(int timeReward) {
        this.timeReward = timeReward;
    }

    public int PickUp() {
        Destroy(gameObject);
        return timeReward;
    }
}
