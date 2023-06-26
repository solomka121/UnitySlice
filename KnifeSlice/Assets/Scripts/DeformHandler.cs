using UnityEngine;
using Deform;

public class DeformHandler : MonoBehaviour
{
    [SerializeField] private BendDeformer _deform;
    [SerializeField] private Vector3 _maxPosition; 
    [SerializeField] private Vector3 _minPosition;

    private LevelConfig _config;

    public void Init(LevelConfig config , SliceTool sliceTool)
    {
        _config = config;
        SetClampPositions(sliceTool);
    }

    private void SetClampPositions(SliceTool _sliceTool)
    {
        _maxPosition = transform.position;
        _minPosition = _maxPosition;
        
        _maxPosition.y = _sliceTool.MaxSlicePoint.y;
        _minPosition.y = _sliceTool.MinSlicePoint.y;
    }

    public void UpdatePosition(float progress)
    {
        transform.position = Vector3.Lerp(_maxPosition, _minPosition, progress);
    }

    public void SetAngle(float sliceWidthPercent)
    {
        _deform.Angle = Mathf.Lerp(_config.maxAngle , _config.minAngle, sliceWidthPercent);
    }

    public BendDeformer Deformer => _deform;
}
