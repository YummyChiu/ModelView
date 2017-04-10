using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Com.UI;
using System.Runtime.InteropServices;
using System;
using System.IO;

public class Test : MonoBehaviour
{

    public Button BtnSelect;
    public Button BtnCommit;
    public Text Tips;
    public InputField Remarks;
    string path;
    void Start()
    {
        BtnSelect.onClick.AddListener(TestOpenFileWindow);
        BtnCommit.onClick.AddListener(OnClickUploadFile);
    }

    private void OnClickUploadFile()
    {
        StartCoroutine(TestPostToServer());
    }

    public IEnumerator TestPostToServer()
    {
        FileStream fm = File.OpenRead(path);
        string name = Path.GetFileName(path);
        string filetype =  Path.GetExtension(path).Substring(1);
        Debug.Log(filetype);
        byte[] imgData = new byte[fm.Length];
        fm.Read(imgData, 0, (int)fm.Length);
        fm.Close();
        string url = Tools.WebUrl + "ModelData/AddMobileCompoent";
        WWWForm form = new WWWForm();
        form.AddField("enctype", "multipart/form-data");
        form.AddBinaryData("file", imgData, name, "image/"+filetype);
        form.AddField("componentId", "871714");
        form.AddField("remarks", Remarks.text);

        WWW uploadData = new WWW(url, form);
        yield return uploadData;
        if (uploadData.error != null)
            MessageBox.Show(uploadData.error);
        else
        {

            MessageBox.Show(uploadData.text);
        }
    }

    void TestOpenFileWindow()
    {
        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = "图片文件(*.jpg*.png*)\0*.jpg;*.png";
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        ofn.initialDir = UnityEngine.Application.dataPath;//默认路径
        ofn.title = "请选择图片！";
        ofn.defExt = "jpg";//显示文件的类型 
                           //注意 一下项目不一定要全选 但是0x00000008项不要缺少
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR
        if (DllTest.GetOpenFileName(ofn))
        {
            path = ofn.file;
            Tips.text = path;

            MessageBox.Show(path,"Tips");
        }
    }
}
//    public UTree tree;

//    private List<UTreeNodeData> data1;
//    private List<UTreeNodeData> data2;

//    // Use this for initialization
//    void Start () {
//        //List<string> levelList = new List<string>
//        //{
//        //  "结构B2","结构B1","结构1","结构2","结构3","结构4","结构5","结构6",
//        //  "结构7","结构8","结构9","结构10","结构11","结构12","结构13","结构14",
//        //  "结构15","结构16","结构17","结构18","结构RMF","结构RF","结构TF"
//        //};

//        ////UTreeNodeData levelChildNode = new UTreeNodeData(2, "结构B2");
//        //UTreeNodeData levelTree = new UTreeNodeData(1, "楼梯");
//        ////levelTree.AddChild(levelChildNode);
//        //int count = 0;
//        //foreach (var item in levelList)
//        //{
//        //    count++;
//        //    UTreeNodeData levelChildNode = new UTreeNodeData(count, item);
//        //    levelTree.AddChild(levelChildNode);
//        //}
//        //data1 = new List<UTreeNodeData>();
//        //data1.Add(levelTree);
//        //UTreeNodeData a1111 = new UTreeNodeData(11111,"a1111");
//        UTreeNodeData a1111 = new UTreeNodeData(1001111,"a1111");
//        UTreeNodeData a111 =  new UTreeNodeData(100111, "a111");
//        a111.AddChild(a1111);
//        UTreeNodeData a11 = new UTreeNodeData(10011, "a11");
//        //a111.AddChild(a1111);
//        a11.AddChild(a111);
//        UTreeNodeData a1 = new UTreeNodeData(1001, "a1");
//        a1.AddChild(a11);
//        UTreeNodeData a2 = new UTreeNodeData(1002, "a2");
//        UTreeNodeData a3 = new UTreeNodeData(1003, "a3");
//        UTreeNodeData b1 = new UTreeNodeData(2001, "b1");
//        UTreeNodeData b2 = new UTreeNodeData(2002, "b2");
//        UTreeNodeData b3 = new UTreeNodeData(2003, "b3");
//        UTreeNodeData c1 = new UTreeNodeData(3001, "c1");
//        UTreeNodeData a = new UTreeNodeData(1000, "a");
//        a.AddChild(a1);
//        a.AddChild(a2);
//        a.AddChild(a3);
//        UTreeNodeData b = new UTreeNodeData(2000, "b");
//        b.AddChild(b1);
//        b.AddChild(b2);
//        b.AddChild(b3);
//        UTreeNodeData c = new UTreeNodeData(3000, "c");
//        c.AddChild(c1);
//        UTreeNodeData d = new UTreeNodeData(4000, "d");
//        UTreeNodeData e = new UTreeNodeData(5000, "e");
//        data1 = new List<UTreeNodeData>();
//        data1.Add(a);
//        data1.Add(b);
//        data1.Add(c);
//        data1.Add(d);
//        data1.Add(e);

//        UTreeNodeData i1 = new UTreeNodeData(4001, "i1");
//        UTreeNodeData i = new UTreeNodeData(4000, "i");
//        i.AddChild(i1);
//        UTreeNodeData j1 = new UTreeNodeData(5001, "j1");
//        UTreeNodeData j = new UTreeNodeData(5000, "j");
//        data2 = new List<UTreeNodeData>();
//        data2.Add(i);
//        data2.Add(j);
//    }

//	// Update is called once per frame
//	void Update () {

//	}

//    public void SetTreeData1() {
//        tree.SetDataProvider(data1);
//    }

//    public void SetTreeData2() {
//        tree.SetDataProvider(data2);
//    }
//}

