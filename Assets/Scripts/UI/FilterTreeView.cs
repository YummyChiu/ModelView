using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Com.UI;
using UnityEngine.UI;
using System.Linq;
using System;
using Mono.Data;
using Mono.Data.Sqlite;

public class FilterTreeView : MonoBehaviour
{

    // Use this for initialization
    public GameObject ModelCenter;

    public UTree levelTree;
    public UTree compoentTree;
    public Transform rebarPanel;
    private List<UTreeNodeData> levelData;
    private List<UTreeNodeData> compoentData;

    private Button floorCheckOnBtn;
    private Button floorCheckOffBtn;
    private Button compoentCheckOnBtn;
    private Button compoentCheckOffBtn;
    private Button ConfirmBtn;
    private Button CanelBtn;

    private Transform levelTreeTr;
    private Transform compoentTreeTr;

    List<string> levels = new List<string>();
    List<string> compoents = new List<string>();
    Walls PPTCPN_Walls;
    Floors PPTCPN_Floors;
    Structures PPTCPN_Structure;
    Frameworks PPTCPN_FrameWorks;
    Commons PPTCPN_Commmon;
    void Start()
    {
        levelTreeTr = transform.Find("LevelTree");
        floorCheckOnBtn = transform.Find("FloorCommitAll").GetComponent<Button>();
        floorCheckOffBtn = transform.Find("FloorCanelAll").GetComponent<Button>();

        compoentTreeTr = transform.Find("CompoentTree");
        compoentCheckOnBtn = transform.Find("CompoentCommitAll").GetComponent<Button>();
        compoentCheckOffBtn = transform.Find("CompoentCanelAll").GetComponent<Button>();
        floorCheckOnBtn.onClick.AddListener(delegate () { CheckAll(true, levelTreeTr); });
        floorCheckOffBtn.onClick.AddListener(delegate () { CheckAll(false, levelTreeTr); });
        compoentCheckOnBtn.onClick.AddListener(delegate () { CheckAll(true, compoentTreeTr); });
        compoentCheckOffBtn.onClick.AddListener(delegate () { CheckAll(false, compoentTreeTr); });

        ConfirmBtn = transform.Find("Confirm").GetComponent<Button>();
        CanelBtn = transform.Find("Canel").GetComponent<Button>();

        ConfirmBtn.onClick.AddListener(delegate () { Confirm(); });
        CanelBtn.onClick.AddListener(delegate () { ClosePanel(); });
        LevelTreeInit();


        CompoentTreeInit();
        setdata();
        SetScaleTo1();


    }

