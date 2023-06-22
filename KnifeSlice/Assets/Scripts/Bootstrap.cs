using System;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private InputHandler _input;
    [SerializeField] private SliceTool _sliceTool;

    private void Awake()
    {
        _input.Init();
        _sliceTool.Init(_input);
    }
}
