using System;
using System.Collections.Generic;
using Hitbox.System;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace A_Stages.Wacky_Woods.Scripts
{
    public class BigCharacterSpawner : MonoBehaviour
    {
        private enum SpawnState
        {
            Invalid,
            Waiting,
            Entering,
            Attacking,
            Hitstop,
            Leaving
        }

        [Serializable]
        private struct CharPath
        {
            public List<Transform> EnteringWaypoints;
            public List<Transform> LeavingWaypoints;
        }

        [Header("Depends")]

        [SerializeField]
        private WackyTimelineCharacter attackingCharacterPrefab;

        [Header("Path Config")]

        [SerializeField]
        private List<CharPath> paths;

        [SerializeField]
        private float moveSpeed;

        [Header("Timing Config")]

        [SerializeField]
        private float cooldownTime;

        [SerializeField]
        private float initialStartDelay;

        [ShowNonSerializedField]
        private SpawnState spawnState = SpawnState.Waiting;

        [ShowNonSerializedField]
        private float waitTimer;

        [ShowNonSerializedField]
        private int currentWayPointIndex;

        private WackyTimelineCharacter currentCharacter;

        private float hitStopTimer;
        private CharPath currentPath;

        private void Start()
        {
            if (paths == null || paths.Count < 1)
            {
                Debug.LogWarning("Requires at least one path");
                spawnState = SpawnState.Invalid;
                return;
            }

            waitTimer = initialStartDelay;
            currentCharacter = Instantiate(attackingCharacterPrefab, gameObject.transform);
            currentCharacter.SetVisible(false);
            currentCharacter.OnLandHit += HandleCharacterLandHit;
        }

        private void FixedUpdate()
        {
            if (spawnState == SpawnState.Invalid)
            {
                return;
            }

            switch (spawnState)
            {
                case SpawnState.Waiting:
                    waitTimer -= Time.fixedDeltaTime;
                    if (waitTimer <= 0f)
                    {
                        spawnState = SpawnState.Entering;
                        currentCharacter.SelectRandomModel();
                        currentCharacter.SetVisible(true);
                        currentWayPointIndex = 0;
                        currentPath = paths[Random.Range(0, paths.Count)];
                        currentCharacter.transform.position = currentPath.EnteringWaypoints[0].position;
                    }

                    break;
                case SpawnState.Entering:
                    UpdatedEntering();
                    break;
                case SpawnState.Attacking:
                    currentCharacter.Evaluate(Time.fixedDeltaTime);
                    break;
                case SpawnState.Hitstop:
                    hitStopTimer -= Time.fixedDeltaTime;
                    if (hitStopTimer <= 0f)
                    {
                        spawnState = SpawnState.Attacking;
                    }

                    break;
                case SpawnState.Leaving:
                    UpdateLeaving();
                    break;
            }
        }

        private void OnDrawGizmos()
        {
            if (paths == null || paths.Count < 1)
            {
                return;
            }

            foreach (CharPath path in paths)
            {
                List<Transform> wayPoints = path.EnteringWaypoints;
                if (wayPoints == null || wayPoints.Count < 2)
                {
                    return;
                }

                Gizmos.color = Color.green;
                for (var i = 0; i < wayPoints.Count - 1; i++)
                {
                    Gizmos.DrawLine(wayPoints[i].position, wayPoints[i + 1].position);
                }

                foreach (Transform wayPoint in wayPoints)
                {
                    Gizmos.DrawSphere(wayPoint.position, 0.25f);
                }

                wayPoints = path.LeavingWaypoints;
                if (wayPoints == null || wayPoints.Count < 2)
                {
                    return;
                }

                Gizmos.color = Color.red;
                for (var i = 0; i < wayPoints.Count - 1; i++)
                {
                    Gizmos.DrawLine(wayPoints[i].position, wayPoints[i + 1].position);
                }

                foreach (Transform wayPoint in wayPoints)
                {
                    Gizmos.DrawSphere(wayPoint.position, 0.25f);
                }
            }
        }

        private void UpdatedEntering()
        {
            List<Transform> wayPoints = currentPath.EnteringWaypoints;
            bool isMoving = MoveAlongPath(Time.fixedDeltaTime, wayPoints, ref currentWayPointIndex);
            if (!isMoving)
            {
                spawnState = SpawnState.Attacking;
                currentCharacter.OnFinishAttack += HandleFinishAttack;
                currentCharacter.InitializeAttack(currentCharacter.FacingDirection);
            }
        }

        private void HandleFinishAttack()
        {
            currentCharacter.OnFinishAttack -= HandleFinishAttack;
            spawnState = SpawnState.Leaving;
            currentWayPointIndex = 0;
            currentCharacter.StopAttacking();
        }

        private void UpdateLeaving()
        {
            List<Transform> wayPoints = currentPath.LeavingWaypoints;
            bool isMoving = MoveAlongPath(Time.fixedDeltaTime, wayPoints, ref currentWayPointIndex);
            if (!isMoving)
            {
                spawnState = SpawnState.Waiting;
                waitTimer = cooldownTime;
                currentCharacter.SetVisible(false);
                currentCharacter.InitializeAttack(currentCharacter.FacingDirection);
            }
        }

        private bool MoveAlongPath(float deltaTime, List<Transform> wayPoints, ref int waypointIndex)
        {
            if (currentWayPointIndex >= wayPoints.Count - 1)
            {
                return false;
            }

            Vector3 fromPosition = wayPoints[waypointIndex].position;
            Vector3 targetPosition = wayPoints[waypointIndex + 1].position;

            int dir = (targetPosition - fromPosition).x > 0 ? 1 : -1;

            currentCharacter.SetFacingDirection(dir);

            Vector3 currentPos = currentCharacter.transform.position;

            currentPos = Vector3.MoveTowards(
                currentPos,
                targetPosition,
                moveSpeed * deltaTime);

            currentCharacter.transform.position = currentPos;

            if (currentPos == targetPosition)
            {
                waypointIndex++;
            }

            return true;
        }

        private void HandleCharacterLandHit(HitboxInstantiateResult hitLandResult)
        {
            if (hitLandResult.HitboxInstance.HitboxEffect.GiveAttackerHitStop)
            {
                hitStopTimer = hitLandResult.HitboxInstance.HitboxEffect.HitStopDuration;
                spawnState = SpawnState.Hitstop;
            }
        }
    }
}