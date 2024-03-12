using UnityEngine;

public class SimpleDeactivate : MonoBehaviour
{
    public Animation customAnimation; // Assign the Animation component in the inspector
    public AnimationClip closeAnimationClip; // Drag and drop your .anim file here in the inspector

    public void StartDeactivation()
    {
        if (customAnimation != null && closeAnimationClip != null)
        {
            // Add the close animation clip to the animation component if not already present
            if (!customAnimation.GetClip(closeAnimationClip.name))
            {
                customAnimation.AddClip(closeAnimationClip, closeAnimationClip.name);
            }

            // Play the specified closing animation
            customAnimation.Play(closeAnimationClip.name);
            // Invoke the deactivation after the animation is done
            Invoke("DeactivateGameObject", closeAnimationClip.length);
        }
    }

    void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}
