using UnityEngine;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour
{

    [SerializeField]
    Terrain terrain;

    [SerializeField]
    Texture2D[] textures;

    [SerializeField]
    int width, height;

    int heightMapResolution = 1; // its a flat map

    SplatPrototype[] splats = new SplatPrototype[2];

    void Start()
    {
        pathPlanner.GraphGridMap gridMap = GetComponent<GameInterface>().pathplannerMap;
        width = gridMap.GetWidth();
        height = gridMap.GetHeight();

        TerrainData data = new TerrainData();

        //data.heightmapResolution = heightMapResolution;
        //data.SetHeights(0, 0, new float[1, 1] { { 0.0f } });

        data.size = new Vector3(512, 512, 512);

        splats[0] = new SplatPrototype();
        splats[0].texture = textures[0];
        splats[0].tileSize = new Vector2(4, 4);

        splats[1] = new SplatPrototype();
        splats[1].texture = textures[1];
        splats[1].tileSize = new Vector2(4, 4);

        data.splatPrototypes = splats;

        float[, ,] alphaMap = new float[512, 512, 2];
        for (int x = 0; x < width; ++x)
        {

            for(int y = 0; y < height; ++y)
            {
                if(gridMap.GetNodeByIndex(x, y).GetTraversable())
                {
                    alphaMap[y, x, 0] = 1;
                    alphaMap[y, x, 1] = 0;
                }
                else
                {
                    alphaMap[y, x, 0] = 0;
                    alphaMap[y, x, 1] = 1;
                }

            }

        }

        data.alphamapResolution = 512;
        data.SetAlphamaps(0, 0, alphaMap);

        terrain = Terrain.CreateTerrainGameObject(data).GetComponent<Terrain>();

        terrain.transform.position = new Vector3(-0.5f, 0, -0.5f);

    }
}
