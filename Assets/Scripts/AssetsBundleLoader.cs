using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AssetsBundleLoader : MonoBehaviour
{
    [SerializeField] private AssetBundle assetBundle;

    private string path = string.Empty;

    public void LoadFirstAsset()
    {
        UnloadAssetBundle();
        LoadAssetBundle("prefabcubeone", (isSuccess, asset) => { OnLoadAsset(isSuccess, asset, "prefabcubeone"); });
    }

    public void LoadSecondAsset()
    {
        UnloadAssetBundle();
        LoadAssetBundle("prefabcubetwo", (isSuccess, asset) => { OnLoadAsset(isSuccess, asset, "prefabcubetwo"); });
    }

    private void OnLoadAsset(bool isSuccess, AssetBundle asset, string assetName)
    {
        if (isSuccess)
        {
            Debug.Log("trying to instiantiate..");
            Instantiate(assetBundle.LoadAsset<GameObject>(assetName));
        }
        else
        {
            Debug.Log("Error.");
        }
    }

    #region Download
    public void DownloadAssetBundle(string url)
    {
        UnloadAssetBundle();
        StartCoroutine(LoadFromCacheOrDownload(url));
    }

    public void LoadAssetBundle(string bundleName, Action<bool, AssetBundle> callback)
    {
        if (assetBundle != null)
            return;

        Debug.Log(Path.Combine(System.IO.Directory.GetCurrentDirectory() + "/AssetsBundle/", bundleName));
        //return;

#if UNITY_EDITOR
        path = Path.Combine(System.IO.Directory.GetCurrentDirectory() + "/AssetsBundle/", bundleName);
#elif UNITY_IOS
        string filePath = Path.Combine (Application.streamingAssetsPath + "/Raw", fileName); 
#elif UNITY_ANDROID
        //path = Path.Combine("jar:file://" + Application.streamingAssetsPath + "/AssetBundle/", bundleName);
         path = Path.Combine(Application.streamingAssetsPath + "/AssetBundle/", bundleName);
#endif

        assetBundle = AssetBundle.LoadFromFile(path);

        if (assetBundle != null)
        {
            Debug.Log("Succes to load AssetBundle!");
            callback?.Invoke(true, assetBundle);
        }
        else
        {
            Debug.Log("You haveNot AssetBundle!");
            callback?.Invoke(false, null);
        }
    }

    private IEnumerator LoadFromCacheOrDownload(string url)
    {
        while (!Caching.ready)
            yield return null;

        var www = WWW.LoadFromCacheOrDownload(url, 1);
        yield return www;

        while (!www.isDone)
        {
            Debug.Log("Downloading..." + www.progress);
            yield return null;
        }

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
            yield return null;
        }

        if (www.assetBundle != null)
        {
            assetBundle = www.assetBundle;
            Debug.Log("Sucess Load");
        }
        else
        {
            Debug.Log("Fail Load");
        }
    }

    private IEnumerator DownloadAsync(string url)
    {
        UnityWebRequest uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        uwr.downloadHandler = new DownloadHandlerAssetBundle(url, 0);
        UnityWebRequestAsyncOperation asyncOperation = uwr.SendWebRequest();

        while (!asyncOperation.isDone)
        {
            Debug.Log("Downloading..." + uwr.downloadProgress);
            yield return null;
        }

        Debug.Log("Downloading..." + uwr.downloadProgress + "Download Complete");

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            // Get downloaded asset bundle
            assetBundle = DownloadHandlerAssetBundle.GetContent(uwr);
        }
    }

    #endregion
    public void UnloadAssetBundle()
    {
        if (assetBundle != null)
            assetBundle.Unload(false);
    }

    private void OnDestroy()
    {
        UnloadAssetBundle();
    }
}
