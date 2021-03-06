﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System;

public class GetRebarInfo {

    //public static string defaultPath = @"E:\UnityProjects\20161118\Messages\";

    public static List<MyRebar> GetRebar()
    {
        int start = System.Environment.TickCount;
        Debug.Log("start:" + start);
        List<MyRebar> myRebars = new List<MyRebar>();
        //List<MyRebar> AfterAddCardNumList = new List<MyRebar>() { };
        int end = 0;
        string temp = ReadModelXML._instance.strGeometryPath;
       // string path = defaultPath  +"模型.xml";
        string path = temp.Substring(0, temp.LastIndexOf('\\') + 1) + "模型.xml";

        //bool isFileExisted = File.Exists(defaultPath + "；模型.xml");
        bool isFileExisted = File.Exists(path);

        if (isFileExisted)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNodeList nodeList = xmlDoc.SelectSingleNode("MyRebars").ChildNodes;
            foreach (XmlNode childNode in nodeList)
            {
                MyRebar myRebar = new MyRebar();
                XmlElement childElement = (XmlElement)childNode;
                foreach (var rebarItems in childElement.ChildNodes)
                {
                    XmlElement rebarItem = (XmlElement)rebarItems;
                    switch (rebarItem.Name)
                    {
                        case "Name":
                            myRebar.Name = rebarItem.InnerText;
                            break;
                        case "Shape":
                            myRebar.Shape = rebarItem.InnerText;
                            break;
                        case "Length":
                            myRebar.Length = Convert.ToDouble(rebarItem.InnerText);
                            break;
                        case "Quantity":
                            myRebar.Quantity = Convert.ToInt32(rebarItem.InnerText);
                            break;
                        case "CalculatingFormula":
                            myRebar.CalculatingFormula = rebarItem.InnerText;
                            break;
                        case "Param_A":
                            myRebar.Param_A = rebarItem.InnerText;
                            break;
                        case "Param_B":
                            myRebar.Param_B = rebarItem.InnerText;
                            break;
                        case "Param_C":
                            myRebar.Param_C = rebarItem.InnerText;
                            break;
                        case "CardNum":
                            myRebar.CardNum = rebarItem.InnerText;
                            break;
                        case "Host":
                            RebarHost rebarHost = new RebarHost();
                            XmlNodeList childList = rebarItem.ChildNodes;
                            foreach (var item in childList)
                            {
                                XmlElement element = (XmlElement)item;
                                switch (element.Name)
                                {
                                    case "HostId":
                                        rebarHost.HostId = element.InnerText;
                                        break;
                                    case "HostName":
                                        rebarHost.HostName = element.InnerText;
                                        break;
                                    case "Level":
                                        rebarHost.Level = element.InnerText;
                                        break;
                                    case "HostCode":
                                        rebarHost.HostCode = element.InnerText;
                                        break;
                                    case "RebarSet":
                                        XmlNodeList rebarSets = element.ChildNodes;
                                        foreach (var rebarSet in rebarSets)
                                        {
                                            XmlElement rebarSetElem = (XmlElement)rebarSet;
                                            rebarHost.RebarSet.Add(rebarSetElem.Name, rebarSetElem.InnerText);
                                        }
                                        break;
                                }

                            }
                            myRebar.Host = rebarHost;
                            break;
                    }
                }
                myRebars.Add(myRebar);

            }

            //return myRebars;
            //AfterAddCardNumList = myRebars;
        }

        end = System.Environment.TickCount;
        Debug.Log("读取结束：" + end);
        Debug.Log("消耗的时间：" + (end - start) + "毫秒");
        //// return myRebars;
        return myRebars;
    }
}
