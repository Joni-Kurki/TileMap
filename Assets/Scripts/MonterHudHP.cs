using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonterHudHP : MonoBehaviour {

    MonsterScript mScript;
    private const float MAX_BAR_WIDTH = 3f;

	void Start () {
        transform.localScale = new Vector3(3f, 1f, 1f);
        // haeutaan parentilta monsterscript komponentti
        mScript = transform.parent.GetComponent<MonsterScript>();
	}
	
	// Update is called once per frame
	void Update () {
        // päivitellään localscalea, eli healthbari spriten leveyttä. Max 3
        transform.localScale = new Vector3(mScript.GetMonsterHpPercentage() * MAX_BAR_WIDTH, 1f, 1f);
	}
}
