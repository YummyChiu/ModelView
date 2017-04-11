using UnityEngine;
using System.Collections;
using Com.UI;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Com.Logical;
using UnityEngine.UI;
using UnityEngine.Events;
using Mono.Data.Sqlite;
using System.IO;


//public delegate void Handler(Dictionary<string, List<string>> resultsList);
public class TreeView : MonoBehaviour
{
    //public event Handler CallBack;

    List<string> floorNum = new List<string>();//楼层的集合
    List<string> fullCode = new List<string>();//12位编码
    List<string> nineCode = new List<string>();//9位编码
    List<string> treeList = new List<string>();//构件分类集合
    Dictionary<string, string> xmlNodePathDic = new Dictionary<string, string>();//根据前9位编码得到xml节点路径
    List<DBCount> dbList = new List<DBCount>();//通过筛选查询到数据库表InventoryDB的集合
    List<CountNum> tableList = new List<CountNum>();//工程量清单集合（最终输出的工程量表单信息）
    XmlDocument xmldoc;

    public UTree levelTree;
    public UTree compoentTree;
    //public static string path = @"E:\翻模\38\";
    public static string path = "38XML/";
    private string _path;
    private List<UTreeNodeData> levelData;
    private List<UTreeNodeData> compoentData;

    private Button floorCheckOnBtn;
    private Button floorCheckOffBtn;
    private Button compoentCheckOnBtn;
    private Button compoentCheckOffBtn;
    private Button ConfirmBtn;
    private Button CanelBtn;

    private Transform levelTreeTr;
    private Transform compoentTreeTr;

    public Transform countPanel;
    public Transform treePanel;
    private Dictionary<string, List<string>> _elementList;

    //singleton
    private static TreeView instance;

    //Construct
    private TreeView() { }
    //Instance
    public static TreeView Instance
    {
        get {
            if (instance == null)
                instance = GameObject.FindObjectOfType(typeof(TreeView)) as TreeView;
            return instance;
        }
    }
    void Start()
    {
        
        levelTreeTr = transform.Find("Panel/LevelTree");
        floorCheckOnBtn = transform.Find("Panel/FloorCommitAll").GetComponent<Button>();
        floorCheckOffBtn = transform.Find("Panel/FloorCanelAll").GetComponent<Button>();

        compoentTreeTr = transform.Find("Panel/CompoentTree");
        compoentCheckOnBtn = transform.Find("Panel/CompoentCommitAll").GetComponent<Button>();
        compoentCheckOffBtn = transform.Find("Panel/CompoentCanelAll").GetComponent<Button>();
        floorCheckOnBtn.onClick.AddListener(delegate () { CheckAll(true, levelTreeTr); });
        floorCheckOffBtn.onClick.AddListener(delegate () { CheckAll(false, levelTreeTr); });
        compoentCheckOnBtn.onClick.AddListener(delegate () { CheckAll(true, compoentTreeTr); });
        compoentCheckOffBtn.onClick.AddListener(delegate () { CheckAll(false, compoentTreeTr); });

        ConfirmBtn = transform.Find("Panel/Confirm").GetComponent<Button>();
        CanelBtn = transform.Find("Panel/Canel").GetComponent<Button>();

        ConfirmBtn.onClick.AddListener(delegate () { Confirm(); });
        CanelBtn.onClick.AddListener(delegate () { ClosePanel(); });
        LevelTreeInit();


       // CompoentTreeInit();
        StartCoroutine(DelayToInvoke.DelayToInvokeDo(() =>
        {
            setdata();
            SetScaleTo1();

        }, 0.2f
        ));
    }
    private void SetScaleTo1()
    {
        foreach (var item in transform.GetComponentsInChildren<RectTransform>())
        {
            item.localScale = Vector3.one;
        }
        
    }

