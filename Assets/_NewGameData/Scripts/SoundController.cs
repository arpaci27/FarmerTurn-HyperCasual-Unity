using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;


    public AudioSource backgroundSong;

    public AudioSource stackSound;
    public AudioSource unStackSound;

    private void Awake()
    {
        instance = this;
    }
}
