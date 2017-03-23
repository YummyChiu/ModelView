using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
public class ToolsClass : MonoBehaviour
{

    public static EventSystem eventSystem;
    public static GraphicRaycaster graphicRaycaster;
    public static ToolsClass Instance;
    public static bool mouseScale = true;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        //eventSystem = transform.Find("EventSystem").GetComponent<EventSystem>();

        eventSystem = FindObjectOfType<EventSystem>();
        //Debug.Log(eventSystem.name);
        //graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
        //graphicRaycaster = GameObject.FindWithTag("Canvas").GetComponent<GraphicRaycaster>();
        graphicRaycaster = GameObject.Find("Canvas").GetComponent<GraphicRaycaster>();
        //Debug.Log(graphicRaycaster.name);

    }
    public bool CheckGuiRaycastObjects()
    {
        if (graphicRaycaster != null)
        {
            PointerEventData eventData = new PointerEventData(eventSystem);
            // PointerEventData eventData = new PointerEventData();
            eventData.pressPosition = Input.mousePosition;
            eventData.position = Input.mousePosition;

            List<RaycastResult> list = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, list);
            return list.Count > 0;
        }
        else
        {
            //Debug.Log("graphicRaycaster.name is null");
            return false;

        }

        //Debug.Log(list.Count);

    }

    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);   
        return results.Count > 0;
    }





}
