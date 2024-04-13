using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Volume")]
    [Range(0, 0.5f)]
    public float masterVolume = 1;
    [Range(0, 0.5f)]
    public float menuMusicVolume = 1;
    [Range(0, 0.5f)]
    public float gameMusicVolume = 1;
    // private GameEntryMenu gameEntryMenu;

    private Bus masterBus;
    private Bus menuMusicBus;
    private Bus gameMusicBus;

    private List<EventInstance> eventInstances;
    private EventInstance menuMusicEventInstance;
    private EventInstance backgroundMusicEventInstance;
    public static AudioManager instance { get; private set; }

    public new void Awake()
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

    public void Update()
    {
        masterBus.setVolume(masterVolume);
        menuMusicBus.setVolume(menuMusicVolume);
        gameMusicBus.setVolume(gameMusicVolume);
    }
    public void Start()
    {
        // gameEntryMenu = FindObjectOfType<GameEntryMenu>();
        // if (!gameEntryMenu.isNewGameClicked)
        //     InitializeMenuMusic(FMODEvents.instance.MenuMusic);
        // else
        //     InitializeMenuMusic(FMODEvents.instance.BackgroundMusic);
        InitializeMenuMusic(FMODEvents.instance.MenuMusic);
    }
    public void InitializeMenuMusic(EventReference menuMusicEventReference)
    {
        menuMusicEventInstance = CreateInstance(menuMusicEventReference);
        menuMusicEventInstance.start();
    }
    public void InitializeBackgroundMusic(EventReference backgroundMusicEventReference)
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

    public void CleanUp()
    {
        // stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    public void OnDestroy()
    {
        CleanUp();
    }
}
