using UnityEngine;

/*
 * This code mantains the default object rotation
 */

public class LookRotation : MonoBehaviour
{
    // Start is called before the first frame update
    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
