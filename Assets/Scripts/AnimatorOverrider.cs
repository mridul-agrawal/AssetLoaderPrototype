using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorOverrider : MonoBehaviour
{
    private Animator player_animator;
    [SerializeField] private AnimatorOverrideController[] overrideControllers;

    /// <summary>
    /// Set Current active animator controller to be overriden by the specified controller.
    /// </summary>
    /// <param name="overrideController"></param>
    public void SetAnimatorController(AnimatorOverrideController overrideController)
    {
        player_animator.runtimeAnimatorController = overrideController;
    }

    public void SetAnimatorControllerByIndex(int i)
    {
        SetAnimatorController(overrideControllers[i]);
    }

    public void SetAnimatorControllerWithAnimation(int controllerIndex, AnimationClip animationClip, int animationIndex)
    {
        // Overrides animation clips in AnimatorOverrideController of goven index.
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(overrideControllers[controllerIndex].animationClips[animationIndex], animationClip));
        overrideControllers[controllerIndex].ApplyOverrides(anims);
        
        SetAnimatorController(overrideControllers[controllerIndex]);
    }

    public void AddPlayerAnimatorReference(Animator playerAnimator, RuntimeAnimatorController playerAnimatorController)
    {
        if (player_animator == null)
        {
            player_animator = playerAnimator;
        }
        player_animator.runtimeAnimatorController = playerAnimatorController;
        foreach(AnimatorOverrideController overrideController in overrideControllers)
        {
            overrideController.runtimeAnimatorController = playerAnimatorController;
        }
    }

}
