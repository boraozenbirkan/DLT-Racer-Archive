using System;
using UnityEngine;

public class AudioManager : MonoBehaviour{

    public Sound[] sounds;
    public static AudioManager instance;

    private void Awake() {
        if (instance == null)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach( Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }


    // Start is called before the first frame update
    void Start()    {
        Play("Background Music");
    }

    public void Play(string name) {
        Sound theSound = Array.Find(sounds, sound => sound.name == name);
        if (theSound == null) {
            Debug.LogError("There is no sound file named as " + name);
            return;
        }
        else
            theSound.source.Play();
    }
    public void Stop(string name) {
        Sound theSound = Array.Find(sounds, sound => sound.name == name);
        if (theSound == null) {
            Debug.LogError("There is no sound file named as " + name);
            return;
        }
        else
            theSound.source.Stop();
    }
    public void SetVolume(string name, float volume) {
        Sound theSound = Array.Find(sounds, sound => sound.name == name);
        if (theSound == null) {
            Debug.LogError("There is no sound file named as " + name);
            return;
        }
        else {
            theSound.source.volume = volume;
        }
    }
    public void SetLoop(string name, bool loop) {
        Sound theSound = Array.Find(sounds, sound => sound.name == name);
        if (theSound == null) {
            Debug.LogError("There is no sound file named as " + name);
            return;
        }
        else {
            theSound.source.loop = loop;
        }
    }
    public void SetPause(string name, bool pause) {
        Sound theSound = Array.Find(sounds, sound => sound.name == name);
        if (theSound == null) {
            Debug.LogError("There is no sound file named as " + name);
            return;
        }
        else if (pause) {
            theSound.source.Pause();
        }
        else {
            theSound.source.UnPause();
        }
    }

}
