using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] public AudioSource
        PlayerEffect_Source,
        EnemyEffect_Source,
        EnvironmentEffect_Source,
        Music_Source,
        RunWalk_Source;

    [SerializeField] AudioClip fireballThrow;
    [SerializeField] AudioClip fireballImpact;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip playerHurt;
    [SerializeField] AudioClip playerWalk;
    [SerializeField] AudioClip playerRun;   //

    [SerializeField] AudioClip snowballThrow;
    [SerializeField] AudioClip snowballImpact;
    [SerializeField] AudioClip enemyDeath;  //


    public AudioClip FireballThrow { get => fireballThrow; set => fireballThrow = value; }
    public AudioClip FireballImpact { get => fireballImpact; set => fireballImpact = value; }
    public AudioClip Jump { get => jump; set => jump = value; }
    public AudioClip PlayerHurt { get => playerHurt; set => playerHurt = value; }
    public AudioClip PlayerWalk { get => playerWalk; set => playerWalk = value; }
    public AudioClip PlayerRun { get => playerRun; set => playerRun = value; }

    public AudioClip SnowballThrow { get => snowballThrow; set => snowballThrow = value; }
    public AudioClip SnowballImpact { get => snowballImpact; set => snowballImpact = value; }
    public AudioClip EnemyDeath { get => enemyDeath; set => enemyDeath = value; }


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

    public void PlayWalkingSound()
    {
        if (!RunWalk_Source.isPlaying || RunWalk_Source.clip != playerWalk)
        {
            RunWalk_Source.clip = playerWalk;
            RunWalk_Source.volume = 0.07f;
            RunWalk_Source.loop = true;
            RunWalk_Source.Play();
        }
    }

    public void PlayRunningSound()
    {
        if (!RunWalk_Source.isPlaying || RunWalk_Source.clip != playerRun)
        {
            RunWalk_Source.clip = playerRun;
            RunWalk_Source.volume = 0.05f;
            RunWalk_Source.loop = true;
            RunWalk_Source.Play();
        }
    }

    public void StopMovementSound()
    {
        if (RunWalk_Source.isPlaying)
        {
            RunWalk_Source.Stop();
        }
    }


    // Single Method approach
    // Runner: SoundManager.instance.PlayEffect(SoundManager.instance.PlayerEffect_Source, someClip, someVolume);
    public void PlayEffectSound(AudioSource source, AudioClip clipToPlay, float effectVolume)
    {
        source.volume = effectVolume;
        source.PlayOneShot(clipToPlay);
    }

}
