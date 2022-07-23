using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;

public class JSonDataService : IDataService
{
    public bool SaveData<T>(string RelativePath, T Data)
    {
        try
        {
            string path = Application.persistentDataPath + RelativePath;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(Data, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }
            ));
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public T LoadData<T>(string RelativePath)
    {
        string path = Application.persistentDataPath + RelativePath;
        if (!File.Exists(path))
        {
            Debug.LogError($"Can't load file at {path}. File Doesn't Exist!");
            throw new FileNotFoundException($"{path} does not exist!");
        }
        try
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return data;
        }
        catch(Exception e)
        {
            Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
            throw e;
        }
    }
}
