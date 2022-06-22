using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AudioClipTag
{
    themeAudio = 0,
    gameoverAudio = 1,
    levelDoneAudio = 2
}

[System.Serializable]
public class AudioBlock
{
    public AudioClip audioClip;
    public AudioClipTag audioTag;
}

public class AudioManager : MonoBehaviour
{
    public List<AudioBlock> audioSampleList;
    public AudioSource bgmSource;
    public UnityEvent onAudioStop;
    private bool gameEnd = false;
    
    public void Update()
    {
        if (!gameEnd && !bgmSource.isPlaying)
        {
            bgmSource.Play();
        }
    }

    public void gameOverResponse()
    {
        gameEnd = true;
        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
        foreach(AudioBlock item in audioSampleList)
        {
            if(item.audioTag == AudioClipTag.gameoverAudio)
            {
                bgmSource.PlayOneShot(item.audioClip);
                StartCoroutine(waitBeforeSceneChange());
                break;
            }
        }
        
    }

    public void levelDoneResponse()
    {
        gameEnd = true;
        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
        foreach(AudioBlock item in audioSampleList)
        {
            if(item.audioTag == AudioClipTag.levelDoneAudio)
            {
                bgmSource.PlayOneShot(item.audioClip);
                StartCoroutine(waitBeforeSceneChange());
                break;
            }
        }
    }

    IEnumerator waitBeforeSceneChange()
    {
        yield return new WaitUntil(()=> !bgmSource.isPlaying);
        onAudioStop.Invoke();
    }
}
