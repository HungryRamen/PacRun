using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;

public class MainUIMgr : MonoBehaviour
{

    private string path;
    public GameObject RecordTextObj;
    public GameObject HelpPanelObj;

    void Awake()
    {
        path = Application.persistentDataPath + "/Record.txt";
    }

    public void SaveRecord(float fRecord)
    {
        float fTempRecord = 0f;
        if (File.Exists(path))
        {
            StreamReader Read = File.OpenText(path);
            string Input = "";
            Input = Read.ReadLine();
            if (StringNullCheck(Input)) { return; }
            fTempRecord = Convert.ToSingle(Input);

            Read.Close();
        }
        if (fTempRecord< fRecord)
        {
            StreamWriter Write = new StreamWriter(path);
            Write.WriteLine(fRecord);
            Write.Flush();
            Write.Close();
        }
    }

    public void OnRecordTextObj()
    {
        float fRecord = 0f;
        if (File.Exists(path))
        {
            StreamReader Read = File.OpenText(path);
            string Input = "";
            Input = Read.ReadLine();
            if (StringNullCheck(Input)) { return; }
            fRecord = Convert.ToSingle(Input);
            Read.Close();
        }
        StringBuilder str = new StringBuilder("");
        str.Append("Best Time: ");
        str.Append(fRecord.ToString("N2"));
        RecordTextObj.GetComponentInChildren<Text>().text = str.ToString();
        RecordTextObj.SetActive(true);
    }

    public void OnHelpPanel()
    {
        HelpPanelObj.SetActive(true);
    }

    private bool StringNullCheck(string s)
    {
        return s == null;
    }
}
