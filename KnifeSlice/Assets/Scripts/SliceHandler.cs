using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceHandler : MonoBehaviour
{
    // [SerializeField] private Sliceble _mainPart;
    [SerializeField] private Sliceble _slicedPart;
    [SerializeField] private List<Sliceble> _slicedParts;

    public event System.Action OnPartSliced;
    public event System.Action OnKnifeRechargedAfterSlice;
        
    private DeformHandler _deformHandler;
    private float _currentProgress;

    private bool _completedSlice = false;

    public void Init(SliceTool sliceTool , DeformHandler deformer)
    {
        _deformHandler = deformer;
        sliceTool.OnProgressChange += UpdateProgress;
    }

    public void SetSlicedPart(Sliceble part)
    {
        // Could also reference the deformer
        _slicedPart = part;
        _slicedParts.Add(_slicedPart);
        
        _slicedPart.SearchForComponents();
        _slicedPart.ActivateDeform();
    }

    private void UpdateProgress(float progress)
    {
        // wait for knife to set back in start position (after slice)
        if(HasRechargedAfterSlice(progress) == false)
            return;

        if (progress > _currentProgress)
        {
            _currentProgress = progress;
            _deformHandler.UpdatePosition(_currentProgress);

            if (_currentProgress == 1)
            {
                _completedSlice = true;
                OnPartSliced?.Invoke();
                DropParts();
            }
        }
    }

    private bool HasRechargedAfterSlice(float progress)
    {
        if (_completedSlice)
        {
            if (progress == 0)
            {
                _completedSlice = false;
                OnKnifeRechargedAfterSlice?.Invoke();
                
                // Reset progress (put deformer on start point)
                _currentProgress = progress;
                _deformHandler.UpdatePosition(_currentProgress);

                return true;
            }

            return false;
        }

        return true;
    }

    private void DropParts()
    {
        for (int i = 0; i < _slicedParts.Count; i++)
        {
            _slicedParts[i].ActivateDeform(false);
            StartCoroutine(DropPart(_slicedParts[i]));
        }
        
        _slicedParts.Clear();
    }

    private IEnumerator DropPart(Sliceble part)
    {
        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime;
            part.transform.position += new Vector3(0 , -1 , -0.6f) * Time.deltaTime;
            
            yield return null;
        }
    }
}
