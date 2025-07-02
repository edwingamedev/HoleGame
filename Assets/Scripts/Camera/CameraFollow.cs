using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float baseOffset = 40f;
    [SerializeField] private float offsetStep = 10f;
    [SerializeField] private int growthsPerZoomStep = 3;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private float offsetLerpSpeed = 3f;
    
    private float offsetValue;
    private float currentOffsetValue;
    private readonly Vector3 offsetVector = new(0, 1, -1);

    private int growthCount;
    private int currentZoomStep;

    private Transform target;
    
    private void Awake()
    {
        offsetValue = baseOffset;
        currentOffsetValue = baseOffset;
    }

    private void LateUpdate()
    {
        if (!target)
        {
            return;
        }

        currentOffsetValue = Mathf.Lerp(currentOffsetValue, offsetValue, Time.deltaTime * offsetLerpSpeed);
        
        Vector3 desiredPosition = target.position + (offsetVector * currentOffsetValue);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void OnTargetSizeChanges()
    {
        growthCount++;

        int newZoomStep = growthCount / growthsPerZoomStep;
        
        if (newZoomStep <= currentZoomStep)
        {
            return;
        }

        currentZoomStep = newZoomStep;
        offsetValue = baseOffset + (offsetStep * currentZoomStep);
    }
}