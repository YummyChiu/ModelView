using System.Collections;
using System.Collections.Generic;
using TinyTeam.UI;
using UnityEngine;

public class GameEntry : MonoBehaviour {


    private static GameEntry instance;
    private GameEntry() { }

    public static GameEntry Instance
    {
        get {
            if(instance == null)
                instance = GameObject.FindObjectOfType(typeof(GameEntry)) as GameEntry;
            return instance;
        }
    }

    public void LoadModel()
    {
        ReadModelSQLite r = Camera.main.GetComponent<ReadModelSQLite>();
        r.LoadModel();
    }
	void Start () {

         TTUIPage.ShowPage<UILogin>();
       // TTUIPage.ShowPage<UICompoentInfo>();
       //TTUIPage.ShowPage<UIIssuePanel>("[871714]");
    }
	

}
