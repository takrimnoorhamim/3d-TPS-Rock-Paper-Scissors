using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public Slider VolumeSlider;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            load();
        }

        else {
            
            load();
        
        }
    }

    public void ChangeSlider()
    {
        AudioListener.volume = VolumeSlider.value;
        save();
    }

    private void save()
    {
        PlayerPrefs.SetFloat("musicSlider", VolumeSlider.value);
    }

    private void load()
    {
        VolumeSlider.value = PlayerPrefs.GetFloat("musicSlider");
    }

}
