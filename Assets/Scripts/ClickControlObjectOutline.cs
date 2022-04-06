using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickControlObjectOutline : MonoBehaviour
{
    public void ChangeOutlineThroughClick()
    {
        if (this.GetComponent<Outline>().enabled == true)
        {
            this.GetComponent<Outline>().enabled = false;
        }
        else
        {
            this.GetComponent<Outline>().enabled = true;
        }
    }
}