using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class testmessage : MonoBehaviour {

    string strPhotoFile = Tools.Instance.SavedPath + "/" + Tools.BaseFolder + "/" + "test.jpg";
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void test()
    {
        StartCoroutine(fnTestPostImgToServer());
    }

    public IEnumerator fnTestPostImgToServer()
    {
        FileStream fm = File.OpenRead(strPhotoFile);
        byte[] imgData = new byte[fm.Length];
        fm.Read(imgData, 0, (int)fm.Length);
        fm.Close();

        string strPHPURL = "http://192.168.0.130:8089/User/FileUpload";
        WWWForm form = new WWWForm();
        form.AddField("enctype", "multipart/form-data");
        form.AddBinaryData("file", imgData, "test.jpg", "image/jpg");
        WWW uploadData = new WWW(strPHPURL, form);
        yield return uploadData;
        if (uploadData.error != null)
            MessageBox.Show(uploadData.error);
        else
        {
            
            MessageBox.Show(uploadData.text);
        }
           
       
    }
}
