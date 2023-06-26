using System.Collections.Generic;
using UnityEngine;
using BzKovSoft.ObjectSlicer.Samples;
using Deform;

[RequireComponent(typeof(ObjectSlicerSample))]
[RequireComponent(typeof(Deformable))]

public class Sliceble : MonoBehaviour
{
    private ObjectSlicerSample _sliceComponent;
    private Deformable _deformComponent;

    public void Init(DeformHandler handler)
    {
        SearchForComponents();
        _deformComponent.AddDeformer(handler.Deformer);
    }

    public void SearchForComponents()
    {
        _sliceComponent = GetComponent<ObjectSlicerSample>();
        _deformComponent = GetComponent<Deformable>();
        _deformComponent.UpdateMode = UpdateMode.Pause;
    }

    public void ActivateDeform(bool state = true)
    {
        _deformComponent.UpdateMode = state ? UpdateMode.Auto : UpdateMode.Pause;
    }
}
