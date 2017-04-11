using UnityEngine;
using System.Collections.Generic;
using SLS.Widgets.Table;
using System.Xml;
using System.Linq;
using System.Diagnostics;
public class TableControl : MonoBehaviour
{

    private Table table;
    public Dictionary<string, List<string>> resultsList;
    private string path;
    private List<string> level;
    void Start()
    {
        //TreeView.CallBack += test;
        //table = transform.GetComponentInChildren<Table>();
        UnityEngine.Debug.Log("start");
        UnityEngine.Debug.Log(path);

    }

    public void test(List<string> level, Dictionary<string, List<string>> resultsList, string path)
    {
        table = transform.GetComponentInChildren<Table>();

        this.level = level;
        this.path = path;
        this.resultsList = resultsList;
        LoadDatas();
    }


    public void Load(List<CountNum> list)
    {
        table = transform.GetComponentInChildren<Table>();

        table.ResetTable();
        table.AddTextColumn("序号");
        table.AddTextColumn("项目编码");
        table.AddTextColumn("项目名称");
        table.AddTextColumn("项目特征");
        table.AddTextColumn("计量单位");
        table.AddTextColumn("工程量");
        table.Initialize();
        int i = 0;
        foreach (var item in list)
        {
            i++;
            Datum d = Datum.Body(i.ToString());
            d.elements.Add(i.ToString());//序号
            d.elements.Add(item.ProjectNum);//项目编码
            d.elements.Add(item.ProjectName);//项目名称
            d.elements.Add(item.ProjectFeature);//项目特征
            d.elements.Add(item.ProjectUnit);//计量单位
            d.elements.Add(item.ProjectQuantities);//工程量
            table.data.Add(d);
        }

        table.StartRenderEngine();


    }
    public void LoadDatas()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        #region 

        UnityEngine.Debug.Log(path);
        UnityEngine.Debug.Log(level);
        UnityEngine.Debug.Log(table.name);
        table.ResetTable();
        table.AddTextColumn("序号");
        table.AddTextColumn("项目编码");
        table.AddTextColumn("项目名称");
        table.AddTextColumn("项目特征");
        table.AddTextColumn("计量单位");
        table.AddTextColumn("工程量");
        table.Initialize();

        XmlDocument xml = new XmlDocument();
        //xml.Load(path);
        string s1 = Resources.Load(path).ToString();
        xml.LoadXml(s1);
        XmlNode mainNode = xml.SelectSingleNode("算量信息");

