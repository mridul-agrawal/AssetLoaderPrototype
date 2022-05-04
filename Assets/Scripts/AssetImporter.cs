using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TriLibCore;
using UnityEngine;

public class AssetImporter : SingletonGeneric<AssetImporter>
{
    private ModelImporter modelImporter;
    private AnimationImporter animationImporter;
    [SerializeField] List<string> ModelUrls;
    [SerializeField] List<string> AnimationUrls;
    [SerializeField] List<string> TextureUrls;
    public GameObject modelParent;


    protected override void Awake()
    {
        base.Awake();
        modelImporter = new ModelImporter();
    }

    private void OnEnable()
    {
        modelImporter.OnModelLoaded += LoadAnimations;
    }

    private void Start()
    {
        animationImporter = GetComponent<AnimationImporter>();
        LoadModels();
    }

    private void LoadModels()
    {
        modelImporter.LoadMultipleModelsFromURL(ModelUrls, modelParent);
    }

    private void LoadAnimations()
    {
        LoadTextures();
        animationImporter.SetupAnimatorOnModel(modelParent.transform.GetChild(0).gameObject);
        StartCoroutine(animationImporter.ImportAnimationFile(AnimationUrls[0]));
    }

    private void LoadTextures()
    {
        List<SkinnedMeshRenderer> renderersToApplyOn = modelParent.GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
        Debug.Log("number of renderers detected:    " + renderersToApplyOn.Count);
        StartCoroutine(TextureImporter.ImportAndApplyTextures(TextureUrls[0], renderersToApplyOn));
    }

    private void OnDisable()
    {
        modelImporter.OnModelLoaded -= LoadAnimations;
    }

}


