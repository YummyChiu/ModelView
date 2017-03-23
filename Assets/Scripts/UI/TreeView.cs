using UnityEngine;
using System.Collections;
using Com.UI;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Com.Logical;
using UnityEngine.UI;
using UnityEngine.Events;

//public delegate void Handler(Dictionary<string, List<string>> resultsList);
public class TreeView : MonoBehaviour
{
    //public event Handler CallBack;

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


        CompoentTreeInit();
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
        List<string> levelList = new List<string>();
        foreach (var item in ReadModelXML.idAndLevels.Values)
        {
            if (!levelList.Contains(item))
                levelList.Add(item);
        }
        //List<string> levelList = new List<string>
        //{
        //  "结构B2","结构B1","结构1","结构2","结构3","结构4","结构5","结构6",
        //  "结构7","结构8","结构9","结构10","结构11","结构12","结构13","结构14",
        //  "结构15","结构16","结构17","结构18","结构RMF","结构RF","结构TF"
        //};

        //UTreeNodeData levelChildNode = new UTreeNodeData(2, "结构B2");
        UTreeNodeData leveltree = new UTreeNodeData(1, "楼梯");
        //levelTree.AddChild(levelChildNode);
        int count = 0;
        foreach (var item in levelList)
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
      ;

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
        foreach (UTreeNodeData node1 in compoentTree.nodeData)
        {
            foreach (UTreeNodeData node2 in node1.Children)
            {
                foreach (UTreeNodeData node3 in node2.Children)
                {
                    foreach (UTreeNodeData node4 in node3.Children)
                    {
                        if (node4.Check)
                        {
                            string codeNum = node4.Title;
                            resultsList.Add(codeNum, new List<string>());
                            resultsList[codeNum].AddRange(_elementList[codeNum]);
                        }
                    }
                }
            }
        }
        Debug.Log(_path);
        //this.transform.gameObject.SetActive(false);
        
        countPanel.gameObject.SetActive(true);
        TableControl tControl = countPanel.GetComponent<TableControl>();

        tControl.test(level, resultsList, _path);
        //触发事件
        //CallBack(resultsList);




    }
}
