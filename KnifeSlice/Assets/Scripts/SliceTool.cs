using BzKovSoft.ObjectSlicer;
using UnityEngine;

public class SliceTool : MonoBehaviour
{
    [SerializeField] private float _maxHeight;
    [SerializeField] private float _minHeight;
    [SerializeField] private Transform _slicePoint;
    [SerializeField] private LayerMask _slicebleMask;
    [SerializeField] private float _sliceToolLenght = 1f;
    
    public float _rechargeSpeed;
    public float _sliceSpeed;

    private bool _isSlicing = false;
    private bool _canMove = true;

    private bool _moveDown = false; 
    private float _currentMoveProgress;
    
    public void Init(InputHandler input)
    {
        input.OnScreenTouchDown += MoveToSlice;
        input.OnScreenTouchUp += MoveToStart;
    }

    public void MoveToSlice()
    {
        _moveDown = true;
    }

    public void MoveToStart()
    {
        _moveDown = false;
    }

    private void Update()
    {
        CheckForSlice();
        
        if(_canMove == false)
            return;
        
        transform.position = GetNextPosition();
    }
    
    public void CheckForSlice()
    {
        RaycastHit hit = new RaycastHit();
        if(Physics.Raycast(_slicePoint.transform.position , _slicePoint.forward , out hit , _sliceToolLenght , _slicebleMask))
        {
            if(hit.transform.TryGetComponent<IBzSliceable>(out IBzSliceable sliceble))
            _isSlicing = true;
            Debug.Log("Slice " + hit.transform.name);
            Plane slicePlane = new Plane(transform.right , _slicePoint.position); 
            sliceble.Slice(slicePlane , null);
        }
        else
        {
            _isSlicing = false;
        }
    }

    private Vector3 GetNextPosition()
    {
        _currentMoveProgress += _moveDown ? Time.deltaTime * _sliceSpeed : -(Time.deltaTime * _rechargeSpeed);
        _currentMoveProgress = Mathf.Clamp(_currentMoveProgress , 0, 1); 

        return new Vector3(transform.position.x , Mathf.Lerp(_maxHeight, _minHeight, _currentMoveProgress) , transform.position.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _isSlicing ? Color.red : Color.green;
        Gizmos.DrawRay(_slicePoint.transform.position , _slicePoint.forward * _sliceToolLenght);
    }
}
