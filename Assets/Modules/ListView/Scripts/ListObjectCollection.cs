using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public enum ListLayout {
    Vertical = 0,
    Horizontal = 1,
}

/// <summary>
/// this is an extend class for BaseObjectCollection to accomplish ListView / TreeView with expander
/// </summary>
/// <remarks>
/// 1. current version has only vertikal orientation
/// 2. each child should have a BoxCollider in topmost Hierarchy
/// 3. the origin point is top-left corner of the first element, future version can manually setup
/// </remarks>
[ExecuteAlways]
public class ListObjectCollection : BaseObjectCollection {
    /// <remarks>
    /// 1. did not use mrtk LayoutOrder since it contains other two possibilities
    /// </remarks>
    [SerializeField]
    private ListLayout layout;

    [SerializeField]
    private LayoutHorizontalAlignment hAlignment;

    [SerializeField]
    private LayoutVerticalAlignment vAlignment;

    [Range(0, 1)]
    [SerializeField]
    private float gap = 0;

    protected override void LayoutChildren() {
        if (NodeList.Count == 0) {
            Debug.LogWarning("no elements in NodeList!");
            return;
        }

        LastElement lastElement = new LastElement();
        int minorOffset = (layout == ListLayout.Horizontal ? ((int)vAlignment) : ((int)hAlignment)) - 1;
        foreach (var node in NodeList) {
            lastElement = UpdateCurrentObjectTransform(layout, minorOffset, lastElement, node);
        }
    }

    public override void UpdateCollection() {
        base.UpdateCollection();
        foreach (var node in NodeList) {
            if (node.Transform.GetComponent<BoxCollider>() == null) {
                Debug.LogError($"{node.Name} does not have a BoxCollider!");
            }
        }
    }

    /// <summary>
    /// this method is to update the nodes transform in NodeList, all transform elements are calculated via BoxCollider
    /// </summary>
    /// <param name="mainDirection">horizontal or vertical</param>
    /// <param name="minorDirectionOffset">-1, 0, 1 according to the switch result</param>
    /// <param name="last">last node information</param>
    /// <param name="current">current node</param>
    /// <returns>updated transform information of current node</returns>
    private LastElement UpdateCurrentObjectTransform(ListLayout mainDirection, int minorDirectionOffset, LastElement last, ObjectCollectionNode current) {
        var collider = current.Transform.GetComponent<BoxCollider>();

        current.Transform.localRotation = new Quaternion();

        float x = 0, y = 0, z;
        if (mainDirection == ListLayout.Horizontal) {
            x =
                last.localPosition.x + last.colliderCenter.x * last.localScale.x // last collider center
                + last.colliderSize.x / 2f * last.localScale.x + collider.size.x / 2f * current.Transform.localScale.x + gap  // colliders center distance
                - collider.center.x * current.Transform.localScale.x; // offset of current collider center from transform
            y =
               minorDirectionOffset * (collider.size.y - collider.center.y) * current.Transform.localScale.y / 2f;
        } else if (mainDirection == ListLayout.Vertical) {
            x =
               -minorDirectionOffset * (collider.size.x - collider.center.x) * current.Transform.localScale.x / 2f;
            y =
               (last.localPosition.y + last.colliderCenter.y * last.localScale.y) // last collider center
               - ((last.colliderSize.y / 2f * last.localScale.y + collider.size.y / 2f * current.Transform.localScale.y) + gap) // colliders center distance
               - collider.center.y * current.Transform.localScale.y;
        }
        z = -collider.center.z;
        current.Transform.localPosition = new Vector3(x, y, z);

        return new LastElement {
            colliderSize = collider.size,
            colliderCenter = collider.center,

            localPosition = current.Transform.localPosition,
            localScale = current.Transform.localScale,
        };
    }

    /// <summary>
    /// contains necessary information of last element in NodeList
    /// this is to avoid null exception for calculating the first node
    /// </summary>
    private class LastElement {
        public Vector3 colliderSize;
        public Vector3 colliderCenter;

        public Vector3 localPosition;
        public Vector3 localScale;
    }
}
