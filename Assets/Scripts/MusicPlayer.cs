using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    static MusicPlayer instance = null;

    [SerializeField] AudioSource source;

    [SerializeField] AudioClip intro;
    [SerializeField] AudioClip gameOver;
    [SerializeField] AudioClip game;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            source.clip = intro;
            source.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGameMusic()
    {
        source.clip = game;
        source.loop = true;
        source.Play();
    }

    public void PlayLoadingScreenMusic()
    {
        source.clip = intro;
        source.loop = false;
        source.Play();
    }

    public void PlayGameOverMusic()
    {
        source.clip = gameOver;
        source.loop = true;
        source.Play();
    }
}
