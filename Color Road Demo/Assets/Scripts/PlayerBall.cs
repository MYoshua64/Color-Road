using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;
using TMPro;
using DG.Tweening;

public enum BallColor
{
    Red,
    Yellow,
    Blue
}

public class PlayerBall : MonoBehaviour
{
    public static PlayerBall instance;

    private void Awake()
    {
        instance = this;
        inputActions = new PlayerInputActions();

        inputActions.Gameplay.Movement.performed += ctx => move = Mathf.Clamp(ctx.ReadValue<float>(), -1,1);
        inputActions.Gameplay.Movement.canceled += _ => move = 0f;

        inputActions.Gameplay.StartGame.performed += _ => 
        {
            if (!gameOver)
            {
                CanvasManager.instance.FadeAwayStartText();
                follower.StartGame();
                inputActions.Gameplay.Movement.Enable();
            }
            else
            {
                //reload scene
                SceneLoader.instance.ReloadScene();
            }
        };
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }
    
    private void OnDisable()
    {
        inputActions.Disable();
    }

    PlayerInputActions inputActions;

    public BallColor PlayerColor { get; set; } = BallColor.Red;
    public System.Action<BallColor> OnColorChanged;
    
    [SerializeField] PathFollower follower;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] TextMeshProUGUI addedScoreText;

    MeshRenderer ballRenderer;
    Animator ballAnim;
    Rigidbody ballRB;
    CanvasGroup gp_Score;

    float move;
    float distanceFromCenter;
    float maxDistanceFromCenter = 0.25f;
    bool followingPath;
    bool gameOver;

    private void Start()
    {
        inputActions.Gameplay.Movement.Disable();
        ballRenderer = GetComponentInChildren<MeshRenderer>();
        ballAnim = GetComponent<Animator>();
        ballRB = GetComponent<Rigidbody>();
        gp_Score = addedScoreText.GetComponent<CanvasGroup>();
    }

    public void Init(PathFollower initialFollower)
    {
        follower = initialFollower;
        transform.position = follower.transform.position - 0.075f * follower.transform.right;
        followingPath = true;
    }

    private void Update()
    {
        if (followingPath)
        {
            distanceFromCenter += move * Time.deltaTime * moveSpeed;
            distanceFromCenter = Mathf.Clamp(distanceFromCenter, -maxDistanceFromCenter, maxDistanceFromCenter);
            Vector3 desiredPos = follower.transform.position - 0.075f * follower.transform.right + distanceFromCenter * follower.transform.up;
            transform.SetPositionAndRotation(desiredPos, follower.transform.rotation);
            transform.GetChild(0).Rotate(Vector3.up * follower.speed * Time.deltaTime * 75, Space.Self);
        }

    }

    public void ChangeColor(BallColor newColor)
    {
        ballRenderer.material = ColorMaterialLibrary.instance.GetMaterialByColor(newColor);
        PlayerColor = newColor;
        Debug.Log("New color is now: " + newColor);
        OnColorChanged?.Invoke(newColor);
    }

    public void Launch()
    {
        ballAnim.SetTrigger("Jump");
    }

    public void Die()
    {
        gameOver = true;
        follower.speed = 0;
        inputActions.Gameplay.Movement.Disable();
        GetComponent<Collider>().enabled = false;
        CanvasManager.instance.DisplayGameOver();
    }

    public void LaunchToPoint(Vector3 targetPoint, float v)
    {
        followingPath = false;
        ballRB.useGravity = true;
        Vector3 initialVel = PhysicsSolver.InitialVelocity(transform.position, targetPoint, v);
        ballRB.velocity = initialVel;
        Invoke("StartFollow", v);
    }

    void StartFollow()
    {
        ballRB.useGravity = false;
        followingPath = true;
        follower.StartGame();
        PathGenerator.instance.DeletePreviousTrack();
        StaticBall.ResetPitch();
    }

    public void SetFollower(PathFollower targetFollower)
    {
        follower = targetFollower;
    }

    public void RaiseSpeed(float increase)
    {
        follower.speed += increase;
        ballAnim.speed += increase * 0.4f;
    }

    public void DisplayGainedPoints()
    {
        addedScoreText.transform.DOKill();
        addedScoreText.text = $"+{ScoreManager.instance.multiplier}";
        addedScoreText.transform.localPosition = -.2f * Vector3.right;
        Sequence fadeSequence = DOTween.Sequence();
        fadeSequence.OnPlay(() => gp_Score.alpha = 1).Append(gp_Score.DOFade(0, 0.5f));
        fadeSequence.Play();
    }

    public void DisplayMiss()
    {
        addedScoreText.transform.DOKill();
        addedScoreText.text = "Miss";
        addedScoreText.transform.localPosition = -.2f * Vector3.right;
        Sequence fadeSequence = DOTween.Sequence();
        fadeSequence.OnPlay(() => gp_Score.alpha = 1).Append(gp_Score.DOFade(0, 0.5f)).SetUpdate(true);
        fadeSequence.Play();
    }

    public void Pause(bool pause)
    {
        Time.timeScale = pause ? 0f : 1f;
    }
}
