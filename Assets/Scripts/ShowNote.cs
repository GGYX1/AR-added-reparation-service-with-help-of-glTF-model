using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowNote : MonoBehaviour
{
    [SerializeField]
    private ImportObject ImportResult;
    GameObject Note;
    GameObject NoteContent;
    private void Start()
    {
        Note = this.transform.Find("Note").gameObject;
        NoteContent = Note.transform.Find("NoteContent").gameObject;
    }
    public void ChangeNoteState()
    {
        if (Note.activeSelf)
        {
            Note.SetActive(false);
        }
        else
        {
            Note.SetActive(true);
        }
        if (string.IsNullOrEmpty(NoteContent.GetComponent<TMP_Text>().text))
        {
            RemoveNoteHint();
        }
        else
        {
            AddNoteHint();
        }
    }
    public void AddNoteHint()
    {
        int i = int.Parse(this.name);
        this.transform.Find("Title").GetComponent<TMP_Text>().text = ImportResult.gltfObj.nodes[i].name + "\n<color=red>(Note)</color>";
    }
    public void RemoveNoteHint()
    {
        int i = int.Parse(this.name);
        this.transform.Find("Title").GetComponent<TMP_Text>().text = ImportResult.gltfObj.nodes[i].name;
    }
}