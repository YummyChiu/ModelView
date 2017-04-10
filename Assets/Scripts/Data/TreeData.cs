using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.UI;
using Mono.Data.Sqlite;
using System.Xml;
using Com.Logical;
using System.Linq;

public class TreeData : Singleton<TreeData> {

    public int score;

    XmlDocument xmldoc;
    Dictionary<string, string> xmlNodePathDic = new Dictionary<string, string>();//根据前9位编码得到xml节点路径
    List<string> floorList;
    List<UTreeNodeData> levelNodeData;
    List<UTreeNodeData> comNodeData;
    List<DBCount> dbList;//通过筛选查询到数据库表InventoryDB的集合
    List<CountNum> tableList ;//工程量清单集合（最终输出的工程量表单信息）
    void Awake()
    {

    }

    public  List<UTreeNodeData> GetLevelData()
    {
        floorList = new List<string>();
        #region 查询数据库
        SqliteConnection conn = Tools.Instance.SqlConnection();
        SqliteCommand cmd = conn.CreateCommand();
        cmd.CommandText = "Select distinct Layer from ElementDB";
        SqliteDataReader dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                floorList.Add(dr.GetString(0));//得到所有的层数
            }
        }
        #endregion
        UTreeNodeData leveltree = new UTreeNodeData(1, "楼层");
        int count = 0;
        foreach (var item in floorList)
        {
            count++;
            UTreeNodeData levelChildNode = new UTreeNodeData(count, item);
            leveltree.AddChild(levelChildNode);
        }
        levelNodeData = new List<UTreeNodeData>();
        levelNodeData.Add(leveltree);

        return levelNodeData;
    }

    public List<UTreeNodeData> GetComData()
    {
        List<string> fullCode = new List<string>();//12位编码
        List<string> nineCode = new List<string>();//9位编码
       
        List<string> treeList = new List<string>();//构件分类集合

        //Dictionary<string, string> xmlNodePathDic = new Dictionary<string, string>();//根据前9位编码得到xml节点路径

        comNodeData = new List<UTreeNodeData>();

        #region 查询数据库和xml清单规则得到一个list<string>  E混凝土及钢筋混凝土工程_E.3现浇混凝土梁_010503002001
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
        #endregion

        #region
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

            comNodeData.Add(node0);
        }

        #endregion

        return comNodeData;
    }


    public List<CountNum> GetCountTable(List<string> levels,List<string> componentsCode)
    {
        tableList = new List<CountNum>();
        dbList = new List<DBCount>();
        string strFloor = "";
        for (int i = 0; i < levels.Count; i++)
        {
            if (levels.Count == 0)
            {
                break;
            }
            if (levels.Count == 1)
            {
                strFloor = "'" + levels[i] + "'";
                break;
            }
            if (i == levels.Count - 1)
            {
                strFloor = strFloor + "'" + levels[i] + "'";
                break;
            }
            else
            {
                strFloor = strFloor + "'" + levels[i] + "',";
            }
        }
        string strCode = "";
        for (int j = 0; j < componentsCode.Count; j++)
        {
            if (componentsCode.Count == 0)
            {
                break;
            }
            if (componentsCode.Count == 1)
            {
                strCode = "'" + componentsCode[j] + "'";
                break;
            }
            if (j == componentsCode.Count - 1)
            {
                strCode = strCode + "'" + componentsCode[j] + "'";
                break;
            }
            else
            {
                strCode = strCode + "'" + componentsCode[j] + "',";
            }

        }

        SqliteConnection conn2 = Tools.Instance.SqlConnection();
        SqliteCommand cmd2 = conn2.CreateCommand();
        cmd2.CommandText = "select * from InventoryDB where Code in(" + strCode + ") and ID in( Select ID  from ElementDB where Layer In(" + strFloor + "))";
        // Debug.Log(cmd2.CommandText);
        SqliteDataReader dr2 = cmd2.ExecuteReader();
        
        //xmlNodePathDic.Clear();
        if (dr2.HasRows)
        {
            while (dr2.Read())
            {   
                DBCount dbc = new DBCount();
                dbc.Code = dr2.GetString(1);
                dbc.Detail = dr2.GetString(2);
                dbc.Quantity = dr2.GetDouble(3);
                dbList.Add(dbc);
            }
        }

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
                        {if(i!= node.ChildNodes.Count-1)
                            myDetail += node.ChildNodes[i].InnerText.Substring(0, index) + detailArray[i] + "\n";
                         else
                                myDetail += node.ChildNodes[i].InnerText.Substring(0, index) + detailArray[i];

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
        return tableList;
    }

   

}
