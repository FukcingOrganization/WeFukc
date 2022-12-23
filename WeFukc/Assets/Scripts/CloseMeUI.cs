using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CloseMeUI : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    GraphicRaycaster raycaster;

    PointerEventData clickData;
    List<RaycastResult> clickResults;

    void Start()
    {
        raycaster = canvas.GetComponent<GraphicRaycaster>();
        clickData = new PointerEventData(EventSystem.current);
        clickResults = new List<RaycastResult>();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            clickData.position = Mouse.current.position.ReadValue();
            clickResults.Clear();

            raycaster.Raycast(clickData, clickResults);

            foreach(RaycastResult result in clickResults)
            {
                //Debug.Log("Result: " + result.gameObject.name);
                if (result.gameObject == this.gameObject)
                {
                    //Debug.Log("It's me!");
                    return;
                }
            }
            //Debug.Log("Can't find!");
            gameObject.SetActive(false);
        }
    }
}
