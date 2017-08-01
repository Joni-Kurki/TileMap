using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHitRangeIndicatorScript : MonoBehaviour {

	public Sprite [] hitRangeIndicator;
	public GameObject hitRangeIndicatorPrefab;
	Transform player;
	MonsterScript ms;
	SpriteRenderer sr;
	public int xOrY;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		ms = transform.parent.GetComponent<MonsterScript> ();
		sr = GetComponent<SpriteRenderer>();
		//GameObject parentGO = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		CreateHitRangeEffect ();
	}

	void CreateHitRangeEffect(){
		Monster tMon = ms.GetMonsterData ();
		//spriteToUse = hitRangeIndicator[tMon.GetMonsterTier()];
		sr.sprite = hitRangeIndicator[tMon.GetMonsterTier()];
		if (xOrY == 0) {
			if (ms.GetHitRange () == 1) {
				transform.position = new Vector3 ((int)transform.parent.position.x - 1, (int)transform.parent.position.y, (int)transform.parent.position.z);
				transform.localScale = new Vector3 (3f, 1f, 1f);
			} else if (ms.GetHitRange () == 2) {
				transform.position = new Vector3 ((int)transform.parent.position.x - 2, (int)transform.parent.position.y, (int)transform.parent.position.z);
				transform.localScale = new Vector3 (5f, 1f, 1f);
			}
		}else if (xOrY == 1) {
			if (ms.GetHitRange () == 1) {
				transform.position = new Vector3 ((int)transform.parent.position.x, (int)transform.parent.position.y, (int)transform.parent.position.z - 1);
				transform.localScale = new Vector3 (1f, 3f, 1f);
			} else if (ms.GetHitRange () == 2) {
				transform.position = new Vector3 ((int)transform.parent.position.x, (int)transform.parent.position.y, (int)transform.parent.position.z - 2);
				transform.localScale = new Vector3 (1f, 5f, 1f);
			}
		}

		//Debug.Log ("Using sprite: " + tMon.GetMonsterTier ());
	}
}
