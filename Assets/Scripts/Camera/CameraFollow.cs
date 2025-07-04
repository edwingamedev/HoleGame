using EdwinGameDev.EventSystem;
using EdwinGameDev.Gameplay;
using EdwinGameDev.Match;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Setup")] 
    [SerializeField] private MatchManager matchManager;
    
    [Header("Configuration")]
    [SerializeField] private float baseOffset = 40f;
    [SerializeField] private float offsetStep = 10f;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private float offsetLerpSpeed = 3f;

    private int GrowthsPerZoomStep => 3;
    private float offsetValue;
    private float currentOffsetValue;
    private readonly Vector3 offsetVector = new(0, 1, -1);

    private int growthCount;
    private int currentZoomStep;

    private Transform target;
    private Hole hole;

    private void Awake()
    {
        offsetValue = baseOffset;
        currentOffsetValue = baseOffset;
    }

    private void OnEnable()
    {
        GlobalEventDispatcher.AddSubscriber<Events.HoleCreated>(SetTarget);
    }

    private void OnDisable()
    {
        GlobalEventDispatcher.RemoveSubscriber<Events.HoleCreated>(SetTarget);
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

    private void SetTarget(Events.HoleCreated holeEvent)
    {
        if (hole)
        {
            hole.OnIncreaseHoleSize -= OnTargetSizeChanges;
        }
        
        hole = holeEvent.Hole;
        hole.OnIncreaseHoleSize += OnTargetSizeChanges;

        target = hole.transform;
    }

    private void OnTargetSizeChanges()
    {
        growthCount++;

        int newZoomStep = growthCount / GrowthsPerZoomStep;

        if (newZoomStep <= currentZoomStep)
        {
            return;
        }

        currentZoomStep = newZoomStep;
        offsetValue = baseOffset + (offsetStep * currentZoomStep);
    }
    
    private void OnDestroy()
    {
        hole.OnIncreaseHoleSize -= OnTargetSizeChanges;
    }
}