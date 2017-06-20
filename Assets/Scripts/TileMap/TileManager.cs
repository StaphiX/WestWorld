using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    const int CHUNKW = 32;
    const int CHUNKH = 32;

    public ObjectPool tTilePool = null;
 
	// Use this for initialization
	void Start ()
    {
        LoadChunk();
    }

    void LoadChunk()
    {
        if (tTilePool == null)
            return;

        for (int iTileX = 0; iTileX < CHUNKW; ++iTileX)
        {
            for (int iTileY = 0; iTileY < CHUNKH; ++iTileY)
            {
                tTilePool.GetObject();
            }
        }
    }

	// Update is called once per frame
	void Update () 
    {
		
	}
}
