using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownShooter
{
    public class AudioManager : MonoBehaviour
    {
        float masterVolumePercent = .2f;
        float sfxVolumePercent = 1;
        float musicVolumePercent = 1;

        //Having the audio source object lets you dynamically adjust the settings while audio plays, eg. volume
        //Using two audio sources, we can cross fade music when changing.
        AudioSource[] musicSources;
        int activeMusicSourceIndex;

        public static AudioManager instance; //Make this a singleton, allowing it to be accessed statically.

        //We set our audio listener to follow around the player. This way the audio listener wont get deleted (this happens when its attatched to the player directly)
        Transform audioListener;
        Transform playerT;

        private void Awake()
        {

            instance = this; //Initialize singleton variable

            //Create two audio sources
            musicSources = new AudioSource[2];
            for (int i = 0; i < 2; i++)
            {
                GameObject newMusicSource = new GameObject("Music Source " + (i + 1));
                musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                newMusicSource.transform.parent = transform; //Put the new music sources as the child object of this for organization
            }

            audioListener = FindObjectOfType<AudioListener>().transform;
            playerT = FindObjectOfType<Player>().transform;
        }

        private void Update()
        {
            if (playerT != null) //while the player is alive
            {
                audioListener.position = playerT.position;
            }
        }

        public void PlayMusic(AudioClip clip, float fadeDuration = 1)
        {
            activeMusicSourceIndex = 1 - activeMusicSourceIndex; //0,1,0,1...
            musicSources[activeMusicSourceIndex].clip = clip;
            musicSources[activeMusicSourceIndex].Play();
            StartCoroutine(AnimateMusicCrossfade(fadeDuration));
        }

        IEnumerator AnimateMusicCrossfade(float duration)
        {
            float percent = 0;

            while (percent < 1)
            {
                percent += Time.deltaTime * 1 / duration; // 1/duration = speed of crossfade
                musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
                musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);
                yield return null;
            }
        }

        //Plays a sound at a position. For sound effects
        public void PlaySound(AudioClip clip, Vector3 pos)
        {
            if (clip != null)
            {
                AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent * masterVolumePercent); //Cant change volume durring sound, not ideal for music
            }
            else
            {
                print("Audio is null!");
            }
        }


    }
}
