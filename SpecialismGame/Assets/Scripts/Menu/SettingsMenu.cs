using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

public class SettingsMenu : MonoBehaviour
{

    PlayerStateMachine playerStateMachine;
    [Header("Font")]
    public List<TMP_FontAsset> fonts;
    [SerializeField] TMP_Dropdown fontDropdown;

    [Header("Font size")]
    [SerializeField] private float FZ_actualValue;
    [SerializeField] private Slider fontSizeSlider;
    [SerializeField] TMP_Text[] texts;
    [SerializeField] List<TMP_Text> textsWithTag;
    [Header("Subtitle size")]
    [SerializeField] private float SZ_actualValue;
    [SerializeField] private Slider subtitleSizeSlider;
    [SerializeField] TMP_Text[] subtitles;
    [SerializeField] TMP_Text subtitleText;
    [SerializeField] GameObject subSizeSettings;
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
    [Header("Interact Toggle")]
    [SerializeField] TMP_Text[] interactToggleText;
    [Header("Post-Processing")]
    [SerializeField] private Volume postProcessingVolume;
    [Header("Post Processing Effects")]
    private ColorAdjustments colorAdjustments;
    private Vignette vignette;
    private FilmGrain filmGrain;
    private LiftGammaGain liftGammaGain;

    [Header("PlayerPref Buttons,Sliders,Etc")]
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] Slider sensitivitySlider;
    [SerializeField] Toggle crosshairToggle;
    [SerializeField] Toggle invertCrosshairToggle;
    [SerializeField] Slider brightnessSlider;
    [SerializeField] Slider contrastSlider;
    [SerializeField] Slider gammaSlider;
    [SerializeField] Toggle vignetteToggle;
    [SerializeField] Toggle filmGrainToggle;
    [SerializeField] Toggle subtitleToggle;

    

    private void Awake()
    {
        playerStateMachine = FindObjectOfType<PlayerStateMachine>();
        Screen.fullScreen = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        FontStart();
        ResolutionStart();
        PostProcessingStart();
        LoadPlayerPrefs();
    }

    private void LoadPlayerPrefs()
    {
        Screen.fullScreen = PlayerPrefs.GetInt("FullscreenValue")!=1;
        sensitivityMultiplier = PlayerPrefs.GetFloat("SensitivityValue", 1f);
        foreach (TMP_Text text in texts)
        {
            TMP_FontAsset font = fonts[PlayerPrefs.GetInt("ActiveFont", 0)];
            text.font = font;
        }
        ToggleCrosshair(PlayerPrefs.GetInt("CrosshairActive") != 0);
        InvertCrosshair(PlayerPrefs.GetInt("InvertedCrosshair") != 0);
        PlayerStateMachine.controlScheme = PlayerPrefs.GetInt("ControlSchemeValue", 0);
        colorAdjustments.postExposure.value = PlayerPrefs.GetFloat("BrightnessValue", 0f);
        colorAdjustments.contrast.value = PlayerPrefs.GetFloat("ContrastValue", 0f);
        liftGammaGain.gamma.value = new Vector4(PlayerPrefs.GetFloat("GammaValue", 1f), PlayerPrefs.GetFloat("GammaValue", 1f), PlayerPrefs.GetFloat("GammaValue", 1f), PlayerPrefs.GetFloat("GammaValue", 1f));
        if (PlayerPrefs.HasKey("VignetteActive")) { vignette.active = PlayerPrefs.GetInt("VignetteActive") != 0; }
        else { vignette.active = true; }
        if (PlayerPrefs.HasKey("FilmGrainActive")) { filmGrain.active = PlayerPrefs.GetInt("FilmGrainActive") != 0;}
        else { filmGrain.active = true;}
        fontSizeSlider.value = PlayerPrefs.GetFloat("FontSize");
        float SlValue = fontSizeSlider.value;
        FZ_actualValue = fontSizeSlider.value;
        SliderTextFontSize(SlValue);
        subtitleSizeSlider.value = PlayerPrefs.GetFloat("SubtitleSize");
        SlValue = subtitleSizeSlider.value;
        SZ_actualValue = subtitleSizeSlider.value;
        SliderSubtitleFontSize(SlValue);
        loadToggleValues();
        ToggleSubtitles(PlayerPrefs.GetInt("SubtitleToggle") != 0);
    }

    private void loadToggleValues()
    {
        fullscreenToggle.isOn = Screen.fullScreen;
        sensitivitySlider.value = sensitivityMultiplier;
        fontDropdown.value = PlayerPrefs.GetInt("ActiveFont", 0);
        crosshairToggle.isOn = PlayerPrefs.GetInt("CrosshairActive") != 0;
        invertCrosshairToggle.isOn = PlayerPrefs.GetInt("InvertedCrosshair") != 0;
        ControlSchemeText();
        subtitleToggle.isOn = PlayerPrefs.GetInt("SubtitleToggle") != 0;
        brightnessSlider.value = colorAdjustments.postExposure.value;
        contrastSlider.value = colorAdjustments.contrast.value;
        gammaSlider.value = PlayerPrefs.GetFloat("GammaValue", 1f);
        vignetteToggle.isOn = vignette.active;
        filmGrainToggle.isOn = filmGrain.active;
    }

    private void PostProcessingStart()
    {
        postProcessingVolume = GameObject.FindGameObjectWithTag("VolumeMain").GetComponent<Volume>();
        postProcessingVolume.profile.TryGet(out colorAdjustments);
        postProcessingVolume.profile.TryGet(out vignette);
        postProcessingVolume.profile.TryGet(out filmGrain);
        postProcessingVolume.profile.TryGet(out liftGammaGain);
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
    public void SliderTextFontSize(float FZ_sameValue)
    {
        FZ_sameValue = (int)fontSizeSlider.value / (int)fontSizeSlider.value;
        while(fontSizeSlider.value > FZ_actualValue)
        {
            foreach (TMP_Text text in textsWithTag)
            {
                text.fontSize += FZ_sameValue*2;
            }
            FZ_actualValue += FZ_sameValue*2;
        }
        while (fontSizeSlider.value < FZ_actualValue)
        {
            foreach (TMP_Text text in textsWithTag)
            {
                text.fontSize -= FZ_sameValue*2;
            }
            FZ_actualValue-= FZ_sameValue * 2;
        }
        PlayerPrefs.SetFloat("FontSize", FZ_actualValue);
    }
    public void SliderSubtitleFontSize(float SZ_sameValue)
    {
        SZ_sameValue = (int)subtitleSizeSlider.value / (int)subtitleSizeSlider.value;
        while (subtitleSizeSlider.value > SZ_actualValue)
        {
            foreach (TMP_Text subtitle in subtitles)
            {
                subtitle.fontSize += SZ_sameValue * 2;
            }
            SZ_actualValue += SZ_sameValue * 2;
        }
        while (subtitleSizeSlider.value < SZ_actualValue)
        {
            foreach (TMP_Text subtitle in subtitles)
            {
                subtitle.fontSize -= SZ_sameValue * 2;
            }
            SZ_actualValue -= SZ_sameValue * 2;
        }
        PlayerPrefs.SetFloat("SubtitleSize", SZ_actualValue);
    }

    public void ToggleSubtitles(bool value)
    {
        subtitleText.enabled = value;
        subSizeSettings.SetActive(value);
        PlayerPrefs.SetInt("SubtitleToggle",(value ? 1 : 0));
    }

    public void SetResolution()
    {
        Resolution resolution = filteredResolutions[resolutionDropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetFullscreen(bool value)
    {
        Screen.fullScreen = value;
        PlayerPrefs.SetInt("FullscreenValue", (Screen.fullScreen ? 1 : 0));
    }
    public void SetSensitivity(float value)
    {
        sensitivityMultiplier = value;
        PlayerPrefs.SetFloat("SensitivityValue", sensitivityMultiplier);
    }
    public void SetFont()
    {
        TMP_FontAsset font = fonts[fontDropdown.value];
        foreach(TMP_Text text in texts)
        {
            text.font = font;
        }
        PlayerPrefs.SetInt("ActiveFont", fontDropdown.value);
    }    
    public void ToggleCrosshair(bool value)
    {
        crosshair.enabled = value;
        invertCrosshair.SetActive(value);
        exampleCrosshair.enabled = crosshair.enabled;
        exampleCrosshair.sprite = crosshair.sprite;
        PlayerPrefs.SetInt("CrosshairActive", (value ? 1 : 0));
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
        PlayerPrefs.SetInt("InvertedCrosshair", (value ? 1 : 0));
    }

    public void ControlSchemeLeft()
    {
        PlayerStateMachine.controlScheme--;
        if (PlayerStateMachine.controlScheme < 0)
        {
            PlayerStateMachine.controlScheme = 1;
        }
        ControlSchemeText();
    }
    public void ControlSchemeRight()
    {
        PlayerStateMachine.controlScheme++;
        if (PlayerStateMachine.controlScheme >= 2)
        {
            PlayerStateMachine.controlScheme = 0;
        }
        ControlSchemeText();
    }
    private void ControlSchemeText()
    {
        switch (PlayerStateMachine.controlScheme)
        {
            case 0:
                foreach (TMP_Text text in interactToggleText)
                {
                    text.text = "Press Interact to Interact";
                }
                break;
            case 1:
                foreach (TMP_Text text in interactToggleText)
                {
                    text.text = "Interacting is Automatic";
                }
                break;
        }
        PlayerPrefs.SetInt("ControlSchemeValue", PlayerStateMachine.controlScheme);
    }

    public void AdjustBrightness(float value)
    {
        colorAdjustments.postExposure.value= value;
        PlayerPrefs.SetFloat("BrightnessValue", colorAdjustments.postExposure.value);
    }

    public void AdjustContrast(float value)
    {
        colorAdjustments.contrast.value = value*10;
        PlayerPrefs.SetFloat("ContrastValue", colorAdjustments.contrast.value);
    }
    
    public void AdjustGamma(float value)
    {
        liftGammaGain.gamma.value = new Vector4(value, value, value, value);
        PlayerPrefs.SetFloat("GammaValue", value);
    }



    public void ToggleVignette(bool value)
    {
        vignette.active = value;
        PlayerPrefs.SetInt("VignetteActive", (vignette.active ? 1 : 0));
    }

    public void ToggleFilmGrain(bool value)
    {
        filmGrain.active = value;
        PlayerPrefs.SetInt("FilmGrainActive", (filmGrain.active ? 1 : 0));
    }
}
