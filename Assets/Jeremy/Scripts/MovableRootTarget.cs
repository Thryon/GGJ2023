using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using KinematicCharacterController;

public class MovableRootTarget : MonoBehaviour
{
    public enum BehaviorRoot { followTarget, avoidTarget, shrink};

    public TreeRoot root;
    public BehaviorRoot behavior;
    public LayerMask detectionLayer;
    public bool targetCamera;
    public float detectionRadius;
    public float followSpeed;

    [Header("Idle")]
    public float wanderRadius = 2f;
    public float wanderSpeed = 2f;
    public float wanderFocusTime = 1f;
    public bool alwaysVisible = false;

    private bool detection;
    private Transform targetFollow;
    private Vector3 startPosition;

    private float currentWanderTime = 0f;
    private Vector3 wanderDestination;

    private IEnumerator Start()
    {
        yield return null;

        startPosition = transform.position;
        wanderDestination = transform.position;
        currentWanderTime = 0f; 

        switch (behavior)
        {
            case BehaviorRoot.followTarget:
                if (alwaysVisible)
                    root.ActivateOnTarget(transform);
                else
                    root.Shrink();
                break;
            case BehaviorRoot.avoidTarget:
                root.ActivateOnTarget(transform);
                break;
            case BehaviorRoot.shrink:
                root.ActivateOnTarget(transform);
                break;
        }
    }

    private void Update()
    {
        DetectEntities();

        // Detection Behavior
        if (detection)
        {
            switch (behavior)
            {
                case BehaviorRoot.followTarget:
                    FollowTarget();
                    break;
                case BehaviorRoot.avoidTarget:
                    AvoidTarget();
                    break;
                case BehaviorRoot.shrink:
                    Shrink();
                    break;
            }
        }
        else
            Idle();
    }

    private void DetectEntities()
    {
        Collider[] closeColliders = Physics.OverlapSphere(startPosition, detectionRadius, detectionLayer);
        if (closeColliders.Length > 0)
        {
            Collider closest = closeColliders
                .Select(collider => (collider: collider, distance: Vector3.Distance(transform.position, collider.transform.position)))
                .Aggregate((min, next) => next.distance < min.distance ? next : min)
                .collider;

            if (targetFollow == null && detection == false)
                detection = true;

            if (targetCamera)
            {
                // Maintain focus
                targetFollow = closest.GetComponentInParent<Player>().GetComponentInChildren<Camera>().transform;
            }
            else
            {
                // Maintain focus
                targetFollow = closest.transform;
            }
            
        }
        else
            targetFollow = null;
    }

    private void FollowTarget()
    {
        if (detection && root.endPoint == null)
            root.ActivateOnTarget(transform, !alwaysVisible);

        if (targetFollow)
        {
            Vector3 destination = targetFollow.position + (transform.position - targetFollow.position).normalized * 3f;
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * followSpeed);
            return;
        }
        else if (Vector3.Distance(transform.position, startPosition) > .1f)
            transform.position = Vector3.Lerp(transform.position, startPosition, Time.deltaTime * followSpeed / 2);
        else
        {
            if (alwaysVisible)
                root.ActivateOnTarget(transform);
            else
                root.Shrink();
            detection = false;
        }
    }

    private void AvoidTarget()
    {
        if (!detection) return;

        if (targetFollow)
        {
            Vector3 destination = root.transform.position + Vector3.up * 3f + Random.insideUnitSphere * 1.5f
                                + (root.transform.position - targetFollow.position).normalized * detectionRadius * 1.5f;
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * followSpeed);
        }
        else if (Vector3.Distance(transform.position, startPosition) > .1f)
            transform.position = Vector3.Lerp(transform.position, startPosition, Time.deltaTime * followSpeed / 2);
        else
        {
            detection = false;
        }
    }

    private void Shrink()
    {
        if (detection)
        {
            if (root.endPoint == transform)
            {
                root.Shrink();
                return;
            }
            else if (targetFollow == null)
            {
                detection = false;
                root.ActivateOnTarget(transform, true);
            }
        }
    }

    private void Idle()
    {
        if (currentWanderTime > wanderFocusTime)
        {
            wanderDestination = startPosition + Random.insideUnitSphere * wanderRadius;
            currentWanderTime = 0 - Random.Range(0, wanderFocusTime * 0.25f);
        }
        else
            currentWanderTime += Time.deltaTime;

        transform.position = Vector3.Lerp(transform.position, wanderDestination, Time.deltaTime * wanderSpeed);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
