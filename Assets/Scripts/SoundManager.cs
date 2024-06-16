using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource 
        PlayerEffect_Source,
        EnemeyEffect_Source,
        EnvironmentEffect_Source,
        Music_Source;

    [SerializeField] AudioClip fireballThrow;
    [SerializeField] AudioClip doubleJump;
    [SerializeField] AudioClip playerHurt;
    [SerializeField] AudioClip playerWalk;
    [SerializeField] AudioClip playerRun;

    [SerializeField] AudioClip snowballThrow;
    [SerializeField] AudioClip enemyDeath;


    public AudioClip FireballThrow  { get => fireballThrow; set => fireballThrow = value; }
    public AudioClip DoubleJump     { get => doubleJump; set => doubleJump = value; }
    public AudioClip PlayerHurt     { get => playerHurt; set => playerHurt = value; }
    public AudioClip PlayerWalk     { get => playerWalk; set => playerWalk = value; }
    public AudioClip PlayerRun      { get => playerRun; set => playerRun = value; }

    public AudioClip SnowballThrow  { get => snowballThrow; set => snowballThrow = value; }
    public AudioClip EnemyDeath     { get => enemyDeath; set => enemyDeath = value; }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
    }

    public void PlayPlayerEffect(AudioClip clipToPlay, float effectVolume)
    {
        PlayerEffect_Source.PlayOneShot(clipToPlay);
        
        PlayerEffect_Source.volume = effectVolume;
        
        PlayerEffect_Source.PlayOneShot(clipToPlay);
    }

    public void PlayEnemyEffect(AudioClip clipToPlay, float effectVolume)
    {
        EnemeyEffect_Source.PlayOneShot(clipToPlay);

        EnemeyEffect_Source.volume = effectVolume;

        EnemeyEffect_Source.PlayOneShot(clipToPlay);
    }

    public void PlayEnvironmentEffect(AudioClip clipToPlay, float effectVolume)
    {
        EnvironmentEffect_Source.PlayOneShot(clipToPlay);

        EnvironmentEffect_Source.volume = effectVolume;

        EnvironmentEffect_Source.PlayOneShot(clipToPlay);
    }

    // Single Method approach
    // Runner: SoundManager.instance.PlayEffect(SoundManager.instance.PlayerEffect_Source, someClip, someVolume);
    public void PlayEffectSound(AudioSource source, AudioClip clipToPlay, float effectVolume)
    {
        source.volume = effectVolume;
        source.PlayOneShot(clipToPlay);
    }

}
