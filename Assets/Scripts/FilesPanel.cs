using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLS.Widgets.Table;
using System;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;
using System.Xml;

public class FilesPanel : MonoBehaviour
{
    public Transform loginPanel;
    public Transform localFilesTr;
    public Transform internetFilesTr;
    public Sprite iconDownLoad;
    public Sprite iconDownDone;
    public Sprite iconModified;


    private Table localFilesTable;
    private Table internetFilesTable;

    private List<Resource> localFiles;
    private List<Resource> internetFiles;

    private Resource selectedFile;

    private Dictionary<string, Sprite> spriteDict;

    string url = "";
    string action = "Files/GetFileInfo";
    // Use this for initialization
    void Start()
    {
        url = Tools.WebUrl + action;
        localFilesTable = localFilesTr.GetComponent<Table>();
        internetFilesTable = internetFilesTr.GetComponent<Table>();
        spriteDict = new Dictionary<string, Sprite>();
        spriteDict.Add("0", iconDownLoad);
        spriteDict.Add("1", iconDownDone);
        spriteDict.Add("2", iconModified);
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
        internetFilesTable.AddImageColumn("文件状态", null, 50);

        internetFilesTable.Initialize(OnCellSelected, spriteDict);
        internetFilesTable.StartRenderEngine();

        StartCoroutine(WaitForRequest());
    }

    IEnumerator WaitForRequest()
    {
        WWW data = new WWW(url,null,LoginManager.responseheaders);
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

    private void OnCellSelected(Datum datum, Column column)
    {
        int row = int.Parse(datum.uid);
        int col = column.idx;
        if (col == 4 && internetFilesTable.data[row - 1].elements[col].value != ((int)(ResourceStatus.DOWNLOADED)).ToString())
        {
            Debug.Log("icon" + internetFilesTable.data[row - 1].elements[col].value);
            Debug.Log("row:" + row + "col:" + col);
            Resource file = datum.rawObject as Resource;
            StartCoroutine(WaitRequest(file, row - 1, col));
        }
        if (internetFilesTable.data[row - 1].elements[4].value == ((int)(ResourceStatus.DOWNLOADED)).ToString())
        {
            Resource file = datum.rawObject as Resource;

            selectedFile = file;
        }

    }

    IEnumerator WaitRequest(Resource file, int row, int col)
    {
        string url =Tools.WebUrl+ file.FilePath;
        WWW data = new WWW(url);
        yield return data;
        if (data.error == null)
        {
            if (data.isDone)
            {
                Debug.Log("Download is done! Start to save to the local");

                byte[] f = data.bytes;
                int length = f.Length;
                CreatFile(Tools.Instance.SavedPath + "//" + Tools.BaseFolder + "//" + Tools.FilesFolder, file.FileName, f, length);
                internetFilesTable.data[row].elements[col].value = ((int)ResourceStatus.DOWNLOADED).ToString();

            }
        }
        else
        {
            MessageBox.Show(data.error, "connect error");
        }
    }



    void CreatFile(string path, string name, byte[] info, int length)
    {
        Stream sw;
        FileInfo fi = new FileInfo(path + "//" + name);
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


    private void OnRowSelected(Datum datum)
    {
        Resource file = datum.rawObject as Resource;

        selectedFile = file;

        //DatumElementList t = datum.elements;
        print("you clicked :" + datum.uid);

    }

    public void LoadModel()
    {
        //write config
        string temp = Tools.Instance.SavedPath + "/" + Tools.BaseFolder + "/" + Tools.ConfigFolder + "/" + Tools.ConfigFile;
        //Debug.Log(temp);
        Resource f = selectedFile;
        if (!File.Exists(temp))
        {
            GenerateXmlFile(temp);
        }
        WriteXmlFile(temp,Tools.Instance.SavedPath+"/"+Tools.BaseFolder+"/"+Tools.FilesFolder+"/"+selectedFile.FileName,"CurrentFile");
        UIManger.Instance.StartToload();
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

    private void WriteXmlFile(string path,string filepath,string node)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);
        XmlNode xn = xmlDoc.SelectSingleNode("Setting/CurrentFile");
        xn.InnerText = filepath;
        xmlDoc.Save(path);
    }

    public void ReturnLogin()
    {
        File.Delete(Tools.Instance.SavedPath + "/" + Tools.BaseFolder + "/" + Tools.UserFolder + "/" + Tools.UserFile);
        transform.gameObject.SetActive(false);
        loginPanel.gameObject.SetActive(true);
        loginPanel.GetComponent<LoginManager>().InitInput();
    }
}
public enum ResourceStatus
{
    NEW, DOWNLOADED, MODIFIED
}
