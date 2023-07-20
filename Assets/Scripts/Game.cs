using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class Game : MonoBehaviour
{
    public int mapHeight;
    public int mapWidth;
    public GameObject prefabEmpTowerGhost;
    public GameObject prefabLaserTowerGhost;
    public GameObject prefabMachineGunGhost;
    public GameObject prefabRockerGhost;
    public GameObject prefabSuperGhost;
    public GameObject prefabTeslaGhost;

    public GameObject prefabEntryGate;
    public GameObject prefabExitGate;
    public GameObject prefabRegularEnemy;
    public GameObject raycastReceiver;
    public Tilemap groundTileMap;
    public Tile[] groundTiles;
    public Transform playerPlaneTransform;
    public Transform playerPlaneTeleportTransform;

    private Map map;

    private Transform soldierTransform;
    
    /// <summary>
    /// Singleton
    /// </summary>
    private static Game game;
    public static Game Instance
    {
        get
        {
            if (game == null)
            {
                game = FindObjectOfType<Game>();
                if (game == null)
                {
                    GameObject container = new("Game");
                    game = container.AddComponent<Game>();
                }
            }
            return game;
        }
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        StopAllCoroutines();
        map = new Map();
        Map.Tiles = new CellEntity[mapWidth, mapHeight];
        // set the size of the ground
        groundTileMap.size = new Vector3Int(mapWidth, mapHeight, 0);
        // assign each map cell an entity, and each ground cell a tile
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Map.Tiles[x, y] = new CellEntity(x, y) { Id = (y * mapWidth) + x, HasBuilding = (y * mapWidth) + x % 5 == 0 };
                groundTileMap.SetTile(new Vector3Int(x, (Map.Tiles.GetLength(1) - 1) - y, 0), groundTiles[0]);
            }
        }
        // position and scale the plane where the player can walk, it needs to be a little bigger than the ground tilemap itself, and slightly offset it on each edge (walkable edges)
        playerPlaneTransform.transform.localScale = new Vector3(groundTileMap.size.x / 10.0f + 0.2f, 1, groundTileMap.size.y / 10.0f + 0.2f);
        playerPlaneTransform.position = new Vector3(mapWidth / 2 + (mapWidth % 2 == 0 ? 0 : 0.5f), -0.01f, mapHeight / 2 + (mapHeight % 2 == 0 ? 0 : 0.5f));
        playerPlaneTeleportTransform.transform.localScale = new Vector3(groundTileMap.size.x / 10.0f + 0.2f, 1, groundTileMap.size.y / 10.0f + 0.2f);
        playerPlaneTeleportTransform.position = new Vector3(mapWidth / 2 + (mapWidth % 2 == 0 ? 0 : 0.5f), 0.01f, mapHeight / 2 + (mapHeight % 2 == 0 ? 0 : 0.5f));

        // position the raycast receiver plane to be exactly the size and at the same position as the ground tile map, and slightly above the ground
        // (otherwise, the collider doesn't get hit)
        raycastReceiver.transform.localScale = new Vector3(groundTileMap.size.x / 10.0f, 1, groundTileMap.size.y / 10.0f);
        raycastReceiver.transform.position = new Vector3(mapWidth / 2 + (mapWidth % 2 == 0 ? 0 : 0.5f), 0.02f, mapHeight / 2 + (mapHeight % 2 == 0 ? 0 : 0.5f));
        // instantiate the gates for entry and exit
        Instantiate(prefabEntryGate, new Vector3(0.5f, 0.5f, mapHeight / 2 - 0.5f), Quaternion.identity);
        Instantiate(prefabExitGate, new Vector3(mapWidth - 0.5f, 0.5f, mapHeight / 2 - 0.5f), Quaternion.identity);
        
        //Instantiate(prefabIceTower, new Vector3(-0.5f, 0.5f, -0.5f), Quaternion.identity);
        
        soldierTransform = Instantiate(prefabRegularEnemy, new Vector3(mapWidth - 0.5f, 0.5f, mapHeight / 2 - 0.5f), Quaternion.identity).transform;
    }

    // Update is called once per frame
    private void Update()
    {
        soldierTransform.position = new Vector3(soldierTransform.position.x - 1 * Time.deltaTime, soldierTransform.position.y,
            soldierTransform.position.z);
    }
}
