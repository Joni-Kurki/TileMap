using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster {
    int x = 0;
    int y = 0;
    int hp;
    int artsID;
    string mType;

    public Monster(int hp, string mType, int artsID) {
        this.hp = hp;
        this.mType = mType;
        this.artsID = artsID;
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
    public Sprite [] monsterArts;
    List<Monster> mDBList;
    List<Monster> mList;

	// Use this for initialization
	void Start () {
        mDBList = new List<Monster>();
        mList = new List<Monster>();
        InitMonstersTypes(mDBList);
        Debug.Log("Monsters in db: " + mDBList.Count + " arts in db: "+monsterArts.Length );
	}
    // Luodaan monster database listaan, josta voidaan sitten hakea monstereita spawnerille. 
    // Muutetaan mType tarvittaessa indeksi numeroksi FindMonsterIDByString -metodilla, artseja varten
    void InitMonstersTypes(List<Monster> mDBList) { 
        Monster goblin = new Monster(5, "Goblin", 0);
        mDBList.Add(goblin);
        Monster devil = new Monster(10, "Devil", 1);
        mDBList.Add(devil);
        Monster skeleton = new Monster(6, "Skeleton", 2);
        mDBList.Add(skeleton);
        //FindAndSpawnMonsters(spawnX, spawnY, mType);
    }

    void FindAndSpawnMonsters(int x, int y, string mType, int number = 1) {
        bool found = false;
        for (int i = 0; i < mDBList.Count; i++) {
            if (mDBList[i].GetMType() == mType) {
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                sr.sprite = monsterArts[mDBList[i].GetArtsID()];
                found = true;
                transform.position = new Vector3(x, 0.01f, y);
            }
        }
        if(!found){
            Debug.Log("Monster type dont match!");
        }
    }

    int FindMonsterIDByString(string mType) {
        bool found = false;
        int temp = -1;
        for (int i = 0; i < mDBList.Count; i++) {
            if (mDBList[i].GetMType() == mType) {
                found = true;
                temp = mDBList[i].GetArtsID();
                //Debug.Log("Found monster "+i);
                break;
            }
        }
        if (!found) {
            Debug.Log("Monster type dont match!");
        }
        return temp;
    }

    void InstantiateMonster(int x, int y, string mType) { // instatioidaan monsteri peliin
        Monster tMon = new Monster(5, mType, 2);
        mList.Add(tMon);
        int tempIndex = mList.Count - 1;
        GameObject go = Instantiate(monsterPrefab, new Vector3(x, 0.01f, y), monsterPrefab.transform.rotation, transform);
        MonsterScript mS = go.GetComponent<MonsterScript>();
        mS.setMonsterID(FindMonsterIDByString(mType));
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F5)) {
            InstantiateMonster(13, 15, "Goblin");
            InstantiateMonster(14, 15, "Devil");
            InstantiateMonster(15, 15, "Skeleton");
        }
	}
}
