using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipList", menuName = "ScriptableObjects/AudioClipList", order = 1)]
public class AudioClipList : ScriptableObject
{
    public AudioClip[] audioClips;

    [Range(0.1f, 1.5f)]
    public float minPitch = 0.95f;

    [Range(0.1f, 1.5f)]
    public float maxPitch = 1.05f;

    public float volume = 1;
    public int maxDistance = 5000;

    public AudioClip GetRandomClip()
    {
        if (audioClips.Length > 0)
        {
            return audioClips[Random.Range(0, audioClips.Length)];
        }
        else
        {
            Debug.LogWarning("No audio clips available");
            return null;
        }
    }
    
    public void PlayRandomClip(Vector3 position)
    {
        // 获取一个随机音效
        AudioClip clip = audioClips[Random.Range(0, audioClips.Length)];
        
        // 获取一个随机pitch
        float pitch = Random.Range(minPitch, maxPitch);

        // 创建一个新的AudioSource组件实例
        AudioSource audioSource = new GameObject("AudioSource").AddComponent<AudioSource>();

        // 设置AudioSource的参数
        audioSource.clip = clip;
        audioSource.pitch = pitch;
        audioSource.transform.position = position;
        
        // 播放音效
        audioSource.Play();

        // 音效播放完毕后销毁AudioSource组件
        Object.Destroy(audioSource.gameObject, clip.length);
    }
}