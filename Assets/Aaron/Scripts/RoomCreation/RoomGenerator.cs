// Written by Aaron Williams

// Edited by    :   Isaac Bustad    :   Remove Occulus dependancy 

//using Oculus.Interaction.Locomotion;
using System;
using System.Collections.Generic;
using BugFreeProductions.VRClassroom;
//using Oculus.Interaction.Locomotion;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.Rendering;
//using static OVRControllerTest.BoolMonitor;

public class RoomGenerator : MonoBehaviour
{
    [Header("Is In Virtual Reality")]
    [SerializeField] private bool isVR;

    [Header("Floor Point Settings")]
    [SerializeField] private GameObject floorPointPrefab;

    private const string FLOOR_POINT_ITEM_ID = "Room";
    public List<PlacableFactoryItem> floorPointReferences = new List<PlacableFactoryItem>();
    private FactoryItem factoryItem = null;
    [SerializeField] protected AbstractFactory_SCO itemFactory = null;

    [Header("Controller Settings")]
    [SerializeField] private Transform controllerTransform;

    [Header("Target LineRenderer Settings")]
    [SerializeField] private float maxTargetLineDistanceNoPointSelected = 10f;
    [SerializeField] private float maxTargetLineDistancePointSelected = 100f;
    [SerializeField] private float targetLineWidth = 0.01f;
    [SerializeField] private Color validTargetPlacementColor = Color.green;
    [SerializeField] private Color invalidTargetColor = Color.red;
    [SerializeField] private Color validTargetSelectionColor = Color.yellow;

    [Header("Edge LineRenderer Settings")]
    [SerializeField] private Color edgeLineColor = Color.blue;
    [SerializeField] private float edgeLineWidth = 0.03f;

    [Header("Selection Padding")]
    [SerializeField] private float floorPointSelectionPadding = 0.2f;
    [SerializeField] private float edgeSelectionPadding = 0.2f;

    [Header("Mesh Generation Settings")]
    [SerializeField] private float wallHeight = 2.5f;
    [SerializeField] private Material floorMaterial;
    [SerializeField] private Material wallMaterial;
    [SerializeField] private Material ceilingMaterial;

    private const string EDGE_LINE_RENDERER_NAME = "Edge LineRenderer";
    [SerializeField] private LineRenderer targetLineRenderer;
    private LineRenderer edgeLineRenderer;
    private Color currentTargetLineColor;

    //[SerializeField] private TeleportInteractable preRoomGenFloor;

    public const string FLOOR_MESH_NAME = "Floor Mesh";
    public const string CEILING_MESH_NAME = "Ceiling Mesh";

    private GameObject roomGameObjectsParent;
    private GameObject floorGameObject;
    private GameObject wallGameObject;
    private GameObject ceilingGameObject;

    private bool isTargetValid = false;
    private Vector3 targetHitPoint = Vector3.zero;

    private int selectedPointIndex = -1;
    private GameObject selectedSphere = null;

    private bool isTargetOnEdge = false;
    private int edgeStartIndex = -1;
    private int edgeEndIndex = -1;
    private Vector3 edgeStartPoint;
    private Vector3 edgeEndPoint;

    private int movingPointLayerMask;
    private bool isTargetLineVisible = false;

    private void Start()
    {
        if (targetLineRenderer == null)
        {
            targetLineRenderer = InitializeLineRenderer(targetLineWidth, invalidTargetColor, false);
        }
        else
        {
            targetLineRenderer = InitializeLineRenderer(targetLineWidth, invalidTargetColor, false, null, targetLineRenderer);
        }

        edgeLineRenderer = InitializeLineRenderer(edgeLineWidth, edgeLineColor, true, EDGE_LINE_RENDERER_NAME);
        movingPointLayerMask = ~(LayerMask.GetMask("Floor", "Walls"));
    }

    private void OnEnable()
    {
        if (targetLineRenderer == null)
        {
            targetLineRenderer = InitializeLineRenderer(targetLineWidth, invalidTargetColor, false);
        }
        else
        {
            targetLineRenderer = InitializeLineRenderer(targetLineWidth, invalidTargetColor, false, null, targetLineRenderer);
        }
    }

