using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{





    [Header("Font size")]
    [SerializeField] private float FZ_actualValue;
    [SerializeField] private Slider fontSizeSlider;

    [SerializeField] TMP_Text[] texts;
    [SerializeField] List<TMP_Text> textsWithTag;
    // Start is called before the first frame update
    void Start()
    {
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

}
