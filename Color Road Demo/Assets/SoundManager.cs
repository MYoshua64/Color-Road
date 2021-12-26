using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum SoundType
{
    Pickup,
    Jump,
    Bonus,
    Coin,
    GameOver
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private void Awake()
    {
        instance = this;
    }

    [System.Serializable]
    public class NameToSound
    {
        public SoundType type;
        public AudioClip clip;
    }

    [SerializeField] List<NameToSound> soundLibrary;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundType _type, float pitch = 1)
    {
        audioSource.pitch = pitch;
        NameToSound _nts = soundLibrary.First(item => item.type == _type);
        audioSource.clip = _nts.clip;
        audioSource.Play();
    }
}
