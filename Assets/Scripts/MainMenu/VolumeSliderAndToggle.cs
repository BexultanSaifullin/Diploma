using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType
    {
        MASTER,
        MENU_MUSIC,
        GAME_MUSIC
    }

    [Header("Type")]
    [SerializeField] private VolumeType volumeType;

    private Slider volumeSlider;
    public Toggle musicToggle;

    private void Awake()
    {
        volumeSlider = this.GetComponentInChildren<Slider>();
        musicToggle = this.GetComponentInChildren<Toggle>();
    }
    private void Update()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                volumeSlider.value = AudioManager.instance.masterVolume;
                break;
            case VolumeType.MENU_MUSIC:
                volumeSlider.value = AudioManager.instance.menuMusicVolume;
                break;
            case VolumeType.GAME_MUSIC:
                volumeSlider.value = AudioManager.instance.gameMusicVolume;
                break;
            default:
                Debug.LogWarning("Volume Type not supported: " + volumeType);
                break;
        }
    }

    public void OnSliderValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.MASTER:
                AudioManager.instance.masterVolume = volumeSlider.value;
                break;
            case VolumeType.MENU_MUSIC:
                AudioManager.instance.menuMusicVolume = volumeSlider.value;
                break;
            case VolumeType.GAME_MUSIC:
                AudioManager.instance.gameMusicVolume = volumeSlider.value;
                break;
            default:
                Debug.LogWarning("Volume Type not supported: " + volumeType);
                break;
        }
    }

    public void ToggleMusic()
    {
        if (!musicToggle.isOn)
            AudioManager.instance.gameMusicVolume = 0;
        else
            AudioManager.instance.gameMusicVolume = volumeSlider.value;
    }
}
