using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform cameraTarget;

    private void LateUpdate()
    {
        transform.position = cameraTarget.transform.position - 2 * cameraTarget.transform.forward - 0.2f * cameraTarget.transform.right;
        transform.rotation = Quaternion.LookRotation(cameraTarget.transform.position -  transform.position, -cameraTarget.right);
    }
}
