using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class PathGenerator : MonoBehaviour
{
    public static PathGenerator instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] List<PathCreator> roadPrefabs;
    [SerializeField] PathFollower pathFollower;
    [SerializeField] Camera overviewCamera;

    public List<PathCreator> spawnedTracks = new List<PathCreator>();

    // Start is called before the first frame update
    void Start()
    {
        PathCreator initialPath = Instantiate(roadPrefabs[Random.Range(0, roadPrefabs.Count)], Vector3.zero, Quaternion.identity, transform);
        initialPath.GetComponent<RoadMeshCreator>().CallPathUpdated();
        spawnedTracks.Add(initialPath);
        SetOverviewCamera(initialPath);
        pathFollower.pathCreator = initialPath;
        SpawnNextTrack(initialPath);
        PlayerBall.instance.Init(pathFollower);
    }

    void SpawnNextTrack(PathCreator previous)
    {
        Vector3 finalPathPosition = previous.path.GetPointAtTime(1, EndOfPathInstruction.Stop);
        Quaternion pathRotation = previous.path.GetRotation(1, EndOfPathInstruction.Stop);
        Vector3 newSpawnPoint = finalPathPosition + 6 * (pathRotation * Vector3.forward);
        int spawnedIndex = Random.Range(0, roadPrefabs.Count);
        Debug.Log(spawnedIndex);
        PathCreator secondPath = Instantiate(roadPrefabs[spawnedIndex], newSpawnPoint, Quaternion.identity, transform);
        secondPath.GetComponent<RoadMeshCreator>().CallPathUpdated();
        spawnedTracks.Add(secondPath);
        Vector3 localRight = pathRotation * Vector3.forward;
        Vector3 localForward = Vector3.Cross(localRight, Vector3.up);
        secondPath.transform.rotation = Quaternion.LookRotation(localForward, Vector3.up);
    }

    public void LaunchPlayerToNextTrack()
    {
        PathCreator nextTrack = spawnedTracks[1];
        pathFollower.pathCreator = nextTrack;
        pathFollower.ResetSpeed();
        pathFollower.SetDistanceOnPath(1);
        Vector3 targetPoint = nextTrack.path.GetPointAtDistance(1, EndOfPathInstruction.Stop);
        PlayerBall.instance.LaunchToPoint(targetPoint, 1f);
        SpawnNextTrack(nextTrack);
    }

    public void DeletePreviousTrack()
    {
        SetOverviewCamera(spawnedTracks[1]);
        Destroy(spawnedTracks[0].gameObject);
        spawnedTracks.RemoveAt(0);
    }

    private void SetOverviewCamera(PathCreator path)
    {
        overviewCamera.transform.position = path.transform.position +
                    (path.transform.rotation * path.GetComponent<PathData>().overviewCameraPos);
        Vector3 localRight = path.transform.right;
        Vector3 localUp = Vector3.Cross(Vector3.down, localRight);
        overviewCamera.transform.rotation = Quaternion.LookRotation(Vector3.down, localUp);
        overviewCamera.orthographicSize = path.GetComponent<PathData>().overviewCameraSize;
    }
}
