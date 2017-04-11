using System.Collections;
using System.Collections.Generic;
using TinyTeam.UI;
using UnityEngine;
using SLS.Widgets.Table;
using UnityEngine.UI;
using System;

public class UICountTable : TTUIPage
{

    private Table TableCount;

    private Button BtnClose;
    private Button BtnSave;
    private Button BtnOther;




    private List<CountNum> _tableList;

    // Use this for initialization
    public UICountTable() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPanel/CountTable";
    }

    public override void Awake(GameObject go)
    {
        base.Awake(go);
        
        TableCount = transform.Find("Panel/Table").GetComponent<Table>();
        BtnClose = transform.Find("Header/BtnClose").GetComponent<Button>();

        BtnSave = transform.Find("BtnGroup/BtnSave").GetComponent<Button>();
        BtnClose.onClick.AddListener(OnClickClose);
        BtnSave.onClick.AddListener(OnClickSave);

    }

 

    public override void Active()
    {
        _tableList = data as List<CountNum>;
        base.Active();
        TableCount.ResetTable();
        TableCount.AddTextColumn("序号");
        TableCount.AddTextColumn("项目编码");
        TableCount.AddTextColumn("项目名称");
        TableCount.AddTextColumn("项目特征");
        TableCount.AddTextColumn("计量单位");
        TableCount.AddTextColumn("工程量");
        TableCount.Initialize();
        int i = 0;
        foreach (var item in _tableList)
        {
            i++;
            Datum d = Datum.Body(i.ToString());
            d.elements.Add(i.ToString());//序号
            d.elements.Add(item.ProjectNum);//项目编码
            d.elements.Add(item.ProjectName);//项目名称
            d.elements.Add(item.ProjectFeature);//项目特征
            d.elements.Add(item.ProjectUnit);//计量单位
            d.elements.Add(item.ProjectQuantities);//工程量
            TableCount.data.Add(d);
        }

        TableCount.StartRenderEngine();
    }

    private void OnClickClose()
    {
        ClosePage();
    }

    private void OnClickSave()
    {
       ///TODO Export excel
       ///
    }



   
}
