using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[Serializable]
public class Output : MonoBehaviour
{
    [SerializeField]
    private ImportObject CurrentState;
    private string Json;
    public void DebugData()
    {
        OutputData Data = new OutputData();
        for (int i = 0; i < CurrentState.gltfObj.NodeGameObjectPairs.Count; i++)
        {
            if (CurrentState.gltfObj.NodeGameObjectPairs[i].GetComponent<Outline>().enabled == true)
            {
                Data.highlight.Add(i);
            }
        }
        for (int i = 0; i < CurrentState.Children.Count; i++)
        {
            if (string.IsNullOrEmpty(CurrentState.Children[i].transform.Find("Note/NoteContent").GetComponent<TMP_Text>().text) == false)
            {
                NoteNodeAndText NoteNodeAndText = new NoteNodeAndText();
                NoteNodeAndText.node = i;
                NoteNodeAndText.text = CurrentState.Children[i].transform.Find("Note/NoteContent").GetComponent<TMP_Text>().text;
                Data.note.Add(NoteNodeAndText);
            }
        }
        Json = JsonUtility.ToJson(Data);
        Debug.Log(Json);
    }
}
[Serializable]
public class OutputData
{
    public List<int> highlight = new List<int>();
    public List<NoteNodeAndText> note = new List<NoteNodeAndText>();
}
[Serializable]
public class NoteNodeAndText
{
    public int node;
    public string text;
}