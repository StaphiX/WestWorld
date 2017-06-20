using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class DirectoryUtil
{
    static string sSaveDirectory = Application.persistentDataPath + "/";


    public static bool SaveFile(string sFilePath, object tData)
    {
        string sFullPath = sSaveDirectory + sFilePath;

        CheckDirectory(sFullPath);
     
        Debug.Log("Full Save Path: " + sFullPath);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(sSaveDirectory + sFilePath, FileMode.OpenOrCreate);

        if (file != null)
        {
            bf.Serialize(file, tData);
            file.Close();

            return true;
        }

        return false;
    }

    public static T LoadFile<T>(string sFilePath)
    {
        string sPath = sSaveDirectory + sFilePath;
        if (File.Exists(sPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(sPath, FileMode.Open);
            T dataObject = (T)bf.Deserialize(file);
            file.Close();

            return dataObject;
        }

        return default(T);
    }

    public static void SavePNGBytes(string sFilePath, byte[] tBytes)
    {
        string sFullPath = sSaveDirectory + sFilePath;

        CheckDirectory(sFullPath);

        FileStream file = File.Open(sSaveDirectory + sFilePath, FileMode.OpenOrCreate);
        var binary = new BinaryWriter(file);
        binary.Write(tBytes);
        file.Close();
    }

    static void CheckDirectory(string sFilePath)
    {
        string sDirectory = Path.GetDirectoryName(sFilePath);
        if (!Directory.Exists(sDirectory))
        {
            Directory.CreateDirectory(sDirectory);
        }
    }
}
