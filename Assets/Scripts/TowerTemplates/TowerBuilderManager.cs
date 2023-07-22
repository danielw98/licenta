using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Validator for tower ghosts while hovering over valid or invalid tiles
/// </summary>
public class TowerBuilderManager : MonoBehaviour
{
    public Tilemap tilemap;
    private LineRenderer lineRenderer;
    private bool isPlacementAllowed;
    private Vector3 defaultPosition;
    public GameObject towerPrefab;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        defaultPosition = transform.position;
    }

    public void HandleDrop(XRBaseInteractor interactor) 
    {
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
        CellEntity hoveredCell = Map.Tiles[cellPosition.x, cellPosition.y];
        // when the tower ghost is dropped over a valid tile, return it to its default position and create the real tower on the valid tile 
        if (!hoveredCell.HasBuilding) // TODO: || enemyIsBlocked)
        {
            hoveredCell.HasBuilding = true;
            Game.Instance.towers.Add(Instantiate(towerPrefab, new Vector3(hoveredCell.X + 0.5f, 0, hoveredCell.Y + 0.5f), Quaternion.identity));
        }
        // return the ghost tower to the initial position
        transform.position = defaultPosition;
        Debug.Log("Object was dropped");
    } 

    private void Update()
    {
        // set the start point of the line to the object's position
        lineRenderer.SetPosition(0, transform.position);
        // set the end point of the line to the ground (y = 0)
        lineRenderer.SetPosition(1, new Vector3(transform.position.x, 0, transform.position.z));
        // perform a raycast from the object downwards
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            // if the raycast hits the buildable area, check if the tile is buildable
            if (hit.collider.gameObject.CompareTag("BuildableArea"))
            {
                Vector3Int cellPosition = tilemap.WorldToCell(hit.point);
                isPlacementAllowed = !Map.Tiles[cellPosition.x, cellPosition.y].HasBuilding;
            }
            else
                isPlacementAllowed = false;
        }
        else
            isPlacementAllowed = false;
        // change the color of the line based on whether placement is allowed
        if (isPlacementAllowed)
            lineRenderer.material.color = Color.green;
        else
            lineRenderer.material.color = Color.red;
    }
}