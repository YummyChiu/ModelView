using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserInfo : MonoBehaviour {

    public Text UserName;
    public Text UserTitle;
    public Button SignoutBtn;
    public Button SettingBtn;

    private string userfilePath ;

    // Use this for initialization
    void Start () {
        userfilePath = Tools.Instance.SavedPath + "/" + Tools.BaseFolder + "/" + Tools.UserFolder + "/" + Tools.UserFile;;

        GetUserInfo();

    }
	
	
    public void GetUserInfo()
    {
        User user = ReserializeLocalFile();
        //UserName.text = user.Name;
       // UserTitle.text = user.Role;
        //SignoutBtn.onClick.AddListener
    }

    private  User ReserializeLocalFile()
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
            return new User { UserName= "null", Gender="null" };
        }
    }


    //get from internet

    private void Signout()
    {
        File.Delete(userfilePath);
        SceneManager.LoadScene("login");
    }

}
