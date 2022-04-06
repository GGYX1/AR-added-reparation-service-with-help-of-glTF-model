using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities.Gltf.Serialization;
using Microsoft.MixedReality.Toolkit.Utilities.Gltf.Schema;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
public class ImportObject : MonoBehaviour
{
    public string uri;
    private ReadJson ReadResult;
    public GltfObject gltfObj;
    private GameObject TempGmaeObject;
    void Start()
    {
        ReadResult = this.GetComponent<ReadJson>();
        ReadResult.DeserializeData(ReadResult.InputJson);
        ImportGLTF(uri);
    }
    public async void ImportGLTF(string uri)
    {
        gltfObj = await GltfUtility.ImportGltfObjectFromPathAsync(uri);
        for (int i = 0; i < gltfObj.NodeGameObjectPairs.Count; i++)
        {
            TempGmaeObject = gltfObj.NodeGameObjectPairs[i];
            TempGmaeObject.AddComponent<Outline>();
            TempGmaeObject.GetComponent<Outline>().OutlineColor = new Color(0, 0, 255, 255);
            TempGmaeObject.GetComponent<Outline>().OutlineWidth = 4;
            TempGmaeObject.GetComponent<Outline>().enabled = false;
            TempGmaeObject.AddComponent<MeshCollider>();
            TempGmaeObject.AddComponent<ClickControlObjectOutline>();
            var interactable = TempGmaeObject.AddComponent<Interactable>();
            interactable.OnClick.AddListener(TempGmaeObject.GetComponent<ClickControlObjectOutline>().ChangeOutlineThroughClick);
        }
        if (ReadResult.data.highlight != null)
        {
            for (int i = 0; i < ReadResult.data.highlight.Length; i++)
            {
                gltfObj.NodeGameObjectPairs[ReadResult.data.highlight[i]].GetComponent<Outline>().enabled = true;
            }
        }
        Generate();
        SetNumber();
        SetParents();
        MatchAndNote();
        Destroy();
    }
    public GameObject Title;
    public GameObject TitleWithChildren;
    private List<int> Parents = new List<int>();
    private int ChildCount;
    public List<Transform> Children = new List<Transform>();
    public void Generate()
    {
        for (int i = 0; i < gltfObj.nodes.Length; i++)
        {
            if (gltfObj.nodes[i].children != null)
            {
                Parents.Add(i);
                var p = Instantiate(TitleWithChildren);
                p.transform.parent = this.transform;
                p.transform.Find("Title").GetComponent<TMP_Text>().text = gltfObj.nodes[i].name;
                p.GetComponent<BoxCollider>().center = Vector3.zero;
                p.GetComponent<BoxCollider>().size = new Vector3(1, 1, 6);
                p.name = i.ToString();
            }
            else
            {
                var p = Instantiate(Title);
                p.transform.parent = this.transform;
                p.transform.Find("Title").GetComponent<TMP_Text>().text = gltfObj.nodes[i].name;
                p.name = i.ToString();
            }
        }
    }
    public void SetNumber()
    {
        ChildCount = this.transform.childCount;
        for (int i = 0; i < ChildCount; i++)
        {
            Children.Add(this.transform.GetChild(i));
        }
    }
    public void SetParents()
    {
        for (int i = 0; i < Parents.Count; i++)
        {
            {
                for (int j = 0; j < gltfObj.nodes[Parents[i]].children.Length; j++)
                {
                    Children[gltfObj.nodes[Parents[i]].children[j]].SetParent(Children[Parents[i]].transform.Find("Container").Find("NodeName"));
                }
            }
        }
    }
    public void MatchAndNote()
    {
        if (ReadResult.data.note != null)
        {
            for (int i = 0; i < ReadResult.data.note.Length; i++)
            {
                Children[ReadResult.data.note[i].node].transform.Find("Note/NoteContent").GetComponent<TMP_Text>().text = ReadResult.data.note[i].text;
                Children[ReadResult.data.note[i].node].transform.Find("Title").GetComponent<TMP_Text>().text = gltfObj.nodes[ReadResult.data.note[i].node].name + "\n<color=red>(Note)</color>";
            }
        }
    }
    private void Destroy()
    {
        Destroy(Title);
        Destroy(TitleWithChildren);
    }
}