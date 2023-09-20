using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    [Range(0.0f, 1.0f)] public float musicTransitionProgress;

    [SerializeField] private AudioClip[] musicTracks;
    [SerializeField] private float transitionTimeSpeed = 1.0f;
    [SerializeField] private float targetVolume = 0.3f;

    private List<AudioSource> audioSources;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate audioSources List
        audioSources = new List<AudioSource>();

        //For loop used instead of foreach (because we need the index)
        for (int i = 0; i < musicTracks.Length; i++)
        {
            AudioClip currentAudioClip = musicTracks[i];

            //Instiating new empty GameObject (This will hold the Audio Sources for each track)
            GameObject audioObject = new GameObject();
            audioObject.name = "MusicProgress" + i.ToString();

            //Adding AudioSource component to audioObject, and setting up properties
            AudioSource currentAudioSource = audioObject.AddComponent<AudioSource>();
            currentAudioSource.clip = currentAudioClip;
            currentAudioSource.loop = true;
            currentAudioSource.Play();
            currentAudioSource.volume = 0;

            //Add currentAudioSource to audioSources List
            audioSources.Add(currentAudioSource);

            //Add created audioObject as a child of the transform this script is attached to
            audioObject.transform.SetParent(transform);
        }
    }

    // Update is called once per frame
    void Update()
    {

        //Not very optimal to loop through the audioSources list every frame, but this works for now.
        //Optimize later if possible 
        for (int i = 0; i < audioSources.Count; i++)
        {
            AudioSource currentAudioSource = audioSources[i];

            float startFactor = (float)(i) / audioSources.Count;
            float endFactor = (float)(i + 1) / audioSources.Count;

            if (musicTransitionProgress >= startFactor && musicTransitionProgress <= endFactor) 
            {
                currentAudioSource.volume = Mathf.Lerp(currentAudioSource.volume, targetVolume, Time.deltaTime * transitionTimeSpeed);
            } else
            {
                currentAudioSource.volume = Mathf.Lerp(currentAudioSource.volume, 0.0f, Time.deltaTime * transitionTimeSpeed);
            }
        }
    }
}
