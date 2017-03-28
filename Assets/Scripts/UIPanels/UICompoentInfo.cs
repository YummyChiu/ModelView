using System.Collections;
using System.Collections.Generic;
using TinyTeam.UI;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

    private string componentId;


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
        //string id = data.ToString();
        componentId = data.ToString();

        SqliteConnection conn = Tools.Instance.SqlConnection();

        SqliteCommand cmd = conn.CreateCommand();
        cmd.CommandText = "select * from Attribute where EId =" + componentId;
        SqliteDataReader dr = cmd.ExecuteReader();
        string attributeInfo = "";
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                attributeInfo = dr["AttributeInfo"] as string;
            }

            JObject jObjAttr = JObject.Parse(attributeInfo);
            foreach (var itemkey in jObjAttr)
            {
                string title = itemkey.Key;
                Transform parentInstance = GameObject.Instantiate(Resources.Load("Prefabs/ComponentParentNode", typeof(Transform))) as Transform;
                Transform parentElement = parentInstance.Find("Element");
                Transform parentChildContent = parentInstance.Find("ChildContent");
                Button Btn = parentElement.Find("Button").GetComponent<Button>();

                parentElement.Find("Text").GetComponent<Text>().text = title;

                Btn.onClick.AddListener(() => { OnClickShowOrHideContent(Btn, parentChildContent); });
                parentElement.GetComponent<Button>().onClick.AddListener(() => { OnClickShowOrHideContent(Btn, parentChildContent); });
                parentInstance.SetParent(mContent);
                parentInstance.gameObject.SetActive(true);
                parentInstance.localScale = new Vector3(1, 1, 1);

                foreach (var itemlist in itemkey.Value)
                {

                    string para = "";
                    string value = "";
                    foreach (JProperty item in itemlist)
                    {
                        para = item.Name;
                        value = item.Value.ToString();
                        Transform childInstance = GameObject.Instantiate(Resources.Load("Prefabs/ComponentInfoChildRow", typeof(Transform))) as Transform;
                        childInstance.SetParent(parentChildContent);
                        childInstance.localScale = new Vector3(1, 1, 1);
                        childInstance.gameObject.SetActive(true);
                        childInstance.FindChild("TAttribute").GetComponent<Text>().text = para;
                        childInstance.FindChild("TValue").GetComponent<Text>().text = value;
                    }
                }
            }
            //create button to view issue
            Transform trViewIssue = GameObject.Instantiate(Resources.Load("Prefabs/BtnViewIssue", typeof(Transform))) as Transform;
            trViewIssue.GetComponent<Button>().onClick.AddListener(OnClickShowIssuePanel);
            trViewIssue.SetParent(mContent);
            trViewIssue.localScale = new Vector3(1, 1, 1);
        }
    
        conn.Close();
        conn.Dispose(); 
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

    private void OnClickShowOrHideContent(Button btn, Transform content)
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

    private void OnClickShowIssuePanel()
    {
        TTUIPage.ShowPage<UIIssuePanel>("[871714]");
    }
}