    private void LevelTreeInit()
    {
        compoentData = new List<UTreeNodeData>();
        List<string> levelList = new List<string>();
        SqliteConnection conn = Tools.Instance.SqlConnection();
        SqliteCommand cmd = conn.CreateCommand();
        cmd.CommandText = "Select distinct Layer from ElementDB";
        SqliteDataReader dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                floorNum.Add(dr.GetString(0));//得到所有的层数
            }
        }

        int num = floorNum.Count;
        SqliteConnection conn2 = Tools.Instance.SqlConnection();
        SqliteCommand cmd2 = conn2.CreateCommand();
        cmd2.CommandText = "Select distinct Code from InventoryDB";
        SqliteDataReader dr2 = cmd2.ExecuteReader();
        if (dr2.HasRows)
        {
            while (dr2.Read())
            {
                string temp = dr2.GetString(0);
                fullCode.Add(temp);//得到12位编码
            }
        }

        string tempCode = "";
        foreach (string item in fullCode)
        {
            string code9 = item.Substring(0, 9);
            string it = item;
            if (code9 != tempCode)
            {
                nineCode.Add(code9);//得到9位编码
                tempCode = code9;
            }

        }
        string str2013 = Resources.Load("国标清单2013").ToString();
        xmldoc = new XmlDocument();
        xmldoc.LoadXml(str2013);


        XmlNodeList node = xmldoc.SelectSingleNode("//工程量计算规范").ChildNodes;
        foreach (XmlNode item in node)//item D砌筑工程
        {
            foreach (XmlNode it in item.ChildNodes)//it  D.1砖砌体
            {
                foreach (XmlNode i in it.ChildNodes)// i  砖基础
                {
                    foreach (XmlNode b in i.ChildNodes)
                    {
                        for (int j = 0; j < fullCode.Count; j++)
                        {
                            if (b.InnerText == fullCode[j].Substring(0, 9))
                            {
                                string treeNode = "";

                                treeNode = item.Name + "_" + it.Name + "_" + fullCode[j];
                                string nodePath = "//工程量计算规范//" + item.Name + "//" + it.Name + "//" + i.Name;
                                if (!xmlNodePathDic.ContainsKey(fullCode[j].Substring(0, 9)))
                                {
                                    xmlNodePathDic.Add(fullCode[j].Substring(0, 9), nodePath);//根据9位编码获取Xml节点的路径

                                }


                                if (fullCode[j].Contains(b.InnerText))
                                {
                                    treeList.Add(treeNode);// 存放：  E混凝土及钢筋混凝土工程_E.3现浇混凝土梁_010503002001

                                }
                               
                                //Debug.Log(treeNode);
                             //   break;
                            }
                        }
                    }
                }
            }
        }
        List<CompoentCount> countList = new List<CompoentCount>();
        int z = 0;
        Debug.Log(treeList.Count);
        foreach (string temptree in treeList)
        {
            z++;
            string nodeClass = temptree.Split('_')[0];
            string codeName = temptree.Split('_')[1];
            string codeNum = temptree.Split('_')[2];
            CompoentCount mycount = new CompoentCount()
            {
                ProjectName = "",
                NodeClass = nodeClass,
                CodeName = codeName,
                CodeNum = codeNum
            };
            countList.Add(mycount);
        }
        var datas = countList.GroupBy(c => new { c.NodeClass }).ToList();
        foreach (var dataA in datas)
        {
            z++;
            UTreeNodeData node0 = new UTreeNodeData(z, dataA.First().NodeClass);
            Debug.Log("0" + dataA.First().NodeClass);
            var item1 = dataA.GroupBy(c => new { c.CodeName }).ToList();
            foreach (var item11 in item1)
            {
                z++;
                UTreeNodeData node1 = new UTreeNodeData(z, item11.First().CodeName); //E.3现浇混凝土梁
                Debug.Log("1" + item11.First().CodeName);
                node0.AddChild(node1);
                var item2 = item11.GroupBy(c => new { c.CodeNum }).ToList();//矩形梁
                foreach (var item22 in item2)
                {
                    z++;
                    UTreeNodeData node2 = new UTreeNodeData(z, item22.First().CodeNum);
                    Debug.Log("2" + item22.First().CodeNum);
                    node1.AddChild(node2);

                }
            }

            compoentData.Add(node0);
        }


        //foreach (var item in ReadModelXML.idAndLevels.Values)
        //{
        //    if (!levelList.Contains(item))
        //        levelList.Add(item);
        //}
        //List<string> levelList = new List<string>
        //{
        //  "结构B2","结构B1","结构1","结构2","结构3","结构4","结构5","结构6",
        //  "结构7","结构8","结构9","结构10","结构11","结构12","结构13","结构14",
        //  "结构15","结构16","结构17","结构18","结构RMF","结构RF","结构TF"
        //};

        //UTreeNodeData levelChildNode = new UTreeNodeData(2, "结构B2");
        UTreeNodeData leveltree = new UTreeNodeData(1, "楼层");
        //levelTree.AddChild(levelChildNode);
        int count = 0;
        foreach (var item in floorNum)
        {
            count++;
            UTreeNodeData levelChildNode = new UTreeNodeData(count, item);
            leveltree.AddChild(levelChildNode);
        }
        levelData = new List<UTreeNodeData>();
        levelData.Add(leveltree);

    }
    public void setdata()
    {
        levelTree.SetDataProvider(levelData);
        compoentTree.SetDataProvider(compoentData);
    }
    private void CompoentTreeInit()
    {
        compoentData = new List<UTreeNodeData>();
        path += "38#结构模型算量信息";
        this._path = path;
        //ReadComponentXml();
        XmlDocument xml = new XmlDocument();
        //xml.Load(path);
        string s1 = Resources.Load(path).ToString();
        xml.LoadXml(s1);
        List<string> elementTypes = new List<string>();
        List<CompoentCount> countList = new List<CompoentCount>();
        //List<CompoentCount> afterList = new List<CompoentCount>();
        Dictionary<string, List<string>> elementsList = new Dictionary<string, List<string>>();
        XmlNode mainNode = xml.SelectSingleNode("算量信息");
        var nodes = mainNode.ChildNodes;
        foreach (XmlNode childNode in nodes)
        {
            string elementId = childNode.Name;
            XmlNode name = childNode.SelectSingleNode("项目名称");
            XmlNode code = childNode.SelectSingleNode("项目编码");
            string codeText = code.InnerText;
            string nameText = name.InnerText;

            if (elementsList.ContainsKey(codeText))
            {
                elementsList[codeText].Add(elementId);
            }
            else
            {
                elementsList.Add(codeText, new List<string>());
                elementsList[codeText].Add(elementId);
                elementTypes.Add(nameText + " " + codeText);
            }
        }
        this._elementList = elementsList;
        elementTypes.Sort();
        //编码规范

        XmlDocument xmlRule = new XmlDocument();
        path = path.Replace("算量信息", "清单规则");
        // xmlRule.Load(path);
        string s2 = Resources.Load(path).ToString();
        xmlRule.LoadXml(s2);
        //List<string> projectNameList = new List<string>();
        //List<string> nodeClassList = new List<string>();
        //List<string> codeNameList = new List<string>();
        //List<string> codeNumList = new List<string>();
        //int i = 0; int j = 0; int k = 0; int l = 0;

        int i = 0;
        foreach (string elementType in elementTypes)
        {
            i++;
            string temp = GetCountData(elementType.Split(' ')[1], path);
            string projectName = temp.Split('$')[0];
            string nodeClass = temp.Split('$')[1];
            string codeName = elementType.Split(' ')[1];
            string codeNum = elementType.Split(' ')[2];
            CompoentCount mycount = new CompoentCount()
            {
                ProjectName = projectName,
                NodeClass = nodeClass,
                CodeName = codeName,
                CodeNum = codeNum
            };
            countList.Add(mycount);

        }
      

        var datas = countList.GroupBy(c => new { c.ProjectName }).ToList();
        foreach (var dataA in datas)
        {
            i++;
            UTreeNodeData node0 = new UTreeNodeData(i, dataA.First().ProjectName);
            var item1 = dataA.GroupBy(c => new { c.NodeClass }).ToList();

            foreach (var item11 in item1)
            {
                i++;
                UTreeNodeData node1 = new UTreeNodeData(i, item11.First().NodeClass);
                node0.AddChild(node1);
                var item2 = item11.GroupBy(c => new { c.CodeName }).ToList();
                foreach (var item22 in item2)
                {
                    i++;
                    UTreeNodeData node2 = new UTreeNodeData(i, item22.First().CodeName);
                    node1.AddChild(node2);
                    var item3 = item22.GroupBy(c => new { c.CodeNum }).ToList();

                    foreach (var item33 in item3)
                    {
                        i++;
                        UTreeNodeData node3 = new UTreeNodeData(i, item33.First().CodeNum);
                        node2.AddChild(node3);


                    }

                }
            }
            compoentData.Add(node0);
        }

        //compoentTree.SetDataProvider(compoentData);
    }
    private string GetCountData(string temp, string path)
    {
        XmlDocument xml = new XmlDocument();
      //  xml.Load(path);
        string s2 = Resources.Load(path).ToString();
        xml.LoadXml(s2);
        XmlNode childNode = xml.SelectSingleNode("工程量计算规范");

        foreach (XmlNode xmlNode in childNode.ChildNodes)
        {
            foreach (XmlNode xmlElement in xmlNode.ChildNodes)
            {
                foreach (XmlNode childElem in xmlElement.ChildNodes)
                {
                    if (childElem.Name == temp)
                    {
                        return xmlNode.Name + "$" + xmlElement.Name;
                    }
                }
            }
        }
        return "";
    }

    private void CheckAll(bool state, Transform tr)
    {
        foreach (var item in tr.GetComponentsInChildren<Toggle>())
        {
            //Debug.Log("HelloAgain!");
            item.isOn = state;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void ClosePanel()
    {
        transform.gameObject.SetActive(false);
    }
    private void Confirm()
    {
        tableList.Clear();
        List<string> level = new List<string>();

        foreach (var item in levelTree.nodeData[0].Children)
        {
            if (item.Check)
            {
                level.Add(item.Title);
                //Debug.Log(item.Title);
            }
        }


        Dictionary<string, List<string>> resultsList = new Dictionary<string, List<string>>();
        List<string> result = new List<string>();
        foreach (UTreeNodeData node1 in compoentTree.nodeData)
        {
            foreach (UTreeNodeData node2 in node1.Children)
            {
                foreach (UTreeNodeData node3 in node2.Children)
                {
                    if (node3.Check)
                    {
                        string codeNum = node3.Title;
                        result.Add(codeNum);
                    }
                    //foreach (UTreeNodeData node4 in node3.Children)
                    //{
                    //    if (node4.Check)
                    //    {
                    //        string codeNum = node4.Title;
                    //        resultsList.Add(codeNum, new List<string>());
                    //        resultsList[codeNum].AddRange(_elementList[codeNum]);
                    //    }
                    //}
                }
            }
        }

        Debug.Log(_path);

        Debug.Log(level.Count);
        Debug.Log(result);
        //this.transform.gameObject.SetActive(false);
        
        countPanel.gameObject.SetActive(true);
        SelectFloorNum(level, result);
        TableControl tControl = countPanel.GetComponent<TableControl>();
        tControl.Load(tableList);
       // tControl.test(level, resultsList, _path);
        //触发事件
        //CallBack(resultsList);




    }
    /// <summary>
    /// floorNum为勾选的楼层集合，code为勾选的编码集合
    /// </summary>
    /// <param name="floorNum"></param>
    /// <param name="code"></param>
    /// <returns></returns>
    List<int> SelectFloorNum(List<string> floorNum, List<string> code)
    {

        string strFloor = "";
        for (int i = 0; i < floorNum.Count; i++)
        {
            if (floorNum.Count == 0)
            {
                break;
            }
            if (floorNum.Count == 1)
            {
                strFloor ="'"+ floorNum[i]+"'";
                break;
            }
            if (i == floorNum.Count - 1)
            {
                strFloor = strFloor + "'" + floorNum[i] + "'";
                break;
            }
            else
            {
                strFloor = strFloor + "'" + floorNum[i] + "',";
            }
        }
        string strCode = "";
        for (int j = 0; j < code.Count; j++)
        {
            if (code.Count == 0)
            {
                break;
            }
            if (code.Count == 1)
            {
                strCode = "'"+code[j]+"'";
                break;
            }
            if (j == code.Count - 1)
            {
                strCode = strCode + "'" + code[j] + "'";
                break;
            }
            else
            {
                strCode = strCode + "'" + code[j] + "',";
            }

        }
        List<int> elementId = new List<int>();
        SqliteConnection conn2 = Tools.Instance.SqlConnection();
        SqliteCommand cmd2 = conn2.CreateCommand();
        cmd2.CommandText = "select * from InventoryDB where Code in(" + strCode + ") and ID in( Select ID  from ElementDB where Layer In(" + strFloor + "))";
        // Debug.Log(cmd2.CommandText);
        SqliteDataReader dr2 = cmd2.ExecuteReader();
        dbList.Clear();
        //xmlNodePathDic.Clear();
        if (dr2.HasRows)
        {
            while (dr2.Read())
            {

                elementId.Add(dr2.GetInt32(0));
                DBCount dbc = new DBCount();
                dbc.Code = dr2.GetString(1);
                dbc.Detail = dr2.GetString(2);
                dbc.Quantity = dr2.GetDouble(3);
                dbList.Add(dbc);
            }
        }
        List<CountNum> cnList = new List<CountNum>();


        var groupDB = dbList.GroupBy(g => g.Code);
        foreach (var item in groupDB)
        {

            string codestr = item.First().Code;
            string projectName = xmldoc.SelectSingleNode(xmlNodePathDic[item.First().Code.Substring(0, 9)]).Name;
            XmlNodeList nodeList = xmldoc.SelectSingleNode(xmlNodePathDic[item.First().Code.Substring(0, 9)]).ChildNodes;

            string myDetail = "";
            string detail = item.First().Detail.Replace("'", "");
            string[] detailArray = detail.Split('$');
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == "项目特征")
                {
                    for (int i = 0; i < node.ChildNodes.Count; i++)
                    {
                        int index = node.ChildNodes[i].InnerText.IndexOf('[');
                        if (index != -1)
                        {
                            myDetail = node.ChildNodes[i].InnerText.Substring(0, index) + detailArray[i];
                            // Debug.Log("得到的项目特征：" + myDetail);
                        }
                    }
                }
            }
            string strUnit = xmldoc.SelectSingleNode(xmlNodePathDic[item.First().Code.Substring(0, 9)] + "//计量单位").InnerText;
            double total = item.Sum(s => s.Quantity);



            //添加数据到表
            CountNum cn = new CountNum();
            cn.ProjectNum = codestr;
            cn.ProjectName = projectName;
            cn.ProjectFeature = myDetail;
            cn.ProjectUnit = strUnit;
            cn.ProjectQuantities = total.ToString("0.00");
            tableList.Add(cn);
        }
        //int tableNum = tableList.Count;
        return elementId;
    }

}
