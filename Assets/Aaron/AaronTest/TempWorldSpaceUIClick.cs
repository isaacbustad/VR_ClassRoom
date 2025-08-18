using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Temporary utility that enables mouse interaction with world space UI elements.
/// Uses raycasting to detect UI elements in the scene and forwards click events to them.
/// </summary>
public class TempWorldSpaceUIClick : MonoBehaviour
{
    /// <summary>
    /// Check for mouse clicks each frame and forward them to world space UI elements
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.GetComponent<CanvasRenderer>() != null)
                {
                    PointerEventData pointer = new PointerEventData(EventSystem.current);
                    pointer.position = Input.mousePosition;

                    List<RaycastResult> results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointer, results);

                    foreach (var result in results)
                    {
                        ExecuteEvents.Execute(result.gameObject, pointer, ExecuteEvents.pointerClickHandler);
                    }
                }
            }
        }
    }
}