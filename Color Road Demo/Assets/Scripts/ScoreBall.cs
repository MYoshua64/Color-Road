using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBall : StaticBall
{
    protected override void HandlePlayerIn()
    {
        base.HandlePlayerIn();
        if (PlayerBall.instance.PlayerColor == color)
        {
            SoundManager.instance.PlaySound(SoundType.Pickup, pitch);
            pitch = Mathf.Min(pitch + .25f, 3f);
            ScoreManager.instance.Score();
            PlayerBall.instance.RaiseSpeed(0.03f);
            Debug.Log("Point!");
            Destroy(gameObject);
        }
        else
        {
            SoundManager.instance.PlaySound(SoundType.GameOver);
            PlayerBall.instance.Die();
            Debug.Log("Destroyed!");
        }
    }

    public override void SetNewColor(BallColor n_color)
    {
        base.SetNewColor(n_color);
        if (m_renderer)
        {
            m_renderer.material = ColorMaterialLibrary.instance.GetMaterialByColor(color);
        }
    }
}
