using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ReplaceButtons : MonoBehaviour
{
    public GamepadIcons icons;
    public void OnUpdateBindingDisplay(RebindUI component,  string controlPath)
    {
        if (string.IsNullOrEmpty(controlPath))
            return;

        var icon = default(Sprite);
        icon = icons.GetSprite(controlPath);

        var textComponent = component.rebindText;

        // Grab Image component.
        var imageGO = textComponent.transform.parent.Find("ActionBindingIcon");
        if (imageGO != null)
        {
            var imageComponent = imageGO.GetComponent<Image>();

            if (icon != null)
            {
                textComponent.gameObject.SetActive(false);
                imageComponent.sprite = icon;
                imageComponent.gameObject.SetActive(true);
            }
            else
            {
                textComponent.gameObject.SetActive(true);
                imageComponent.gameObject.SetActive(false);
            }
        } 
        else
        {
            textComponent.gameObject.SetActive(true);
        }
    }


}


[Serializable]
public struct GamepadIcons
{
    public Sprite buttonSouth;
    public Sprite buttonNorth;
    public Sprite buttonEast;
    public Sprite buttonWest;
    public Sprite startButton;
    public Sprite selectButton;
    public Sprite leftTrigger;
    public Sprite rightTrigger;
    public Sprite leftShoulder;
    public Sprite rightShoulder;
    public Sprite dpad;
    public Sprite dpadUp;
    public Sprite dpadDown;
    public Sprite dpadLeft;
    public Sprite dpadRight;
    public Sprite leftStick;
    public Sprite rightStick;
    public Sprite leftStickPress;
    public Sprite rightStickPress;

    public Sprite GetSprite(string controlPath)
    {
        // From the input system, we get the path of the control on device. So we can just
        // map from that to the sprites we have for gamepads.
        switch (controlPath)
        {
            case "Button South": return buttonSouth;
            case "Button North": return buttonNorth;
            case "Button East": return buttonEast;
            case "Button West": return buttonWest;
            case "Start": return startButton;
            case "Select": return selectButton;
            case "LT": return leftTrigger;
            case "RT": return rightTrigger;
            case "LB": return leftShoulder;
            case "RB": return rightShoulder;
            case "D-Pad Up/D-Pad Left/D-Pad Down/D-Pad Right": return dpad;
            case "D-Pad/Up/D-Pad/Left/D-Pad/Down/D-Pad/Right": return dpad;
            case "D-Pad Up": return dpadUp;
            case "D-Pad Down": return dpadDown;
            case "D-Pad Left": return dpadLeft;
            case "D-Pad Right": return dpadRight;
            case "LS": return leftStick;
            case "RS": return rightStick;
            case "Left Stick Press": return leftStickPress;
            case "Right Stick Press": return rightStickPress;
        }
        return null;
    }
}
