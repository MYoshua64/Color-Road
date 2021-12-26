using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class LaunchSlope : MonoBehaviour
{
    [SerializeField] PathCreator path;

    public void SnapToPath()
    {
        float pointOnPath = path.path.GetClosestDistanceAlongPath(transform.position);
        transform.SetPositionAndRotation(
            path.path.GetPointAtDistance(pointOnPath, EndOfPathInstruction.Stop), path.path.GetRotationAtDistance(pointOnPath, EndOfPathInstruction.Stop));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PathGenerator.instance.LaunchPlayerToNextTrack();
            SoundManager.instance.PlaySound(SoundType.Jump);
        }
    }
}
