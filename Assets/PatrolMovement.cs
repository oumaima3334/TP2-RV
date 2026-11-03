using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    public enum MovementType { MoveTowards, Lerp, Smooth, Custom }

    [SerializeField] private Transform pointA;                
    [SerializeField] private Transform pointB;                 
    [SerializeField] private float speed = 1f;                 
    [SerializeField] private MovementType movementType = MovementType.MoveTowards;
    [SerializeField] private AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1); 

    private const float arrivalThreshold = 0.01f; 
    private bool goingToB = true;                  

    void Start()
    {
        
        transform.position = pointA.position;
    }

    void Update()
    {
        switch (movementType)
        {
            case MovementType.MoveTowards:
                MoveWithMoveTowards();
                break;
            case MovementType.Lerp:
                MoveWithLerp();
                break;
            case MovementType.Smooth:
                MoveWithSmooth();
                break;
            case MovementType.Custom:
                MoveWithCurve();
                break;
        }
    }

    
    private void MoveWithMoveTowards()
    {
        Vector3 target = goingToB ? pointB.position : pointA.position;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        
        if (Vector3.Distance(transform.position, target) < arrivalThreshold)
            goingToB = !goingToB;
    }

    
    private void MoveWithLerp()
    {
        float t = GetPingPongT();
        transform.position = Vector3.Lerp(pointA.position, pointB.position, t);
    }

    
    private void MoveWithSmooth()
    {
        float t = Mathf.SmoothStep(0f, 1f, GetPingPongT());
        transform.position = Vector3.Lerp(pointA.position, pointB.position, t);
    }

    
    private void MoveWithCurve()
    {
        float t = curve.Evaluate(GetPingPongT());
        transform.position = Vector3.Lerp(pointA.position, pointB.position, t);
    }

    
    private float GetPingPongT()
    {
        float distance = Vector3.Distance(pointA.position, pointB.position);
        if (distance < 0.0001f) return 0f;

        float duration = distance / speed;   
        return Mathf.PingPong(Time.time / duration, 1f);
    }
}