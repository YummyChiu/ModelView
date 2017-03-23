using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIManger : MonoBehaviour
{
    private static UIManger instance;
    //public GameObject[] panels;
    public List<GameObject> activePanel;

    //public Button UserInfoBtn;
    //public Button CountBtn;
    //public Button RebarBtn;
    //public Button FilterBtn;
    //public Button ProgressBtn;

    public GameObject loginPanel;
    private UIManger() { }

    public static UIManger Instance
    {
        get {
            if (instance == null)
                instance = GameObject.FindObjectOfType(typeof(UIManger)) as UIManger;
            return instance;
        }
    }
    public void ClosePanel()
    {
        
    }

    public void OpenPanel(GameObject panel)
    {
        foreach (var go in activePanel)
            go.SetActive(false);
        if (activePanel.Count != 0)
            activePanel.RemoveAt(0);
        activePanel.Add(panel);
        
        panel.SetActive(true);

    }
    public void CloseLoginAndFileUI()
    {
        
    }

    public void StartToload()
    {

        //GameObject[] gameobjects = GameObject.FindGameObjectsWithTag("FirstUI");
        //foreach (var go in gameobjects)
        //{
        //    go.SetActive(false);
        //}
        ReadModelSQLite r = Camera.main.GetComponent<ReadModelSQLite>();
        r.LoadModel();

    }

    public void LoginOut()
    {
        if (File.Exists(Tools.Instance.SavedPath + "/" + Tools.BaseFolder + "/" + Tools.UserFolder + "/" + Tools.UserFile))
        {
            File.Delete(Tools.Instance.SavedPath + "/" + Tools.BaseFolder + "/" + Tools.UserFolder + "/" + Tools.UserFile);

        }
    }
    void Start()
    {
        //StartToload();
        if (File.Exists(Tools.Instance.SavedPath + "/" + Tools.BaseFolder + "/" + Tools.UserFolder + "/" + Tools.UserFile))
        {
            //SceneManager.LoadScene("1125_builtinorder");
            //UIManger.Instance.StartToload();
            //loginPanel.SetActive(true);
           // StartToload();
        }
        else
        {
            loginPanel.SetActive(true);
        }

    }

}
