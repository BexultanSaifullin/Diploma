using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolume = 1;
    [Range(0, 1)]
    public float menuMusicVolume = 1;
    [Range(0, 1)]
    public float gameMusicVolume = 1;

    private Bus masterBus;
    private Bus menuMusicBus;
    private Bus gameMusicBus;

    private List<EventInstance> eventInstances;
    private EventInstance menuMusicEventInstance;
    private EventInstance backgroundMusicEventInstance;
    public static AudioManager instance { get; private set; }

    private new void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }
        instance = this;

        eventInstances = new List<EventInstance>();

        masterBus = RuntimeManager.GetBus("bus:/");
        menuMusicBus = RuntimeManager.GetBus("bus:/MenuMusic");
        gameMusicBus = RuntimeManager.GetBus("bus:/GameMusic");
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        menuMusicBus.setVolume(menuMusicVolume);
        gameMusicBus.setVolume(gameMusicVolume);
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
            InitializeMenuMusic(FMODEvents.instance.MenuMusic);
        else
            InitializeMenuMusic(FMODEvents.instance.BackgroundMusic);
    }
    private void InitializeMenuMusic(EventReference menuMusicEventReference)
    {
        menuMusicEventInstance = CreateInstance(menuMusicEventReference);
        menuMusicEventInstance.start();
    }
    private void InitializeBackgroundMusic(EventReference backgroundMusicEventReference)
    {
        backgroundMusicEventInstance = CreateInstance(backgroundMusicEventReference);
        backgroundMusicEventInstance.start();
    }
    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp()
    {
        // stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}
