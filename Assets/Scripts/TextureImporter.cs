using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class TextureImporter
{

    /// <summary>
    /// This method imports a texture from the given URL and applies it on to the specified list of Renderers.
    /// </summary>
    /// <param name="texture_url"> URL address to download texture from. </param>
    /// <param name="renderersToApplyOn"> List of Renderers to aaply this texture on. </param>
    /// <returns></returns>
    public static IEnumerator ImportAndApplyTextures(string texture_url, List<SkinnedMeshRenderer> renderersToApplyOn)
    {
        Debug.Log("Inside TextureImporter");
        using (UnityWebRequest webRequestForTexture = UnityWebRequestTexture.GetTexture(texture_url))
        {
            yield return webRequestForTexture.SendWebRequest();

            if ((webRequestForTexture.result == UnityWebRequest.Result.ConnectionError)
                || (webRequestForTexture.result == UnityWebRequest.Result.ProtocolError))
            {
                Debug.Log("Error occurred: " + webRequestForTexture.error);
            }
            else
            {
                Debug.Log("Successfully connected");

                DownloadHandlerTexture textureDownloadHandler = webRequestForTexture.downloadHandler as DownloadHandlerTexture;

                Texture2D downloaded_texture = textureDownloadHandler.texture;

                foreach (SkinnedMeshRenderer renderer in renderersToApplyOn)
                {
                    renderer.material.SetTexture("_MainTex", downloaded_texture);
                }
            }
        }
    }
}
