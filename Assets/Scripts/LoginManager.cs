using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{

    public InputField userNameInput;
    public InputField passWordInput;
    public Transform filePanel;
    public Transform loginPanel;

    //private string userName;
    //private string passWord;
    string url = "";
     string action = "Account/LoginTest?";
    //string action = "User/GetUserData?";

    private string sessionId;
    public static Dictionary<string, string> headers = new Dictionary<string, string>();
    public static Dictionary<string, string> responseheaders = new Dictionary<string, string>();


    void Start()
    {
        //if (File.Exists(Tools.SavedPath + "/" + Tools.BaseFolder + "/" + Tools.UserFolder + "/" + Tools.UserFile))
        //{
        //    //SceneManager.LoadScene("1125_builtinorder");
        //    //UIManger.Instance.StartToload();
        //}
        //else
        //{
        //    url = Tools.WebUrl + action;
        //    Tools.CreatDirectory();
        //}
        //url = Tools.WebUrl + action;
    }

    public void Login()
    {
        //if (userNameInput.text != string.Empty && passWordInput.text != string.Empty)
        //{
        //    StartCoroutine(ConnectWeb());
        //}
        //else if (userNameInput.text == string.Empty && passWordInput.text == string.Empty)
        //    MessageBox.Show("用户名,密码不能为空！", "Error");
        //else if (passWordInput.text == string.Empty)
        //    MessageBox.Show("密码不能为空！", "Error");
        //else
        //    MessageBox.Show("用户名不能为空！", "Error");

        StartCoroutine(ConnectWeb());
    }

    IEnumerator ConnectWeb()
    {
        //string requesturl = url + "name=" + userNameInput.text + "&" + "pwd=" + passWordInput.text;
        string requesturl = url + "name=" + "2507295011@qq.com" + "&" + "pwd=" + "Running@10km";

        WWW data = headers.ContainsKey("COOKIE") ? new WWW(requesturl) : new WWW(requesturl, null, headers);

        //WWW data = new WWW(requesturl);
        yield return data;
        if (data.error == null)
        {
            if (!data.text.Contains("失败"))
            {
                //responseheaders = data.responseHeaders;
                //this.GetSessionId(data.responseHeaders);
                //var d = data.responseHeaders["SET-COOKIE"];
                User user = JsonConvert.DeserializeObject<User>(data.text);
                SerializeMethod(user);

                MessageBox.Show("succeed!");

                loginPanel.gameObject.SetActive(false);
                filePanel.gameObject.SetActive(true);
                // SceneManager.LoadScene("main");
               // GetSessionId(data.responseHeaders);
            }
        }
        else
        {
            MessageBox.Show(data.error, "Connect Error");
        }

        //WWWForm form = new WWWForm();
        //form.AddField("name", userNameInput.text);
        //form.AddField("pwd", passWordInput.text);

        //WWW data = new WWW(url, form);
        //yield return data;
        //if (data.error == null)
        //{
        //    if (!data.text.Contains("失败"))
        //    {
        //        User user = JsonConvert.DeserializeObject<User>(data.text);
        //        SerializeMethod(user);
        //        MessageBox.Show("succeed!");
        //        loginPanel.gameObject.SetActive(false);
        //        filePanel.gameObject.SetActive(true);
        //        // SceneManager.LoadScene("main");
        //    }
        //}
        //else
        //{
        //    MessageBox.Show(data.error, "Connect Error");
        //}
    }
    private void GetSessionId(Dictionary<string, string> responseHeaders)
    {
        //Key = "SET-COOKIE"
        if (responseHeaders.ContainsKey("SET-COOKIE"))
        {
            string[] cookies = responseHeaders["SET-COOKIE"].Split(';');

            string temp = cookies[0];
            if (temp.Split('=')[0] == "ASP.NET_SessionId")
            {
                sessionId = temp.Split('=')[1];
                if (headers.ContainsKey("Cookie"))
                    headers["Cookie"] = sessionId;
                else
                headers.Add("Cookie", sessionId);
            }
        }
      
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

    public void InitInput()
    {
        userNameInput.text = "";
        passWordInput.text = "";
    }

}
[Serializable]
public class User
{
    //public string Id { get; set; }

    //public string Name { get; set; }

    //public string Password { get; set; }

    //public string Gender { get; set; }

    //public string Phone { get; set; }

    //public string Role { get; set; }
    public string Id { get; set; }
    public string UserName { get; set; }

    public List<string> Roles { get; set; }

    public string Gender { get; set; }
    public string Department { get; internal set; }
}