    private void Update()
    {
        if (isTargetLineVisible)
        {
            UpdateTargetLine();
        }
        else
        {
            DeactivateTargetLine();
        }
        // Controls note in case they're unassigned in the editor:
            // OVRInput.GetDown(PrimaryIndexTrigger) -> HandlePlaceSelectPoint();
            // OVRInput.GetUp(PrimaryIndexTrigger) -> DeselectPoint();
            // OVRInput.GetDown(PrimaryHandTrigger) -> HandleToggleTargetLineInputVR(true);
            // OVRInput.GetUp(PrimaryHandTrigger) -> HandleToggleTargetLineInputVR(false);
            // OVRInput.GetDown(OVRInput.Button.One) -> GenerateRoom();

        if (selectedPointIndex >= 0)
        {
            UpdateSelectedPointPosition();
        }
    }

    public void HandleToggleTargetLineInputKBM(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isTargetLineVisible = true;
        }
        if (context.canceled)
        {
            isTargetLineVisible = false;
        }
    }

    public void HandlePlaceFloorButtonInputKBM(InputAction.CallbackContext context)
    {
        if (context.started && isTargetLineVisible)
        {
            HandlePlaceSelectPoint();
        }
        else if (context.canceled)
        {
            if (selectedPointIndex >= 0)
            {
                DeselectPoint();
            }
        }     
    }

    public void HandleGenerateRoomButtonInputKBM(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            GenerateRoom();
        }
    }

    public void HandleToggleTargetLineInputVR(bool gripPressed)
    {
        if (gripPressed)
        {
            isTargetLineVisible = true;
        }
        else if (!gripPressed)
        {
            isTargetLineVisible = false;
        }
    }

    private LineRenderer InitializeLineRenderer(float width, Color color, bool isDistinctGameObject, string gameObjectName = null, LineRenderer lineRenderer = null)
    {
        if(lineRenderer == null)
        {
            lineRenderer = isDistinctGameObject ? new GameObject(gameObjectName).AddComponent<LineRenderer>() : gameObject.AddComponent<LineRenderer>();
            if (isDistinctGameObject) { lineRenderer.transform.parent = transform; }
        }

        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.material.color = color;
        lineRenderer.enabled = false;

        return lineRenderer;
    }

    private void UpdateTargetLine()
    {
        targetLineRenderer.enabled = true;
        edgeLineRenderer.enabled = false;
        isTargetValid = false;

        bool isPointNearby = false;

        Vector3 targetLineOrigin = controllerTransform.position;
        Vector3 targetLineDirection = controllerTransform.forward;

        int layerMask;
        float maxTargetDistance;
        if (selectedPointIndex >= 0)
        {
            maxTargetDistance = maxTargetLineDistancePointSelected;
            layerMask = movingPointLayerMask;
        }
        else
        {
            maxTargetDistance = maxTargetLineDistanceNoPointSelected;
            layerMask = -1;
        }

        targetHitPoint = targetLineOrigin + targetLineDirection * maxTargetDistance;

        if (Physics.Raycast(targetLineOrigin, targetLineDirection, out RaycastHit hit, maxTargetDistance, layerMask))
        {
            targetHitPoint = hit.point;

            isTargetValid = hit.point.y < 0.01f;

            if (selectedPointIndex < 0 && floorGameObject != null && floorPointReferences.Count >= 3)
            {
                isPointNearby = IsFloorPointNearHit(targetHitPoint);
                if (!isPointNearby) { CheckForEdgeHit(targetHitPoint); }
            }
        }

        currentTargetLineColor = (isPointNearby || selectedPointIndex >= 0) ? validTargetSelectionColor : (isTargetValid ? validTargetPlacementColor : invalidTargetColor);
        UpdateLineRenderer(targetLineRenderer, targetLineOrigin, targetHitPoint, currentTargetLineColor);
    }

    private void DeactivateTargetLine()
    {
        targetLineRenderer.enabled = false;
        edgeLineRenderer.enabled = false;
        DeselectPoint();
    }

    private void UpdateSelectedPointPosition()
    {
        if (isTargetValid && selectedPointIndex >= 0)
        {
            floorPointReferences[selectedPointIndex].transform.position = targetHitPoint;
            selectedSphere.transform.position = targetHitPoint;
            RegenerateRoom();
        }
    }

    public void HandlePlaceSelectPoint()
    {
        selectedPointIndex = TrySelectClosestPoint();

        if (selectedPointIndex >= 0)
        {
            selectedSphere = floorPointReferences[selectedPointIndex].gameObject;
        }
        else if (isTargetOnEdge)
        {
            InsertFloorPointOnEdge();
        }
        else if (isTargetValid)
        {
            CreateFloorPoint();
        }
    }

    public void HideFloorPoints()
    {
        foreach (PlacableFactoryItem floorPoint in floorPointReferences)
        {
            floorPoint.gameObject.SetActive(false);
        }
    }
    public void ShowFloorPoints()
    {
        foreach (PlacableFactoryItem floorPoint in floorPointReferences)
        {
            floorPoint.gameObject.SetActive(true);
        }
    }

    private int TrySelectClosestPoint()
    {
        int closestIndex = -1;
        float closestDistance = floorPointSelectionPadding;

        for (int i = 0; i < floorPointReferences.Count; i++)
        {
            float distanceToHit = Vector3.Distance(floorPointReferences[i].transform.position, targetHitPoint);
            if (distanceToHit < closestDistance)
            {
                closestDistance = distanceToHit;
                closestIndex = i;
            }
        }
        return closestIndex;
    }

    public void DeselectPoint()
    {
        selectedPointIndex = -1;
        selectedSphere = null;
    }

    private void RegenerateRoom()
    {
        if (floorGameObject != null && wallGameObject != null && ceilingGameObject != null && floorPointReferences.Count >= 3)
        {
            GenerateRoom();
        }
    }

    public void GenerateRoom()
    {
        if (floorPointReferences.Count >= 3)
        {
            if (floorGameObject != null) { Destroy(floorGameObject); }
            if (ceilingGameObject != null) { Destroy(ceilingGameObject); }

            if (wallGameObject != null) { Destroy(wallGameObject); }

            if (roomGameObjectsParent != null) { Destroy(roomGameObjectsParent); }

            List<Vector3> floorVertices = new();
            List<Vector3> ceilingVertices = new();
            foreach (PlacableFactoryItem point in floorPointReferences)
            {
                floorVertices.Add(point.transform.position);
                ceilingVertices.Add(new Vector3(point.transform.position.x, wallHeight, point.transform.position.z));
            }

            roomGameObjectsParent = new GameObject("RoomMeshes");

            ceilingGameObject = MeshGenerator.GenerateFlatMesh(ceilingVertices, ceilingMaterial, CEILING_MESH_NAME);
            ceilingGameObject.transform.SetParent(roomGameObjectsParent.transform);

            floorGameObject = MeshGenerator.GenerateFlatMesh(floorVertices, floorMaterial, FLOOR_MESH_NAME);
            floorGameObject.transform.SetParent(roomGameObjectsParent.transform);

            wallGameObject = MeshGenerator.GenerateWallMeshes(floorVertices, wallMaterial, wallHeight);
            wallGameObject.transform.SetParent(roomGameObjectsParent.transform);
            //preRoomGenFloor.enabled = false;
        }
    }

    private bool IsFloorPointNearHit(Vector3 hitPoint)
    {
        foreach (PlacableFactoryItem floorPoint in floorPointReferences)
        {
            if (Vector3.Distance(floorPoint.transform.position, hitPoint) < floorPointSelectionPadding) { return true; }
        }
        return false;
    }

    private void UpdateLineRenderer(LineRenderer lineRenderer, Vector3 start, Vector3 end, Color color)
    {
        lineRenderer.SetPositions(new Vector3[] { start, end });
        lineRenderer.material.color = color;
    }

    private void CreateFloorPoint()
    {
        factoryItem = null;

        itemFactory.CreateItem(ref factoryItem, CreateObjectPlacementData());
        floorPointReferences.Add(factoryItem.GetComponent<PlacableFactoryItem>());
    }

    private void CheckForEdgeHit(Vector3 hitPoint)
    {
        edgeLineRenderer.enabled = false;
        isTargetOnEdge = false;
        edgeStartIndex = -1;
        edgeEndIndex = -1;

        float closestDistance = edgeSelectionPadding;

        for (int i = 0; i < floorPointReferences.Count; i++)
        {
            int nextIndex = (i + 1) % floorPointReferences.Count;

            Vector3 edgeStartPoint = floorPointReferences[i].transform.position;
            Vector3 edgeEndPoint = floorPointReferences[nextIndex].transform.position;
            Vector3 edge = edgeEndPoint - edgeStartPoint;
            Vector3 edgeDirection = edge / edge.magnitude;

            Vector3 edgeStartPointToHitPoint = hitPoint - edgeStartPoint;
            float projection = Vector3.Dot(edgeStartPointToHitPoint, edgeDirection);
            if (projection < 0 || projection > edge.magnitude) { continue; }
            Vector3 closestPoint = edgeStartPoint + edgeDirection * projection;

            float distance = Vector3.Distance(hitPoint, closestPoint);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                edgeStartIndex = i;
                edgeEndIndex = nextIndex;
                isTargetOnEdge = true;

                this.edgeStartPoint = edgeStartPoint;
                this.edgeEndPoint = edgeEndPoint;

                edgeLineRenderer.enabled = true;
                UpdateLineRenderer(edgeLineRenderer, new Vector3(edgeStartPoint.x, 0.01f, edgeStartPoint.z), new Vector3(edgeEndPoint.x, 0.01f, edgeEndPoint.z), edgeLineColor);
            }
        }
    }

    private void InsertFloorPointOnEdge()
    {
        Vector3 edgeStartPoint = floorPointReferences[edgeStartIndex].transform.position;
        Vector3 edgeEndPoint = floorPointReferences[edgeEndIndex].transform.position;
        Vector3 edge = edgeEndPoint - edgeStartPoint;
        Vector3 edgeDirection = edge / edge.magnitude;

        float projection = Vector3.Dot(targetHitPoint - edgeStartPoint, edgeDirection);
        Vector3 pointPosition = edgeStartPoint + edgeDirection * projection;
        targetHitPoint = new Vector3(pointPosition.x, 0, pointPosition.z);

        CreateFloorPoint();

        int insertIndex = (edgeStartIndex < edgeEndIndex) ? edgeEndIndex : (edgeEndIndex == 0) ? floorPointReferences.Count : edgeEndIndex;

        if (insertIndex < floorPointReferences.Count - 1)
        {
            PlacableFactoryItem newEdgeInsertedFloorPoint = floorPointReferences[floorPointReferences.Count - 1];

            floorPointReferences.RemoveAt(floorPointReferences.Count - 1);

            if (insertIndex < floorPointReferences.Count)
            {
                floorPointReferences.Insert(insertIndex, newEdgeInsertedFloorPoint);
                JSONPlacementMannager.Instance.Pool.ReorderQueue(insertIndex);
            }
            else
            {
                floorPointReferences.Add(newEdgeInsertedFloorPoint);
            }
        }

        RegenerateRoom();

        edgeStartIndex = -1;
        edgeEndIndex = -1;
    }

    // From Isaac's placer gun script
    private ObjectPlacement CreateObjectPlacementData()
    {
        ObjectPlacement objectPlacement = new ObjectPlacement();

        objectPlacement.id = FLOOR_POINT_ITEM_ID;

        objectPlacement.tpX = targetHitPoint.x;
        objectPlacement.tpY = targetHitPoint.y;
        objectPlacement.tpZ = targetHitPoint.z;

        objectPlacement.trX = 0;
        objectPlacement.trY = 0;
        objectPlacement.trZ = 0;

        return objectPlacement;
    }

    private void CollectRoomPoints()
    {
        floorPointReferences.Clear();
        foreach (Poolable poolable in JSONPlacementMannager.Instance.Pool.PoolList)
        {
            PlacableFactoryItem floorPoint = poolable.GetComponent<PlacableFactoryItem>();
            if (floorPoint != null)
            {
                floorPointReferences.Add(floorPoint);
            }
        }
    }

    public void LoadIntoRoom()
    {
        CollectRoomPoints();
        GenerateRoom();
    }
}