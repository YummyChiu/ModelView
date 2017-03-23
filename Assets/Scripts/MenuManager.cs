using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

    public GameObject menuCanvas;//指定为“menuCanvas”对象

    public static bool is3DWander = true;
    public static bool isDrawingControl = false;

    public static bool TOPVIEW = false;
    public static bool FORWARDVIEW = false;
    public static bool RIGHTVIEW = false;




    int intSwitch = 0;
    public GameObject BtnDrawLine;
    public GameObject BtnClearLine;
    public static bool isSwitchRenderObjInfoActive = true;


    public GameObject DetailDetectCanvas;//指定为“DetailDetectCanvas”对象
    public void btnDetailDetectCanvasClose()
    {
        if (DetailDetectCanvas.activeSelf == true)
        {
            DetailDetectCanvas.SetActive(false);
        }
        if (menuCanvas.activeSelf == false)
        {
            menuCanvas.SetActive(true);
        }

    }
    public void btnDetailDetectCanvasOpen()
    {
        if (menuCanvas.activeSelf == true)
        {
            menuCanvas.SetActive(false);
        }
        if (DetailDetectCanvas.activeSelf == false)
        {
            DetailDetectCanvas.SetActive(true);
        }


    }

    public void AniCeLiang()
    {

        switch (intSwitch)
        {
            case 0:
                MenuManager.isSwitchRenderObjInfoActive = false;
                //CtrlCalculatedegree.SetBool("btnCalculatedegreeShow", true);
                //CtrlClear.SetBool("btnClearShow", true);
                //CtrlDrawLine.SetBool("btnDrawLineShow", true);
                BtnDrawLine.SetActive(true);
                BtnClearLine.SetActive(true);
                intSwitch = 1;
                break;
            case 1:
                MenuManager.isSwitchRenderObjInfoActive = true;
                //CtrlCalculatedegree.SetBool("btnCalculatedegreeShow", false);
                //CtrlClear.SetBool("btnClearShow", false);
                //CtrlDrawLine.SetBool("btnDrawLineShow", false);
                intSwitch = 0;
                BtnDrawLine.SetActive(false);
                BtnClearLine.SetActive(false);
                //CalculateSpace.instance.ClearAllConfig();
                //DrawOnScreen.instance.ClearAllConfig();
                break;
        }
    }

}
