using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class FixedLengthSetting : MonoBehaviour
{

    private Button confirmBtn;
    private Button cancelBtn;
    private Button resetBtn;
    private Transform content;
    private List<MyRebar> _myRebars;
    List<string> KindOfRebarList = new List<string>();

    //private GameObject go;
    //private  Transform tran;
    
    public static Dictionary<string, string> fixedLengthDict = new Dictionary<string, string>();
    // Use this for initialization
    void Start()
    {
        confirmBtn = transform.Find("ConfirmBtn").GetComponent<Button>();
        cancelBtn = transform.Find("CancelBtn").GetComponent<Button>();
        resetBtn = transform.Find("ResetBtn").GetComponent<Button>();
        confirmBtn.onClick.AddListener(ConfirmClick);
        cancelBtn.onClick.AddListener(CancelClick);
        resetBtn.onClick.AddListener(ResetClick);

        content = transform.Find("Scroll View/Viewport/Content");

        foreach (var item in _myRebars)
        {
            if (!KindOfRebarList.Contains(item.Name))
            {
                if (item.Name == "6 HPB300" || item.Name == "6 HRB400" ||
                item.Name == "8 HPB300" || item.Name == "8 HRB400" ||
                item.Name == "10 HPB300" || item.Name == "10 HRB400")
                {

                }
                else
                {
                    KindOfRebarList.Add(item.Name);
                }
            }
        }

        foreach (var item in KindOfRebarList)
        {
            GameObject go = GameObject.Instantiate(Resources.Load("Prefabs/ToggleItem", typeof(GameObject))) as GameObject;
            Transform tran = go.transform;
            tran.parent = content;
            tran.localScale = Vector3.one;
            ToggleSelectItem ts = go.GetComponent<ToggleSelectItem>();
            ts.Init(item);
        }

        
    }
    public void Init(List<MyRebar> rebars)
    {
        _myRebars = rebars;

    }
    /// <summary>
    /// 定长设置窗口 确定按钮（待解决），有更好的方法
    /// </summary>
    void ConfirmClick()
    {
        fixedLengthDict.Clear();
        foreach (Transform item in content)
        {
            Toggle toggle = item.GetComponentInChildren<Toggle>();
            Text text = toggle.GetComponentInChildren<Text>();
            Dropdown dropdown = item.GetComponentInChildren<Dropdown>();
            string toggleText = text.text;
            bool check = toggle.isOn;
            string dropdownText = dropdown.options[dropdown.value].text;
            // Dictionary<string, string> temp = new Dictionary<string, string>();
            if (check)
            {
                fixedLengthDict.Add(toggleText, dropdownText);
            }
        }
        foreach (var item in fixedLengthDict)
        {
            Debug.Log(item.Key + item.Value);
        }
        //Debug.Log("123");
        this.transform.gameObject.SetActive(false);

        RebarTableControl.Instance.RebarOptimizeTabletemp();
    }

    void ResetClick()
    {
        foreach (var item in content.GetComponentsInChildren<Toggle>())
        {
            item.isOn = false;
        }
    }
    void CancelClick()
    {
        this.transform.gameObject.SetActive(false);
    }
}
