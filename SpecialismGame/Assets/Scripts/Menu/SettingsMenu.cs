using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{



    [Header("Font")]
    public List<TMP_FontAsset> fonts;
    [SerializeField] TMP_Dropdown fontDropdown;

    [Header("Font size")]
    [SerializeField] private float FZ_actualValue;
    [SerializeField] private Slider fontSizeSlider;
    [SerializeField] TMP_Text[] texts;
    [SerializeField] List<TMP_Text> textsWithTag;
    [Header("Resolution")]
    [SerializeField] TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    List<Resolution> filteredResolutions;
    RefreshRate currentRefreshRate;
    int currentResIndex = 0;
    [Header("Sensitivity")]
    public float sensitivityMultiplier = 1f;
    [Header("Crosshair")]
    [SerializeField] Image crosshair;
    [SerializeField] Sprite whiteCrosshair;
    [SerializeField] Sprite blackCrosshair;
    [SerializeField] GameObject invertCrosshair;
    [SerializeField] Image exampleCrosshair;

    // Start is called before the first frame update
    void Start()
    {
        FontStart();
        ResolutionStart();
        SensitivityStart();
    }

    private void FontStart()
    {
        List<string> fontOptions = new List<string>();
        fontDropdown.ClearOptions();
        foreach (TMP_FontAsset font in fonts)
        {
            string fontName = font.name;
            fontOptions.Add(fontName);
        }
        fontDropdown.AddOptions(fontOptions);
        texts = TMP_Text.FindObjectsByType<TMP_Text>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (TMP_Text text in texts)
        {
            if (text.tag == "Text" || text.tag == "InteractPromptText")
            {
                textsWithTag.Add(text);
            }
        }
        FZ_actualValue = fontSizeSlider.value;
    }
    private void ResolutionStart()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRateRatio;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRateRatio.value == currentRefreshRate.value)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height;
            options.Add(resolutionOption);
            if (i == filteredResolutions.Count - 1)
            {
                currentResIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void SensitivityStart()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SliderTextFontSize()
    {
        int FZ_sameValue = (int)fontSizeSlider.value / (int)fontSizeSlider.value;
        if(fontSizeSlider.value > FZ_actualValue)
        {
            foreach (TMP_Text text in textsWithTag)
            {
                text.GetComponent<TMP_Text>().fontSize += FZ_sameValue*2;
            }
            FZ_actualValue += FZ_sameValue*2;
        }
        if (fontSizeSlider.value < FZ_actualValue)
        {
            foreach (TMP_Text text in textsWithTag)
            {
                text.GetComponent<TMP_Text>().fontSize -= FZ_sameValue*2;
            }
            FZ_actualValue-= FZ_sameValue * 2;
        }
    }
    
    public void SetResolution()
    {
        Resolution resolution = filteredResolutions[resolutionDropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetFullscreen(bool value)
    {
        Screen.fullScreen = value;
    }

    public void SetSensitivity(float value)
    {
        sensitivityMultiplier = value;
        //PlayerPrefs.SetFloat("SensKey", sensitivityMultiplier);
    }

    public void SetFont()
    {
        TMP_FontAsset font = fonts[fontDropdown.value];
        foreach(TMP_Text text in texts)
        {
            text.font = font;
        }
    }    

    public void ToggleCrosshair(bool value)
    {
        if (value)
        {
            crosshair.enabled = true;
            invertCrosshair.SetActive(true);
        }
        else
        {
            crosshair.enabled = false;
            invertCrosshair.SetActive(false);
        }
        exampleCrosshair.enabled = crosshair.enabled;
        exampleCrosshair.sprite = crosshair.sprite;
    }

    public void InvertCrosshair(bool value)
    {
        if (value)
        {
            crosshair.sprite = whiteCrosshair;
        }
        else
        {
            crosshair.sprite = blackCrosshair;
        }
        exampleCrosshair.sprite = crosshair.sprite;
    }
}
