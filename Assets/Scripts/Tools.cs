using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Mono.Data.Sqlite;
using System;
using System.Xml;

public class Tools :MonoBehaviour
{


    public  string SavedPath ;

    public static string BaseFolder = "Configs";


    public static string UserFolder = "user";

    public static string FilesFolder = "files";

    public static string ConfigFolder = "config";

    public static string WebUrl = "http://192.168.0.130:8086/";

    public static string UserFile = "user.bin";

    public static string ConfigFile = "setting.xml";


    private string dbPath = "";

    private static Tools _instance;
    public static Tools Instance
    {
        get { return _instance; }
    }
    private Tools(){ }
  

    void Awake()
    {

#if UNITY_EDITOR
        SavedPath =  Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));
#elif UNITY_ANDROID
         SavedPath =  Application.persistentDataPath;
#elif UNITY_STANDALONE_WIN
        SavedPath =  Application.persistentDataPath;
#endif
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        
    }
    void Start()
    {
        CreatDirectory();
    }
    public void CreatDirectory()
    {
        Debug.Log(SavedPath);
        if (!Directory.Exists(SavedPath + "/" + BaseFolder))
            Directory.CreateDirectory(SavedPath + "/" + BaseFolder);
        if (!Directory.Exists(SavedPath + "/" + BaseFolder + "/" + UserFolder))
            Directory.CreateDirectory(SavedPath + "/" + BaseFolder + "/" + UserFolder);
        if (!Directory.Exists(SavedPath + "/" + BaseFolder + "/" + FilesFolder))
            Directory.CreateDirectory(SavedPath + "/" + BaseFolder + "/" + FilesFolder);
        if (!Directory.Exists(SavedPath + "/" + BaseFolder + "/" + ConfigFolder))
            Directory.CreateDirectory(SavedPath + "/" + BaseFolder + "/" + ConfigFolder);

    }

    public  void CreatDirectory(string foldername)
    {

        if (!Directory.Exists(SavedPath + "/" + foldername))
        {
            Directory.CreateDirectory(SavedPath + "/" + foldername);
        }
    }
    public  List<Resource> GetLocalFiles(string foldername)
    {
        List<Resource> FileList = new List<Resource>();
        string temp = SavedPath + "/" + foldername;
        DirectoryInfo dir = new DirectoryInfo(@temp);
        if (dir.GetFiles().Length != 0)
        {
            foreach (var item in dir.GetFiles())
            {
                Resource file = new Resource()
                {
                    FileName = item.Name,
                    FileSize = item.Length,
                    FileType = item.Extension,
                    FileDate = item.CreationTime,
                    FilePath = item.FullName

                };
                FileList.Add(file);
            }
            return FileList;
        }
        else
            return null;
    }


    public SqliteConnection SqlConnection()
    {

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        dbPath = "Data Source=" + LoadXMLGetModelPath();

#elif UNITY_ANDROID
         dbPath= "URI=file:"+LoadXMLGetModelPath();
        
#endif
        SqliteConnection conn = new SqliteConnection(dbPath);

        try
        {
            conn.Open();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Eroor");
        }

        return conn;
    }

    private  string LoadXMLGetModelPath()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(SavedPath + "/" + Tools.BaseFolder + "/" + Tools.ConfigFolder + "/" + Tools.ConfigFile);
        XmlNode xn = xmlDoc.SelectSingleNode("Setting/CurrentFile");
        string path = xn.InnerText;
        return path;
    }
}

public class Resource
{

    public string FileName { get; set; }
    public string FileType { get; set; }

    public long FileSize { get; set; }
    public DateTime FileDate { get; set; }

    public string FilePath { get; set; }
    public ResourceStatus FileStatus { get; set; }
}
