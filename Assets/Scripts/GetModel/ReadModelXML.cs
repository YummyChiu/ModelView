using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Xml;
using System.Collections.Generic;
using UnityEngine.UI;

public class ReadModelXML : MonoBehaviour
{
    public static ReadModelXML _instance;
    public  string strGeometryPath = "";//XML模型文件路径
    public GameObject go;//实例化一个预制体
    public static List<GameObject> goList = new List<GameObject>();//定义一个游戏物体集合，用于存放场景的所有物体
    public static Dictionary<string,string> idAndLevels = new Dictionary<string,string >();

    string eleGeo = "";//每一个物体的顶点信息
    string eleTri = "";//每一个物体的三角信息
    string eleNor = "";//每一个物体的法线信息
    string strCol = "";//每一个物体的颜色信息
    string goName = "";//每一个物体的名称信息
    string ID = "";//每一个物体的ID
    public Image loadingImg;//精度条图片
    GameObject myModelCenter;//模型的中心点位置
    public Text sliderText;
    GameObject LoadingSlider;
    GameObject OpenModelButton;
    public Transform CountBtn;
    public Transform RebarBtn;
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        myModelCenter = GameObject.Find("ModelCenter");
        LoadingSlider = GameObject.Find("Loading");
        OpenModelButton = GameObject.Find("OpenModelBtn");
        StartCoroutine(GenerateModel());
        LoadingSlider.SetActive(true);//激活进度条

        //OpenModelButton.SetActive(false);
        //CountBtn = transform.Find("CountBtn");
    }
    //public void OpenXMLFile()//获取本地文件路径
    //{
    //    OpenFileName ofn = new OpenFileName();
    //    ofn.structSize = Marshal.SizeOf(ofn);
    //    ofn.filter = "xml File\0*.xml\0\0";
    //    ofn.file = new string(new char[256]);
    //    ofn.maxFile = ofn.file.Length;
    //    ofn.fileTitle = new string(new char[64]);
    //    ofn.maxFileTitle = ofn.fileTitle.Length;
    //    ofn.initialDir = UnityEngine.Application.dataPath;//默认路径
    //    ofn.title = "请选择XML模型文件！";
    //    ofn.defExt = "Xml";//显示文件的类型  
    //    //注意 一下项目不一定要全选 但是0x00000008项不要缺少
    //    ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR
    //    if (DllTest.GetOpenFileName(ofn))
    //    {
    //        strGeometryPath = ofn.file;
    //        //string temp = strGeometryPath.Substring(0, strGeometryPath.LastIndexOf('\\') + 1);
    //        //Debug.Log(temp);
    //        Debug.Log(strGeometryPath);
    //        StartCoroutine(GenerateModel());
    //        LoadingSlider.SetActive(true);//激活进度条
    //    }
    //}
    IEnumerator GenerateModel()//绘制模型
    {
        
        sliderText.text = "正在读取模型...";
        string myXmlResources = Resources.Load("myRevitMessage").ToString();//读取几何资源
        yield return new WaitForSeconds(0.1f);
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(myXmlResources);
        XmlNodeList nodeList = doc.SelectSingleNode("RevitMessage").ChildNodes;//查找节点集合.

        sliderText.text = "正在生产模型...";
        float f = 0.0f;
        int index = 0;
        int count = nodeList.Count;
        foreach (XmlElement xe in nodeList)//遍历所有物体的几何信息节点集合
        {
            index++;
            f += 1;
            if(index >=count/20)
            {
                loadingImg.fillAmount = f / count;
                yield return new WaitForSeconds(0.1f);                
                index = 0;
            }
            foreach (XmlElement xl in xe.ChildNodes)//遍历单个物体的节点集合
            {
                if (xl.Name == "Geometry")//如果该节点是几何信息则遍历该物体所有的顶点集合
                {
                    foreach (XmlElement xmgo in xl.ChildNodes)
                    {
                        string myElemGeo = xmgo.InnerText;
                        eleGeo += myElemGeo + "\n";
                    }
                }
                if (xl.Name == "triangles")//如果是第二种方式绘制模型，则有三角形信息
                {
                    if (!string.IsNullOrEmpty(xl.InnerText))
                    {
                        foreach (XmlElement tri in xl.ChildNodes)
                        {
                            string triangle = tri.InnerText;
                            eleTri += triangle;
                        }
                    }
                }
                if (xl.Name == "normals")
                {
                    if (!string.IsNullOrEmpty(xl.InnerText))
                    {
                        foreach (XmlElement nor in xl.ChildNodes)
                        {
                            string normals = nor.InnerText;
                            eleNor += normals + "\n";
                        }
                    }
                }
                if (xl.Name == "Color")//颜色信息
                {
                    strCol = xl.InnerText;
                }
                if (xl.Name == "id")//物体的ID
                {
                    ID = xl.InnerText;
                }
                if (xl.Name == "name")//物体的名称（如：墙_215484_[2016-7-12]）
                {
                    goName = xl.InnerText;
                }
            }

            GameObject myGo = Instantiate(go) as GameObject;//实例化一个物体出来
            myGo.name = goName;//设置物体的名称
            myGo.AddComponent<myMesh>();//把脚本作为组件附加给游戏物体
            myMesh mygoMesh = myGo.GetComponent<myMesh>();
           // mygoMesh.DrawModel(eleGeo, eleTri, eleNor, strCol, ID, goName);//调用物体的本身组件的方法，把物体的形状绘制出来
            goList.Add(myGo);//把所有物体添加到一个集合里面，以便以后查询或调用等等。
            myGo.tag = "Player";

            myGo.transform.localEulerAngles = new Vector3(270, 0, 0);
            myGo.transform.parent = myModelCenter.transform;

            //清空以下各个值以便下次使用
            eleGeo = "";
            strCol = "";
            goName = "";
            eleTri = "";
            eleNor = "";

        }
        
        sliderText.text = "模型生成完毕！";
        StartCoroutine(HideLoading());
    }
    //隐藏进度条和打开模型按钮
    IEnumerator HideLoading()
    {
        yield return new WaitForSeconds(1.5f);
        LoadingSlider.SetActive(false);
        OpenModelButton.SetActive(false);
        CountBtn.gameObject.SetActive(true);
        RebarBtn.gameObject.SetActive(true);
    }

}
