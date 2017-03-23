using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class BuildInOrder : MonoBehaviour
{

    //public Transform BuildInOrderPanel;
    private Text StartTimeText;
    private Text EndTimeText;
    private Slider ProgressSlider;
    private Button BuildBtn;
    private Button CancelBtn;
    private Button BuildBtn2x;

    private List<string> timeList;
    private List<DateTime> dateList;
    public InputField startTime;
    public InputField endTime;
    void Start()
    {
        Transform Text = transform.Find("Text");
        StartTimeText = Text.Find("StartTimeText").GetComponent<Text>();
        EndTimeText = Text.Find("EndTimeText").GetComponent<Text>();
        BuildBtn = transform.Find("BuildBtn").GetComponent<Button>();
        CancelBtn = transform.Find("CancelBtn").GetComponent<Button>();
        BuildBtn2x = transform.Find("BuildBtn2x").GetComponent<Button>();

        BuildBtn.onClick.AddListener(delegate()
        {
            BuildMethod(0.5f);
        }
        );

        BuildBtn2x.onClick.AddListener(delegate()
        {
            BuildMethod(0.1f);
        });
        CancelBtn.onClick.AddListener(CancelClick);
        timeList = test();
        dateList = testDate();
        
        StartTimeText.text = timeList[0];
        EndTimeText.text = timeList[timeList.Count - 1];

    }

    private List<DateTime> testDate()
    {
        List<DateTime> tempdateList = new List<DateTime>();
        foreach (var item in ReadModelXML.goList)
        {
            string name = item.name;
            string data = name.Substring(name.IndexOf('[') + 1, name.LastIndexOf(']') - name.IndexOf('[') - 1);
            DateTime date = DateTime.Parse(data);

            if (!tempdateList.Contains(date))
            {
                tempdateList.Add(date);
            }
           
        }
        return tempdateList;

    }

    List<string> test()
    {
        List<string> temptimeList = new List<string>();
       

        foreach (var item in ReadModelXML.goList)
        {
            string name = item.name;
            string data = name.Substring(name.IndexOf('[') + 1, name.LastIndexOf(']') - name.IndexOf('[') - 1);
            DateTime date = DateTime.Parse(data);
            if (!temptimeList.Contains(data))
            {
                temptimeList.Add(data);
            }
        }
        return temptimeList;

    }

    void BuildMethod(float rate)
    {
        StartCoroutine(Build(rate));
    }
    IEnumerator Build(float rate)
    {
        foreach (var item in ReadModelXML.goList)
        {
            item.SetActive(false);
        }
        int end = 0;
        int start = 0;
        if (startTime.text == "")
        {
            start = 0;
        }
        else
        {
            string temptext = startTime.text;
            DateTime tempdate = DateTime.Parse(temptext);

            for (int i = 0; i < dateList.Count; i++)
            {

                if (tempdate.CompareTo(dateList[i]) > 0)
                {
                    start++;
                }
                else {
                    break;
                }

            }
        }
        if (endTime.text == "")
        {
            end = dateList.Count;
        }
        else
        {
            string temptext = endTime.text;
            DateTime tempdate = DateTime.Parse(temptext);

            for (int i = 0; i < dateList.Count; i++)
            {

                if (tempdate.CompareTo(dateList[i]) > 0)
                {
                    end++;
                }
                else {
                    break;
                }

            }
        }

        for (int i =start; i < end; i++)
        {
            List<GameObject> tempGameobjects = new List<GameObject>();
            tempGameobjects = ReadModelXML.goList.FindAll(o => o.name.Substring(o.name.IndexOf('[') + 1, o.name.LastIndexOf(']') - o.name.IndexOf('[') - 1) == timeList[i]);
            foreach (var go in tempGameobjects)
            {
                go.SetActive(true);

            }
            yield return new WaitForSeconds(rate);

        }
    }

    void CancelClick()
    {
        transform.gameObject.SetActive(false);
        foreach (var item in ReadModelXML.goList)
        {
            item.SetActive(true);
        }
    }
}
