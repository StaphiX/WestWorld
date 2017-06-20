using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

public class DropboxTextures : MonoBehaviour
{
    const string sTexFolder = "/Textures";
    const string sTileFolder = "/Tiles";
    public Button dlButton;
    List<DownloadTile> tTiles = new List<DownloadTile>();

    void Start()
    {
        if(dlButton)
        {
            dlButton.onClick.AddListener(OnDLButtonClick);
        }
    }

    void OnDLButtonClick()
    {
        StartCoroutine(DownloadAll());
    }

    IEnumerator DownloadAll()
    {
        Debug.Log("Start Form Upload");

        string JSONPostURL = "{\"path\":\"" + sTexFolder + sTileFolder + "\"}";

        using (UnityWebRequest wwwGetFiles = GetDropboxRequest("list_folder", JSONPostURL))
        {
            yield return wwwGetFiles.Send();

            string sImageJSON = null;
            if (wwwGetFiles.isError)
            {
                Debug.Log(wwwGetFiles.error);
            }
            else
            {
                JObject tJSON = JObject.Parse(wwwGetFiles.downloadHandler.text);
                JArray tEntries = tJSON.Value<JArray>("entries");
                foreach (JObject tObject in tEntries)
                {
                    string sTag = tObject.Value<string>(".tag");
                    Debug.Log("Tag: " + sTag);

                    if (sTag.CompareTo("file") == 0)
                    {
                        yield return DownloadTileImage(tObject);
                    }
                }
            }
        }
    }

    UnityWebRequest GetDropboxRequest(string sURLExtension, string sJSON)
    {
        string TOKEN = "1rY4OcE1fM8AAAAAAAACFLglJOI46CYMEQgtVPxfFuCoziNv9t8SUqvBiCFjK3jJ";

        // Post mangles the JSON data - Setting Put with method POST works
        UnityWebRequest www = UnityWebRequest.Put("https://api.dropboxapi.com/2/files/" + sURLExtension, sJSON);
        www.method = "POST";
        www.SetRequestHeader("Authorization", "Bearer " + TOKEN);
        www.SetRequestHeader("Content-Type", "application/json");

        return www;
    }

    IEnumerator DownloadTileImage(JObject tFileJSON)
    {
        string sName = tFileJSON.Value<string>("name");
        string sContentHash = tFileJSON.Value<string>("content_hash");
        // Debug.Log("Name: " + sName);
        string sPath = tFileJSON.Value<string>("path_lower");
        string sImageJSON = "{\"path\":\"" + sPath + "\"}";
        Debug.Log("Name: "+ sName + "Path: " + sPath);

        string sLink = null;
        if (sImageJSON != null)
        {
            using (UnityWebRequest wwwGetLink = GetDropboxRequest("get_temporary_link", sImageJSON))
            {
                yield return wwwGetLink.Send();

                if (wwwGetLink.isError)
                {
                    Debug.Log(wwwGetLink.error);
                }
                else
                {
                    JObject tLinkJSON = JObject.Parse(wwwGetLink.downloadHandler.text);
                    sLink = tLinkJSON.Value<string>("link");
                    Debug.Log("Link: " + sLink);
                }
            }
        }

        if (sLink != null)
        {

            using (UnityWebRequest wwwGetImage = UnityWebRequest.Get(sLink))
            {
                yield return wwwGetImage.Send();

                if (wwwGetImage.isError)
                {
                    Debug.Log(wwwGetImage.error);
                }
                else
                {
                    Debug.Log("Image Bytes: " + wwwGetImage.downloadedBytes.ToString());
                    if (wwwGetImage.downloadedBytes > 0)
                    {
                        string sLocalImagePath = "Tile/" + sName;
                        DirectoryUtil.SavePNGBytes("Tile/" + sName, wwwGetImage.downloadHandler.data);

                        Texture2D tImage = new Texture2D(2, 2);
                        tImage.LoadImage(wwwGetImage.downloadHandler.data);

                        DownloadTile tTile = new DownloadTile(sName, sContentHash, sLocalImagePath, tImage);
                        tTiles.Add(tTile);

                        tTile.Save();
                    }
                }
            }
        }
    }
}