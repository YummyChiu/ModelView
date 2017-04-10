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
using Mono.Data.Sqlite;

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
    private Image mImg;
    private Button mBtnImg;
    private InputField mIptRemarks;
    private Button mBtnCommit;

    private Table issueTable;

    private string uploadAction = "";
    private string getAction = "";

    string originData = "";

    public UIIssuePanel() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
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
        mBtnImg = trUploadPanel.Find("BtnImg").GetComponent<Button>();
        mImg = trUploadPanel.Find("BtnImg").GetComponent<Image>();
        mIptRemarks = trUploadPanel.Find("IptRemarks").GetComponent<InputField>();
        mBtnCommit = trUploadPanel.Find("BtnCommit").GetComponent<Button>();

        issueTable = trDetailsPanel.GetComponentInChildren<Table>();

        mBtnClose.onClick.AddListener(OnClickClose);
        mBtnDetail.onClick.AddListener(OnClickDetail);
        mBtnUpload.onClick.AddListener(OnClickUpload);
        mBtnTakeImg.onClick.AddListener(OnClickTakeImg);
        mBtnLoadImg.onClick.AddListener(OnClickLoadImg);
        mBtnCommit.onClick.AddListener(OnClickCommit);
        mBtnImg.onClick.AddListener(OnClickOpenImg);
        uploadAction = "ModelData/AddMobileCompoent?";
        getAction = "ModelData/ShowModelData?";
    }



    public override void Refresh()
    {

        originData = data.ToString();
        mTextHeader.text = originData;
        mImg.sprite = null;
        mTextTips.text = "";
        mIptRemarks.text = "";

        // InitIssueTable();
        Task task = new Task(GetComponentInfo(originData));
        base.Refresh();
    }
    #region Buttons Onclick Events
    private void OnClickClose()
    {

        ClosePage<UIIssuePanel>();
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
        AndroidCamera.Instance.OpenCamera();
        //AndroidCamera.Instance.OnImagePicked += OnImagePicked;
        //AndroidCamera.Instance.GetImageFromCamera();
    }

    private void OnClickLoadImg()
    {
        AndroidCamera.Instance.OnImagePicked += OnImagePicked;
        AndroidCamera.Instance.OpenGallery();
    }

    private void OnClickCommit()
    {

        Task task = new Task(PostToServer());
    }
    private void OnClickOpenImg()
    {
        if (mTextTips.text != null)
            AndroidCamera.Instance.OpenImg("file://" + mTextTips.text);

    }
    #endregion
    private IEnumerator GetComponentInfo(string id)
    {
        issueTable.ResetTable();
        issueTable.AddTextColumn("问题材料", null, 100, 200);
        issueTable.AddTextColumn("问题描述", null, 150, 300);
        issueTable.AddTextColumn("问题记录者", null, 100, 200);
        issueTable.Initialize(OnCellSelected);
        int count = 0;

        string url = Tools.WebUrl + getAction;
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        //form.AddField("id", "258879");
        WWW getdata = new WWW(url, form);
        yield return data;
        if (getdata.error != null)
            MessageBox.Show(getdata.error);
        else
        {
            if (getdata.isDone)
            {
                if (getdata.text != "\"没有构件的数据\"")
                {
                    Debug.Log("json数据：" + getdata.text);
                    JObject obj = JObject.Parse(getdata.text);
                    Debug.Log("json数据：" + getdata.text);
                    string t = obj["dataSource"].ToString();
                    if (t == "listdata")
                    {
                        JArray jar = JArray.Parse(obj["listdata"].ToString());
                        //Debug.Log(jar.ToString());
                        SqliteConnection conn = Tools.Instance.SqlConnection();

                        foreach (var item in jar)
                        {
                            //得到问题反馈列表，并写进ComponentInfos表里面去
                            //-Id-ComponentId-FileInfo-FileServerInfo-FileLocalInfo-Remarks
                            //如果表没有这个

                            count++;
                            string ComponentId = item["ComponentId"].ToString();
                            //string ComponentName = item["componentName"].ToString();
                            string FileInfo = item["FlieInfo"].ToString();
                            string FilePath = item["filePath"].ToString();

                            string Remarks = item["FeedbackInfo"].ToString();
                            Debug.Log(Remarks);
                            string CreateTime = item["createTime"].ToString();
                            Datum d = Datum.Body(count.ToString());
                            d.rawObject = Tools.WebUrl + FilePath;
                            d.elements.Add(FileInfo);
                            d.elements.Add(Remarks);
                            d.elements.Add(CreateTime);
                            issueTable.data.Add(d);
                            Debug.Log(FilePath);
                            // FilePath = count.ToString();
                            SqliteCommand cmd = conn.CreateCommand();
                            cmd.CommandText = "select * from ComponentInfos where FileInfo ='" + FileInfo + "'";
                            Debug.Log("cmd:" + cmd.CommandText);

                            SqliteDataReader dr = cmd.ExecuteReader();

                            if (dr.HasRows)
                            {
                                Debug.Log("The componentInfos doesn't have !");
                            }
                            else
                            {

                                //Insert
                                Debug.Log("start to insert to componentInfos");
                                SqliteCommand cmd2 = conn.CreateCommand();
                                // cmd2.CommandText = "Insert Into ComponentInfos(ComponentId,FileInfo,FileServerInfo,FileLocalInfo)values("+ ComponentId +","+ FlieInfo + ","+ FilePath + "," + "a" +  ")";
                                cmd2.CommandText = "Insert Into [ComponentInfos]([ComponentId],[FileInfo],[FileServerInfo],[FileLocalInfo],[Remarks])values(@ComponentId,@FileInfo,@FileServerInfo,@FileLocalInfo,@Remarks)";

                                cmd2.Parameters.AddWithValue("ComponentId", ComponentId);
                                cmd2.Parameters.AddWithValue("FileInfo", FileInfo);
                                cmd2.Parameters.AddWithValue("FileServerInfo", FilePath);
                                cmd2.Parameters.AddWithValue("FileLocalInfo", "");
                                cmd2.Parameters.AddWithValue("Remarks", Remarks);
                                Debug.Log("cm2:" + cmd2.CommandText);
                                cmd2.ExecuteNonQuery();
                            }

                        }
                        conn.Close();
                        conn.Dispose();
                    }
                }
                else { Debug.Log("json数据：" + getdata.text); }
                
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
        form.AddBinaryData("file", imgData, fileName, "image/" + fileType);
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

    //private void OnImagePicked(AndroidImagePickResult result)
    //{
    //    Debug.Log("OnImagePicked");
    //    if (result.IsSucceeded)
    //    {
    //        MessageBox.Show("Succeeded, path: " + result.ImagePath, "Image Pick Rsult");

    //        Texture2D texture2d = result.Image;
    //        Sprite sprite = Sprite.Create(texture2d, new Rect(0.0f, 0.0f, texture2d.width, texture2d.height), new Vector2(0.5f, 0.5f));
    //        mSprite = sprite;
    //        mTextTips.text = result.ImagePath;
    //    }
    //    else {
    //        MessageBox.Show("Failed", "Image Pick Rsult");
    //    }

    //    AndroidCamera.Instance().OnImagePicked -= OnImagePicked;
    //}
    private void OnImagePicked(string result)
    {
        mImg.sprite = ImgToSprite.instance.LoadNewSprite(result);

        if (mImg.sprite == null)
            MessageBox.Show("图片为空");
        mTextTips.text = result;
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
        string downloadPath = Tools.Instance.SavedPath + "/" + Tools.BaseFolder + "/" + Tools.FilesFolder + "/" + Tools.TempFolder;

        //查询数据库是否有这个构件的反馈的本地记录
        SqliteConnection conn = Tools.Instance.SqlConnection();
        SqliteCommand cmd = conn.CreateCommand();
        //cmd.CommandText = "select * from ComponentInfos where FileInfo ='" + FileInfo + "'";
        cmd.CommandText = "select * from ComponentInfos where FileInfo ='" + fileName + "'";
        Debug.Log(cmd.CommandText);
        SqliteDataReader dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            if (dr["FileLocalInfo"].ToString() == "")
            {
                Debug.Log("本地目录为空，请添加！");
                //下载图片
                WWW imgData = new WWW(url);
                yield return imgData;

                if (imgData.error == null)
                {
                    if (imgData.isDone)
                    {
                        Debug.Log("The Selected Image :" + fileName + "is downloaded!");
                        string imgpath = downloadPath + "/" + fileName;
                        byte[] bytes = imgData.bytes;
                        int length = bytes.Length;
                        Tools.Instance.CreateFile(downloadPath, fileName, bytes, length);
                        SqliteCommand cmd2 = conn.CreateCommand();
                        cmd2.CommandText = "Insert Into [ComponentInfos]([ComponentId],[FileInfo],[FileServerInfo],[FileLocalInfo],[Remarks])values(@ComponentId,@FileInfo,@FileServerInfo,@FileLocalInfo,@Remarks)";
                        cmd2.CommandText = "Update [ComponentInfos] Set [FileLocalInfo] = '" + downloadPath + "/" + fileName + "' where [FileInfo] = '" + fileName + "'";
                        //cmd2.Parameters.AddWithValue("FileLocalInfo", downloadPath + "/"+fileName);
                        Debug.Log("cm2:" + cmd2.CommandText);
                        cmd2.ExecuteNonQuery();
#if UNITY_ANDROID
                        AndroidCamera.Instance.OpenImg("file://" + imgpath);
#endif
                    }
                }
                else
                {
                    MessageBox.Show(imgData.error);
                }
            }
            else
            {
                // Debug.Log("已经存在！");
                Debug.Log("The Selected Image :" + fileName + "is existed!");
                string imgpath = dr["FileLocalInfo"].ToString();
                AndroidCamera.Instance.OpenImg("file://" + imgpath);
                Debug.Log("The Selected Image :" + imgpath + "is existed!");
                //#if UNITY_ANDROID
                //                AndroidCamera.Instance().OpenSingleImg();
                //                dbPath = "Data Source=" + LoadXMLGetModelPath();



                //TODO:Add Android native invoke

            }
        }
        conn.Close();
        conn.Dispose();


        //string downloadPath = Tools.Instance.SavedPath + "//" + Tools.BaseFolder + "//" + Tools.FilesFolder + "//" + Tools.TempFolder;
        //string takePhotoPath = Tools.Instance.SavedPath + "//" + Tools.TakePhotoFolder;
        //if (!File.Exists(downloadPath + "//" + fileName) || !File.Exists(takePhotoPath+"//" +fileName))
        //{
        //    WWW imgData = new WWW(url);
        //    yield return imgData;

        //    if (imgData.error == null)
        //    {
        //        if (imgData.isDone)
        //        {
        //            Debug.Log("The Selected Image :" + fileName + "is downloaded!");
        //            byte[] bytes = imgData.bytes;
        //            int length = bytes.Length;
        //            Tools.Instance.CreateFile(downloadPath, fileName, bytes, length);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show(imgData.error);
        //    }
        //}
        //else
        //{
        //    Debug.Log("The Selected Image :" + fileName + "is existed!");
        //    //TODO:Add Android native invoke
        //}

    }
}

public class Issue
{

    public string IssueDescription { get; set; }
    public string IssueName { get; set; }
    public string IssuePoster { get; set; }
}

