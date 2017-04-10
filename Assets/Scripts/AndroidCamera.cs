using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AndroidCamera : MonoBehaviour
{

    // Use this for initialization
    public Action<string> OnImagePicked = delegate { };
    private static AndroidCamera _instance;
    public static AndroidCamera Instance
    {
        get { return _instance; }
    }
    private AndroidCamera() { }

    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }
    public void OpenCamera()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("TakePhoto");
    }

    public void OpenGallery()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("OpenGallery");
    }

    public void OpenImg(string path)
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

        string imageUri = path;
        // Debug.Log(imageUri);
        jo.Call("ShowImges", imageUri);
    }

    public void GetImagePath(string imagePath)
    {
        if (imagePath == null)
            return;
        OnImagePicked(imagePath);
    }

    public void GetTakeImagePath(string imagePath)
    {
        if (imagePath == null)
            return;
        OnImagePicked(imagePath);

    }


}