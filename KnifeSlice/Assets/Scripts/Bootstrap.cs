using System;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private LevelConfig _levelConfig;
    [SerializeField] private InputHandler _input;
    [SerializeField] private SliceTool _sliceTool;
    [SerializeField] private SliceObjectPath _sliceObjectPath;
    [SerializeField] private SliceHandler _sliceHandler;
    [SerializeField] private DeformHandler _deformHandler;

    private void Awake()
    {
        _input.Init();
        
        _sliceHandler.Init(_sliceTool , _deformHandler);
        _sliceTool.Init(_input , _sliceHandler);
        _sliceObjectPath.Init(_levelConfig , _deformHandler , _sliceHandler , _sliceTool);
        
        _deformHandler.Init(_levelConfig , _sliceTool);
    }
}
