using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class AnimationImporter : MonoBehaviour
{
    [SerializeField] private AnimatorOverrider animatorOverrider;
    private Animator playerAnimator;
    [SerializeField] RuntimeAnimatorController runtimePlayerAnimatorController;

    private void Start()
    {
        animatorOverrider = GetComponent<AnimatorOverrider>();
    }

    public IEnumerator ImportAnimationFile(string animationURL)
    {
        UnityWebRequest www = new UnityWebRequest(animationURL);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Retrieve results as binary data
            byte[] results = www.downloadHandler.data;
            string path = Application.dataPath + "/Resources/SillyDanceRecreation.anim";
            // Create a .anim file
            File.WriteAllBytes(path, results);
            Debug.Log("File Created");
            
            // LoadAnimationFile("SillyDanceRecreation");
            StartCoroutine(LoadAnimationFileAsync("SillyDanceRecreation"));
        }
    }

    /// <summary>
    /// Loads animation file from resource folder.
    /// </summary>
    /// <param name="filePath"></param>
    private void LoadAnimationFile(string filePath)
    {
        AnimationClip animationFile = Resources.Load<AnimationClip>(filePath);
#if UNITY_EDITOR
        RefreshDatabase();
#endif
        Debug.Log(animationFile);
        SetAnimation(animationFile);
    }


    /// <summary>
    /// Loads animation file asynchronously from resource folder.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private IEnumerator LoadAnimationFileAsync(string filePath)
    {
        ResourceRequest resourceRequest = Resources.LoadAsync<AnimationClip>(filePath);
        yield return new WaitUntil(() => resourceRequest.isDone);
#if UNITY_EDITOR
        RefreshDatabase();
#endif
        AnimationClip animationFile = resourceRequest.asset as AnimationClip;
        Debug.Log(animationFile);
        SetAnimation(animationFile);
    }

#if UNITY_EDITOR
    /// <summary>
    /// Need to Refresh the database cause the loaded animation file is not being detected in Resource folder while in Unity Editor.
    /// </summary>
    void RefreshDatabase()
    {
        AssetDatabase.Refresh();
    }
#endif

    /// <summary>
    /// This method is used to Play the given animation.
    /// </summary>
    /// <param name="clipToSet"></param>
    private void SetAnimation(AnimationClip clipToSet)
    {
        LoopAnimationClip(clipToSet);
        Debug.Log("Animation Clip has been looped", clipToSet);
        animatorOverrider.SetAnimatorControllerWithAnimation(1, clipToSet, 0);
    }

    /// <summary>
    /// Modify the Animation Clip Settings to loop the the animation.
    /// </summary>
    /// <param name="clipToSet"></param>
    private void LoopAnimationClip(AnimationClip clipToSet)
    {
        /*AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clipToSet);
        settings.loopTime = true;
        AnimationUtility.SetAnimationClipSettings(clipToSet, settings);*/
        clipToSet.wrapMode = WrapMode.Loop;
    }

    public void SetupAnimatorOnModel(GameObject model)
    {
        Destroy(model.GetComponent<Animation>());
        playerAnimator = model.AddComponent<Animator>();
        animatorOverrider.AddPlayerAnimatorReference(playerAnimator, runtimePlayerAnimatorController);

    }

}
