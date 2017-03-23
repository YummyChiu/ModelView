using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Com.UI;
using UnityEngine.UI;
using System.Linq;

public class RebarTreeView : MonoBehaviour {

    // Use this for initialization
    public UTree levelTree;
    public UTree compoentTree;
    public Transform rebarPanel;
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

    
    private List<MyRebar> _myRebars;
    //int start;
    //void Awake()
    //{
    //    //start = System.Environment.TickCount;
    //   // Debug.Log("start:" + start);
    //    int start = System.Environment.TickCount;
    //    Debug.Log("------" + "kaishi");

    //    List<MyRebar> list = GetRebarInfo.GetRebar();

    //    //for (int i = 0; i < list.Count; i++)
    //    //{
    //    //    Debug.Log("------" + list[i].Name);
    //    //}
    //    //// Debug.Log("------" + "end");
    //    ////Debug.Log("time:" + Time.deltaTime);
    //    //int end = System.Environment.TickCount;
    //    //Debug.Log("strat - end :" + (end - start));//单位毫秒
    //}
    void Start () {
        levelTreeTr = transform.Find("LevelTree");
        floorCheckOnBtn = transform.Find("FloorCommitAll").GetComponent<Button>();
        floorCheckOffBtn = transform.Find("FloorCanelAll").GetComponent<Button>();

        compoentTreeTr = transform.Find("CompoentTree");
        compoentCheckOnBtn = transform.Find("CompoentCommitAll").GetComponent<Button>();
        compoentCheckOffBtn = transform.Find("CompoentCanelAll").GetComponent<Button>();
        floorCheckOnBtn.onClick.AddListener(delegate () { CheckAll(true, levelTreeTr); });
        floorCheckOffBtn.onClick.AddListener(delegate () { CheckAll(false, levelTreeTr); });
        compoentCheckOnBtn.onClick.AddListener(delegate () { CheckAll(true, compoentTreeTr); });
        compoentCheckOffBtn.onClick.AddListener(delegate () { CheckAll(false, compoentTreeTr); });

        ConfirmBtn = transform.Find("Confirm").GetComponent<Button>();
        CanelBtn = transform.Find("Canel").GetComponent<Button>();

        ConfirmBtn.onClick.AddListener(delegate () { Confirm(); });
        CanelBtn.onClick.AddListener(delegate () { ClosePanel(); });
        LevelTreeInit();


        CompoentTreeInit();
        setdata();
        SetScaleTo1();
        _myRebars = GetRebarInfo.GetRebar();
        //StartCoroutine(DelayToInvoke.DelayToInvokeDo(() =>
        //{
        //    setdata();
        //    SetScaleTo1();
        //    _myRebars = GetRebarInfo.GetRebar();
        //}, 0.2f
        //));

    }
   
    private void CompoentTreeInit()
    {
        List<string> compoentList = new List<string>() {"墙","梁","板","柱","基础" };
        UTreeNodeData compoenttree = new UTreeNodeData(1,"构件");
        int count = 0;
        foreach (var item in compoentList)
        {
            count++;
            UTreeNodeData levelChildNode = new UTreeNodeData(count, item);
            compoenttree.AddChild(levelChildNode);
        }
        compoentData = new List<UTreeNodeData>();
        compoentData.Add(compoenttree);
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
        /********读取模型***********/
        //List<string> levelList = new List<string>();
        //foreach (var item in ReadModelXML.idAndLevels.Values)
        //{
        //    if (!levelList.Contains(item))
        //        levelList.Add(item);
        //}
        /********读取模型***********/

        List<string> levelList = new List<string>
        {
          "结构B2层","结构B1层","结构1层","结构2层","结构3层","结构4层","结构5层","结构6层",
          "结构7层","结构8层","结构9层","结构10层","结构11层","结构12层","结构13层","结构14层",
          "结构15层","结构16层","结构17层","结构18层","结构RMF层","结构RF层","结构TF层"
        };

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

      //  StartCoroutine(DelayToInvoke.DelayToInvokeDo(() =>
      //  {
      //      _myRebars = GetRebarInfo.GetRebar();

      //  }, 1f
      //));
        
    }
   
    private void CheckAll(bool state, Transform tr)
    {
        foreach (var item in tr.GetComponentsInChildren<Toggle>())
        {
            //Debug.Log("HelloAgain!");
            item.isOn = state;
        }
    }
    private void ClosePanel()
    {
        transform.gameObject.SetActive(false);
    }
    private void Confirm()
    {
        Debug.Log("Start");
        var myRebars = _myRebars;
        List<string> levels = new List<string>();
        foreach (var item in levelTree.nodeData[0].Children)
        {
            if (!item.Check)
            {
                levels.Add(item.Title);
                //Debug.Log(item.Title);
            }
        }
        foreach (var item in compoentTree.nodeData[0].Children)
        {
            if (!item.Check)
            {
                switch (item.Title)
                {
                    case "墙":
                        myRebars = _myRebars.Where(a => a.Host.HostName != item.Title).ToList();
                        break;
                    case "梁":
                        myRebars = myRebars.Where(a => a.Host.HostName != item.Title).ToList();
                        break;
                    case "板":
                        myRebars = myRebars.Where(a => a.Host.HostName != item.Title).ToList();
                        break;
                    case "柱":
                        myRebars = myRebars.Where(a => a.Host.HostName != item.Title).ToList();
                        break;
                }

            }
           
        }
        foreach (var level in levels)
        {
            myRebars = myRebars.Where(a => a.Host.Level != level).ToList();
        }
        this.transform.gameObject.SetActive(false);
        rebarPanel.gameObject.SetActive(true);
        RebarTableControl rControl = rebarPanel.GetComponent<RebarTableControl>();
        rControl.RebarSummaryTable(myRebars);

        //Debug.Log("所选择的list有：" + myRebars.Count);
    }
  
}
