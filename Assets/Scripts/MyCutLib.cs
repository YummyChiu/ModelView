using UnityEngine;
using System.Collections.Generic;
using CutGLib;
using System;
using System.Linq;

public class MyCutLib  {
    private List<MyRebar> _myRebars;

    public MyCutLib(List<MyRebar> myRebars)
    {
        _myRebars = myRebars;
    }
    public List<List<MyCutRebar>> GetAllKindOfCutRebarList()
    {
        List<List<MyCutRebar>> allCutRebarList = new List<List<MyCutRebar>>();
        
        foreach (var item in FixedLengthSetting.fixedLengthDict)
        {
            double length = Convert.ToDouble (item.Value);
            allCutRebarList.Add(LinearOneSheetSize(item.Key, length));
        }

        return allCutRebarList;
    }
    public List<MyCutRebar> LinearOneSheetSize(string name, double fixedlength)//求线性规划方程组
    {

        List<MyCutRebar> mycutRebar = new List<MyCutRebar>();

        // First we create a new instance of the cut engine
        CutEngine Calculator = new CutEngine();
        Calculator.AddLinearStock(fixedlength, 1000000);
        List<MyRebar> tempRebarList = SelectDiffKindRebar(name, fixedlength);

        foreach (MyRebar item in tempRebarList)
        {
            Calculator.AddLinearPart(item.Length, item.Quantity, item.CardNum);
        }


        string result = Calculator.ExecuteLinear();
        if (result == "")
        {
            mycutRebar = OutputLinearResults_by_Layout(Calculator, name, tempRebarList.Min(a => a.Length));
        }
        else
        {
            //Console.Write("%S\n", result);

        }


        return mycutRebar;
    }
    public List<MyCutRebar> OutputLinearResults_by_Layout(CutEngine aCalculator, string name, double shortestLength)
    {

        List<MyCutRebar> tempCutRebar = new List<MyCutRebar>();

        string[] strTemp = new string[4];
        int totalAmount = 0;//每组需要的数量
        int StockIndex, StockCount, iPart, iLayout, partCount, partIndex, tmp, iStock;
        double partLength, X, StockLength, angleStart, angleEnd;
        bool rotated, StockActive;
        bool isAngle = aCalculator.CrossSection > 0;
        string ID = "";
        //aCalculator.GetStockInfo();



        // Iterate by each layout and output information about each layout,
        // such as number and length of used stocks and part indices cut from the stocks
        for (iLayout = 0; iLayout < aCalculator.LayoutCount; iLayout++)//6
        {

            aCalculator.GetLayoutInfo(iLayout, out StockIndex, out StockCount);
            // StockIndex is global index of the first stock used in the layout iLayout
            // StockCount is quantity of stocks of the same length as StockIndex used for this layout
            if (StockCount > 0)
            {
                // sw.WriteLine("Layout={0}:  Start Stock={1};  Count of Stock={2}", iLayout, StockIndex, StockCount);
                bool istrue = true;
                MyCutRebar cm = new MyCutRebar();
                cm.totalNumber = aCalculator.UsedLinearStockCount;//获取总数
                                                                  // Output information about each stock, such as stock Length
                for (iStock = StockIndex; iStock < StockIndex + StockCount; iStock++)//86
                {

                    if (aCalculator.GetLinearStockInfo(iStock, out StockLength, out StockActive, out ID))
                    {

                        //sw.WriteLine("Stock={0}:  Length={1}", iStock, StockLength);
                        // Output the information about parts cut from this stock
                        // First we get quantity of parts cut from the stock
                        partCount = aCalculator.GetPartCountOnStock(iStock);
                        // Iterate by parts and get indices of cut parts
                        for (iPart = 0; iPart < partCount; iPart++)//3
                        {

                            // Get global part index of iPart cut from the current stock
                            partIndex = aCalculator.GetPartIndexOnStock(iStock, iPart);
                            // Get length and location of the part
                            // X - coordinate on the stock where the part beggins.
                            if (isAngle)
                            {
                                aCalculator.GetResultLinearPart(partIndex, out tmp, out partLength, out angleStart, out angleEnd, out X, out rotated);
                                // Output the part information
                                Console.Write("Part= {0}:  X={1};  Length={2}; AngleStart={3}; AngleEnd={4}{5}\n",
                                              partIndex, X, partLength, angleStart, angleEnd, LinearTurn(rotated));
                            }
                            else
                            {
                                aCalculator.GetResultLinearPart(partIndex, out tmp, out partLength, out X, out ID);
                                // Output the part information
                                if (istrue)
                                {
                                    //List<RebarsCharacter> rebarsCharacter = new List<RebarsCharacter>();
                                    //Console.Write("Part= {0}:  截取的位置X={1};  截取的长度Length={2}\n", partIndex, X, partLength);
                                    cm.NeedNumber = StockCount;
                                    cm.lenghtList.Add(partLength);
                                    cm.locationLength.Add(X);
                                    cm.cardNumList.Add(ID);
                                    cm.name = name;
                                    cm.FixedLength = StockLength;
                                    cm.ShortestLength = shortestLength;
                                    if (iPart == partCount - 1)
                                    {
                                        /**************添加钢筋断料组合中的特征（长度*数量*料牌号）*******************/
                                        foreach (var v in cm.lenghtList.GroupBy(x => x).Select(x => new { length = x.Key, count = x.Count() }))
                                        {
                                            RebarCharacter rebarcharacter = new RebarCharacter();

                                            rebarcharacter.Length = v.length;
                                            rebarcharacter.Count = v.count;
                                            rebarcharacter.CardNum = null;
                                            cm.rebarCharacter.Add(rebarcharacter);
                                        }
                                        /*********************************/
                                        tempCutRebar.Add(cm);

                                        istrue = false;

                                    }

                                }

                                if (iLayout == aCalculator.LayoutCount - 1 && iStock == StockIndex + StockCount - 1)
                                {
                                    totalAmount = iStock;
                                }

                            }
                        }
                    }
                }

            }


        }
        return tempCutRebar;
    }
    private List<MyRebar> SelectDiffKindRebar(string name, double fixedlength)
    {
        List<MyRebar> selectedRebar = new List<MyRebar>();
        foreach (var item in _myRebars)
        {
            if (item.Name == name)
            {
                if (item.Length < fixedlength)

                    selectedRebar.Add(item);
                else
                {
                    //MessageBox.Show("长度为：" + item.Length.ToString() + "_" + "数量为：" + item.Quantity.ToString());
                }
            }

        }
        return selectedRebar;
    }
    private static string LinearTurn(bool aRotated)
    {
        return aRotated ? "; Turned" : string.Empty;
    }
}
