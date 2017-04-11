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
    public static GraphicRaycaster graphicRaycaster1;
    //public static GraphicRaycaster graphicRaycaster2;
    //public static GraphicRaycaster graphicRaycaster3;

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
        graphicRaycaster1 = GameObject.Find("Canvas/NormalRoot").GetComponent<GraphicRaycaster>();
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

        bool state1 = false;
        bool state2 = false;

        if (graphicRaycaster1 != null)
        {
            PointerEventData eventData = new PointerEventData(eventSystem);
            // PointerEventData eventData = new PointerEventData();
            eventData.pressPosition = Input.mousePosition;
            eventData.position = Input.mousePosition;

            List<RaycastResult> list1 = new List<RaycastResult>();
            graphicRaycaster1.Raycast(eventData, list1);

            state1 = list1.Count > 0;
            //return list.Count > 0;
        }
        if (graphicRaycaster != null)
        {
            PointerEventData eventData = new PointerEventData(eventSystem);
            // PointerEventData eventData = new PointerEventData();
            eventData.pressPosition = Input.mousePosition;
            eventData.position = Input.mousePosition;

            List<RaycastResult> list1 = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, list1);

            state2 = list1.Count > 0;
        }
        if (state1 || state2)
        {
            return true;
        }
        else
        {
            return false;
        }


        //if (graphicRaycaster != null)
        //{
        //    PointerEventData eventData = new PointerEventData(eventSystem);
        //    // PointerEventData eventData = new PointerEventData();
        //    eventData.pressPosition = Input.mousePosition;
        //    eventData.position = Input.mousePosition;

        //    List<RaycastResult> list = new List<RaycastResult>();
        //    graphicRaycaster.Raycast(eventData, list);
        //    return list.Count > 0;
        //}

        //else
        //{
        //    //Debug.Log("graphicRaycaster.name is null");
        //    return false;

        //}
        //bool state1 = false;
        //bool state2 = false;
        ////return false;
        //if (graphicRaycaster1 != null)
        //{
        //    PointerEventData eventData = new PointerEventData(eventSystem);
        //    // PointerEventData eventData = new PointerEventData();
        //    eventData.pressPosition = Input.mousePosition;
        //    eventData.position = Input.mousePosition;

        //    List<RaycastResult> list = new List<RaycastResult>();
        //    graphicRaycaster1.Raycast(eventData, list);
        //    state1 = list.Count > 0;
        //    // return list.Count > 0;
        //}
        //if (graphicRaycaster != null)
        //{
        //    PointerEventData eventData = new PointerEventData(eventSystem);
        //    // PointerEventData eventData = new PointerEventData();
        //    eventData.pressPosition = Input.mousePosition;
        //    eventData.position = Input.mousePosition;

        //    List<RaycastResult> list = new List<RaycastResult>();
        //    graphicRaycaster.Raycast(eventData, list);
        //    state2 = list.Count > 0;
        //    // return list.Count > 0;
        //}
        //if (state2 || state1)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
        //PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        //eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        //List<RaycastResult> results = new List<RaycastResult>();
        //EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        ////Debug.Log("event system's count  " + results.Count);
        //if (results.Count > 0)
        //    return true;
        //else
        //    return false;

    }





}
