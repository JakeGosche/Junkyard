using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{


    public AudioClip jump, death, shoot, shadowIn, shadowOut, goal, bloop, click, spring, enemyHit;
    public AudioSource sfxSource, bgMusic, endJingle, shootSource;
    void Start()
    {
        //bgMusic.clip = songs[0];
    }

    public void PlaySfx (Enums.SoundEffect soundEffect)
    {
        if (soundEffect == Enums.SoundEffect.Goal)
        {
            endJingle.Play();
        }
        else if(soundEffect == Enums.SoundEffect.Shoot)
        {
            shootSource.Play();
        }
        else
        {
            switch (soundEffect)
            {
                case Enums.SoundEffect.Jump:
                    sfxSource.clip = jump;
                    break;
                case Enums.SoundEffect.Bloop:
                    sfxSource.clip = bloop;
                    break;
                case Enums.SoundEffect.Shoot:
                    sfxSource.clip = shoot;
                    break;
                case Enums.SoundEffect.ShadowIn:
                    sfxSource.clip = shadowIn;
                    break;
                case Enums.SoundEffect.ShadowOut:
                    sfxSource.clip = shadowOut;
                    break;
                case Enums.SoundEffect.Goal:
                    sfxSource.clip = goal;
                    break;
                case Enums.SoundEffect.Click:
                    sfxSource.clip = click;
                    break;
                case Enums.SoundEffect.Death:
                    sfxSource.clip = death;
                    break;
                case Enums.SoundEffect.Spring:
                    sfxSource.clip = spring;
                    break;
                case Enums.SoundEffect.EnemyHit:
                    sfxSource.clip = enemyHit;
                    break;
            }

            sfxSource.Play();
        }
    }
}