        int i = 0;
        #region 
        int start = System.Environment.TickCount;
        UnityEngine.Debug.Log("开始：" + start);
        for (int j = 0; j < resultsList.Keys.Count; j++)
        {
            i++;
            string codeNum = resultsList.Keys.ElementAt(j);
            string elementId = resultsList[codeNum][0];
            XmlNode elemId = mainNode.SelectSingleNode(elementId);
            XmlNode unitNode = elemId.SelectSingleNode("单位");
            XmlNode textNode = elemId.SelectSingleNode("项目特征");
            XmlNode nameNode = elemId.SelectSingleNode("项目名称");
            string unit = unitNode.InnerText;
            string text = textNode.InnerText;
            string name = nameNode.InnerText.Split(' ')[1];
            double result = 0;

            for (int k = 0; k < resultsList[codeNum].Count; k++)
            {
                string id = resultsList[codeNum][k];
                //如果所选的楼层包含该ID的level就计入工程量
                if (!ReadModelXML.idAndLevels.ContainsKey(id))
                {
                    UnityEngine.Debug.Log("id:" + id + "______" + "codingnum :" + codeNum + "_____");
                }
                else
                {
                    if (level.Contains(ReadModelXML.idAndLevels[id]))
                    {
                        XmlNode countNode = mainNode.SelectSingleNode(id).SelectSingleNode("工程量");
                        result += double.Parse(countNode.InnerText);
                    }
                }
            
                //foreach (GameObject go in ReadModelXML.goList)
                //{
                //    string[] goName = go.name.Split('_');
                //    if (id == goName[1])
                //    {
                //        switch (goName[0])
                //        {
                //            case "墙":
                //                Walls wall = go.GetComponent<Walls>();
                //                if (level.Contains(wall.FLOORSNUM))
                //                {
                //                    XmlNode countNode = mainNode.SelectSingleNode(id).SelectSingleNode("工程量");
                //                    //result = 1;
                //                    result += double.Parse(countNode.InnerText);
                //                }
                //                break;
                //            case "结构框架":
                //                Frameworks frameworks = go.GetComponent<Frameworks>();
                //                if (level.Contains(frameworks.FLOORSNUM))
                //                {
                //                    XmlNode countNode = mainNode.SelectSingleNode(id).SelectSingleNode("工程量");
                //                    //result = 1;
                //                    result += double.Parse(countNode.InnerText);
                //                }
                //                break;
                //            case "楼板":
                //                Floors floors = go.GetComponent<Floors>();
                //                if (level.Contains(floors.FLOORSNUM))
                //                {
                //                    XmlNode countNode = mainNode.SelectSingleNode(id).SelectSingleNode("工程量");
                //                    //result = 1;
                //                    result += double.Parse(countNode.InnerText);
                //                }
                //                break;
                //            default:
                //                break;
                //        }
                //    }

            //}
                /**************************************************************************************/
                //XmlNode countNode = mainNode.SelectSingleNode(id).SelectSingleNode("工程量");
                //result += double.Parse(countNode.InnerText);
                /**************************************************************************************/
            }
            if (result == 0) continue;
                Datum d = Datum.Body(i.ToString());
                d.elements.Add(i.ToString());//序号
                d.elements.Add(codeNum);//项目编码
                d.elements.Add(name);//项目名称
                d.elements.Add(text);//项目特征
                d.elements.Add(unit);//计量单位
                d.elements.Add(result.ToString("0.00"));//工程量
                table.data.Add(d);
            
         
            #endregion  
            #endregion
            #region 无筛选条件 显示表格
            //table.ResetTable();
            //table.AddTextColumn();
            //table.AddTextColumn();
            //table.AddTextColumn();
            //table.AddTextColumn();
            //table.AddTextColumn();
            //table.AddTextColumn();
            //// Initialize Your Table
            //table.Initialize();
            //List<CountNum> cn = new List<CountNum>();
            //XmlDocument xml = new XmlDocument();
            //xml.Load(path);
            //XmlNodeList xnl = xml.SelectSingleNode("算量信息").ChildNodes;
            //foreach (XmlNode item in xnl)
            //{
            //    CountNum c = new CountNum();
            //    foreach (XmlNode i in item)
            //    {
            //        if (i.Name == "项目名称")
            //        {
            //            c.ProjectName = i.InnerText;
            //        }
            //        if (i.Name == "项目编码")
            //        {
            //            c.ProjectNum = i.InnerText;
            //        }
            //        if (i.Name == "项目特征")
            //        {
            //            c.ProjectFeature = i.InnerText;
            //        }
            //        if (i.Name == "工程量")
            //        {
            //            c.ProjectQuantities = i.InnerText;
            //        }
            //        if (i.Name == "单位")
            //        {
            //            c.ProjectUnit = i.InnerText;
            //        }
            //    }
            //    cn.Add(c);
            //}
            //var cList = cn.OrderBy(a => a.ProjectNum).GroupBy(b => new { b.ProjectNum }).ToList();
            //Datum d = Datum.Body(0.ToString());
            //d.elements.Add("序号");
            //d.elements.Add("项目编码");
            //d.elements.Add("项目名称");
            //d.elements.Add("项目特征");
            //d.elements.Add("计量单位");
            //d.elements.Add("工程量");
            //table.data.Add(d);

            //Datum d1 = Datum.Body(1.ToString());
            //d1.elements.Add("混凝土及钢筋混凝土工程");
            //d1.elements.Add("");
            //d1.elements.Add("");
            //d1.elements.Add("");
            //d1.elements.Add("");
            //d1.elements.Add("");
            //table.data.Add(d1);
            //int i1 = 2;//在第2行遍历数据
            //int m = 0;//序号
            //foreach (var item in cList)
            //{
            //    m++;
            //    Datum dm = Datum.Body(i1.ToString());
            //    dm.elements.Add(m.ToString());
            //    dm.elements.Add(item.First().ProjectNum.ToString());
            //    dm.elements.Add(item.First().ProjectName.ToString());
            //    dm.elements.Add(item.First().ProjectFeature.ToString());
            //    dm.elements.Add(item.First().ProjectUnit.ToString());
            //    dm.elements.Add(item.Sum(a => double.Parse(a.ProjectQuantities)).ToString("0.00"));
            //    table.data.Add(dm);
            //    i1++;
            //}
            //table.StartRenderEngine();
            #endregion
          
        }
        sw.Stop();
        UnityEngine.Debug.Log(sw.Elapsed.TotalSeconds);
        int end = System.Environment.TickCount;
        UnityEngine.Debug.Log("结束：" + start);
        UnityEngine.Debug.Log("消耗的时间：" + (end - start) + "毫秒");
        table.StartRenderEngine();
    }


   
}
public class DBCount
{
    public string Code { get; set; }
    public string ProjectName { get; set; }
    public string Detail { get; set; }
    public string Unit { get; set; }
    public double Quantity { get; set; }
}

public class CountNum
{
    public string ProjectFeature { get; internal set; }
    public string ProjectName
    {
        get; set;
    }
    public string ProjectNum
    {
        get; set;
    }
    public string ProjectQuantities { get; internal set; }
    public string ProjectUnit { get; internal set; }
}


