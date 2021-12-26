using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierBall : StaticBall
{
    protected override void HandlePlayerIn()
    {
        if (PlayerBall.instance.PlayerColor == color)
        {
            SoundManager.instance.PlaySound(SoundType.Bonus);
            ScoreManager.instance.multiplier++;
            ScoreManager.instance.Score();
            Destroy(gameObject);
        }
        base.HandlePlayerIn();
    }

    public override void SetNewColor(BallColor n_color)
    {
        base.SetNewColor(n_color);
        if (m_renderer)
        {
            m_renderer.material = ColorMaterialLibrary.instance.GetMaterialByColor(color, true);
        }
    }

    protected override void HandleMiss()
    {
        ScoreManager.instance.multiplier = 1;
        PlayerBall.instance.DisplayMiss();
    }
}
