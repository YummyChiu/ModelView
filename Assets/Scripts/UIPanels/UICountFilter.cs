using System.Collections;
using System.Collections.Generic;
using TinyTeam.UI;
using UnityEngine;
using UnityEngine.UI;
using Com.UI;
using System;

public class UICountFilter : TTUIPage
{

    private Button BtnClose;
    private Button BtnLevelAll;
    private Button BtnLevelCancel;
    private Button BtnComAll;
    private Button BtnComCancel;
    private Button BtnCommit;
    private Button BtnCancel;

    private Transform TrPanel;

    private UTree LevelTree;
    private UTree ComponentTree;

    private Transform TrLevelTree;
    private Transform TrComponentTree;


    public UICountFilter() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPanel/CountFilter";
    }

    public override void Active()
    {
        base.Active();
    }

    public override void Awake(GameObject go)
    {
        BtnClose = transform.Find("Header/BtnClose").GetComponent<Button>();

        TrPanel = transform.Find("Panel");
        BtnLevelAll = TrPanel.Find("BtnLevelAll").GetComponent<Button>();
        BtnLevelCancel = TrPanel.Find("BtnLevelCancel").GetComponent<Button>();
        BtnComAll = TrPanel.Find("BtnComAll").GetComponent<Button>();
        BtnComCancel = TrPanel.Find("BtnComCancel").GetComponent<Button>();
        BtnCommit = TrPanel.Find("BtnCommit").GetComponent<Button>();
        BtnCancel = TrPanel.Find("BtnCancel").GetComponent<Button>();
        BtnClose.onClick.AddListener(OnClickClose);
        BtnLevelAll.onClick.AddListener(OnClickLevelAll);
        BtnLevelCancel.onClick.AddListener(OnClickLevelCancel);
        BtnComAll.onClick.AddListener(OnClickComAll);
        BtnComCancel.onClick.AddListener(OnClickComCancel);
        BtnCommit.onClick.AddListener(OnClickCommit);
        BtnCancel.onClick.AddListener(OnClickCancel);

        TrLevelTree = TrPanel.Find("LevelTree");
        TrComponentTree = TrPanel.Find("ComponentTree");

        LevelTree = TrPanel.Find("LevelTree").GetComponent<UTree>();
        ComponentTree = TrPanel.Find("ComponentTree").GetComponent<UTree>();

        base.Awake(go);
    }
    #region 按钮事件
    private void OnClickClose()
    {
        ClosePage<UICountFilter>();
    }

    private void OnClickLevelAll()
    {
        CheckAll(true, TrLevelTree);
    }

    private void OnClickLevelCancel()
    {
        CheckAll(false, TrLevelTree);
    }

    private void OnClickComAll()
    {
        CheckAll(true, TrComponentTree);
    }

    private void OnClickComCancel()
    {
        CheckAll(false, TrComponentTree);
    }

    private void OnClickCommit()
    {
        List<string> levels = new List<string>();

        foreach (var item in LevelTree.nodeData[0].Children)
        {
            if (item.Check)
            {
                levels.Add(item.Title);
            }
        }

        List<string> componentsCode = new List<string>();
        foreach (UTreeNodeData node1 in ComponentTree.nodeData)
        {
            foreach (UTreeNodeData node2 in node1.Children)
            {
                foreach (UTreeNodeData node3 in node2.Children)
                {
                    if (node3.Check)
                    {
                        string codeNum = node3.Title;
                        componentsCode.Add(codeNum);
                    }
                
                }
            }
        }
        TTUIPage.ShowPage<UICountTable>(TreeData.Instance.GetCountTable(levels, componentsCode));

    }

    private void OnClickCancel()
    {
        ClosePage<UICountFilter>();
    }

    #endregion

    private void CheckAll(bool state, Transform tr)
    {
        foreach (var item in tr.GetComponentsInChildren<Toggle>())
        {
            //Debug.Log("HelloAgain!");
            item.isOn = state;
        }
    }
    public override void Refresh()
    {
        base.Refresh();
        LevelTree.SetDataProvider(TreeData.Instance.GetLevelData());
        ComponentTree.SetDataProvider(TreeData.Instance.GetComData());
        SetScaleTo1();
    }

    private void SetScaleTo1()
    {
        foreach (var item in transform.GetComponentsInChildren<RectTransform>())
        {
            item.localScale = Vector3.one;
        }
    }





}
