using System;
using System.Collections.Generic;
using TriLibCore;
using TriLibCore.General;
using UnityEngine;

/// <summary>
/// This class is used to import models with the default loading options provided by TriLib 2 from the specified URL. 
/// These loading options will need to be customized according to our requirements for different parts of the game in the future.
/// </summary>
public class ModelImporter
{
    public event Action OnModelLoaded;

    // Loads a Model in the scene with default Loading Options given by TriLib. 
    public void LoadModelFromURL(string urlPath, GameObject ModelParent)
    {
        var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
        var webRequest = AssetDownloader.CreateWebRequest(urlPath);

        // Important: If you're downloading models from files that are not Zipped, you must pass the model extension as the last parameter from this call (Eg: "fbx")
        // Begins the model downloading.
        AssetDownloader.LoadModelFromUri(webRequest, OnLoad, OnMaterialsLoad, OnProgress, OnError, ModelParent /*Wrapper/Parent GameObject*/, assetLoaderOptions, null, null);
    }

    // Loads all the Models specified in the given URLs with default Loading Options given by TriLib. 
    public void LoadMultipleModelsFromURL(List<string> urlPathList, GameObject ModelParent)
    {
        foreach (string url in urlPathList)
        {
            LoadModelFromURL(url, ModelParent);
        }
    }

    #region TriLib Loading CallBacks

    private void OnProgress(AssetLoaderContext assetLoaderContext, float progress)
    {
        Debug.Log(progress);
    }

    private void OnError(IContextualizedError contextualizedError)
    {
        Debug.Log("error while loading: " + contextualizedError.ToString());
    }

    private void OnLoad(AssetLoaderContext assetLoaderContext)
    {
        var myLoadedGameObject = assetLoaderContext.RootGameObject;
        myLoadedGameObject.SetActive(false);
        Debug.Log("asset loaded");
    }

    private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
    {
        var myLoadedGameObject = assetLoaderContext.RootGameObject;
        myLoadedGameObject.SetActive(true);
        Debug.Log("Materials loaded");
        OnModelLoaded?.Invoke();
    }

    #endregion



}