using System.Collections;
using System.Collections.Generic;
using TinyTeam.UI;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Mono.Data.Sqlite;
using System;

public class UICompoentInfo : TTUIPage
{
    private Transform Header;
    private Text Name;
    private Button BtnClose;

    private Transform mContent;

    private Button BtnViewPaper;
    private Button BtnViewIssue;

    private Sprite SpFold;
    private Sprite SpUnFold;

    public UICompoentInfo() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPanel/CompoentAttribute";
    }
    public override void Awake(GameObject go)
    {
        Header = transform.Find("Header");
        Name = Header.GetComponentInChildren<Text>();
        BtnClose = Header.GetComponentInChildren<Button>();
        BtnClose.onClick.AddListener(OnClickClose);



        mContent = transform.Find("Scroll View/Viewport/Content");
        //mParentNode = mContent.Find("ParentNode");
        //mChildContent = mParentNode.Find("ChildContent/Row");



        BtnViewPaper = mContent.Find("BtnViewPaper").GetComponent<Button>();
        BtnViewIssue = mContent.Find("BtnViewIssue").GetComponent<Button>();
       // mParentNode.gameObject.SetActive(false);
        BtnViewPaper.gameObject.SetActive(false);
        BtnViewIssue.gameObject.SetActive(false);
        SpFold = Resources.Load<Sprite>("RawImg/icon_left");
        SpUnFold = Resources.Load<Sprite>("RawImg/icon_down");
        base.Awake(go);
    }
    public override void Active()
    {
        base.Active();
    }

    public override void Refresh()
    {

        foreach (Transform tr in mContent)
        {
            GameObject.Destroy(tr.gameObject);
        }
        string id = data.ToString();


        SqliteConnection conn = Tools.Instance.SqlConnection();
       
        SqliteCommand cmd = conn.CreateCommand();
        cmd.CommandText = "select * from Attribute where EId ="+id;
        SqliteDataReader dr = cmd.ExecuteReader();
        string componentinfo = "";
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                 componentinfo = dr["AttributeInfo"] as string;
            }
        
        #region test for once

        // string temp =" {\"限制条件\":[{\"与体量相关\":\"否\"},{\"底部偏移\":\"0\"},{\"房间边界\":\"是\"},{\"顶部延伸距离\":\"0\"},{\"已附着底部\":\"否\"},{\"底部限制条件\":\"结构1层\"},{\"定位线\":\"墙中心线\"},{\"底部延伸距离\":\"0\"},{\"顶部偏移\":\"0\"},{\"顶部约束\":\"未连接\"},{\"已附着顶部\":\"否\"},{\"无连接高度\":\"4400\"}],\"文字\":[{\"楼层\":null},{\"施工时间\":null}],\"其他\":[{\"类别\":\"墙\"},{\"族名称\":null},{\"族\":\"基本墙\"},{\"族与类型\":\"基本墙: 直形墙_300mm_C50\"},{\"类型\":\"直形墙_300mm_C50\"},{\"类型 ID\":\"219311\"}],\"结构\":[{\"钢筋保护层 - 外部面\":\"钢筋保护层 15 <15 mm>\"},{\"钢筋保护层 - 其他面\":\"钢筋保护层 15 <15 mm>\"},{\"钢筋保护层 - 内部面\":\"钢筋保护层 15 <15 mm>\"},{\"结构用途\":\"承重\"},{\"结构\":\"是\"},{\"启用分析模型\":\"否\"}],\"阶段化\":[{\"拆除的阶段\":\"无\"},{\"创建的阶段\":\"新构造\"}],\"钢筋集\":[{\"是否已配筋\":\"否\"},{\"墙身拉筋\":null},{\"暗柱箍筋\":null},{\"竖向分布钢筋\":null},{\"暗柱箍筋类型\":null},{\"暗柱纵筋\":null},{\"墙编号\":null},{\"水平分布钢筋\":null}],\"尺寸标注\":[{\"体积\":\"1.12 m³\"},{\"长度\":\"725\"},{\"面积\":\"4 m²\"}],\"标识数据\":[{\"设计选项\":\"-1\"},{\"图像\":\"<无>\"},{\"类型名称\":null},{\"标记\":null},{\"注释\":null}]} ";
        // ComponentAttribute att = new ComponentAttribute() { ComponentId = "219326", ComponentInfo = temp };

          Header.GetComponentInChildren<Text>().text = id;

        Dictionary<string, List<Dictionary<string, string>>> jsondata = JsonExtensions.DeserializeStringToDictionary<string, List<Dictionary<string, string>>>(componentinfo);

        foreach (var itemkey in jsondata)
        {
            string title = itemkey.Key;
            //Debug.Log("父标题："+title);
            Transform parentInstance = GameObject.Instantiate(Resources.Load("Prefabs/ComponentParentNode", typeof(Transform))) as Transform;
            Transform parentElement = parentInstance.Find("Element");
            Transform parentChildContent = parentInstance.Find("ChildContent");
            Button Btn = parentElement.Find("Button").GetComponent<Button>();
            
            parentElement.Find("Text").GetComponent<Text>().text = title;

            Btn.onClick.AddListener(()=> { OnClickShowOrHideContent(Btn,parentChildContent); });
            parentElement.GetComponent<Button>().onClick.AddListener(() => { OnClickShowOrHideContent(Btn, parentChildContent); });
            parentInstance.SetParent(mContent);
            parentInstance.gameObject.SetActive(true);
            parentInstance.localScale = new Vector3(1, 1, 1);


            foreach (var itemlist in itemkey.Value)
            {
                string para = "";
                string value = "";
                foreach (var dict in itemlist)
                {
                    para = dict.Key;
                    value = dict.Value;
                }
                Transform childInstance = GameObject.Instantiate(Resources.Load("Prefabs/ComponentInfoChildRow", typeof(Transform)))as Transform;
                childInstance.SetParent(parentChildContent);
                childInstance.localScale = new Vector3(1, 1, 1);
                childInstance.gameObject.SetActive(true);
                childInstance.FindChild("TAttribute").GetComponent<Text>().text = para;
                childInstance.FindChild("TValue").GetComponent<Text>().text = value;
                // Debug.Log("参数：" + para + "---" +"参数值："+value);
            }

        }
        }
        conn.Close();
        conn.Dispose();
        #endregion
        base.Refresh();
    }

    public override void Hide()
    {
        base.Hide();
    }

    private void OnClickClose()
    {
        //close current page
        ClosePage<UICompoentInfo>();
        Debug.Log("close the current ui");
    }

    private void OnClickShowOrHideContent(Button btn,Transform content)
    {
        //to do (change image)
        bool state = content.GetChild(0).gameObject.activeSelf;

        if (state)//if children is active ,sprite change to folded;
        {
            btn.GetComponent<Image>().sprite = SpFold;
        }
        else
            btn.GetComponent<Image>().sprite = SpUnFold;

        foreach (Transform tr in content)
        {
            tr.gameObject.SetActive(!state);
        }

    }
}
