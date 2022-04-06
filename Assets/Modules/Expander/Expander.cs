using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expander : MonoBehaviour {
    [SerializeField]
    private AnimationCurve animationCurve;

    [SerializeField]
    private float animationLength;

    [SerializeField]
    private bool expanded = true;

    /// <summary>
    /// main collider in expander gameobject, should have only one boxCollider
    /// </summary>
    [SerializeField]
    private AnimationCollider TargetCollider;

    /// <summary>
    /// since children could have different moving direction, use list here
    /// </summary>
    [Tooltip("the children gameobjects under expander")]
    [SerializeField]
    private List<AnimationTransform> TargetObjects;

    [SerializeField]
    private List<System.Action> callbackDuringCoroutine;

    [SerializeField]
    private List<System.Action> callbackAfterCoroutine;

    

    private void Start() {
        if (callbackAfterCoroutine == null) callbackAfterCoroutine = new List<Action>();
        if (!callbackAfterCoroutine.Contains(updateState)) callbackAfterCoroutine.Add(updateState);

        if (callbackDuringCoroutine == null) callbackDuringCoroutine = new List<Action>();
        if (!callbackDuringCoroutine.Contains(notifyParent)) callbackDuringCoroutine.Add(notifyParent);
    }

    private void notifyParent() {
        var listCollection = GetComponentInParent<ListObjectCollection>();
        if (listCollection == null) return;
        listCollection.UpdateCollection();
    }

    private void updateState() {
        expanded = !expanded;
    }

    [EasyButtons.Button]
    [SerializeField]
    private void SetExpandedToCurrent() {
        foreach(var o in TargetObjects) {
            o.Expanded.LocalPosition = o.Transform.localPosition;
            o.Expanded.LocalRotation = o.Transform.localRotation;
            o.Expanded.LocalScale = o.Transform.localScale;
        }

        TargetCollider.Expanded.Center = TargetCollider.Collider.center;
        TargetCollider.Expanded.Size = TargetCollider.Collider.size;
    }

    [EasyButtons.Button]
    [SerializeField]
    private void SetCollapsedToCurrent() {
        foreach (var o in TargetObjects) {
            o.Collapsed.LocalPosition = o.Transform.localPosition;
            o.Collapsed.LocalRotation = o.Transform.localRotation;
            o.Collapsed.LocalScale = o.Transform.localScale;
        }

        TargetCollider.Collapsed.Center = TargetCollider.Collider.center;
        TargetCollider.Collapsed.Size = TargetCollider.Collider.size;
    }

    [EasyButtons.Button]
    [SerializeField]
    private void RestoreToExpanded() {
        foreach (var o in TargetObjects) {
            o.Transform.localPosition = o.Expanded.LocalPosition;
            o.Transform.localRotation = o.Expanded.LocalRotation;
            o.Transform.localScale = o.Expanded.LocalScale;
        }

        TargetCollider.Collider.center = TargetCollider.Expanded.Center;
        TargetCollider.Collider.size = TargetCollider.Expanded.Size;

        expanded = true;
    }

    [EasyButtons.Button]
    [SerializeField]
    private void RestoreToCollapsed() {
        foreach (var o in TargetObjects) {
            o.Transform.localPosition = o.Collapsed.LocalPosition;
            o.Transform.localRotation = o.Collapsed.LocalRotation;
            o.Transform.localScale = o.Collapsed.LocalScale;
        }

        TargetCollider.Collider.center = TargetCollider.Collapsed.Center;
        TargetCollider.Collider.size = TargetCollider.Collapsed.Size;

        expanded = false;
    }

    [EasyButtons.Button]
    public void Toggle() {
        StartCoroutine(Animate());
    }

    /// <summary>
    /// Animation control for a group of GameObjects and colliders
    /// </summary>
    /// <param name="callbackAfterCoroutine">Optional callback action to be invoked after animation coroutine has finished</param>
    public IEnumerator Animate() {
        var curve = animationCurve;
        var time = animationLength;

        float counter = 0.0f;

        //todo: fix the last frame bug
        // in low FPS scenario, the expander could not fully expand or collapse
        while (counter <= time) {
            float eva = curve.Evaluate(counter / (float)time);

            foreach(var o in TargetObjects) {
                o.Transform.localPosition =
                    expanded ?
                    Vector3.Lerp(o.Expanded.LocalPosition, o.Collapsed.LocalPosition, eva) :
                    Vector3.Lerp(o.Collapsed.LocalPosition, o.Expanded.LocalPosition, eva);
                o.Transform.localRotation =
                    expanded ?
                    Quaternion.Lerp(o.Expanded.LocalRotation, o.Collapsed.LocalRotation, eva) :
                    Quaternion.Lerp(o.Collapsed.LocalRotation, o.Expanded.LocalRotation, eva);
                o.Transform.localScale =
                    expanded ?
                    Vector3.Lerp(o.Expanded.LocalScale, o.Collapsed.LocalScale, eva) :
                    Vector3.Lerp(o.Collapsed.LocalScale, o.Expanded.LocalScale, eva);
            }

            TargetCollider.Collider.center =
                    expanded ?
                    Vector3.Lerp(TargetCollider.Expanded.Center, TargetCollider.Collapsed.Center, eva) :
                    Vector3.Lerp(TargetCollider.Collapsed.Center, TargetCollider.Expanded.Center, eva);
            TargetCollider.Collider.size =
                expanded ?
                Vector3.Lerp(TargetCollider.Expanded.Size, TargetCollider.Collapsed.Size, eva) :
                Vector3.Lerp(TargetCollider.Collapsed.Size, TargetCollider.Expanded.Size, eva);

            counter += Time.deltaTime;
            if (callbackDuringCoroutine != null && callbackDuringCoroutine.Count > 0) {
                foreach (var c in callbackDuringCoroutine)
                    c?.Invoke();
            }
            yield return null;
        }

        if (callbackAfterCoroutine != null && callbackAfterCoroutine.Count > 0) {
            foreach (var c in callbackAfterCoroutine)
                c?.Invoke();
        }
    }
}

[Serializable]
public struct TransformState {
    public Vector3 LocalPosition;
    public Quaternion LocalRotation;
    public Vector3 LocalScale;
    public TransformState(Transform t) {
        LocalPosition = t != null ? t.localPosition : new Vector3();
        LocalRotation = t != null ? t.localRotation : new Quaternion();
        LocalScale = t != null ? t.localScale : new Vector3();
    }
    public TransformState(Vector3 pos, Quaternion rot, Vector3 sca) {
        LocalPosition = pos;
        LocalRotation = rot;
        LocalScale = sca;
    }
}

[Serializable]
public class AnimationTransform {
    public Transform Transform;
    public TransformState Expanded;
    public TransformState Collapsed;
}

[Serializable]
public struct ColliderState {
    public Vector3 Center;
    public Vector3 Size;
}

[Serializable]
[ExecuteInEditMode]
public class AnimationCollider {
    public BoxCollider Collider;
    public ColliderState Expanded;
    public ColliderState Collapsed;
}

