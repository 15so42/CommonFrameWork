using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource bgmSource; // 背景音乐音源
    public List<AudioClip> bgms = new List<AudioClip>();
    public AudioSource sfxSource; // 音效音源

    public List<SoundConfig> sfxList = new List<SoundConfig>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(RandomBGM());
    }


    IEnumerator RandomBGM()
    {
        while (true)
        {
            var waitTime = UnityEngine.Random.Range(60, 360);
            yield return new WaitForSeconds(waitTime);
            var clip = bgms.RandomElement();
            PlayBGM(clip);
            yield return new WaitForSeconds(clip.length);
        }
        
    }

    public void PlayAudio2D(string clipName)
    {
        
        var sound = sfxList.Find(s => s.name == clipName);

        if(sfxSource==null)
            return;
        if (sound != null)
        {
            sfxSource.PlayOneShot(sound.GetRandomClip(),sound.GetVolume());
            
        }
        else
        {
            Debug.LogError("音效" + clipName + "不存在！");
        }
    }

    public GameObject PlaySfxAtPoint(string clipName, Vector3 pos)
    {
        var sound = sfxList.Find(s => s.name == clipName);

        if (sound != null)
        {
          return PlayClipAtPoint(sound.GetRandomClip(),pos,sound.GetVolume(),sound.GetRandomPitch(),sound.GetMaxDistance());
            
        }
        else
        {
            Debug.LogError("音效" + clipName + "不存在！");
            return null;
        }
    }

    private GameObject PlayClipAtPoint(AudioClip clip, Vector3 pos, float volume,float pitch,int maxDistance)
    {
        GameObject go = new GameObject("One shot audio");
        go.transform.position = pos;
        AudioSource audioSource = (AudioSource) go.AddComponent(typeof (AudioSource));
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        audioSource.minDistance = 3;
        audioSource.maxDistance = maxDistance;
        audioSource.pitch = pitch;
        audioSource.dopplerLevel = 0;
        audioSource.Play();
       
        Object.Destroy((Object) go, clip.length * ((double) Time.timeScale < 0.009999999776482582 ? 0.01f : Time.timeScale));
        return go;
    }

    public void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}

[System.Serializable]
public class SoundConfig
{
    public string name;
    
    public UnityEngine.Object clip;
    
    public AudioClip GetRandomClip()
    {
        if (clip is AudioClip audioClip)
        {
            return audioClip;
        }
        else if (clip is AudioClipList audioClipList)
        {
            return audioClipList.GetRandomClip();
        }
        else
        {
            Debug.LogError("Invalid clipObject type");
            return null;
        }
    }

    public float GetRandomPitch()
    {
        if (clip is AudioClipList audioClipList)
        {
            return UnityEngine.Random.Range(audioClipList.minPitch,audioClipList.maxPitch);
        }

        return 1;
    }
    
    public int GetMaxDistance()
    {
        if (clip is AudioClipList audioClipList)
        {
            return audioClipList.maxDistance;
        }

        return 5000;
    }

    public float GetVolume()
    {
        if (clip is AudioClip audioClip)
        {
            return 1;
        }
        else if (clip is AudioClipList audioClipList)
        {
            return audioClipList.volume;
        }
        else
        {
            Debug.LogError("Invalid clipObject type");
            return 1;
        }
    }
}