using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackOnPlayer : PlayerDetection
{
    public EnemyState state = new EnemyState();
    public Directions direction = new Directions();

    protected float moveSpeed = 3.5f;
    protected float initialMoveSpeed = 3.5f;
    private float moveSpeedMultiplier = 1.75f;
    private float minDistanceToAttack = 1f;

    [SerializeField] private List<Transform> patrolPoints = new List<Transform>();
    private int patrolPointIndex = 0;
    private Vector2 lastPatrolPoint = new Vector2();
    private Vector3 targetPoint = new Vector3();
    Vector3 lookDirection = new Vector3();

    ContactFilter2D contactFilter = new ContactFilter2D();
    [SerializeField] protected PolygonCollider2D swordCollider;
    [SerializeField] protected GameObject sword;
    private int enemyDamage = 1;
    [NonSerialized] public float attackDuration = 0.5f;
    private bool canAttack = true;

    [SerializeField] private GameObject hideBackVision;


    protected void TryTrackPlayer()
    {
        if (TryToDetectPlayer())
        {
            // new detection
            lastPatrolPoint = transform.position;
            targetPoint = transformDetected.position;
            state = EnemyState.Tracking;
        }
    }

    protected void Patrol()
    {
        //targetPoint = patrolPoints[patrolPointIndex].position;
        //targetPoint = patrolPoints[lastPatrolPoint].position;
        // or patrolPointIndex.position

        targetPoint = patrolPoints[patrolPointIndex].position;

        transform.position = Vector2.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);

        // check why, the if is never reached with Vector3.Distance
        if (Vector2.Distance(transform.position, targetPoint) < 0.1f)
        {
            patrolPointIndex++;
            if (patrolPointIndex == patrolPoints.Count) {
                patrolPointIndex = 0;
            }
        }
    }    

    protected void TrackPlayer()
    {
        moveSpeed = initialMoveSpeed * moveSpeedMultiplier;

        transform.position = Vector2.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint) < minDistanceToAttack)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1f, playerLayerMask);
            foreach (Collider2D collider in hitColliders)
            {
                if (collider != null && collider.TryGetComponent<Player>(out Player player))
                {
                    state = EnemyState.Attack;
                    return;
                }                
            }
            state = EnemyState.GoesBack;
        }
    }

    protected void AttackPlayer()
    {
        StartCoroutine(AttackTimer());
    }

    IEnumerator AttackTimer()
    {
        swordCollider.enabled = true;
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = playerLayerMask;
        //contactFilter.layerMask = playerDetectByRayacstLayerMask;

        //Vector3 lookDirection = transform.position - transformDetected.position;

        //lookDirection = transform.position - transformDetected.position;
        //lookDirection = transform.position - targetPoint;
        lookDirection = targetPoint - transform.position;

        direction = GetDirectionInRangeOfFour(lookDirection, ref hideBackVision);
        //GetSwordDirection();
        GetGameObjectDirection(sword);

        float time = 0;
        //while (time < 0.5f)
        while (time < attackDuration)
        {
            time += Time.deltaTime;

            Collider2D[] enemyColliders = new Collider2D[1] { null };
            int collidersAmount = Physics2D.OverlapCollider(swordCollider, contactFilter, enemyColliders);

            if (enemyColliders[0] != null && enemyColliders[0].TryGetComponent<IDamageable>(out IDamageable iDamageable) &&
                canAttack)
            { 
                iDamageable.TakeDamage(enemyDamage);
                EventManager.PlayerInvulnerability();
                //contactFilter = ContactFilter2D.noFilter;
                canAttack = false;
                swordCollider.enabled = false;
            }
            yield return null;
        }
        swordCollider.enabled = false;
        state = EnemyState.GoesBack;

        canAttack = true;
    }


    protected void GoesBackToInitialPosition()
    {
        moveSpeed = initialMoveSpeed;

        targetPoint = lastPatrolPoint;

        //transform.position = Vector2.MoveTowards(transform.position, patrolPoints[lastPatrolPoint].position,
        transform.position = Vector2.MoveTowards(transform.position, lastPatrolPoint,
                                                 moveSpeed * Time.deltaTime);

        //if (Vector3.Distance(transform.position, patrolPoints[lastPatrolPoint].position) < 0.5f) {
        if (Vector3.Distance(transform.position, lastPatrolPoint) < 0.5f) {
            state = EnemyState.Patrol;
        }
    }

    protected void CallCheckDirectionTimer()
    {
        StartCoroutine(CheckDirectionTimer());
    }

    IEnumerator CheckDirectionTimer()
    {
        direction = GetDirectionInRangeOfFour(lookDirection, ref hideBackVision);
        lookDirection = targetPoint - transform.position;

        //yield return new WaitForSeconds(0.2f);
        yield return new WaitForSeconds(0.1f);
        CallCheckDirectionTimer();
    }

    private Directions GetDirectionInRangeOfFour(Vector3 direction, ref GameObject hideBackVision)
    {
        direction = (Quaternion.AngleAxis(45, Vector3.forward) * direction).normalized;

        if (direction.x < 0 && direction.y > 0) {
            ChangeHideBackVision(0, -0.6f, 0);
            return Directions.North;
        }
        else if (direction.x > 0 && direction.y < 0) {
            ChangeHideBackVision(0, 0.25f, 0);
            return Directions.South;
        }
        else if (direction.x > 0 && direction.y > 0) {
            ChangeHideBackVision(-0.5f, 0, 90);
            return Directions.East;
        }
        else if (direction.x < 0 && direction.y < 0) {
            ChangeHideBackVision(0.5f, 0, 90);
            return Directions.West;
        }
        else {
            ChangeHideBackVision(0, -0.6f, 0);
            return Directions.North;
        }
    }

    private void ChangeHideBackVision(float xPos, float yPos, float zRotation)
    {
        hideBackVision.transform.localPosition = new Vector2(xPos, yPos);
        hideBackVision.transform.eulerAngles = new Vector3(0, 0, zRotation);
    }

    private void GetGameObjectDirection(GameObject go)
    {
        switch (direction)
        {
            case Directions.North:
                go.transform.eulerAngles = new Vector3(0, 0, 180);
                break;
            case Directions.South:
                go.transform.eulerAngles = new Vector3(0, 0, 0);
                break;
            case Directions.East:
                go.transform.eulerAngles = new Vector3(0, 0, 90);
                break;
            case Directions.West:
                go.transform.eulerAngles = new Vector3(0, 0, 270);
                break;
            default:
                go.transform.eulerAngles = new Vector3(0, 0, 180);
                break;
        }
    }
}
