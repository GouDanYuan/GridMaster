using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManage : MonoBehaviour
{
    public static SoundManage instance;

    private AudioSource audioSource;

    private Dictionary<string, AudioClip> dictAudio; 
     // Start is called before the first frame update

     private void Awake()
     {
         instance = this;
         audioSource = GetComponent<AudioSource>();
         dictAudio = new Dictionary<string, AudioClip>();
     }
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip LoadAudio(string path)
    {
        return (AudioClip)Resources.Load(path);
    }

    private AudioClip GetAudio(string path)
    {
        if(!dictAudio.ContainsKey(path))
        
            Debug.Log(path);
            dictAudio[path] = LoadAudio(path);
        
        if(dictAudio[path]==null)
            Debug.Log(1111);

        return dictAudio[path];
    }

    public void PlayBGM(string name, float volume = 1.0f)
    {
        Debug.Log("BGMMM");
        audioSource.Stop();
        audioSource.clip = GetAudio(name);
        Debug.Log(audioSource.clip);
        audioSource.Play();
        //Debug.Log("STOP");
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    public void PlaySound(string path, float volume = 1f)
    {
        this.audioSource.PlayOneShot(GetAudio(path),volume);
        this.audioSource.volume = volume;
    }

    public void PlaySound(AudioSource audioSource, string path, float volume = 1f)
    {
        audioSource.PlayOneShot(GetAudio(path),volume);
        
    }
}
