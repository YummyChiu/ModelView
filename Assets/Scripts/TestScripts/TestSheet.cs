using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSheet : MonoBehaviour
{

    //private List<MyRebar> myRebars;
    //public Transform CountTest;
    // Use this for initialization
    void Start()
    {
        //myRebars = GetRebarInfo.GetRebar();

    }

    public void CreateSheet()
    {
        //CountTest.gameObject.SetActive(true);
        //   RebarTableControl rControl = CountTest.GetComponent<RebarTableControl>();
        //rControl.RebarSummaryTable(myRebars);
    }

    public void OpenActivity()
    {
        MessageBox.Show("open");

        try {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            string imageUri = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1490953762661&di=5057ae6222afd0fa6213fb4a899d678a&imgtype=0&src=http%3A%2F%2Fpic.7kk.com%2Fsimg%2F1%2F800_0%2F9%2Fec%2Fc4219e022a38d12e293082f2f699b.jpg";

            jo.Call("ShowImges", imageUri);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message,"error");
        }
      
    }
}
