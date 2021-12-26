using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StaticBall : MonoBehaviour
{
    [SerializeField] protected BallColor color;

    protected MeshRenderer m_renderer;
    protected static float pitch;

    protected void Start()
    {
        m_renderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HandlePlayerIn();
        }
        else if (other.CompareTag("Miss Check"))
        {
            HandleMiss();
        }
    }

    protected virtual void HandlePlayerIn()
    {
        transform.DOKill();
        PlayerBall.instance.DisplayGainedPoints();
    }

    public BallColor GetColor()
    {
        return color;
    }

    public virtual void SetNewColor(BallColor n_color)
    {
        color = n_color;
    }

    protected virtual void HandleMiss()
    {

    }

    public static void ResetPitch()
    {
        pitch = 0;
    }
}