    private void CompoentTreeInit()
    {
        #region
        //List<string> compoentList = new List<string>() { "墙", "梁", "板", "柱", "基础" };
        //UTreeNodeData compoenttree = new UTreeNodeData(1, "构件");
        //int count = 0;
        //foreach (var item in compoentList)
        //{
        //    count++;
        //    UTreeNodeData levelChildNode = new UTreeNodeData(count, item);
        //    compoenttree.AddChild(levelChildNode);
        //}
        //compoentData = new List<UTreeNodeData>();
        //compoentData.Add(compoenttree);
        #endregion
        List<string> compoentList = new List<string>();
        SqliteConnection conn = new SqliteConnection();
        conn = Tools.Instance.SqlConnection();

        SqliteCommand cmd1 = conn.CreateCommand();
        cmd1.CommandText = "Select distinct GategoryName From Attribute order by Id";
        SqliteDataReader dr = cmd1.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                string temp = dr.GetString(0);

                compoentList.Add(temp);
            }
        }
        UTreeNodeData compoenttree = new UTreeNodeData(1, "构件");
        int count = 0;
        foreach (var item in compoentList)
        {
            count++;
            UTreeNodeData levelChildNode = new UTreeNodeData(count, item);
            compoenttree.AddChild(levelChildNode);
        }
        compoentData = new List<UTreeNodeData>();
        compoentData.Add(compoenttree);

    }
    private void SetScaleTo1()
    {
        foreach (var item in transform.GetComponentsInChildren<RectTransform>())
        {
            item.localScale = Vector3.one;
        }

    }

    private void LevelTreeInit()
    {
        /********读取模型***********/
        List<string> levelList = new List<string>();
        SqliteConnection conn = new SqliteConnection();
        conn = Tools.Instance.SqlConnection();

        SqliteCommand cmd1 = conn.CreateCommand();
        cmd1.CommandText = "Select distinct FloorNums From Attribute order by Id";
        SqliteDataReader dr = cmd1.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                string temp = dr.GetString(0);
               // Debug.Log(temp);
                levelList.Add(temp);
            }
        }

        //foreach (var item in ReadModelXML.idAndLevels.Values)
        //{
        //    if (!levelList.Contains(item))
        //        levelList.Add(item);
        //}
        /********读取模型***********/
        UTreeNodeData leveltree = new UTreeNodeData(1, "楼层");

        int count = 0;
        foreach (var item in levelList)
        {
            count++;
            UTreeNodeData levelChildNode = new UTreeNodeData(count, item);
            leveltree.AddChild(levelChildNode);
        }
        levelData = new List<UTreeNodeData>();
        levelData.Add(leveltree);

    }
    public void setdata()
    {
        levelTree.SetDataProvider(levelData);
        compoentTree.SetDataProvider(compoentData);
    }

    private void CheckAll(bool state, Transform tr)
    {
        foreach (var item in tr.GetComponentsInChildren<Toggle>())
        {

            item.isOn = state;
        }
    }
    private void ClosePanel()
    {
        transform.gameObject.SetActive(false);
    }
    private void Confirm()
    {
        levels.Clear();
        compoents.Clear();

        foreach (var item in levelTree.nodeData[0].Children)
        {
            if (item.Check)
            {
                levels.Add(item.Title);
                //Debug.Log(item.Title);
            }
        }

        //foreach (var item in compoentTree.nodeData[0].Children)
        //{
        //    if (item.Check)
        //    {
        //        switch (item.Title)
        //        {
        //            case "墙":
        //                compoents.Add(item.Title);
        //                break;
        //            case "梁":
        //                compoents.Add(item.Title);
        //                break;
        //            case "板":
        //                compoents.Add(item.Title);
        //                break;
        //            case "柱":
        //                compoents.Add(item.Title);
        //                break;
        //        }

        //    }

        //}
        foreach (var item in compoentTree.nodeData[0].Children)
        {
            if (item.Check)
            {
                compoents.Add(item.Title);
            }

        }

        for (int i = 0; i < levels.Count; i++)
        {

        }
        string sqllevels = "";
        string sqlcompoents = "";
        List<string> IdList = new List<string>();
        if (levels.Count != 0 || compoents.Count != 0)
        {
            //查询id
            for (int i = 0; i < levels.Count; i++)
            {
                if (levels.Count == 1)
                {
                    sqllevels = "'"+levels[0]+"'";
                    break;
                }
                if (i == levels.Count - 1)
                {
                    sqllevels = sqllevels + "'" + levels[i] + "'";
                    break;
                }
                else {
                    sqllevels = sqllevels + "'" + levels[i] + "',";
                }
            }
            for (int j = 0; j < compoents.Count; j++)
            {
                if (compoents.Count == 1)
                {
                    sqlcompoents ="'"+ compoents[0] +"'";
                    break;
                }
                if (j == compoents.Count - 1)
                {
                    sqlcompoents = sqlcompoents + "'" + compoents[j] + "'";
                    break;
                }
                else {
                    sqlcompoents = sqlcompoents + "'" + compoents[j] + "',";
                }
            }
            SqliteConnection conn = new SqliteConnection();
            conn = Tools.Instance.SqlConnection();

            SqliteCommand cmd1 = conn.CreateCommand();
            cmd1.CommandText = "Select EId From Attribute where FloorNums in (" + sqllevels + ") and GategoryName in (" + sqlcompoents + ")";
            Debug.Log(cmd1.CommandText);
            SqliteDataReader dr = cmd1.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    string temp = dr.GetInt32(0).ToString();
                    //Debug.Log(temp);
                    IdList.Add(temp);
                }
            }
            foreach (Transform child in ModelCenter.transform)
            {
                string objName = child.name;
                string id = objName.Split('_')[1];
                if (IdList.Contains(id))
                    child.gameObject.SetActive(true);
                else
                    child.gameObject.SetActive(false);

                //string FormName = objName.Substring(0, objName.IndexOf('_'));
                //GameObject.Destroy(child.gameObject);
            }

            //Debug.Log("所过滤的构件的数量有:"+IdList.Count);
        }
        else
        {
            foreach (Transform child in ModelCenter.transform)
            {
                    child.gameObject.SetActive(false);  
            }
            //隐藏全部
        }

        //foreach (GameObject go in ReadModelXML.goList)
        //{
        //    string objName = go.gameObject.name;
        //    string FormName = objName.Substring(0, objName.IndexOf('_'));

        //    switch (FormName)
        //    {
        //        case "墙":
        //            PPTCPN_Walls = go.GetComponent<Walls>();
        //            CheckObjectToActive(go, FormName, PPTCPN_Walls._FLOORSNUM);
        //            break;
        //        case "结构柱":
        //            PPTCPN_Structure = go.gameObject.GetComponent<Structures>();
        //            CheckObjectToActive(go, FormName, PPTCPN_Structure._FLOORSNUM);

        //            break;
        //        case "楼板":
        //            PPTCPN_Floors = go.gameObject.GetComponent<Floors>();
        //            CheckObjectToActive(go, FormName, PPTCPN_Floors._FLOORSNUM);

        //            break;
        //        case "结构框架":
        //            PPTCPN_FrameWorks = go.gameObject.GetComponent<Frameworks>();
        //            CheckObjectToActive(go, FormName, PPTCPN_FrameWorks._FLOORSNUM);

        //            break;
        //        case "常规模型":
        //            PPTCPN_Commmon = go.gameObject.GetComponent<Commons>();
        //            CheckObjectToActive(go, FormName, PPTCPN_Commmon._FLOORSNUM);

        //            break;

        //    }
        //}
        transform.gameObject.SetActive(false);
    }

    private void CheckObjectToActive(GameObject go, string formName, string _FLOORSNUM)
    {
        if (compoents.Contains(formName))
        {
            if (levels.Contains(_FLOORSNUM))
            {
                go.SetActive(true);
            }
            else
                go.SetActive(false);
        }
        else
            go.SetActive(false);



    }
}
