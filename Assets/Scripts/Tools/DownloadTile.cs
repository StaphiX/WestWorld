using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DownloadTile
{
    string sFilename = null;
    string sContentHash = null;
    string sLocalImagePath = null;
    [NonSerialized]
    Texture2D tTexture = null;

    public DownloadTile(string _sFilename, string _sContentHash, string _sLocalImagePath, Texture2D _tTexture)
    {
        sFilename = _sFilename;
        sContentHash = _sContentHash;
        sLocalImagePath = _sLocalImagePath;
        tTexture = _tTexture;
    }

    public void Save()
    {
        if (sLocalImagePath != null && sFilename != null && sContentHash != null)
        {
            string sSaveFileName = sFilename.Remove(sFilename.LastIndexOf('.')) + ".dat";
            Debug.Log("Save Name: " + sSaveFileName);
            string sPath = sSaveFileName;
            DirectoryUtil.SaveFile(sPath, this);
        }
    }

    public static DownloadTile LoadFromFile(string sFile)
    {
        return DirectoryUtil.LoadFile<DownloadTile>(sFile);
    }
}
