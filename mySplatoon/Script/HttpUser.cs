using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System;

public class HttpUser : MonoBehaviour
{
    PlayerCharacter player;

    void Start()
    {
        player = FindObjectOfType<PlayerCharacter>();
        SendSave();
    }

    void Update ()
    {

        if(Input.GetKeyDown(KeyCode.I))
        {
            SendLoad();
        }
	}

    public static string Save(string user,string data)
    {
        string senData = "user=" + user + "&" + "data=" + data;
        string url = "http://192.168.199.194:8080/save/?" + senData;

        string backMsg = "";

        try
        {
            System.Net.WebRequest httpRquest = System.Net.HttpWebRequest.Create(url);
            httpRquest.Method = "GET";

            System.Net.WebResponse response = httpRquest.GetResponse();
            System.IO.Stream responseStream = response.GetResponseStream();
            System.IO.StreamReader reader = new System.IO.StreamReader(responseStream, System.Text.Encoding.UTF8);
            backMsg = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();
            responseStream.Close();
            responseStream.Dispose();
        }
        catch(Exception e1)
        {
            Debug.Log(e1.ToString());
        }
        return backMsg;
    }

    public static string Load(string user)
    {
        string senData = "user=" + user;
        string url = "http://192.168.199.194:8080/load/?" + senData;

        string backMsg = "";
        try
        {
            System.Net.WebRequest httpRquest = System.Net.HttpWebRequest.Create(url);
            httpRquest.Method = "GET";
            System.Net.WebResponse response = httpRquest.GetResponse();
            System.IO.Stream responseStream = response.GetResponseStream();
            System.IO.StreamReader reader = new System.IO.StreamReader(responseStream, System.Text.Encoding.UTF8);//将返回的字符流以UTF8格式赋给StreamReader
            backMsg = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();
            responseStream.Close();
            responseStream.Dispose();

        }
        catch (Exception e1)
        {
            Debug.Log(e1.ToString());
        }
        return backMsg;
    }

    public void SendSave()
    {
        string username = "my";

        string data = player.health.ToString();
        string backmsg = Save(username, data);
    }

    public void SendLoad()
    {
        string username = "my";
        string backmsg = Load(username);
        Debug.Log(backmsg);
    }
}
