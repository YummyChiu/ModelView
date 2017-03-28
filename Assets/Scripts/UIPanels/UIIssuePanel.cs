using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
using System;
using System.IO;
using SLS.Widgets.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class UIIssuePanel : TTUIPage
{

    private Transform trTop;
    private Transform trUploadPanel;
    private Transform trDetailsPanel;
    private Transform trBtnGroup;

    private Text mTextHeader;
    private Button mBtnClose;
    private Button mBtnDetail;
    private Button mBtnUpload;

    private Button mBtnTakeImg;
    private Button mBtnLoadImg;
    private Text mTextTips;
    private Sprite mSprite;
    private InputField mIptRemarks;
    private Button mBtnCommit;

    private Table issueTable;

    private string uploadAction = "";
    private string getAction = "";

    string originData = "";

    public UIIssuePanel() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPanel/ComponentIssue";
    }

    public override void Awake(GameObject go)
    {
        trTop = transform.Find("Top");
        trUploadPanel = transform.Find("Panel/UploadPanel");
        trDetailsPanel = transform.Find("Panel/DetailsPanel");
        trBtnGroup = transform.Find("BtnGroup");

        mTextHeader = trTop.Find("Header").GetComponent<Text>();
        mBtnClose = trTop.Find("BtnClose").GetComponent<Button>();
        mBtnDetail = trBtnGroup.Find("BtnDetail").GetComponent<Button>();
        mBtnUpload = trBtnGroup.Find("BtnUpload").GetComponent<Button>();

        mBtnTakeImg = trUploadPanel.Find("BtnTakeImg").GetComponent<Button>();
        mBtnLoadImg = trUploadPanel.Find("BtnLoadImg").GetComponent<Button>();
        mTextTips = trUploadPanel.Find("TextTips").GetComponent<Text>();
        mSprite = trUploadPanel.Find("Image").GetComponent<Sprite>();
        mIptRemarks = trUploadPanel.Find("IptRemarks").GetComponent<InputField>();
        mBtnCommit = trUploadPanel.Find("BtnCommit").GetComponent<Button>();

        issueTable = trDetailsPanel.GetComponentInChildren<Table>();

        mBtnClose.onClick.AddListener(OnClickClose);
        mBtnDetail.onClick.AddListener(OnClickDetail);
        mBtnUpload.onClick.AddListener(OnClickUpload);
        mBtnTakeImg.onClick.AddListener(OnClickTakeImg);
        mBtnLoadImg.onClick.AddListener(OnClickLoadImg);
        mBtnCommit.onClick.AddListener(OnClickCommit);

        uploadAction = "ModelData/AddMobileCompoent?";
        getAction = "ModelData/ShowModelData?";
    }

    public override void Refresh()
    {

        originData =data.ToString();
        mTextHeader.text = "";
        mSprite = null;
        mTextTips.text = "";
        mIptRemarks.text = "";

        // InitIssueTable();
        Task task = new Task(GetComponentInfo(originData));
        base.Refresh();
    }
    #region Buttons Onclick Events
    private void OnClickClose()
    {

    }

    private void OnClickDetail()
    {
        if (trUploadPanel.gameObject.activeSelf)
            trUploadPanel.gameObject.SetActive(false);
        if (trDetailsPanel.gameObject.activeSelf == false)
            trDetailsPanel.gameObject.SetActive(true);
        Task task = new Task(GetComponentInfo(originData));
        //InitIssueTable();
    }

    private void OnClickUpload()
    {
        if (trDetailsPanel.gameObject.activeSelf)
            trDetailsPanel.gameObject.SetActive(false);
        if (trUploadPanel.gameObject.activeSelf == false)
            trUploadPanel.gameObject.SetActive(true);

    }

    private void OnClickTakeImg()
    {
        AndroidCamera.Instance.OnImagePicked += OnImagePicked;
        AndroidCamera.Instance.GetImageFromCamera();
    }

    private void OnClickLoadImg()
    {
        AndroidCamera.Instance.OnImagePicked += OnImagePicked;
        AndroidCamera.Instance.GetImageFromGallery();
    }

    private void OnClickCommit()
    {
        Task task = new Task(PostToServer());
    }

    #endregion
    private IEnumerator GetComponentInfo(string id)
    {
        issueTable.ResetTable();
        issueTable.AddTextColumn("问题材料", null, 100,200);
        issueTable.AddTextColumn("问题描述", null, 150,300);
        issueTable.AddTextColumn("问题记录者", null, 100,200);
        issueTable.Initialize(OnCellSelected);
        int count = 0;

        string url = Tools.WebUrl + getAction;
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        WWW getdata = new WWW(url, form);
        yield return data;
        if (getdata.error != null)
            MessageBox.Show(getdata.error);
        else
        {  
            JObject obj = JObject.Parse(getdata.text);
            string t = obj["dataSource"].ToString();
            if (t == "listdata")
            {
                JArray jar = JArray.Parse(obj["listdata"].ToString());
                //Debug.Log(jar.ToString());
                foreach (var item in jar)
                {
                    count++;
                    string ComponentId = item["ComponentId"].ToString();
                    string ComponentName = item["componentName"].ToString();
                    string FlieInfo = item["FlieInfo"].ToString();
                    string FilePath = item["filePath"].ToString();
                    string Remarks = item["Remarks"].ToString();
                    string CreateTime = item["createTime"].ToString();
                    Datum d = Datum.Body(count.ToString());
                    d.rawObject = Tools.WebUrl + FilePath;
                    d.elements.Add(FlieInfo);
                    d.elements.Add(Remarks);
                    d.elements.Add(CreateTime);
                    issueTable.data.Add(d);
                }

                //JsonExtensions.DeserializeJsonToObject<>
            }
        }
        
        issueTable.StartRenderEngine();
    }
    private IEnumerator PostToServer()
    {
        string filepath = mTextTips.text;
        FileStream fm = File.OpenRead(filepath);
        string fileName = Path.GetFileName(filepath);
        string fileType = Path.GetExtension(filepath).Substring(1);
        byte[] imgData = new byte[fm.Length];
        fm.Read(imgData, 0, (int)fm.Length);
        fm.Close();
        string url = Tools.WebUrl + uploadAction;
        WWWForm form = new WWWForm();
        form.AddField("enctype", "multipart/form-data");
        form.AddBinaryData("file", imgData, name, "image/" + fileType);
        form.AddField("componentId", originData);
        form.AddField("remarks", mIptRemarks.text);

        WWW uploadData = new WWW(url, form);
        yield return uploadData;
        if (uploadData.error != null)
            MessageBox.Show(uploadData.error);
        else
        {

            MessageBox.Show("成功！");
        }
    }

    private void OnImagePicked(AndroidImagePickResult result)
    {
        Debug.Log("OnImagePicked");
        if (result.IsSucceeded)
        {
            MessageBox.Show("Succeeded, path: " + result.ImagePath, "Image Pick Rsult");

            Texture2D texture2d = result.Image;
            Sprite sprite = Sprite.Create(texture2d, new Rect(0.0f, 0.0f, texture2d.width, texture2d.height), new Vector2(0.5f, 0.5f));
            mSprite = sprite;
            mTextTips.text = result.ImagePath;
        }
        else {
            MessageBox.Show("Failed", "Image Pick Rsult");
        }

        AndroidCamera.Instance.OnImagePicked -= OnImagePicked;
    }

    void InitIssueTable()
    {
      //  Task task = new Task(GetComponentInfo(originData));

        #region Test Data

        List<Issue> IssueList = new List<Issue>() {
            new Issue() { IssueName = "Chrysanthemum.jpg", IssueDescription = "null", IssuePoster = "zhao" },
            new Issue() { IssueName = "Hydrangeas.jpg", IssueDescription = "Hello", IssuePoster = "lee" }
    };
        #endregion
        issueTable.ResetTable();
        issueTable.AddTextColumn("问题材料", null, 100);
        issueTable.AddTextColumn("问题描述", null, 100);
        issueTable.AddTextColumn("问题记录者", null, 100);
        issueTable.Initialize(OnCellSelected);

        int count = 0;
        foreach (var item in IssueList)
        {
            count++;
            Datum d = Datum.Body(count.ToString());
            d.elements.Add(item.IssueName);
            d.elements.Add(item.IssueDescription);
            d.elements.Add(item.IssuePoster);

            issueTable.data.Add(d);
        }

        issueTable.StartRenderEngine();
    }

    //TODO: Invoke Android's Native code to open image viewer
    private void OnCellSelected(Datum datum, Column column)
    {
        //MessageBox.Show("selected! To open the image!");

        //DO: CellSelected to save image to some folder
        string imgServerPath = datum.rawObject.ToString();
        Debug.Log(imgServerPath);
        Task task = new Task(DownloadImgAndSave(imgServerPath));

    }

    IEnumerator DownloadImgAndSave(string url)
    {
      
        string fileName = Path.GetFileName(url);
        string savedFileFolder = Tools.Instance.SavedPath + "//" + Tools.BaseFolder + "//" + Tools.FilesFolder + "//" + Tools.TempFolder;
        if (!File.Exists(savedFileFolder + "//" + fileName))
        {
            WWW imgData = new WWW(url);
            yield return imgData;

            if (imgData.error == null)
            {
                if (imgData.isDone)
                {
                    byte[] bytes = imgData.bytes;
                    int length = bytes.Length;
                    Tools.Instance.CreateFile(Tools.Instance.SavedPath + "//" + Tools.BaseFolder + "//" + Tools.FilesFolder + "//" + Tools.TempFolder, fileName, bytes, length);
                }
            }
            else
            {
                MessageBox.Show(imgData.error);
            }
        }
        else
        {
            //TODO:Add Android native invoke
        }
      
    }
}

public class Issue
{
    
    public string IssueDescription { get; set; }
    public string IssueName { get; set; }
    public string IssuePoster { get; set; }
}

