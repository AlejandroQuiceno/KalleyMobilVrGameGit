using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;
using UnityEngine.Events;
using System;

public class ShowKeyboard : MonoBehaviour
{
    private TMP_InputField inputField;

    void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onSelect.AddListener(x => OpenKeyboard());
    }

    private void SubmitText(object sender, EventArgs e)
    {
        Debug.Log(inputField.text);
    }

    private void Instance_Onclosed(object sender, EventArgs e)
    {
        SetCaretColorAlpha(0);
        NonNativeKeyboard.Instance.OnClosed -= Instance_Onclosed;
        NonNativeKeyboard.Instance.OnTextSubmitted -= SubmitText;
    }
    public void OpenKeyboard()
    {
        NonNativeKeyboard instance = NonNativeKeyboard.Instance;
        instance.InputField = inputField;
        instance.PresentKeyboard(inputField.text);
        NonNativeKeyboard.Instance.OnClosed += Instance_Onclosed;
        NonNativeKeyboard.Instance.OnTextSubmitted += SubmitText;

        SetCaretColorAlpha(1);
    }
    public void SetCaretColorAlpha(float value)
    {
        inputField.customCaretColor = true;
        Color caretColor = inputField.caretColor;
        caretColor.a = value;
        inputField.caretColor = caretColor;
    }
}
