using BzKovSoft.ObjectSlicer;
using UnityEngine;

public class SliceTool : MonoBehaviour
{
    [SerializeField] private float _maxHeight;
    [SerializeField] private float _minHeight;
    [SerializeField] private Transform _slicePoint; // Need to be at max position on Init
    [SerializeField] private LayerMask _slicebleMask;
    [SerializeField] private float _sliceToolLenght = 1f;

    private SliceHandler _sliceHandler;

    public event System.Action OnObjectSlice;
    
    public float _rechargeSpeed;
    public float _sliceSpeed;

    private bool _isSlicing = false;
    
    private bool _isActive = true;
    private bool _canMoveDown = true;

    private bool _moveDown = false; 
    private float _currentMoveProgress;

    public event System.Action<float> OnProgressChange;
    
    public Vector3 MaxSlicePoint => _slicePoint.position;
    public Vector3 MinSlicePoint => _slicePoint.localPosition + new Vector3(0 , _minHeight , 0);

    public void Init(InputHandler input , SliceHandler sliceHandler)
    {
        input.OnScreenTouchDown += MoveToSlice;
        input.OnScreenTouchUp += MoveToStart;

        _sliceHandler = sliceHandler;
    }

    public void MoveToSlice()
    {
        _moveDown = true;
    }

    public void MoveToStart()
    {
        _moveDown = false;
        _canMoveDown = false;
    }

    private void Update()
    {
        CheckForSlice();
        
        if(_isActive == false)
            return;
        
        transform.position = GetNextPosition();
    }
    
    public void CheckForSlice()
    {
        RaycastHit[] hit = Physics.RaycastAll(_slicePoint.transform.position , _slicePoint.forward , _sliceToolLenght , _slicebleMask);
        if(hit.Length > 0)
        {
            OnObjectSlice?.Invoke();

            for (int i = 0; i < hit.Length; i++)
            {
                if(hit[i].transform.TryGetComponent<IBzSliceable>(out IBzSliceable sliceble))
                    _isSlicing = true;
            
                Plane slicePlane = new Plane(-transform.right , _slicePoint.position); 
                sliceble.Slice(slicePlane , sliceCallBack =>
                {
                    if(sliceCallBack.sliced)
                    {
                        _sliceHandler.SetSlicedPart(sliceCallBack.outObjectPos.GetComponent<Sliceble>());
                    }
                });
            }
        }
        else
        {
            _isSlicing = false;
        }
    }

    private Vector3 GetNextPosition()
    {
        if (_moveDown && _canMoveDown)
        {
            _currentMoveProgress += Time.deltaTime * _sliceSpeed;
        }
        else
        {
            _currentMoveProgress += -(Time.deltaTime * _rechargeSpeed);
        }
        
        _currentMoveProgress = Mathf.Clamp(_currentMoveProgress , 0, 1);
        OnProgressChange?.Invoke(_currentMoveProgress);

        if (_currentMoveProgress == 0)
        {
            _canMoveDown = true;
        }

        return new Vector3(transform.position.x , Mathf.Lerp(_maxHeight, _minHeight, _currentMoveProgress) , transform.position.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _isSlicing ? Color.red : Color.green;
        Gizmos.DrawRay(_slicePoint.transform.position , _slicePoint.forward * _sliceToolLenght);
    }
}
