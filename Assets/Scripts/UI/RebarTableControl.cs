using UnityEngine;
using System.Collections.Generic;
using SLS.Widgets.Table;
using System.Xml;
using System.Linq;
using System.Diagnostics;
using UnityEngine.UI;
using System;

public class RebarTableControl : MonoBehaviour
{
    public Transform fixedlengthSetting;
    private Table table;
    private List<MyRebar> _myRebars;
    private Button RebarSummaryBtn;
    private Button RebarCompoentBtn;
    private Button RebarCardBtn;
    private Button RebarOptimizeBtn;

    public static List<SameLength> mySameLenght = new List<SameLength>();
    //
    public delegate void ClickAction();
    public static event ClickAction OnClicked;

    //
    private static RebarTableControl instance;
    List<List<MyCutRebar>> tables = new List<List<MyCutRebar>>();
    bool FirstClick = true;

    private RebarTableControl()
    { }
    public static RebarTableControl Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType(typeof(RebarTableControl)) as RebarTableControl;
            return instance;

        }
    }
    void Start()
    {
        RebarSummaryBtn = transform.Find("Panel/Left/RebarSummaryBtn").GetComponent<Button>();
        RebarCompoentBtn = transform.Find("Panel/Left/RebarCompoentBtn").GetComponent<Button>();
        RebarCardBtn = transform.Find("Panel/Left/RebarCardBtn").GetComponent<Button>();
        RebarOptimizeBtn = transform.Find("Panel/Left/RebarOptimizeBtn").GetComponent<Button>();

        RebarSummaryBtn.onClick.AddListener(RebarSummaryTable);
        RebarCompoentBtn.onClick.AddListener(RebarCompoentTable);
        RebarCardBtn.onClick.AddListener(RebarCardTable);
        RebarOptimizeBtn.onClick.AddListener(RebarOptimizeTable);
    }
    private void SetButtonNormal()
    {
        RebarSummaryBtn.GetComponent<Image>().enabled = false;
        RebarCompoentBtn.GetComponent<Image>().enabled = false;
        RebarCardBtn.GetComponent<Image>().enabled = false;
        RebarOptimizeBtn.GetComponent<Image>().enabled = false;
    }
    public void RebarSummaryTable()
    {
        SetButtonNormal();
           table = transform.GetComponentInChildren<Table>();
        table.ResetTable();
        table.AddTextColumn("序号").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("料牌编号").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("钢筋规格").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("钢筋形状").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("断料长度（mm）").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("总数（根）").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("总重（kg）").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("备注").horAlignment = Column.HorAlignment.CENTER;

        table.Initialize();

        var datas = _myRebars.OrderBy(a => int.Parse(a.Name.Split(' ')[0])).GroupBy(myRebar => new { myRebar.Name });
        int i = 0;
        int j = 0;
        foreach (var data in datas)
        {
            i++;
            var items = data.GroupBy(myRebar => new { myRebar.Shape, myRebar.Length });
            Datum mergeName = Datum.Body(i.ToString());
            #region 显示合并后的单元格的文字
            mergeName.elements.Add(data.First().Name);//合并后的名字
            mergeName.elements.Add("");//料牌编号
            mergeName.elements.Add(""); //钢筋规格
            mergeName.elements.Add("");// 钢筋形状
            mergeName.elements.Add("");// 断料长度（mm）
            mergeName.elements.Add("");// 总数（根）
            mergeName.elements.Add("");// 总重（kg）
            mergeName.elements.Add("");// 备注
            mergeName.elements[0].color = Color.red;
            table.data.Add(mergeName);
            Sprite s = Resources.Load<Sprite>("") as Sprite; 

            #endregion
            foreach (var item in items)
            {
                i++;
                j++;
                Datum rows = Datum.Body(i.ToString());
                rows.elements.Add(j.ToString());//序号
                rows.elements.Add(item.First().CardNum);//料牌编号
                rows.elements.Add(item.First().Name);//钢筋规格

                rows.elements.Add(item.First().Shape);//钢筋形状

                rows.elements.Add(item.First().Length.ToString());//断料长度（mm）
                rows.elements.Add(item.Sum(a => a.Quantity).ToString());//总数（根）
                rows.elements.Add("1");//总重（kg）
                rows.elements.Add("");//备注
                table.data.Add(rows);
            }
        }
        table.StartRenderEngine();
        RebarSummaryBtn.GetComponent<Image>().enabled = true;
    }
    public void RebarSummaryTable(List<MyRebar> rebars)
    {
        
        _myRebars = rebars;
        table = transform.GetComponentInChildren<Table>();
        table.ResetTable();
        table.AddTextColumn("dfadf",null, 100.0f,200.0f);
        table.AddTextColumn("序号").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("料牌编号").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("钢筋规格").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("钢筋形状").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("断料长度（mm）").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("总数（根）").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("总重（kg）").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("备注").horAlignment = Column.HorAlignment.CENTER;

        table.Initialize();

        long totalCountQuantity = 0;
        double totalCountWeight = 0.0;
        var datas = _myRebars.OrderBy(a => int.Parse(a.Name.Split(' ')[0])).GroupBy(myRebar => new { myRebar.Name });
        int i = 0;
        int j = 0;
        foreach (var data in datas)
        {
            double sumWeight = 0.0;
            i++;
            var items = data.GroupBy(myRebar => new { myRebar.Shape, myRebar.Length });
            Datum mergeName = Datum.Body(i.ToString());
            #region 显示合并后的单元格的文字
            mergeName.elements.Add(data.First().Name);//合并后的名字
            mergeName.elements.Add("");//料牌编号
            mergeName.elements.Add(""); //钢筋规格
            mergeName.elements.Add("");// 钢筋形状
            mergeName.elements.Add("");// 断料长度（mm）
            mergeName.elements.Add("");// 总数（根）
            mergeName.elements.Add("");// 总重（kg）
            mergeName.elements.Add("");// 备注
            mergeName.elements[0].color = Color.red;
            table.data.Add(mergeName);

            #endregion
            foreach (var item in items)
            {
                i++;
                j++;
                Datum rows = Datum.Body(i.ToString());
                rows.elements.Add(j.ToString());//序号
                rows.elements.Add(item.First().CardNum);//料牌编号
                rows.elements.Add(item.First().Name);//钢筋规格
                rows.elements.Add(item.First().Shape);//钢筋形状
                rows.elements.Add(item.First().Length.ToString());//断料长度（mm）
                rows.elements.Add(item.Sum(a => a.Quantity).ToString());//总数（根）
                #region 设置钢筋重量
                string diameter = item.First().Name.Split(' ')[0];
                double weight = GetRebarWeight(diameter, item.First().Length / 1000, item.Sum(a => a.Quantity));
                rows.elements.Add(weight.ToString("0.0"));//总重（kg）
                sumWeight += weight;
                #endregion
                
                rows.elements.Add("");//备注
                table.data.Add(rows);
            }
            i++;
            Datum mergeSum = Datum.Body(i.ToString());
            mergeSum.elements.Add("小计");//序号
            mergeSum.elements.Add("");//料牌编号
            mergeSum.elements.Add(""); //钢筋规格
            mergeSum.elements.Add("");// 钢筋形状
            mergeSum.elements.Add("");// 断料长度（mm）
            mergeSum.elements.Add(data.Sum(a=>a.Quantity).ToString());// 总数（根）
            mergeSum.elements.Add(sumWeight.ToString("0.0"));// 总重（kg）
            mergeSum.elements.Add("");//备注
           
            mergeSum.elements[0].color = Color.red;
            totalCountQuantity += data.Sum(a => a.Quantity);
            totalCountWeight += sumWeight;
            table.data.Add(mergeSum);

        }
        i++;
        Datum mergeSums = Datum.Body(i.ToString());
        mergeSums.elements.Add("合计");//序号
        mergeSums.elements.Add("");//料牌编号
        mergeSums.elements.Add(""); //钢筋规格
        mergeSums.elements.Add("");// 钢筋形状
        mergeSums.elements.Add("");// 断料长度（mm）
        mergeSums.elements.Add(totalCountQuantity.ToString());// 总数（根）
        mergeSums.elements.Add(totalCountWeight.ToString ("0.000"));// 总重（kg）
        mergeSums.elements.Add();//备注
        table.data.Add(mergeSums);
        table.StartRenderEngine();
        //RebarSummaryBtn.GetComponent<Image>().enabled = true;
    }

    private void RebarCompoentTable()
    {
        SetButtonNormal();
        RebarCompoentBtn.GetComponent<Image>().enabled = true;
    }
    private void RebarCardTable()
    {
        SetButtonNormal();
        table.ResetTable();
        table.AddTextColumn("序号").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("料牌编号").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("钢筋规格").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("钢筋形状").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("计算公式", null, 150, 160).horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("单长（mm）").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("总数量（根）").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("钢筋所在位置").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("备注").horAlignment = Column.HorAlignment.CENTER;

        table.Initialize();
        int i = 0;
        var datas = _myRebars.OrderBy(a => int.Parse(a.Name.Split(' ')[0])).GroupBy(myRebar => new { myRebar.Name, myRebar.Shape, myRebar.Length });
        foreach (var data in datas)
        {
            i++;
            Datum rows = Datum.Body(i.ToString());
            rows.elements.Add(i.ToString());//序号
            rows.elements.Add(data.First().CardNum);//料牌编号
            rows.elements.Add(data.First().Name);//钢筋规格
            rows.elements.Add(data.First().Shape);//钢筋形状
            rows.elements.Add(data.First().CalculatingFormula);//计算公式
            rows.elements.Add(data.First().Length.ToString());//单长（mm）
            rows.elements.Add(data.Sum(a => a.Quantity).ToString());//总数量（根）
            string location = "";
            List<string> hostcodeList = new List<string>();
            foreach (var item in data)
            {
                if (!hostcodeList.Contains(item.Host.HostCode))
                    hostcodeList.Add(item.Host.HostCode);
            }
            foreach (var item in hostcodeList)
            {
                location += "所在构件：" + item + "\n";
            }
            rows.elements.Add(location);//钢筋所在位置
            rows.elements.Add("");//备注
            table.data.Add(rows);
        }
        table.StartRenderEngine();
        RebarCardBtn.GetComponent<Image>().enabled = true;
    }
    private void RebarOptimizeTable()
    {
        if (FirstClick)
        {
            fixedlengthSetting.gameObject.SetActive(true);
            FixedLengthSetting fs = fixedlengthSetting.gameObject.GetComponent<FixedLengthSetting>();
            fs.Init(_myRebars);
        }
        else
        {
            RebarOptimizeTabletemp();
        }
       
    }

    public void RebarOptimizeTabletemp()
    {
        SetButtonNormal();
        if (FirstClick)
        {
            UnityEngine.Debug.Log("FirstClick is " + FirstClick);
            MyCutLib mycurebar = new MyCutLib(ReadTextAndGroupBy());
            tables = mycurebar.GetAllKindOfCutRebarList();
            
        }
       

        //UnityEngine.Debug.Log("count:"+ tables.Count);
        table.ResetTable();
        table.AddTextColumn("断料序号",null,50,60).horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("钢筋规格").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("原长定尺（mm）").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("需要的原材数").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("断料组合").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("料牌编号").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("余料长度").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("损耗率").horAlignment = Column.HorAlignment.CENTER;
        table.AddTextColumn("备注").horAlignment = Column.HorAlignment.CENTER;

        table.Initialize();

        int i = 0; int j = 0;
        foreach (var tableitem in tables)
        {
            i++;
            Datum mergeName = Datum.Body(i.ToString());
            #region 显示合并后的单元格的文字
            mergeName.elements.Add(tableitem.First().Name);//断料序号
            mergeName.elements.Add("");//钢筋规格
            mergeName.elements.Add(""); //原长定尺（mm）
            mergeName.elements.Add("");// 需要的原材数
            mergeName.elements.Add("");// 断料组合
            mergeName.elements.Add("");// 料牌编号 
            mergeName.elements.Add("");// 余料长度
            mergeName.elements.Add("");// 损耗率
            mergeName.elements.Add("");// 备注
            mergeName.elements[0].color = Color.red;
            table.data.Add(mergeName);

            double totalUsed = 0;
            int totalNum = 0;
            double wasteLength = 0;
            double fixedLength = 0;

            foreach (var item in tableitem)
            {
               
                i++;
                j++;
                Datum rows = Datum.Body(i.ToString());
                rows.elements.Add(j.ToString());//断料序号
                rows.elements.Add(item.name);//钢筋规格
                rows.elements.Add(item.FixedLength.ToString());//原长定尺（mm）
                rows.elements.Add(item.NeedNumber.ToString());//需要的原材数
                rows.elements.Add(item.GetTotal());//断料组合
                rows.elements.Add(item.GetCardNum(mySameLenght));//料牌编号
                rows.elements.Add(Math.Round(item.GetUseValue()[0],4).ToString()); //  余料长度
                rows.elements.Add((Math.Round(item.GetUseValue()[2],4)*100).ToString()+"%");// 损耗率
                if (Math.Round(item.GetUseValue()[0], 4) > 0 && Math.Round(item.GetUseValue()[2], 4) == 0.0)
                {
                    string str = "此为余料，可用于下批次加工";
                    rows.elements.Add(str);  //备注
                }
                else
                {
                    wasteLength += Math.Round(item.GetUseValue()[0],4)*item.NeedNumber;
                    rows.elements.Add("");
                }
                fixedLength = item.FixedLength;
                totalUsed += fixedLength * item.NeedNumber;
                table.data.Add(rows);
            }
            i++;
            double wasteRate = (wasteLength / totalUsed) * 100;
            UnityEngine.Debug.Log ("wasteRate :" + wasteRate);
            wasteRate = Math.Round(wasteRate, 2);
            string UtilizationRate = (100 - wasteRate).ToString() + "%";
            Datum mergeSum = Datum.Body(i.ToString());
            mergeSum.elements.Add("小计");//断料序号
            mergeSum.elements.Add("");//钢筋规格
            mergeSum.elements.Add(""); //原长定尺（mm）
            mergeSum.elements.Add(tableitem.Sum(a=>a.NeedNumber).ToString());// 需要的原材数
            mergeSum.elements.Add("");// 断料组合
            mergeSum.elements.Add("");// 料牌编号 
            mergeSum.elements.Add("总的利用率" + UtilizationRate);// 余料长度
            mergeSum.elements.Add("总的损耗率" +wasteRate.ToString()+"%");// 损耗率
            mergeSum.elements.Add("");// 备注
            mergeSum.elements[0].color = Color.red;
            table.data.Add(mergeSum);
        }
        table.StartRenderEngine();

        FirstClick = false;
        RebarOptimizeBtn.GetComponent<Image>().enabled = true;
    }
    private List<MyRebar> ReadTextAndGroupBy()
    {
        List<MyRebar> testList = _myRebars;
        List<MyRebar> sortList = new List<MyRebar>();
        var datas = testList.OrderBy(a => int.Parse(a.Name.Split(' ')[0])).GroupBy(myRebar => new { myRebar.Name }).ToList();
        foreach (var data in datas)
        {
            var items = data.GroupBy(myRebar => new { myRebar.Length }).ToList();

            foreach (var item in items)
            {

                MyRebar mybar = new MyRebar()
                {
                    CalculatingFormula = item.First().CalculatingFormula,
                    CardNum = item.First().CardNum,
                    Host = item.First().Host,
                    Length = item.First().Length,
                    Name = item.First().Name,
                    Param_A = item.First().Param_A,
                    Param_B = item.First().Param_B,
                    Param_C = item.First().Param_C,
                    Quantity = item.Sum(a => a.Quantity),
                    Shape = item.First().Shape
                };
                sortList.Add(mybar);
            }

        }
        mySameLenght = GetSameLenght(sortList);//查找相同长度不同料牌的集合
        return sortList;
    }
    public static List<SameLength> GetSameLenght(List<MyRebar> myRebar)
    {
        List<SameLength> tem = new List<SameLength>();

        //string name = "";
        //double length = 0;

        string name = myRebar[0].Name;
        double length = myRebar[0].Length;
        int same = 1;
        for (int i = 1; i < myRebar.Count; i++)
        {
            bool sameNum = false;//是否有相同的数量
            SameLength temSameLength = new SameLength();
            if (myRebar[i].Name == name && myRebar[i].Length == length)
            {

                same++;//计算相同的数量
                #region 旧方法获取相同长度的料牌
                //double num1 = myRebar[i].Quantity;
                //double num2 = myRebar[i - 1].Quantity;
                //string car1 = myRebar[i].CardNum;
                //string car2 = myRebar[i - 1].CardNum;
                //double len1 = myRebar[i].Length;
                //double len2 = myRebar[i-1].Length;
                //string name1 = myRebar[i].Name;
                //string name2 = myRebar[i - 1].Name;
                //temSameLength.shuliang.Add(num2);
                //temSameLength.shuliang.Add(num1);                   
                //temSameLength.changdu.Add(len2);
                //temSameLength.changdu.Add(len1);                   
                //temSameLength.liaopai.Add(car2);
                //temSameLength.liaopai.Add(car1);
                //temSameLength.mingcheng.Add(name2);
                //temSameLength.mingcheng.Add(name1);
                #endregion

            }
            else if (myRebar[i].Name == name && myRebar[i].Length != length && same != 1)
            {
                sameNum = true;
            }
            if (sameNum)
            {
                for (int m = i - same; m < i; m++)
                {
                    temSameLength.mingcheng.Add(myRebar[m].Name);
                    temSameLength.shuliang.Add(myRebar[m].Quantity);
                    temSameLength.liaopai.Add(myRebar[m].CardNum);
                    temSameLength.changdu.Add(myRebar[m].Length);
                }
                tem.Add(temSameLength);
                same = 1;
            }

            name = myRebar[i].Name;
            length = myRebar[i].Length;
        }
        return tem;
    }


    private static double GetRebarWeight(string diameter, double millimeterToMeter, int totalQuantity)
    {
        double weight = 0.0;
        switch (diameter)
        {
            case "6":
                weight = (millimeterToMeter * 0.222) * totalQuantity;
                break;
            case "8":
                weight = (millimeterToMeter * 0.395) * totalQuantity;
                break;
            case "10":
                weight = (millimeterToMeter * 0.617) * totalQuantity;
                break;
            case "12":
                weight = (millimeterToMeter * 0.888) * totalQuantity;
                break;
            case "14":
                weight = (millimeterToMeter * 1.21) * totalQuantity;
                break;
            case "16":
                weight = (millimeterToMeter * 1.58) * totalQuantity;
                break;
            case "18":
                weight = (millimeterToMeter * 2.00) * totalQuantity;
                break;
            case "20":
                weight = (millimeterToMeter * 2.47) * totalQuantity;
                break;
            case "22":
                weight = (millimeterToMeter * 2.98) * totalQuantity;
                break;
            case "25":
                weight = (millimeterToMeter * 3.85) * totalQuantity;
                break;
            case "28":
                weight = (millimeterToMeter * 4.83) * totalQuantity;
                break;
            case "32":
                weight = (millimeterToMeter * 6.31) * totalQuantity;
                break;
            case "36":
                weight = (millimeterToMeter * 7.99) * totalQuantity;
                break;
            case "40":
                weight = (millimeterToMeter * 9.87) * totalQuantity;
                break;
            case "50":
                weight = (millimeterToMeter * 15.42) * totalQuantity;
                break;
        }

        return weight;
    }

}
#endregion