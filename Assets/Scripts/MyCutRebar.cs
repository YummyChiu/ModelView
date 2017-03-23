using System.Collections.Generic;
using System.Linq;

public class MyCutRebar  {
    public string name;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public int totalNumber;
    public int TotalNumber
    {
        get { return totalNumber; }
        set { totalNumber = value; }
    }

    public string diameter;
    public string Diameter
    {
        get { return diameter; }
        set { diameter = value; }
    }

    int needNumber;//需要的数量
    public int NeedNumber
    {
        get { return needNumber; }
        set { needNumber = value; }
    }
    double fixedLength;//定长，一般9米
    public double FixedLength
    {
        get { return fixedLength; }
        set { fixedLength = value; }
    }
    public double ShortestLength
    {
        get;
        set;
    }
    double remainLength;//余料
    public double RemainLength
    {
        get { return remainLength; }
        set { remainLength = value; }
    }
    public List<double> locationLength = new List<double>();//截取的位置
    public List<double> lenghtList = new List<double>();//每一根的长度

    public List<string> cardNumList = new List<string>(); //每一根的料牌编号

    public List<RebarCharacter> rebarCharacter = new List<RebarCharacter>();


    public double[] GetUseValue()//得到余料长度，使用长度，使用率，损耗率
    {
        double waste = 0;
        double needLenght = 0;
        double wasteRate = 0.0;
        double usedRate = 0.0;
        double[] ValueArray = new double[4];
        for (int i = 0; i < lenghtList.Count; i++)
        {
            needLenght += lenghtList[i];//每组使用的长度
        }

        waste = fixedLength - needLenght;
        //如果余料大于最短的，wasteRate = 0
        if (waste > ShortestLength)
        {
            wasteRate = 0.0;
        }
        else
        {
            wasteRate = waste / fixedLength;

        }
        usedRate = needLenght / fixedLength;
        ValueArray[0] = waste;//余料
        ValueArray[1] = needLenght;//需要的长度
        ValueArray[2] = wasteRate;//损耗率
        ValueArray[3] = usedRate;//使用率
        return ValueArray;
    }
    List<double> difLenght = new List<double>();//获取每一组不同长度集合
    List<int> sameIntList = new List<int>();
    public string GetTotal()//统计每组需要每种类型长度的数量
    {
        difLenght.Clear();
        string sumStr = "";
        string s = "";
        s = lenghtList[0].ToString();
        int sameInt = 0;
        //去掉重复的长度
        foreach (var item in lenghtList.Distinct())
        {
            difLenght.Add(item);
        }
        for (int i = 0; i < lenghtList.Count; i++)
        {

            if (s.Contains(lenghtList[i].ToString()))
            {
                sameInt++;//统计根数
                          // difLenght.Add(lenghtList[i]);//第一次判断肯定有
                if (i == lenghtList.Count - 1)
                {
                    sumStr += lenghtList[i].ToString() + "*" + sameInt + "根  ";
                    sameIntList.Add(sameInt);
                    // difLenght.Add(lenghtList[i]);
                }
            }
            if (!s.Contains(lenghtList[i].ToString()))
            {
                sameIntList.Add(sameInt);
                // difLenght.Add(lenghtList[i]);
                sumStr += lenghtList[i - 1].ToString() + "*" + sameInt + "根  ";//计算根数
                sameInt = 1;
                s = lenghtList[i].ToString();
                if (i == lenghtList.Count - 1)
                {
                    sumStr += lenghtList[i].ToString() + "*" + sameInt + "根  ";
                    sameIntList.Add(sameInt);
                    //difLenght.Add(lenghtList[i]);
                }

            }

        }

        return sumStr;
    }
    public string GetCardNum(List<SameLength> samelength)
    {
        string sumStr = "";
        string s = "";
        List<string> templist = new List<string>();//获取料牌集合
        for (int i = 0; i < cardNumList.Count; i++)
        {
            s = cardNumList[i];
            if (!templist.Contains(s))
            {
                templist.Add(s);
            }
        }

        for (int j = 0; j < difLenght.Count; j++)//单元格内的长度
        {
            bool istrue = true;
            if (samelength.Count == 0)//如果没有不同的料牌号
            {
                sumStr += templist[j] + "*" + (needNumber * sameIntList[j]).ToString() + "根" + "\n";
            }
            else//如果有不同的料牌号
            {
                for (int m = 0; m < samelength.Count; m++)//相同的长度料牌集合
                {
                    if (difLenght[j] == samelength[m].changdu[0] && templist[j] == samelength[m].liaopai[0])//判断长度和料牌是否相同（确定唯一对应）
                    {
                        #region 旧代码
                        //    if (((needNumber * sameIntList[j]) > samelength[m].shuliang[0]) && (samelength[m].shuliang[0] != 0))
                        //    {
                        //        string tem = templist[j] + "*" + (needNumber * sameIntList[j]).ToString() + "根" + "\n";
                        //        sumStr = sumStr.Replace(tem, "");

                        //        double remaind = needNumber * sameIntList[j] - samelength[m].shuliang[0];
                        //        sumStr += samelength[m].liaopai[0] + "*" + samelength[m].shuliang[0] + "根" + "\n";
                        //        samelength[m].shuliang[1] -= remaind;
                        //        samelength[m].shuliang[0] = 0;
                        //        sumStr += samelength[m].liaopai[1] + "*" + remaind + "根" + "\n";
                        //    }
                        //    if (((needNumber * sameIntList[j]) < samelength[m].shuliang[0])&& (samelength[m].shuliang[0] != 0))
                        //    {
                        //        string tem = templist[j] + "*" + (needNumber * sameIntList[j]).ToString() + "根" + "\n";
                        //        sumStr = sumStr.Replace(tem, "");

                        //        sumStr += samelength[m].liaopai[0] + "*" + (needNumber * sameIntList[j]).ToString() + "根" + "\n";
                        //        samelength[m].shuliang[0] -= needNumber * sameIntList[j];
                        //    }
                        //    if (samelength[m].shuliang[0] == 0 && samelength[m].shuliang[1] == 0)
                        //    {
                        //        samelength.Remove(samelength[m]);
                        //    }

                        //    istrue = false;
                        //}
                        //if (istrue)
                        //{
                        //    sumStr += templist[j] + "*" + (needNumber * sameIntList[j]).ToString() + "根" + "\n";
                        //    istrue = false;
                        //}
                        #endregion
                        string tem = templist[j] + "*" + (needNumber * sameIntList[j]).ToString() + "根" + "\n";
                        sumStr = sumStr.Replace(tem, "");
                        istrue = false;

                        double myTotal = needNumber * sameIntList[j];
                        for (int a = 0; a < samelength[m].shuliang.Count;)
                        {

                            if ((myTotal >= samelength[m].shuliang[a]) && (samelength[m].shuliang[a] != 0))
                            {
                                myTotal -= samelength[m].shuliang[a];
                                sumStr += samelength[m].liaopai[a] + "*" + samelength[m].shuliang[a] + "根" + "\n";
                                samelength[m].shuliang[a] = 0;
                                if (samelength[m].shuliang[a] == 0)
                                {
                                    samelength[m].shuliang.Remove(samelength[m].shuliang[a]);
                                    samelength[m].liaopai.Remove(samelength[m].liaopai[a]);
                                    a = 0;
                                }
                            }
                            if (samelength[m].shuliang.Count == 0)
                            {
                                samelength.Remove(samelength[m]);
                                break;
                            }
                            else if ((myTotal < samelength[m].shuliang[a]))
                            {
                                samelength[m].shuliang[a] -= myTotal;
                                sumStr += samelength[m].liaopai[a] + "*" + myTotal + "根" + "\n";
                                break;
                            }
                        }
                        #region
                        //double totalRemaind = 0;
                        //totalRemaind = myTotal - samelength[m].shuliang[a];
                        // myTotal -= samelength[m].shuliang[a];

                        //if (totalRemaind > 0 && totalRemaind > samelength[m].shuliang[a + 1])
                        //{

                        //}


                        //double remaind = needNumber * sameIntList[j] - samelength[m].shuliang[0];
                        //sumStr += samelength[m].liaopai[0] + "*" + samelength[m].shuliang[0] + "根" + "\n";
                        //samelength[m].shuliang[1] -= remaind;
                        //samelength[m].shuliang[0] = 0;
                        //sumStr += samelength[m].liaopai[1] + "*" + remaind + "根" + "\n";


                        //if (((needNumber * sameIntList[j]) > samelength[m].shuliang[0]) && (samelength[m].shuliang[0] != 0))
                        //{
                        //    string tem = templist[j] + "*" + (needNumber * sameIntList[j]).ToString() + "根" + "\n";
                        //    sumStr = sumStr.Replace(tem, "");

                        //    double remaind = needNumber * sameIntList[j] - samelength[m].shuliang[0];
                        //    sumStr += samelength[m].liaopai[0] + "*" + samelength[m].shuliang[0] + "根" + "\n";
                        //    samelength[m].shuliang[1] -= remaind;
                        //    samelength[m].shuliang[0] = 0;
                        //    sumStr += samelength[m].liaopai[1] + "*" + remaind + "根" + "\n";
                        //}
                        //if (((needNumber * sameIntList[j]) > samelength[m].shuliang[1]) && (samelength[m].shuliang[1] != 0))
                        //{

                        //}
                        //if (((needNumber * sameIntList[j]) < samelength[m].shuliang[0]) && (samelength[m].shuliang[0] != 0))
                        //{
                        //    string tem = templist[j] + "*" + (needNumber * sameIntList[j]).ToString() + "根" + "\n";
                        //    sumStr = sumStr.Replace(tem, "");

                        //    sumStr += samelength[m].liaopai[0] + "*" + (needNumber * sameIntList[j]).ToString() + "根" + "\n";
                        //    samelength[m].shuliang[0] -= needNumber * sameIntList[j];
                        //}
                        //if (samelength[m].shuliang[0] == 0 && samelength[m].shuliang[1] == 0)
                        //{
                        //    samelength.Remove(samelength[m]);
                        //}

                        //istrue = false;
                        #endregion
                    }

                    if (istrue)
                    {
                        sumStr += templist[j] + "*" + (needNumber * sameIntList[j]).ToString() + "根" + "\n";
                        istrue = false;
                    }
                }
            }


        }

        return sumStr;
    }

}
public class RebarCharacter
{
    public double Length { get; set; }
    public int Count { get; set; }
    public string CardNum { get; set; }
}

