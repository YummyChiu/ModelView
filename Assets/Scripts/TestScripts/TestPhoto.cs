//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class TestPhoto : MonoBehaviour
//{

//    public Button BtnCamera;
//    public Button BtnPhoto;
//    public Button BtnSave;
//    public Text TextPath;
//    public Image Image;

//    public Toggle toggle;

//    private Texture2D saveTextured;
//    private string savedPath;

//    // Use this for initialization
//    void Start()
//    {
//        BtnCamera.onClick.AddListener(OnClickCamera);
//        BtnPhoto.onClick.AddListener(OnClickPhoto);  
//    }

//    private void OnClickPhoto()
//    {
//        GetImageFromGallery();
//    }

//    private void OnClickCamera()
//    {
//        GetImageFromCamera();
//    }

//    //private void OnClickSave()
//    //{
//    //    SaveImage();
//    //}
//    //public void SaveImage()
//    //{
//    //    AndroidCamera.Instance.OnImageSaved += OnImageSaved;
//    //    AndroidCamera.Instance.SaveImageToGallery(saveTextured);
//    //}

//    public void GetImageFromGallery()
//    {
//        AndroidCamera.Instance.OnImagePicked += OnImagePicked;
//        AndroidCamera.Instance.GetImageFromGallery();
//    }

//    public void GetImageFromCamera()
//    {
//        AndroidCamera.Instance.OnImagePicked += OnImagePicked;
//        AndroidCamera.Instance.GetImageFromCamera();      
//    }

//    private void OnImageSaved(GallerySaveResult result)
//    {
        
//        AndroidCamera.Instance.OnImageSaved -= OnImageSaved;

//        if (result.IsSucceeded)
//        {
//            MessageBox.Show("Image saved to gallery \n" + "Path: " + result.imagePath, "Saved");
//        }
//        else {
//            MessageBox.Show("Image save to gallery failed" + result.imagePath, "Failed");
//        }

//    }

//    private void OnImagePicked(AndroidImagePickResult result)
//    {
//        Debug.Log("OnImagePicked");
//        if (result.IsSucceeded)
//        {
//            MessageBox.Show("Succeeded, path: " + result.ImagePath, "Image Pick Rsult");
//            saveTextured = result.Image;
//            Texture2D texture2d = result.Image;
//            Sprite sprite = Sprite.Create(texture2d, new Rect(0.0f, 0.0f, texture2d.width, texture2d.height), new Vector2(0.5f, 0.5f));
//            Image.sprite = sprite;
//            TextPath.text = result.ImagePath;
//        }
//        else {
//            MessageBox.Show("Failed", "Image Pick Rsult");
//        }

//        AndroidCamera.Instance.OnImagePicked -= OnImagePicked;
//    }
//}
