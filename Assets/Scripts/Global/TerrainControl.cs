using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Chunk
{
	public GameObject chunk;
	public float timeStamp;

	public Chunk (GameObject go, float time)
	{
		chunk = go;
		timeStamp = time;
	}
}

public class TerrainControl : MonoBehaviour
{
	public GameObject terrainChunk;
	public GameObject obj;

	int chunkSize = 10;
	int updateDistance = 3;
	float currTime = 0f;

	Vector3 lastPos;
	Hashtable loadedChunks = new Hashtable();

	public void setUpdateDistance (int dist)
	{
		updateDistance = dist;
	}
	/// <summary>
	/// Generates the terrain chunks around coordinate.
	/// </summary>
	/// <param name="chunkCoor">The coordinate of the center of the chunk.</param>
	void generateTerrainAroundCoordinate (Vector3 chunkCoor)
	{
		for (int x = - updateDistance; x <= updateDistance; x++) 
		{
			for (int z = Mathf.Abs(x) - updateDistance; z <= updateDistance - Mathf.Abs(x); z++) 
			{
				Vector3 pos = new Vector3 (chunkSize * (chunkCoor.x + x), 0, chunkSize * (chunkCoor.z + z));
				string chunkName = "chunk_" + ((int)(pos.x)).ToString () + "_" + ((int)(pos.z)).ToString ();
				float timeStamp = currTime;

				if (!loadedChunks.ContainsKey (chunkName)) 
				{
					GameObject go = (GameObject)Instantiate (terrainChunk, pos, Quaternion.identity);
					go.name = chunkName;
					Chunk chunk = new Chunk (go, timeStamp);
					loadedChunks.Add (chunkName, chunk);
				} 
				else 
				{
					(loadedChunks [chunkName] as Chunk).timeStamp = timeStamp;
				}
			}
		}
	}

	/// <summary>
	/// Updates the timestamp of the chunks in view, unload chunks out of sight.
	/// </summary>
	void updateChunks ()
	{
		Hashtable tempChunkTable = new Hashtable ();
		foreach (Chunk c in loadedChunks.Values) 
		{
			if (c.timeStamp != currTime)
				Destroy (c.chunk);
			else
				tempChunkTable.Add (c.chunk.name, c);
		}
		loadedChunks = tempChunkTable;
	}

	/// <summary>
	/// Gets the coordinate of the center of the chunk, in which the object stands.
	/// </summary>
	/// <returns>The coordinate of the center of the chunk.</returns>
	/// <param name="coor">Coordinate of the object.</param>
	Vector3 getChunkCoor (Vector3 coor)
	{
		Vector3 chunkCoor = Vector3.zero;

		chunkCoor.x = MathExt.getFloorToZero (coor.x / chunkSize);
		chunkCoor.z = MathExt.getFloorToZero (coor.z / chunkSize); 

		return chunkCoor;
	}

	void Start ()
	{
		Vector3 currPos = obj.transform.position;
		Vector3 currChunk = getChunkCoor (currPos);
		currTime = Time.realtimeSinceStartup;
		generateTerrainAroundCoordinate (currPos);
		lastPos = obj.transform.position;
	}

	void Update ()
	{
		Vector3 currPos = obj.transform.position;
		Vector3 currChunk = getChunkCoor (currPos);
		currTime = Time.realtimeSinceStartup;
		if (getChunkCoor(lastPos) != getChunkCoor(currPos))
		{
			Debug.Log ("current pos: " + currPos + " of chunk: " + getChunkCoor(currPos));
			Debug.Log ("last pos: " + lastPos + " of chunk: " + getChunkCoor(lastPos));

			generateTerrainAroundCoordinate (currChunk);
			updateChunks ();
			lastPos = currPos;
		}
	}

}
