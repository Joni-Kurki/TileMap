using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_TileMap{
	public class Data_Room {
		public int left;
		public int top;
		public int width;
		public int height;

        public Data_Room(int left, int top, int width,int height){
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
        }

		public bool isConnected=false;

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
	}

	int tilemap_size_x;
	int tilemap_size_y;

	int [,] tilemap_data;

	public List<Data_Room> rooms;
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
	public Data_TileMap(int tilemap_size_x, int tilemap_size_y){
		this.tilemap_size_x = tilemap_size_x;
		this.tilemap_size_y = tilemap_size_y;

		tilemap_data = new int[tilemap_size_x, tilemap_size_y];

		for (int x = 0; x < tilemap_size_x; x++) {
			for (int y = 0; y < tilemap_size_y; y++) {
				tilemap_data [x, y] = 3; // tähän tyhjä tile
			}
		}

		rooms = new List<Data_Room> ();
        Data_Room dr;
		for (int i = 0; i < 3; i++) {
            int r_size_x = Random.Range(6, 9);
            int r_size_y = Random.Range(6, 8);
            dr = new Data_Room(Random.Range (0, tilemap_size_x - r_size_x), Random.Range (0, tilemap_size_y - r_size_y), r_size_x, r_size_y);

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
        MakeWalls();/*
		if (rooms.Count > 1) {
			for (int i = 1; i < rooms.Count; i++) {
				if (isConnectedToOtherRoom (rooms [0], rooms [i])) {
					Debug.Log ("Can connect to:" + i);
				} else {
					Debug.Log ("Cannot connect to all! "+i);
					break;
				}
			}
		}*/
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
        while (x != r2.center_x()){
            tilemap_data[x, y] = 1;
            x += x < r2.center_x() ? 1 : -1;
        }
        while (y != r2.center_y()) {
            tilemap_data[x, y] = 1;
            y += y < r2.center_y() ? 1 : -1;
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

	/*
    void MakePond(int start_x, int start_y, int spread_p, int size){
        int pondLeft = size;
        int tries = 0;
        if (start_x >= 0 && start_y >= 0 && start_x <= tilemap_size_x-1 && start_y <= tilemap_size_y-1){
            tilemap_data[start_x, start_y] = 1;
            while (pondLeft > 0){
                Vector2 temp = GetRandomAdjecentTile(start_x, start_y);
                if (temp.x >= 0 && temp.y >= 0 && GetTileAt((int)temp.x, (int)temp.y) != 1){
                    tilemap_data[(int)temp.x, (int)temp.y] = 1;
                    pondLeft--;
                    tries = 0;
                }
                if (tries > 4){
                    Debug.Log("Tried " + tries);

                    break;
                }
                tries++;
            }
        }
    }
    Vector2 GetRandomAdjecentTile(int start_x, int start_y){
        int r = Random.Range(0, 4);
        switch (r){
            case 0:
                return new Vector2(start_x - 1, start_y);
            case 1:
                return new Vector2(start_x, start_y-1);
            case 2:
                return new Vector2(start_x + 1, start_y);
            case 3:
                return new Vector2(start_x, start_y+1);
            default:
                return new Vector2(0,0);
        }
    }
	*/
}
