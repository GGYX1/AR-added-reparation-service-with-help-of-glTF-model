using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTitleBar : MonoBehaviour
{
    private float ChildCount;
    private float SumBoxY = 0;
    private bool Folded = true;
    private GameObject ThisNodeName;
    [SerializeField]
    private GameObject EldestNodeName;
    private void Start()
    {
        ThisNodeName = this.transform.Find("Container").Find("NodeName").gameObject;
    }
    public void FoldAndUnfold()
    {
        ChildCount = ThisNodeName.transform.childCount;
        if (Folded)
        {
            Unfold();
            Folded = false;
            for (int i = 0; i < ChildCount; i++)
            {
                ThisNodeName.transform.GetChild(i).Find("BackPlate").localScale = new Vector3(this.transform.Find("BackPlate").localScale.x * 0.95f, 1f, 5f);
                ThisNodeName.transform.GetChild(i).Find("BackPlate").localPosition += new Vector3(this.transform.Find("BackPlate").localPosition.x - 0.015f, 0, 0);
                ThisNodeName.transform.GetChild(i).Find("Note").localPosition += new Vector3(this.transform.Find("Note").localPosition.x - 0.03f, 0, 0);
                ThisNodeName.transform.GetChild(i).Find("Buttons").localPosition = new Vector3(this.transform.Find("Buttons").localPosition.x - 0.03f, 0, -3.08f);
            }
        }
        else
        {
            Fold();
            Folded = true;
            for (int i = 0; i < ChildCount; i++)
            {
                ThisNodeName.transform.GetChild(i).Find("BackPlate").localScale = new Vector3(this.transform.Find("BackPlate").localScale.x / 0.95f, 1f, 5f);
                ThisNodeName.transform.GetChild(i).Find("BackPlate").localPosition = new Vector3(0, 0, 0);
                ThisNodeName.transform.GetChild(i).Find("Note").localPosition = new Vector3(0, 0, 0);
            }
        }
    }
    public void Unfold()
    {
        ThisNodeName.SetActive(true);
        this.GetComponent<BoxCollider>().center = new Vector3(0, -CalculateSumBoxY() / 2, 0);
        this.GetComponent<BoxCollider>().size = new Vector3(1, CalculateSumBoxY() + 1, 6);
    }
    public void Fold()
    {
        ThisNodeName.SetActive(false);
        this.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);
        this.GetComponent<BoxCollider>().size = new Vector3(1, 1, 6);
    }
    public void ChangeBoxCollider()
    {
        this.GetComponent<BoxCollider>().center = new Vector3(0, -CalculateSumBoxY() / 2, 0);
        this.GetComponent<BoxCollider>().size = new Vector3(1, CalculateSumBoxY() + 1, 6);
    }
    public float CalculateSumBoxY()
    {
        SumBoxY = 0;
        int BoxCount;
        BoxCount = ThisNodeName.transform.childCount;
        for (int i = 0; i < BoxCount; i++)
        {
            SumBoxY = SumBoxY + ThisNodeName.transform.GetChild(i).GetComponent<BoxCollider>().size.y;
        }
        return SumBoxY;
    }
    private void Update()
    {
        ThisNodeName.GetComponent<ListObjectCollection>().UpdateCollection();
        EldestNodeName.GetComponent<ListObjectCollection>().UpdateCollection();
        if (!Folded)
        {
            ChangeBoxCollider();
        }
    }
}