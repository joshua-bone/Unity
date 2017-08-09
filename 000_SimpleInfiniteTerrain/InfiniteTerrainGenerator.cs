//InfiniteTerrainGenerator.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteTerrainGenerator : MonoBehaviour
{
	public GameObject player;
	public GameObject plane;
	public float SCALE = 1f;
	public int RADIUS = 1;
	public int PLANE_SIZE_IN_QUADS = 100;
	private float REAL_PLANE_SIZE;
	Hashtable tiles = new Hashtable ();
	Vector2 lastTilePosition;

	// Use this for initialization
	void Start ()
	{
		REAL_PLANE_SIZE = SCALE * PLANE_SIZE_IN_QUADS;
		this.gameObject.transform.position = Vector3.zero;
		lastTilePosition = GetPositionOfTileContainingPlayer ();
		GenerateTiles (lastTilePosition, Time.realtimeSinceStartup);

	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector2 newTilePosition = GetPositionOfTileContainingPlayer ();
		if (!newTilePosition.Equals (lastTilePosition)) {
			GenerateTiles (newTilePosition, Time.realtimeSinceStartup);
			lastTilePosition = newTilePosition;
		}
	}

	Vector2 GetPositionOfTileContainingPlayer ()
	{
		Vector3 playerPos = player.transform.position;
		return new Vector2 (
			(int)Mathf.Floor (playerPos.x / REAL_PLANE_SIZE) * REAL_PLANE_SIZE,
			(int)Mathf.Floor (playerPos.z / REAL_PLANE_SIZE) * REAL_PLANE_SIZE);
	}

	void GenerateTiles (Vector2 center, float updateTime)
	{
		for (int x = -RADIUS; x <= RADIUS; x++) {
			for (int z = -RADIUS; z <= RADIUS; z++) {
				Vector3 position = new Vector3 (x * REAL_PLANE_SIZE + center.x, 
					                   0, 
					                   z * REAL_PLANE_SIZE + center.y);
				CreateOrHashTile (position, Time.realtimeSinceStartup);
			}
		}
		RemoveStaleTiles (updateTime);
	}

	void CreateOrHashTile (Vector3 position, float updateTime)
	{
		string tileName = "Tile_" + position.x + "_" + position.z;
		if (!tiles.ContainsKey (tileName)) {
			GameObject t = Instantiate<GameObject> (plane, position, Quaternion.identity);
			t.SendMessage ("ResizeTo", PLANE_SIZE_IN_QUADS);
			t.SendMessage ("ScaleTo", SCALE);
			t.name = tileName;
			Tile tile = new Tile (t, updateTime);
			tiles.Add (t.name, tile);
		} else {
			(tiles [tileName] as Tile).creationTime = updateTime;
		}
	}

	void RemoveStaleTiles (float updateTime)
	{
		Hashtable newTiles = new Hashtable ();
		foreach (Tile t in tiles.Values) {
			if (updateTime - t.creationTime < 0.00001f) {
				newTiles.Add (t.tile.name, t);
			} else {
				Destroy (t.tile);
			}
		}
		tiles = newTiles;
	}

}

class Tile
{
	public GameObject tile;
	public float creationTime;

	public Tile (GameObject t, float time)
	{
		tile = t;
		creationTime = time;
	}
}
