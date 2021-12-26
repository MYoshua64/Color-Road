using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using System.Linq;
using DG.Tweening;

public enum BallType
{
    Triplet,
    Single,
    SingleBonus,
    Coins
}

public class BallSpawner : MonoBehaviour
{
    [SerializeField] BallType ballToSpawn;
    [SerializeField] BallColor colorToSpawn;
    [SerializeField] PathCreator path;
    [SerializeField] int numberOfRows;
    [SerializeField] float distanceBetweenRows = 1f;
    [SerializeField] float distanceBetweenBalls = 0.15f;
    [SerializeField] List<ScoreBall> scoreBallsPrefabs;
    [SerializeField] List<MultiplierBall> bonusBallsPrefabs;
    [SerializeField] Coin coinPrefab;
    [SerializeField] bool isMoving;

    [Tooltip("Should the spawned balls change color to match the player's ball?")]
    [SerializeField] bool shouldChangeColor;

    List<ScoreBall> spawnOrder = new List<ScoreBall>();

    List<StaticBall> spawnedBalls = new List<StaticBall>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnBalls();
        if (shouldChangeColor)
        {
            PlayerBall.instance.OnColorChanged += ChangeBallsColors;
        }
    }

    void SpawnBalls()
    {
        switch (ballToSpawn)
        {
            case BallType.Triplet:
                SpawnTriplets();
                break;
            case BallType.Single:
                SpawnScoreBall();
                break;
            case BallType.SingleBonus:
                SpawnBonus();
                break;
            case BallType.Coins:
                SpawnCoins();
                break;
        }
    }

    private void SpawnCoins()
    {
        Vector3 middlePoint = transform.position;
        Quaternion pointRot = transform.rotation;

        for (int row = 0; row < numberOfRows; row++)
        {
            int i = Mathf.RoundToInt(Mathf.Sign(Random.Range(-2, 2)));
            Vector3 spawnPoint = middlePoint + i * distanceBetweenBalls * (pointRot * Vector3.up) - 0.1f * (pointRot * Vector3.right);
            Quaternion coinRot = Quaternion.LookRotation(pointRot * Vector3.forward, pointRot * (-Vector3.right));
            Coin inst_coin = Instantiate(coinPrefab, spawnPoint, coinRot, transform);

            if (isMoving)
            {
                Vector3 targetMovePosition = middlePoint - i * distanceBetweenBalls * (pointRot * Vector3.up) - 0.1f * (pointRot * Vector3.right);
                Sequence moveSequence = DOTween.Sequence();
                moveSequence.Append(inst_coin.transform.DOMove(targetMovePosition, 2f)).Append(inst_coin.transform.DOMove(spawnPoint, 2f)).SetLoops(-1)
                    .SetEase(Ease.Linear);
                moveSequence.Play();
            }

            spawnedBalls.Add(inst_coin);

            middlePoint += pointRot * Vector3.forward * distanceBetweenRows;
            middlePoint = path.path.GetClosestPointOnPath(middlePoint);
            float dst = path.path.GetClosestDistanceAlongPath(middlePoint);
            pointRot = path.path.GetRotationAtDistance(dst);
        }
    }

    void SpawnScoreBall()
    {
        ScoreBall s_ball = scoreBallsPrefabs.First(ball => ball.GetColor() == colorToSpawn);
        Vector3 middlePoint = transform.position;
        Quaternion pointRot = transform.rotation;
        for (int row = 0; row < numberOfRows; row++)
        {
            int i = Mathf.RoundToInt(Mathf.Sign(Random.Range(-2, 2)));
            Vector3 spawnPoint = middlePoint + i * distanceBetweenBalls * (pointRot * Vector3.up) - 0.1f * (pointRot * Vector3.right);
            ScoreBall inst_ball = Instantiate(s_ball, spawnPoint, Quaternion.identity, transform);

            if (isMoving)
            {
                Vector3 targetMovePosition = middlePoint - i * distanceBetweenBalls * (pointRot * Vector3.up) - 0.1f * (pointRot * Vector3.right);
                Sequence moveSequence = DOTween.Sequence();
                moveSequence.Append(inst_ball.transform.DOMove(targetMovePosition, 2f)).Append(inst_ball.transform.DOMove(spawnPoint, 2f)).SetLoops(-1)
                    .SetEase(Ease.Linear);
                moveSequence.Play();
            }

            spawnedBalls.Add(inst_ball);

            middlePoint += pointRot * Vector3.forward * distanceBetweenRows;
            middlePoint = path.path.GetClosestPointOnPath(middlePoint);
            float dst = path.path.GetClosestDistanceAlongPath(middlePoint);
            pointRot = path.path.GetRotationAtDistance(dst);
        }
    }

    private void SpawnBonus()
    {
        MultiplierBall m_ball = bonusBallsPrefabs.First(ball => ball.GetColor() == colorToSpawn);
        Vector3 middlePoint = transform.position;
        Quaternion pointRot = transform.rotation;
        for (int row = 0; row < numberOfRows; row++)
        {
            int i = Mathf.RoundToInt(Mathf.Sign(Random.Range(-2, 2)));
            Vector3 spawnPoint = middlePoint + i * distanceBetweenBalls * (pointRot * Vector3.up) - 0.1f * (pointRot * Vector3.right);
            MultiplierBall inst_ball = Instantiate(m_ball, spawnPoint, Quaternion.identity, transform);

            if (isMoving)
            {
                Vector3 targetMovePosition = middlePoint - i * distanceBetweenBalls * (pointRot * Vector3.up) - 0.1f * (pointRot * Vector3.right);
                Sequence moveSequence = DOTween.Sequence();
                moveSequence.Append(inst_ball.transform.DOMove(targetMovePosition, 2f)).SetLoops(-1, LoopType.Yoyo);
                moveSequence.Play();
            }

            spawnedBalls.Add(inst_ball);

            middlePoint += pointRot * Vector3.forward * distanceBetweenRows;
            middlePoint = path.path.GetClosestPointOnPath(middlePoint);
            float dst = path.path.GetClosestDistanceAlongPath(middlePoint);
            pointRot = path.path.GetRotationAtDistance(dst);
        }
    }

    private void SpawnTriplets()
    {
        RandomizeColors();
        Vector3 middlePoint = transform.position;
        Quaternion pointRot = transform.rotation;
        for (int row = 0; row < numberOfRows; row++)
        {
            for (int i = -1; i <= 1; i++)
            {
                Vector3 spawnPoint = middlePoint + i * distanceBetweenBalls * (pointRot * Vector3.up) - 0.1f * (pointRot * Vector3.right);
                ScoreBall ball = Instantiate(spawnOrder[i + 1], spawnPoint, Quaternion.identity, transform);
            }

            middlePoint += pointRot * Vector3.forward * distanceBetweenRows;
            middlePoint = path.path.GetClosestPointOnPath(middlePoint);
            float dst = path.path.GetClosestDistanceAlongPath(middlePoint);
            pointRot = path.path.GetRotationAtDistance(dst);
        }
    }

    private void RandomizeColors()
    {
        spawnOrder.Clear();
        List<ScoreBall> ballData = new List<ScoreBall>();
        ballData.AddRange(scoreBallsPrefabs);
        for (int i = 0; i < 3; i++)
        {
            ScoreBall ball = ballData[Random.Range(0, ballData.Count)];
            spawnOrder.Add(ball);
            ballData.Remove(ball);
        }
    }

    public void SnapToPath()
    {
        float pointOnPath = path.path.GetClosestDistanceAlongPath(transform.position);
        transform.SetPositionAndRotation(
            path.path.GetPointAtDistance(pointOnPath, EndOfPathInstruction.Stop), path.path.GetRotationAtDistance(pointOnPath, EndOfPathInstruction.Stop));
    }

    void ChangeBallsColors(BallColor m_color)
    {
        foreach (StaticBall s_ball in spawnedBalls)
        {
            s_ball.SetNewColor(m_color);
        }
    }
}
