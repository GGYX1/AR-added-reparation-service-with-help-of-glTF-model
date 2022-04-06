using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadJson : MonoBehaviour
{
    public string InputJson;
    public InputData data = new InputData();
    public void DeserializeData(string savedData)
    {
        JsonUtility.FromJsonOverwrite(savedData, data);
    }
}
public class InputData
{
    public int[] highlight;
    public NoteNodeAndText[] note;
}