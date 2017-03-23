using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;


public class UIMenuPanel : TTUIPage
{
    private Button mBtnUserInfo;
    private Button mBtnCount;
    private Button mBtnRebar;
    private Button mBtnFilter;
    private Button mBtnProgress;


    public UIMenuPanel() : base(UIType.Fixed, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPanel/MenuPanel";
    }

    public override void Awake(GameObject go)
    {
        //base.Awake(go);
        mBtnUserInfo = transform.Find("BtnUserInfo").GetComponent<Button>();
        mBtnCount = transform.Find("BtnCount").GetComponent<Button>();
        mBtnRebar = transform.Find("BtnRebar").GetComponent<Button>();
        mBtnFilter = transform.Find("BtnFilter").GetComponent<Button>();
        mBtnProgress = transform.Find("BtnProgress").GetComponent<Button>();

        mBtnUserInfo.onClick.AddListener(OnClickUserInfo);
        mBtnCount.onClick.AddListener(OnClickCount);
        mBtnRebar.onClick.AddListener(OnClickRebar);
        mBtnFilter.onClick.AddListener(OnClickFilter);
        mBtnProgress.onClick.AddListener(OnClickProgress);


    }
    public override void Hide()
    {
        base.Hide();
    }

    private void OnClickUserInfo()
    {
        ShowPage<UIUserInfoPanel>();
    }

    private void OnClickCount()
    {

    }
    private void OnClickRebar()
    {

    }
    private void OnClickFilter()
    {

    }
    private void OnClickProgress()
    {

    }
}
