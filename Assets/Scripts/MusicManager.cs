using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    AudioSource musicManager;
    [SerializeField] AudioClip[] musicTracks;
    // Start is called before the first frame update
    void Start()
    {
        musicManager = GetComponent<AudioSource>();
        int randIndex = (int)Random.Range(0, musicTracks.Length);
        musicManager.clip = musicTracks[randIndex];
    }
}
