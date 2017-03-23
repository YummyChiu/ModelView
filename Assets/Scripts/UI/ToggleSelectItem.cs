using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class ToggleSelectItem : MonoBehaviour {

    // Use this for initialization
    private Toggle toggle;
    private Dropdown dropdown;
    private Text text;
    private Dictionary<Dictionary<string, double>, bool> fixedLengthDict;
	void Awake () {
        toggle = transform.GetComponentInChildren<Toggle>();
        text = toggle.GetComponentInChildren<Text>();
        dropdown = transform.GetComponentInChildren<Dropdown>();
        Debug.Log("Awake" + text);
        Debug.Log("start" + toggle.isOn);
    }
     public void Init(string value)
    {
      
        text.text = value;
    }
    public void GetSelectInfo()
    {
        ////fixedLengthDict = new Dictionary<Dictionary<string, double>, bool>();

        //string toggleText = text.text;
        //bool check = toggle.isOn;
        //string dropdownText = dropdown.options[dropdown.value].text;
        //Debug.Log("Afterclick" + text);
        //Debug.Log("Afterclick" + toggle.isOn);
        //Debug.Log(toggleText + "___" + check + "___" + dropdownText);
    }
    // Update is called once per frame
    void Update () {
	
	}
}
