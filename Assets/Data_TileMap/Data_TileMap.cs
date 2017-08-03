using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_TileMap{
	public class Data_Room {
		public int left;
		public int top;
		public int width;
		public int height;
        bool hasStairsUp;
        bool hasStairsDown;
        public bool isConnected = false;

        public Data_Room(int left, int top, int width,int height){
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
            hasStairsUp = false;
            hasStairsDown = false;
        }

		public int right() {
			return left + width - 1;
		}
		public int bottom(){
			return top + height - 1;
		}
		public int center_x(){
			return left + width / 2;
		}
		public int center_y(){
			return top + height / 2;
		}
		public bool CollidesWithOther(Data_Room other){ // true jos osuu
			if (left > other.right () - 1)
				return false;
			if (top > other.bottom () - 1)
				return false;
			if (right () < other.left + 1)
				return false;
			if (bottom () < other.top + 1)
				return false;
			return true;
		}
        public void HasStairsUp() {
            hasStairsUp = true;
        }
        public void HasStairsDown() {
            hasStairsDown = true;
        }
	}

	int tilemap_size_x;
	int tilemap_size_y;

	int [,] tilemap_data;

	public List<Data_Room> rooms;
    public List<Vector2> stairs;
	/*
	 * 0 = unknown
	 * 1 = fog
	 * 2 = floor
	 * 3 = wall
	 * -------
	 * 0 = unknown
	 * 1 = water
	 * 2 = grass
	 * 3 = sand
	 */
	public Data_TileMap(int tilemap_size_x, int tilemap_size_y, int level){
		this.tilemap_size_x = tilemap_size_x;
		this.tilemap_size_y = tilemap_size_y;
		int roomCount = 5 + level;

		tilemap_data = new int[tilemap_size_x, tilemap_size_y];
        for (int ite = 0; ite < 10; ite++){
            for (int x = 0; x < tilemap_size_x; x++){
                for (int y = 0; y < tilemap_size_y; y++){
                    tilemap_data[x, y] = 3; // tähän tyhjä tile
                }
            }

            rooms = new List<Data_Room>();
            Data_Room dr;
			for (int i = 0; i < roomCount; i++){
                int r_size_x = Random.Range(5, 9);
                int r_size_y = Random.Range(4, 8);
                dr = new Data_Room(Random.Range(0, tilemap_size_x - r_size_x+1), Random.Range(0, tilemap_size_y - r_size_y+1), r_size_x, r_size_y);

                if (!RoomCollides(dr)){
                    rooms.Add(dr);
                    MakeBonds(dr.left, dr.top, dr.width, dr.height);
                }
            }

            for (int i = 0; i < rooms.Count; i++){
                if (!rooms[i].isConnected){
                    int j = Random.Range(1, rooms.Count);
                    MakeCorridor(rooms[i], rooms[(i + j) % rooms.Count]);
                }
            }
			//Test ();
            MakeWalls();
            //Debug.Log("Floortiles: "+GetNumberOfFloorTiles ());
            //Debug.Log("Found tiles connected: "+GetNumberOfConnectedFloorTiles ());

            if (GetNumberOfFloorTiles() == GetNumberOfConnectedFloorTiles()){
                //Debug.Log("Floortilet täsmää. Iteraatioita "+ite);
                ite = 11;
                FillWithFloor();
            }else{
                //Debug.Log("Floortilet failaa, > uudestaan!");
            }
            MakeStairs();

        }
	}
    // täytellään takaisin floortilellä, kun saatu selville kenttä, jossa kaikki huoneet yhdistyy.
    // tänne voi laittaa myös eri tilejä sitten arvottavaksi, esim randomilla.
    void FillWithFloor(){ 
        for (int x = 0; x < tilemap_size_x; x++){
            for (int y = 0; y < tilemap_size_y; y++){
                if(tilemap_data[x,y] == 0){
                    tilemap_data[x, y] = 1;
                }
            }
        }
    }
	// testi funkkari seiniä varten
	void Test(){ 
		tilemap_data [1, 1] = 1;
		tilemap_data [2, 1] = 1;
		tilemap_data [1, 2] = 1;

		tilemap_data [1, 24] = 1;
		tilemap_data [1, 25] = 1;
		tilemap_data [1, 26] = 1;

		tilemap_data [1, tilemap_size_y-2] = 1;
		tilemap_data [1, tilemap_size_y-3] = 1;
		tilemap_data [2, tilemap_size_y-2] = 1;

		tilemap_data [24, tilemap_size_y - 2] = 1;
		tilemap_data [25, tilemap_size_y - 2] = 1;
		tilemap_data [26, tilemap_size_y - 2] = 1;

		tilemap_data [47, tilemap_size_y - 2] = 1;
		tilemap_data [48, tilemap_size_y - 2] = 1;
		tilemap_data [48, tilemap_size_y - 3] = 1;

		tilemap_data [tilemap_size_x - 2, 24] = 1;
		tilemap_data [tilemap_size_x - 2, 25] = 1;
		tilemap_data [tilemap_size_x - 2, 26] = 1;

		tilemap_data [tilemap_size_x - 2, 1] = 1;
		tilemap_data [tilemap_size_x - 2, 2] = 1;
		tilemap_data [tilemap_size_x - 3, 1] = 1;

		tilemap_data [24, 1] = 1;
		tilemap_data [25, 1] = 1;
		tilemap_data [26, 1] = 1;

	}

    void MakeStairs() {
        stairs = new List<Vector2>();
        rooms[0].HasStairsUp();
        stairs.Add(new Vector2((int)rooms[0].center_x(), (int)rooms[0].center_y()));
        rooms[rooms.Count - 1].HasStairsDown();
        stairs.Add(new Vector2((int)rooms[rooms.Count - 1].center_x(), (int)rooms[rooms.Count - 1].center_y()));
    }

    public Vector2 GetStairsUp() {
        return stairs[0];
    }
    public Vector2 GetStairsDown() {
        return stairs[1];
    }
	public int [,] getTilemapData(){
		return tilemap_data;
	}

	public Vector3 GetStartingRoomPosition(){
		return new Vector3 (rooms [0].center_x (),0, rooms [0].center_y ());
	}

	// palauttaa tilenarvon kohdasta x,y
	public int GetTileAt(int x, int y){
		return tilemap_data [x, y];
	}

    bool RoomCollides(Data_Room dr){
        foreach (Data_Room dr2 in rooms){
            if (dr.CollidesWithOther(dr2))
                return true;
        }
        return false;
    }

	void MakeBonds(int left, int top, int width, int heigth){
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < heigth; y++) {
				if (x == 0 || x == width - 1 || y == 0 || y == heigth - 1) {
					tilemap_data [left+x, top+y] = 2;
				}else
					tilemap_data [left+x, top+y] = 1;
			}
		}
	}

    void MakeCorridor(Data_Room r1, Data_Room r2){
        int x = r1.center_x();
        int y = r1.center_y();
		int temp =  Random.Range(0, 2);
		//Debug.Log ("Random " + temp);
		if (temp == 0) {
			while (x != r2.center_x ()) {
				tilemap_data [x, y] = 1;
				x += x < r2.center_x () ? 1 : -1;
			}
			while (y != r2.center_y ()) {
				tilemap_data [x, y] = 1;
				y += y < r2.center_y () ? 1 : -1;
			}
		} else {
			while (y != r2.center_y ()) {
				tilemap_data [x, y] = 1;
				y += y < r2.center_y () ? 1 : -1;
			}
			while (x != r2.center_x ()) {
				tilemap_data [x, y] = 1;
				x += x < r2.center_x () ? 1 : -1;
			}
		}
        r1.isConnected = true;
        r2.isConnected = true;
    }

    void MakeWalls(){
        for (int x = 0; x < tilemap_size_x; x++){
            for (int y = 0; y < tilemap_size_y; y++){
                if(tilemap_data[x,y] == 3 && HasAdjecentFloor(x,y)){
                    tilemap_data[x, y]  = 2;
                }
            }
        }
    }

    bool HasAdjecentFloor(int x, int y){
        if (x > 0 && tilemap_data[x - 1, y] == 1)
            return true;
        if (x < tilemap_size_x-1 && tilemap_data[x + 1, y] == 1)
            return true;
        if (y > 0 && tilemap_data[x, y-1] == 1)
            return true;
        if (y < tilemap_size_y - 1 && tilemap_data[x, y+1] == 1)
            return true;

        if(x > 0 && y > 0 && tilemap_data[x-1,y-1] == 1)
            return true;
        if (x < tilemap_size_x-1 && y > 0 && tilemap_data[x + 1, y - 1] == 1)
            return true;
        if (x > 0 && y < tilemap_size_y-1 && tilemap_data[x - 1, y + 1] == 1)
            return true;
        if (x < tilemap_size_x-1 && y < tilemap_size_x-1 && tilemap_data[x+ 1, y + 1] == 1)
            return true;

        return false;
    }

	int GetNumberOfFloorTiles(){
		int floorCounter = 0;
		for (int x = 0; x < tilemap_size_x; x++) {
			for (int y = 0; y < tilemap_size_y; y++) {
				if (tilemap_data [x, y] == 1)
					floorCounter++;
			}
		}
		return floorCounter;
	}

	int GetNumberOfConnectedFloorTiles(){
		Vector2 startLocation = new Vector2 (0, 0);
		for (int x = 0; x < tilemap_size_x; x++) {
			for (int y = 0; y < tilemap_size_y; y++) {
				if (tilemap_data [x, y] == 1) {
					startLocation = new Vector2 (x, y);
				}
			}
		}
		//Debug.Log (startLocation.x+"|"+startLocation.y);
		List<Vector2> startPointInAList = new List<Vector2>();
		startPointInAList.Add (startLocation);
		int temp = RecuTest (startPointInAList,1);
		//Debug.Log (temp);
		return temp;
	}

	int RecuTest(List<Vector2> newTiles, int iterations){
		if (iterations > 1) {
			//Debug.Log (">10 breaking");
			return -1;
		}
		if (newTiles.Count == 0) {
			//Debug.Log ("0 > returning");
			return -1;
		} else {
			for (int i = 0; i < newTiles.Count; i++) {
				if(tilemap_data[(int)newTiles[i].x-1, (int)newTiles[i].y] == 1){
					//Debug.Log ("-1|0");
					newTiles.Add (new Vector2 (newTiles[i].x-1, newTiles[i].y));
					tilemap_data [(int)newTiles [i].x - 1, (int)newTiles [i].y] = 0;
				}
				if(tilemap_data[(int)newTiles[i].x+1, (int)newTiles[i].y] == 1){
					//Debug.Log ("+1|0");
					newTiles.Add (new Vector2 (newTiles[i].x+1, newTiles[i].y));
					tilemap_data [(int)newTiles [i].x + 1, (int)newTiles [i].y] = 0;
				}
				if(tilemap_data[(int)newTiles[i].x, (int)newTiles[i].y-1] == 1){
					//Debug.Log ("0|-1");
					newTiles.Add (new Vector2 (newTiles[i].x, newTiles[i].y-1));
					tilemap_data [(int)newTiles [i].x, (int)newTiles [i].y-1] = 0;
				}
				if(tilemap_data[(int)newTiles[i].x, (int)newTiles[i].y+1] == 1){
					//Debug.Log ("0|+1");
					newTiles.Add (new Vector2 (newTiles[i].x, newTiles[i].y+1));
					tilemap_data [(int)newTiles [i].x, (int)newTiles [i].y+1] = 0;
				}
			}
			//Debug.Log (iterations+" > continue");
			iterations += 1;
			RecuTest (newTiles, iterations);
			//return;
		}
		//Debug.Log ("List length:"+ (newTiles.Count-1));
		return (newTiles.Count - 1);
	}

	bool isConnectedToOtherRoom(Data_Room r1, Data_Room r2){
		int x = r1.center_x ();
		int y = r1.center_y();

		bool xDone = false;
		bool yDone = false;

		for (int i = 0; i < tilemap_size_x; i++) {
			if (x < r2.center_x () && xDone == false) {
				//Debug.Log ("huone 1, vasemmalla");
				if (HasAdjecentFloor (x + 1, y)) {
					tilemap_data [x+1, y] = 0;
					x++;
				} else {
					Debug.Log ("Found wall, stopping");
					xDone = true;
				}
			}if(x > r2.center_x () && xDone == false){
				Debug.Log ("huone 2, vasemmalla");
				if (HasAdjecentFloor (x - 1, y)) {
					tilemap_data [x-1, y] = 0;
					x--;
				}else {
					Debug.Log ("Found wall, stopping");
					xDone = true;
				}
			}else if(x == r2.center_x() && xDone == false){
				Debug.Log ("Sama x "+x+" = "+r2.center_x());
				xDone = true;
			}
		}
		for (int j = 0; j < tilemap_size_y; j++) {
			if (y < r2.center_y () && yDone == false) {
				//Debug.Log ("huone 1, vasemmalla");
				if (HasAdjecentFloor (x, y+1)) {
					tilemap_data [x, y+1] = 0;
					y++;
				}else {
					Debug.Log ("Found wall, stopping");
					yDone = true;
				}
			}if(y > r2.center_y () && yDone == false){
				Debug.Log ("huone 2, vasemmalla");
				if (HasAdjecentFloor (x, y - 1)) {
					tilemap_data [x, y-1] = 0;
					y--;
				}else {
					Debug.Log ("Found wall, stopping");
					yDone = true;
				}
			}else if(y == r2.center_y() && yDone == false){
				Debug.Log ("Sama y "+y+" = "+r2.center_y());
				yDone = true;
			}
		}
		if (tilemap_data [r2.center_x (), r2.center_y ()] == 0) {
			Debug.Log ("0 > true");
			return true;
		} else {
			Debug.Log ("muut > false");
			return false;
		}
	}
}
