using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// tän skriptin on tarkotus pitää kirjaa millä tasolla mennään pelimaailmassa. 
public class GameManagerScript : MonoBehaviour {

	public int level;
	private int monsterCount;
	private int monsterTier;
	public float difficultyMultiplier;

	G_TileMap map;

	void Awake(){
		level = 1;
		difficultyMultiplier = 1f;
		SetMonsterCount ();
	}

	// Use this for initialization
	void Start () {
		map = GameObject.FindGameObjectWithTag ("GameWorld").GetComponent<G_TileMap> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetMonsterCount(){
		switch (level) {
		case 1:
			monsterCount = 5;
			break;
		default:
			monsterCount = 5 + level * 2;
			break;
		}
	}

	public void NextLevel(){
		level++;
		difficultyMultiplier += .1f;
		map.BuildMesh ();
		Debug.Log ("Went to next level > "+level+ " exp multi also +0.1 >> "+difficultyMultiplier);
	}
	public float GetDifficultyMultiplier(){
		return difficultyMultiplier;
	}
	public int GetLevel(){
		return level;
	}
	public int GetMonsterCount(){
		SetMonsterCount ();
		return monsterCount;
	}
}
