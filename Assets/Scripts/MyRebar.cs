using System.Collections.Generic;

public class MyRebar
{
    public string Name { get; set; }
    public RebarHost Host { get; set; }
    public string Shape { get; set; }
    public double Length { get; set; }
    public int Quantity { get; set; }
    public string CalculatingFormula { get; set; }  //计算公式
    public string Param_A { get; set; }
    public string Param_B { get; set; }
    public string Param_C { get; set; }
    public string CardNum { get; set; }
}
public class RebarHost
{
    public string HostId { get; set; }
    public string HostName { get; set; }
    public string Level { get; set; }
    public string HostCode { get; set; }

    public Dictionary<string, string> RebarSet = new Dictionary<string, string>();
}