using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
using SLS.Widgets.Table;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Xml;

public class UIFilesPanel : TTUIPage
{

    private Transform localFilesTr;
    private Transform internetFilesTr;
    private Sprite iconDownLoad;
    private Sprite iconDownDone;
    private Sprite iconModified;


    private Table localFilesTable;
    private Table internetFilesTable;

    private Button mBtnReturn;
    private Button mBtnLoad;
    private Button mBtnLocal;
    private Button mBtnInternet;


    private List<Resource> localFiles;
    private List<Resource> internetFiles;

    private Resource selectedFile;

    private Dictionary<string, Sprite> spriteDict;


    string url = "";
    string action = "Files/GetFileInfo";

    public UIFilesPanel() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPanel/FilePanel";
    }

    public override void Awake(GameObject go)
    {
        url = Tools.WebUrl + action;
        localFilesTr = this.transform.Find("FilesPanel/LocalFilePanel");
        internetFilesTr = this.transform.Find("FilesPanel/InternetPanel");
        mBtnReturn = this.transform.Find("MenuBtnPanel/BtnReturn").GetComponent<Button>();
        mBtnLoad = this.transform.Find("MenuBtnPanel/BtnLoad").GetComponent<Button>();
        mBtnLocal = this.transform.Find("FilesPanel/FilesBtnPanel/BtnLocalFiles").GetComponent<Button>();
        mBtnInternet = this.transform.Find("FilesPanel/FilesBtnPanel/BtnInternetFiles").GetComponent<Button>();

        mBtnReturn.onClick.AddListener(OnReturn);
        mBtnLoad.onClick.AddListener(OnLoadModel);
        mBtnLocal.onClick.AddListener(InitLocalFiles);
        mBtnInternet.onClick.AddListener(InitInternetFiles);

        iconDownLoad = Resources.Load<Sprite>("RawImg/download") ;
        iconDownDone = Resources.Load<Sprite>("RawImg/done");
        iconModified = Resources.Load<Sprite>("RawImg/edit");
        localFilesTable = localFilesTr.GetComponent<Table>();
        internetFilesTable = internetFilesTr.GetComponent<Table>();
        spriteDict = new Dictionary<string, Sprite>();
        spriteDict.Add("0", iconDownLoad);
        spriteDict.Add("1", iconDownDone);
        spriteDict.Add("2", iconModified);
    }

   
    public override void Refresh()
    {
        // base.Refresh();
        InitLocalFiles();
    }

    public void InitLocalFiles()
    {
        if (localFilesTr.gameObject.activeSelf == false)
            localFilesTr.gameObject.SetActive(true);
        if (internetFilesTr.gameObject.activeSelf == true)
            internetFilesTr.gameObject.SetActive(false);
        localFilesTable.ResetTable();
        localFilesTable.AddTextColumn("序号", null, 50);
        localFilesTable.AddTextColumn("文件名", null, 250);
        localFilesTable.AddTextColumn("文件大小", null, 160);
        localFilesTable.AddTextColumn("修改日期", null, 240);
        localFilesTable.AddTextColumn("文件状态", null, 160);

        localFilesTable.Initialize(OnRowSelected);
        // Debug.Log(Tools.BaseFolder + "/" + Tools.ConfigFolder + "/" + Tools.FilesFolder);
        localFiles = Tools.Instance.GetLocalFiles(Tools.BaseFolder + "/" + Tools.FilesFolder);

        if (localFiles != null)
        {
            int count = 0;
            foreach (var localfile in localFiles)
            {
                count++;
                Datum d = MakeDatum(count.ToString(), localfile);
                localFilesTable.data.Add(d);
            }
        }
        localFilesTable.StartRenderEngine();
    }

    private void OnRowSelected(Datum datum)
    {
        Resource file = datum.rawObject as Resource;

        selectedFile = file;
        Debug.Log("you clicked :" + datum.uid);
        Debug.Log(file.FileName);
    }
    private Datum MakeDatum(string pfx, Resource file)
    {
        Datum d = Datum.Body(pfx);
        d.elements.Add(pfx);
        d.elements.Add(file.FileName);
        d.elements.Add(file.FileSize.ToString());
        d.elements.Add(file.FileDate.ToString());
        d.elements.Add("download!");
        d.rawObject = file;
        return d;
    }

    public void InitInternetFiles()
    {
        if (localFilesTr.gameObject.activeSelf)
            localFilesTr.gameObject.SetActive(false);
        if (internetFilesTr.gameObject.activeSelf == false)
            internetFilesTr.gameObject.SetActive(true);
        internetFilesTable.ResetTable();
        internetFilesTable.AddTextColumn("序号", null, 50);
        internetFilesTable.AddTextColumn("文件名", null, 250);
        internetFilesTable.AddTextColumn("文件大小", null, 160);
        internetFilesTable.AddTextColumn("修改日期", null, 240);
        internetFilesTable.AddImageColumn("文件状态", null, 80);

        internetFilesTable.Initialize(OnCellSelected, spriteDict);
        internetFilesTable.StartRenderEngine();

        Task task = new Task(WaitForRequest());
       // StartCoroutine(WaitForRequest());
    }

    private void OnCellSelected(Datum datum, Column column)
    {
        int row = int.Parse(datum.uid);
        int col = column.idx;
        if (col == 4 && internetFilesTable.data[row - 1].elements[col].value != ((int)(ResourceStatus.DOWNLOADED)).ToString())
        {
            MessageBox.Show("Start To download");
            Debug.Log("icon" + internetFilesTable.data[row - 1].elements[col].value);
            Debug.Log("row:" + row + "col:" + col);
            Resource file = datum.rawObject as Resource;
            Task task = new Task(WaitRequest(file, row - 1, col));
            //StartCoroutine(WaitRequest(file, row - 1, col));
        }
        if (internetFilesTable.data[row - 1].elements[4].value == ((int)(ResourceStatus.DOWNLOADED)).ToString())
        {
            Resource file = datum.rawObject as Resource;

            selectedFile = file;
        }

    }
    IEnumerator WaitForRequest()
    {
        WWW data = new WWW(url);
        yield return data;
        if (data.error == null)
        {
            internetFiles = JsonConvert.DeserializeObject<List<Resource>>(data.text);
            int count = 0;
            Debug.Log(internetFiles.Count.ToString());
            Debug.Log("CHENGGONG");
            foreach (var item in internetFiles)
            {
                count++;
                Datum d = MakeInternetDatum(count.ToString(), item);
                internetFilesTable.data.Add(d);
            }
        }
        else
        {
            MessageBox.Show(data.error, "Error");
        }
    }
    private Datum MakeInternetDatum(string pfx, Resource file)
    {
        //check file status
        ResourceStatus Status;
        string s = "";
        if (localFiles != null)
        {
            if (localFiles.Any(f => f.FileName == file.FileName))
            {
                Resource localfile = localFiles.First(f => f.FileName == file.FileName);
                if (localfile.FileSize == file.FileSize)
                    Status = ResourceStatus.DOWNLOADED;
                else
                    Status = ResourceStatus.MODIFIED;
            }
            else
            {
                Status = ResourceStatus.NEW;
            }
        }
        else
        {
            Status = ResourceStatus.NEW;
        }
        Debug.Log(Status.ToString());
        //int i = (int)(Status);

        Debug.Log(((int)(Status)).ToString());


        Datum d = Datum.Body(pfx);
        d.elements.Add(pfx);
        d.elements.Add(file.FileName);
        d.elements.Add(file.FileSize.ToString());
        d.elements.Add(file.FileDate.ToString());
        d.elements.Add(((int)(Status)).ToString());
        d.rawObject = file;
        return d;

    }

    IEnumerator WaitRequest(Resource file, int row, int col)
    {
        string url = Tools.WebUrl + file.FilePath;
        Debug.Log(url);
        WWW data = new WWW(url);
        yield return data;
        if (data.error == null)
        {
            if (data.isDone)
            {
                Debug.Log("Download is done! Start to save to the local");

                byte[] f = data.bytes;
                int length = f.Length;
                CreatFile(Tools.Instance.SavedPath + "/" + Tools.BaseFolder + "/" + Tools.FilesFolder, file.FileName, f, length);
                internetFilesTable.data[row].elements[col].value = ((int)ResourceStatus.DOWNLOADED).ToString();

            }
        }
        else
        {
            MessageBox.Show(data.error, "connect error");
        }
    }

   public  void CreatFile(string path, string name, byte[] info, int length)
    {
        Stream sw;
        FileInfo fi = new FileInfo(path + "/" + name);
        if (!fi.Exists)
        {
            sw = fi.Create();
        }
        else {
            Debug.Log(name + "is existed in the " + "-------" + path);
            return;
        }
        sw.Write(info, 0, length);
        sw.Close();
        sw.Dispose();
        Debug.Log(name + "is saved to the" + "--------" + path);
    }

    private void OnReturn()
    {
        Hide();
        
        ShowPage<UILogin>();
    }

    public void OnLoadModel()
    {
        
        //write config
        string temp = Tools.Instance.SavedPath + "/" + Tools.BaseFolder + "/" + Tools.ConfigFolder + "/" + Tools.ConfigFile;
        //Debug.Log(temp);
        Resource f = selectedFile;
        if (!File.Exists(temp))
        {
            GenerateXmlFile(temp);
        }
        Debug.Log(Tools.Instance.SavedPath + "/" + Tools.BaseFolder + "/" + Tools.FilesFolder + "/" + selectedFile.FileName);
        WriteXmlFile(temp, Tools.Instance.SavedPath + "/" + Tools.BaseFolder + "/" + Tools.FilesFolder + "/" + selectedFile.FileName, "CurrentFile");
        ClosePage<UIFilesPanel>();
        ShowPage<UIMenuPanel>();

        ReadModelSQLite.Instance.LoadModel();
        //GameEntry.Instance.LoadModel();
        //UIManger.Instance.StartToload();
        //SceneManager.LoadScene("1125_builtinorder");

    }

    private void GenerateXmlFile(string path)
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlElement rootElement = xmlDoc.CreateElement("Setting");
        xmlDoc.AppendChild(rootElement);

        XmlElement currentFile = xmlDoc.CreateElement("CurrentFile");
        rootElement.AppendChild(currentFile);

        xmlDoc.Save(path);
    }

    private void WriteXmlFile(string path, string filepath, string node)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);
        XmlNode xn = xmlDoc.SelectSingleNode("Setting/CurrentFile");
        xn.InnerText = filepath;
        xmlDoc.Save(path);
    }

}
