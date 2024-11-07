using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Title("Gameobject Reference")]
    [SerializeField] TMP_Dropdown resolutionDropdown;
    
    [Title("Gameobject Reference")]
    [SerializeField] List<Slider> volumeSlidersList;


    private SaveManager SaveManager => SaveManager.instance;
    private string settingsFileName => SaveManager.settingsFileName;
    private AudioMixer audioMixer => SaveManager.audioMixer;
    private float defaultVolumeValue => SaveManager.defaultVolumeValue;

    private List<string> volumeSaveNamesList => SaveManager.volumeSaveNamesList;
    private string sliderSavePrefix => SaveManager.sliderSavePrefix;

    private List<string> screenSaveNamesList => SaveManager.screenSaveNamesList;
    private List<Resolution> resolutionsList => SaveManager.resolutionsList;

    // Start is called before the first frame update

    void Start()
    {
        LoadingSave();
        ResolutionsDropdownFill();
    }

    void LoadingSave()
    {
        for (int i = 0; i < volumeSlidersList.Count; i++)
        { LoadingSliderStartValue(i, volumeSaveNamesList[i]); }
    }
    
    void LoadingSliderStartValue(int sliderNumber, string volumeName)
    {
        float volumeValue = ES3.Load(volumeName + sliderSavePrefix, defaultValue: defaultVolumeValue, filePath: settingsFileName);
        volumeSlidersList[sliderNumber].value = volumeValue;

    }

    public void SetVolume(int sliderNumber)
    {
        float sliderValue = volumeSlidersList[sliderNumber].value;
        float volume = Mathf.Log10(sliderValue)*20;
        
        audioMixer.SetFloat(volumeSaveNamesList[sliderNumber], volume);

        ES3.Save(volumeSaveNamesList[sliderNumber] + sliderSavePrefix, sliderValue, filePath: settingsFileName);
        ES3.Save(volumeSaveNamesList[sliderNumber], volume, filePath: settingsFileName);
    }

    void ResolutionsDropdownFill()
    {
        resolutionDropdown.ClearOptions();

        var currentResolutionIndex = 0;
        var resolutionsNameList = new List<string>();

        for (int i = 0; i < resolutionsList.Count; i++)
        {
            //Debug.Log(i);
            var resolution = resolutionsList[i];
            string resolutionName = resolution.width + "x" + resolution.height + " " + (int)resolution.refreshRateRatio.value + "Hz";
            resolutionsNameList.Add(resolutionName);

            if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
            { currentResolutionIndex = i; }


        }

        resolutionDropdown.AddOptions(resolutionsNameList);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        //Debug.Log(resolutionsList.Count + " " + resolutionIndex);
        Resolution resolution = resolutionsList[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        ES3.Save(screenSaveNamesList[0], resolutionIndex, filePath: settingsFileName);
    }

    public void SetWindowMode(bool IsFullscreen)
    {
        Screen.fullScreen = IsFullscreen;

        ES3.Save(screenSaveNamesList[1], IsFullscreen, filePath: settingsFileName);
    }
}
