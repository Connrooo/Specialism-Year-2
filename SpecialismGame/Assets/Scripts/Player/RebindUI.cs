using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class RebindUI : MonoBehaviour
{

    [SerializeField] private InputActionReference inputActionReference;

    [SerializeField] private bool excludeMouse = true;
    [Range(0, 15)]
    [SerializeField] private int selectedBinding;
    [SerializeField] private InputBinding.DisplayStringOptions displayStringOptions;
    [Header("Binding Info - DO NOT EDIT")]
    [SerializeField] private InputBinding inputBinding;
    private int bindingIndex;

    public TMP_Text text;

    private string actionName;

    [Header("UI Fields")]
    
    public bool overrideAction;
    public string customActionText;
    [SerializeField] private TMP_Text actionText;
    [SerializeField] private Button rebindButton;
    [SerializeField] private TMP_Text rebindText;
    [SerializeField] private Button resetButton;


    private void Awake()
    {
        if (inputActionReference == null)
            return;
        GetBindingInfo();
        UpdateUI();
        UpdateActionLabel();
    }
    private void UpdateActionLabel()
    {
        if (actionText!=null)
        {
            if (overrideAction)
            {
                actionText.text = customActionText;
            }
            else
            {
                actionText.text = actionName;
            }
        }
    }

    private void OnEnable()
    {
        rebindButton.onClick.AddListener(() => DoRebind());
        resetButton.onClick.AddListener(() => ResetBinding());

        if (inputActionReference != null) 
        {
            InputManager.LoadBindingOverride(actionName);
            GetBindingInfo();
            UpdateUI();
        }

        InputManager.rebindComplete += UpdateUI;
        InputManager.rebindCancelled+= UpdateUI;
    }

    private void OnDisable()
    {
        InputManager.rebindComplete -= UpdateUI;
        InputManager.rebindCancelled-= UpdateUI;
    }


    private void OnValidate()
    {
        if (inputActionReference == null)
            return;
        GetBindingInfo();
        UpdateUI();
        UpdateActionLabel();
    }

    private void GetBindingInfo()
    {
        if(inputActionReference.action!= null)
        {
            actionName = inputActionReference.action.name;
        }
        if (inputActionReference.action.bindings.Count>selectedBinding)
        {
            inputBinding = inputActionReference.action.bindings[selectedBinding];
            bindingIndex = selectedBinding;
        }
    }
    private void UpdateUI()
    {
        UpdateActionLabel();
        if (rebindText!=null)
        {
            if (Application.isPlaying)
            {
                rebindText.text = InputManager.GetBindingName(actionName, bindingIndex);
            }
            else
                rebindText.text = inputActionReference.action.GetBindingDisplayString(bindingIndex);
        }
    }

    private void DoRebind()
    {
        text.text = "STARTED REBIND";
        InputManager.StartRebind(actionName, bindingIndex, rebindText, excludeMouse);
    }

    private void ResetBinding()
    {
        InputManager.ResetBinding(actionName, bindingIndex);
        UpdateUI();
    }
}
