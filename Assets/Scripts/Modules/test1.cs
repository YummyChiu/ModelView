//using UnityEngine;
//using System.Collections;
//using System.Xml;
//using System.Collections.Generic;
//using Com.UI;
//using Com.Logical;
//using System.Linq;

//public class test1 : MonoBehaviour {
//    public UTree tree1;
//    public static string path = @"E:\翻模\38\";
//    private List<UTreeNodeData> dataList = new List<UTreeNodeData>();
//    void Start()
//    {
       
//        path += "38#结构模型算量信息.xml";
//        //ReadComponentXml();
//        XmlDocument xml = new XmlDocument();
//        xml.Load(path);
//        List<string> elementTypes = new List<string>();
//        List<CompoentCount> countList = new List<CompoentCount>();
//        List<CompoentCount> afterList = new List<CompoentCount>();
//        Dictionary<string, List<string>> elementsList = new Dictionary<string, List<string>>();
//        XmlNode mainNode = xml.SelectSingleNode("算量信息");
//        var nodes = mainNode.ChildNodes;
//        foreach (XmlNode childNode in nodes)
//        {
//            string elementId = childNode.Name;
//            XmlNode name = childNode.SelectSingleNode("项目名称");
//            XmlNode code = childNode.SelectSingleNode("项目编码");
//            string codeText = code.InnerText;
//            string nameText = name.InnerText;

//            if (elementsList.ContainsKey(codeText))
//            {
//                elementsList[codeText].Add(elementId);
//            }
//            else
//            {
//                elementsList.Add(codeText, new List<string>());
//                elementsList[codeText].Add(elementId);
//                elementTypes.Add(nameText + " " + codeText);
//            }
//        }
//        elementTypes.Sort();
//        //编码规范

//        XmlDocument xmlRule = new XmlDocument();
//        path = path.Replace("算量信息", "清单规则");
//        xmlRule.Load(path);

//        //List<string> projectNameList = new List<string>();
//        //List<string> nodeClassList = new List<string>();
//        //List<string> codeNameList = new List<string>();
//        //List<string> codeNumList = new List<string>();
//        //int i = 0; int j = 0; int k = 0; int l = 0;

//        int i = 0;
//        foreach (string elementType in elementTypes)
//        {
//            i++;
//            string temp = GetCountData(elementType.Split(' ')[1], path);
//            string projectName = temp.Split('$')[0];
//            string nodeClass = temp.Split('$')[1];
//            string codeName = elementType.Split(' ')[1];
//            string codeNum = elementType.Split(' ')[2];
//            CompoentCount mycount = new CompoentCount()
//            {
//                ProjectName = projectName,
//                NodeClass = nodeClass,
//                CodeName = codeName,
//                CodeNum = codeNum
//            };
//            countList.Add(mycount);

//            //Debug.Log(projectName + "_______" + nodeClass + "_______" + codeName + "_______" + codeNum);

//            //UTreeNodeData node0 = null;
//            //if (node0 == null)
//            //{
//            //    node0 = new UTreeNodeData(i, projectName);

//            //}
//            //UTreeNodeData node1 = null;
//            //if (node1 == null)
//            //{
//            //    node0 = new UTreeNodeData(i, nodeClass);
//            //}
//            //UTreeNodeData node2 = null;
//            //UTreeNodeData node3 = null;
//            //if (node2 == null)
//            //{
//            //    node2 = new UTreeNodeData(i, codeName);
//            //}
//            //if (node3 == null)
//            //{
//            //    node3 = new UTreeNodeData(i, codeNum);
//            //}
//            //data.Add(node0);
//            //if (!projectNameList.Contains(projectName))
//            //{          
//            //    projectNameList.Add(projectName);
//            //}
//            //if (!nodeClassList.Contains(nodeClass))
//            //{ 
//            //    nodeClassList.Add(nodeClass);
//            //}
//            //if (!codeNameList.Contains(codeName))
//            //{   
//            //    codeNameList.Add(codeName);
//            //}
//            //if (!codeNumList.Contains(codeNum))
//            //{            
//            //    codeNumList.Add(codeNum);
//            //}
//        }
//        //dataList = new List<UTreeNodeData>();

//        var datas = countList.GroupBy(c => new { c.ProjectName }).ToList();
//        foreach (var dataA in datas)
//        {
//            i++;
//            UTreeNodeData node0 = new UTreeNodeData(i,dataA.First().ProjectName);
//            var item1 = dataA.GroupBy(c => new { c.NodeClass }).ToList();

//            foreach (var item11 in item1)
//            {
//                i++;
//                UTreeNodeData node1 = new UTreeNodeData(i,item11.First().NodeClass);
//                node0.AddChild(node1);
//                var item2 = item11.GroupBy(c => new { c.CodeName }).ToList();
//                foreach (var item22 in item2)
//                {
//                    i++;
//                    UTreeNodeData node2 = new UTreeNodeData(i,item22.First().CodeName);
//                    node1.AddChild(node2);
//                    var item3 = item22.GroupBy(c=>new { c.CodeNum}).ToList();

//                    foreach (var item33 in item3)
//                    {
//                        i++;
//                        UTreeNodeData node3 = new UTreeNodeData(i, item33.First().CodeNum);
//                        node2.AddChild(node3);

//                        //CompoentCount count = new CompoentCount()
//                        //{
//                        //    ProjectName = item33.First().ProjectName,
//                        //    NodeClass = item33.First().NodeClass,
//                        //    CodeName = item33.First().CodeName,
//                        //    CodeNum = item33.First().CodeNum

//                        //};
//                        //afterList.Add(count);
//                    }

//                }
//            }
//            dataList.Add(node0);  
//        }

//        //foreach (var item in projectNameList)
//        //{
//        //    i++;
//        //    UTreeNodeData projectNameData = new UTreeNodeData(i,item);
//        //    foreach (var item1 in nodeClassList)
//        //    {
//        //        j++;
//        //        UTreeNodeData nodeClassData = new UTreeNodeData(j,item1);
//        //        projectNameData.AddChild(nodeClassData);
//        //        foreach (var item2 in codeNameList)
//        //        {
//        //            k++;
//        //            UTreeNodeData codeNameData = new UTreeNodeData(k, item2);
//        //            nodeClassData.AddChild(codeNameData);
//        //            foreach (var item3 in codeNumList)
//        //            {
//        //                l++;
//        //                UTreeNodeData codeNumData = new UTreeNodeData(l, item3);
//        //                codeNameData.AddChild(codeNumData);
//        //            }
//        //        }
//        //    }
//        //    data.Add(projectNameData);
//        //}
//        tree1.SetDataProvider(dataList);
//    }

//    private string GetCountData(string temp, string path)
//    {
//        XmlDocument xml = new XmlDocument();
//        xml.Load(path);
//        XmlNode childNode = xml.SelectSingleNode("工程量计算规范");

//        foreach (XmlNode xmlNode in childNode.ChildNodes)
//        {
//            foreach (XmlNode xmlElement in xmlNode.ChildNodes)
//            {
//                foreach (XmlNode childElem in xmlElement.ChildNodes)
//                {
//                    if (childElem.Name == temp)
//                    {
//                        return xmlNode.Name + "$" + xmlElement.Name;
//                    }
//                }
//            }
//        }
//        return "";
//    }
//    // Update is called once per frame
//    void Update () {
	
//	}
//}
