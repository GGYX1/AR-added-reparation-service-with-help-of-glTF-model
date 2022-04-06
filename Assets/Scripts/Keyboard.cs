using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    public TouchScreenKeyboard keyboard;
    public string keyboardText;
    public void OpenSystemKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
    }
    void Update()
    {
        if (keyboard != null)
        {
            keyboardText = keyboard.text;
        }
    }
}