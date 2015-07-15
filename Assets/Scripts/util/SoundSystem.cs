using UnityEngine;
using System.Collections;

public class SoundSystem : MonoBehaviour {

    [SerializeField] private AudioClip finalExplosion = null;
    [SerializeField] private AudioClip[] explosions = null;
    [SerializeField] private AudioClip[] shots = null;
    
    private AudioSource[] sources;
    
    void Awake()
    {
        sources = GetComponents<AudioSource>();
    }
    
    void Update()
    {
    }

    public void playExplosion()
    {
        int max = explosions.Length-1;
        var clip = explosions[Random.Range(0, max)];
        _playOnFreeSource(clip);
    }
    
    public void playFinalExplosion()
    {
        foreach (var s in sources) {
            s.Stop();
        }
        _playOnFreeSource(finalExplosion);
    }
    
    public void playShoot()
    {
        int max = shots.Length-1;
        var clip = shots[Random.Range(0, max)];
        _playOnFreeSource(clip);
    }
    
    private void _playOnFreeSource(AudioClip clip)
    {
        foreach(var s in sources) {
            if (!s.isPlaying) {
                s.clip = clip;
                s.Play();
                return;
            }
        }
    }
}
