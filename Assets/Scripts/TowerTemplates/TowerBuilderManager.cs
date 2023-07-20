using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.XR.Interaction.Toolkit;

public class TowerBuilderManager : MonoBehaviour
{
    public Tilemap tilemap;
    private LineRenderer lineRenderer;
    private bool isPlacementAllowed;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void HandleDrop(XRBaseInteractor interactor)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
        CellEntity hoveredCell = Map.Tiles[cellPosition.x, cellPosition.y];
        if (hoveredCell.HasBuilding ) // TODO: || enemyIsBlocked)
            transform.position = new Vector3(-0.5f, 0.13f, 0.5f);
        else
        {
            transform.position = new Vector3(hoveredCell.X + 0.5f, hoveredCell.Y + 0.5f, transform.position.z);
            
        }
        Debug.Log("Object was dropped");
        // Add your drop handling code here
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