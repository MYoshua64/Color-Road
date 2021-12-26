using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : StaticBall
{
    [SerializeField] Transform coinRenderer;

    static int coinCount;

    protected override void HandlePlayerIn()
    {
        SoundManager.instance.PlaySound(SoundType.Coin);
        coinCount++;
        CanvasManager.instance.UpdateCoinCount(coinCount);
        Destroy(gameObject);
    }

    private void Update()
    {
        coinRenderer.Rotate(transform.up * 120f * Time.deltaTime);
    }

    public static int GetCoinCount()
    {
        return coinCount;
    }
}
