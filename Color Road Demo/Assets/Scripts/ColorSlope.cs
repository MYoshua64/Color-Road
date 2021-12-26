using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class ColorSlope : MonoBehaviour
{
    [SerializeField] PathCreator path;
    [SerializeField] BallColor color;

    private void Start()
    {
        GetComponent<MeshRenderer>().material = ColorMaterialLibrary.instance.GetMaterialByColor(color);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerBall.instance.Launch();
            PlayerBall.instance.ChangeColor(color);
            SoundManager.instance.PlaySound(SoundType.Jump);
        }
    }

    public void SnapToPath()
    {
        float pointOnPath = path.path.GetClosestDistanceAlongPath(transform.position);
        transform.SetPositionAndRotation(
            path.path.GetPointAtDistance(pointOnPath, EndOfPathInstruction.Stop), path.path.GetRotationAtDistance(pointOnPath, EndOfPathInstruction.Stop));
    }
}
