using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListControlObjectOutline :MonoBehaviour
{
    [SerializeField]
    private ImportObject importObj;
    private int NodeNumber;
    public void ChangeOutlineThroughList()
    {
        NodeNumber = int.Parse(this.name);
        if (importObj.gltfObj.NodeGameObjectPairs[NodeNumber].GetComponent<Outline>().enabled == true)
        {
            importObj.gltfObj.NodeGameObjectPairs[NodeNumber].GetComponent<Outline>().enabled = false;
        }
        else
        {
            importObj.gltfObj.NodeGameObjectPairs[NodeNumber].GetComponent<Outline>().enabled = true;
        }
    }
}