using System;
using System.Collections.Generic;
using UnityEngine;

public class SliceObjectPath : MonoBehaviour
{
    [SerializeField] private Transform slicebleParent;
    [SerializeField] private LevelConfig _config;
    
    [SerializeField] private List<Sliceble> _slicebleParts;

    private DeformHandler _deformHandler;
    
    private float currentMoveAmount;
    private bool canMove = true;

    public void Init(LevelConfig config , DeformHandler deformHandler , SliceHandler sliceHandler , SliceTool sliceTool)
    {
        _config = config;
        _deformHandler = deformHandler;
        
        InitSliceObject(deformHandler);

        sliceHandler.OnKnifeRechargedAfterSlice += ResetMoveAmount;
        sliceTool.OnObjectSlice += StopMovement;
    }

    public void InitSliceObject(DeformHandler deformHandler)
    {
        //TODO spawn from SO
        SearchForChildren();
        
        for (int i = 0; i < _slicebleParts.Count; i++)
        {
            _slicebleParts[i].Init(deformHandler);
        }
    }
    
    public void SearchForChildren()
    {
        Sliceble[] parts = transform.GetComponentsInChildren<Sliceble>();
        _slicebleParts.AddRange(parts);
    }

    private void Update()
    {
        if(canMove == false)
            return;
        
        MoveSlicebleObject();
    }

    private void MoveSlicebleObject()
    {
        if (currentMoveAmount < _config.maxSliceWidth)
        {
            float amountToMove = Time.deltaTime * _config.moveSpeed;
            currentMoveAmount += amountToMove;
            slicebleParent.position += transform.forward * amountToMove;
        }
    }

    private void ResetMoveAmount()
    {
        currentMoveAmount = 0;
        canMove = true;
    }

    private void StopMovement()
    {
        _deformHandler.SetAngle(currentMoveAmount / _config.maxSliceWidth);
        canMove = false;
    }
}
