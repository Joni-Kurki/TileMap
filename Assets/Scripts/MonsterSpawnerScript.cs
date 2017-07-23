using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster {
    int x = 0;
    int y = 0;
    int hp;
    int artsID;
    int hitRange;
    string mType;

    public Monster(string mType) {
        this.mType = mType;
        setMonsterStats();
    }
    void setMonsterStats() {
        switch (mType) {
            case "Goblin":
                hp = 5;
                artsID = 0;
                hitRange = 1;
                Debug.Log("Gob "+hp+" "+hitRange);
                break;
            case "Devil":
                hp = 8;
                hitRange = 2;
                artsID = 1;
                Debug.Log("Dev " + hp + " " + hitRange);
                break;
            case "Skeleton":
                hp = 6;
                hitRange = 1;
                artsID = 2;
                Debug.Log("Ske " + hp + " " + hitRange);
                break;
        }
    }

    public int GetHitRange() {
        return hitRange;
    }

    public void SetLocation(int x, int y) {
        this.x = x;
        this.y = y;
    }
    public int GetArtsID() {
        return artsID;
    }
    public string GetMType() {
        return mType;
    }
}

public class MonsterSpawnerScript : MonoBehaviour {

    public int spawnX;
    public int spawnY;
    public string mType;

    public GameObject monsterPrefab;
    //public Sprite [] monsterArts;
    List<Monster> mDBList;
    List<Monster> mList;

	G_TileMap map;
	public GameObject tilemapPrefab;

	// Use this for initialization
	void Start () {
        mDBList = new List<Monster>();
        mList = new List<Monster>();
        InitMonstersTypes(mDBList);
        //Debug.Log("Monsters in db: " + mDBList.Count);
		map = tilemapPrefab.GetComponent<G_TileMap> ();
		//Debug.Log (map.GetTileAt (0, 0) + " " + map.GetTileAt (10, 10));
	}
    // Luodaan monster database listaan, josta voidaan sitten hakea monstereita spawnerille. 
    // Muutetaan mType tarvittaessa indeksi numeroksi FindMonsterIDByString -metodilla, artseja varten
    void InitMonstersTypes(List<Monster> mDBList) { 
        Monster goblin = new Monster("Goblin");
        mDBList.Add(goblin);
        Monster devil = new Monster("Devil");
        mDBList.Add(devil);
        Monster skeleton = new Monster("Skeleton");
        mDBList.Add(skeleton);
        //FindAndSpawnMonsters(spawnX, spawnY, mType);
    }

	public int GetTileAt(int x, int y){
		return map.GetTileAt (x, y);
	}

    int FindMonsterIDByString(string mType) {
        bool found = false;
        int temp = -1;
		if (mType == "Random") {
			temp = Random.Range (0, mDBList.Count);
		} else {
			for (int i = 0; i < mDBList.Count; i++) {
				if (mDBList [i].GetMType () == mType) {
					found = true;
					temp = mDBList [i].GetArtsID ();
					//Debug.Log("Found monster "+i);
					break;
				}
			}
			if (!found) {
				Debug.Log ("Monster type dont match!");
			}
		}
        return temp;
    }

    public void InstantiateMonster(int x, int y, string mType) { // instatioidaan monsteri peliin
        if (mType == "Random") {
            switch(Random.Range(0,mDBList.Count)){
                case 0:
                    mType = "Goblin";
                    break;
                case 1:
                    mType = "Devil";
                    break;
                case 2:
                    mType = "Skeleton";
                    break;
            }
        }
        Monster tMon = new Monster(mType);
        mList.Add(tMon);
        int tempIndex = mList.Count - 1;
        GameObject go = Instantiate(monsterPrefab, new Vector3(x, 0.03f, y), monsterPrefab.transform.rotation, transform);
        MonsterScript mS = go.GetComponent<MonsterScript>();
        mS.setMonsterID(FindMonsterIDByString(mType));
		//mS.tilemapData = tileMapData;
		mS.SetDestination (Random.Range(0, 50), Random.Range(0,50));
        Debug.Log("Instantiate monster " + tMon.GetMType());
        mS.SetHitRange(tMon.GetHitRange());
    }

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F6)) {
			Debug.Log (""+mList.Count);
			Debug.Log (map.GetTileAt (0, 0) + " " + map.GetTileAt (10, 10));
		}
	}
}
