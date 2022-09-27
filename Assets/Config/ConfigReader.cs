using Assets.Scripts;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConfigReader
{
    private static ConfigReader _instance = null;

    public static ConfigReader Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ConfigReader();
            }
            return _instance;
        }
    }

    public T GetInfos<T>()
    {
        var json = Resources.Load(GameManager.Instance.TargetServer + "\\Connect") as TextAsset;
        GameManager.Instance.WriteTxt("./log1.txt", json.ToString());
        var b = json.ToString();
        try
        {
            var a = JsonConvert.DeserializeObject<T>(b);
            return a;
        }
        catch(Exception e)
        {
            GameManager.Instance.WriteTxt("./log.txt", e.ToString());
            
        }
        throw new Exception();
    }
}
