using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class UIUserInfoPanel : TTUIPage
{

    private Text mUserName;
    private Text mUserTitle;
    private Button mBtnSignout;
    private Button mBtnSetting;
    private Image mUserImg;

    private string userfilePath = Tools.Instance.SavedPath + "/" + Tools.BaseFolder + "/" + Tools.UserFolder + "/" + Tools.UserFile;


    public UIUserInfoPanel() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPanel/UserInfoPanel";
    }
    public override void Awake(GameObject go)
    {
        mUserName = transform.Find("UserName").GetComponent<Text>();
        mUserTitle = transform.Find("UserTitle").GetComponent<Text>();
        mBtnSignout = transform.Find("BtnSignout").GetComponent<Button>();
        mBtnSetting = transform.Find("BtnSetting").GetComponent<Button>();
        mBtnSignout.onClick.AddListener(OnClickSignout);
        mBtnSetting.onClick.AddListener(OnClickSetting);
        transform.gameObject.AddComponent<MenuVisibilityCtrl>();
    }


    public override void Refresh()
    {
        //base.Refresh();
        User user = ReserializeLocalFile();
        mUserName.text = user.UserName;
        if (user.Roles.Count != 0)
            user.Roles.ForEach(obj => mUserTitle.text += obj + ",");
        else mUserTitle.text = "无职称";
    }

    private User ReserializeLocalFile()
    {
        if (File.Exists(userfilePath))
        {
            using (FileStream fs = new FileStream(userfilePath, FileMode.Open))
            {
                Debug.Log("在路径：" + userfilePath + "有用户信息");
                BinaryFormatter bf = new BinaryFormatter();
                User user = bf.Deserialize(fs) as User;
                return user;
            }
        }
        else
        {
            MessageBox.Show("在路径：" + userfilePath + "没有用户信息");
            return new User { UserName = "null", Gender = "null" };
        }
    }

    private void OnClickSetting()
    {
        //throw new NotImplementedException();
    }

    private void OnClickSignout()
    {
        ReadModelSQLite.Instance.DestoryGameObjects();
        ClosePage<UIUserInfoPanel>();
        ClosePage<UIMenuPanel>();
        ShowPage<UILogin>();
    }

}
