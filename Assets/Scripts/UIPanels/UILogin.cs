using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class UILogin : TTUIPage {

    InputField mIptUser;
    InputField mIptPassword;
    InputField mIp;
    Button mBtnLogin;
    Button mBtnQuit;
    string url = "";
    string action = "Login/LoginTest?";


    public UILogin() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPanel/LoginPanel";
    }

    public override void Awake(GameObject go)
    {
        //base.Awake(go);
        mIptUser = this.transform.Find("Panel/UserName").GetComponent<InputField>();
        mIptPassword = this.transform.Find("Panel/Password").GetComponent<InputField>();
        mIp = this.transform.Find("Panel/Ip").GetComponent<InputField>();

        mBtnLogin = this.transform.Find("Panel/BtnLogin").GetComponent<Button>();
        mBtnQuit = this.transform.Find("Panel/BtnQuit").GetComponent<Button>();
        mBtnLogin.onClick.AddListener(OnLogin);
        mBtnQuit.onClick.AddListener(()=> { Application.Quit(); });
        url = Tools.WebUrl + action;
    }

    public override void Refresh()
    {
        mIptUser.text = "";
        mIptPassword.text = "";
    }
    public void OnLogin()
    {
        Tools.WebUrl = mIp.text;
        //Task task = new Task(ConnectWeb());
        if (mIptUser.text != string.Empty && mIptPassword.text != string.Empty)
        {
            Task task = new Task(ConnectWeb());
        }
        else
            MessageBox.Show("请输入用户名或者密码！");
        // MessageBox.Show("Helloworld");
    }


    IEnumerator ConnectWeb()
    {
        //string requesturl = url + "name=" + mIptUser.text + "&" + "pwd=" + mIptPassword.text;
        string requesturl = Tools.WebUrl+action + "name=" + mIptUser.text + "&" + "pwd=" + mIptPassword.text;
        //string requesturl = url + "name=" + mIptUser.text + "&" + "pwd=" + mIptPassword.text;

        WWW data = new WWW(requesturl);
        yield return data;
        if (data.error == null)
        {
            if (!data.text.Contains("失败"))
            {
                User user = JsonConvert.DeserializeObject<User>(data.text);
                SerializeMethod(user);
                MessageBox.Show("succeed!");
                ClosePage<UILogin>();
                ShowPage<UIFilesPanel>();
            }
            else
                MessageBox.Show(data.text);
        }
        else
        {
            MessageBox.Show(data.error, "Connect Error");
        }

    }
    public void OnQuit()
    {
        Hide();
    }



    public void SerializeMethod(User user)
    {
        string path = Tools.Instance.SavedPath + "/" + Tools.BaseFolder + "/" + Tools.UserFolder + "/" + Tools.UserFile;

        //using (FileStream fs = new FileStream(Tools.SavedPath+ "/"+Tools.ConfigFolder+ "/" + Tools.UserFolder + "/" + Tools.UserFile, FileMode.Create))
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, user);
        }
    }

}
