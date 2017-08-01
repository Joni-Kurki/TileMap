# Tilemap game, To Be Named

## Map
- Can generate random map
- Map contain rooms and corridors
- Can use different texture from texture palettes
- Mapsize can be scaled from editor
- Check if all rooms are connected
- Can spawn monsters to map via MonsterSpawner
- Level start (stairsup) and end (stairsdown) are now created.

#### Map creation order
- X & Y size , level index
- Creates Rooms & Corriders to map data array.
- Build Mesh() 
- * Build 2D mesh of the size x * y. Creates Vertex, Tris, Normals, UV.
- BuildTexture()
- * Gets textures from palette and creates one texture. The map data is gotten from array.
- CleanMonsters()
- * Cleans monsters from the map. e.g. if we go to next level etc. 
- SpawnMonsters()
- * Spawns number of monster to floor tiles.

## Player
- 4 way moving
- cannot go through walls
- Can detect if standing on stairs etc
- Can loot time
- Time = Combination of Life and money
- Info, says if standing on something
- Can die
- Can do damage
- Can go down the stairs

## Monster
- have basic 4 way moving
- Very basic AI to navigate random spots
- Monsters can have different types, tiers and arts
- Have hitRange, which checks if player is in range (4 way check)
- Monsters have hitrange indicator, which is colored by monster tier.
- Monsters have a scalable hp bar
- Monsters drop time, which is health and currency. 
- To Be Fixed somewher along the road:: bad AI for pathfinding: monster cannot get to waypoints.

## Controls
- WASD movement
- e for entering to lower floors.
- Nothing else atm!
