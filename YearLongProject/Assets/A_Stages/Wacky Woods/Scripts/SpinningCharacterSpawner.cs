using System.Collections.Generic;
using Hitbox.System;
using NaughtyAttributes;
using UnityEngine;

namespace A_Stages.Wacky_Woods.Scripts
{
    public class SpinningCharacterSpawner : MonoBehaviour
    {
        private enum SpawnState
        {
            Invalid,
            Waiting,
            Hitstop,
            CharacterSpinning
        }

        [Header("Depends")]

        [SerializeField]
        private WackyTimelineCharacter wackyTimelineCharacterPrefab;

        [Header("Config")]

        [SerializeField]
        private List<Transform> wayPoints;

        [SerializeField]
        private float moveSpeed;

        [SerializeField]
        private float cooldownTime;

        [SerializeField]
        private float initialStartDelay;

        private SpawnState spawnState = SpawnState.Waiting;

        [ShowNonSerializedField]
        private float waitTimer;

        [ShowNonSerializedField]
        private int currentWayPointIndex;

        private WackyTimelineCharacter currentCharacter;

        private float hitStopTimer;

        private void Start()
        {
            if (wayPoints == null || wayPoints.Count < 2)
            {
                Debug.LogWarning("SpinningCharacterSpawner requires at least two waypoints to function.");
                spawnState = SpawnState.Invalid;
                return;
            }

            waitTimer = initialStartDelay;
            currentCharacter = Instantiate(wackyTimelineCharacterPrefab, gameObject.transform);
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
                        spawnState = SpawnState.CharacterSpinning;
                        currentCharacter.SetVisible(true);
                        currentCharacter.SelectRandomModel();
                        currentCharacter.InitializeAttack(1);
                        currentWayPointIndex = 0;
                        currentCharacter.transform.position = wayPoints[0].position;
                    }

                    break;
                case SpawnState.Hitstop:
                    hitStopTimer -= Time.fixedDeltaTime;
                    if (hitStopTimer <= 0f)
                    {
                        spawnState = SpawnState.CharacterSpinning;
                    }

                    break;
                case SpawnState.CharacterSpinning:
                    if (currentWayPointIndex >= wayPoints.Count - 1)
                    {
                        spawnState = SpawnState.Waiting;
                        currentCharacter.StopAttacking();
                        currentCharacter.SetVisible(false);
                        waitTimer = cooldownTime;
                        return;
                    }

                    Vector3 fromPosition = wayPoints[currentWayPointIndex].position;
                    Vector3 targetPosition = wayPoints[currentWayPointIndex + 1].position;

                    int dir = (targetPosition - fromPosition).x > 0 ? 1 : -1;

                    currentCharacter.SetFacingDirection(dir);

                    Vector3 currentPos = currentCharacter.transform.position;

                    currentPos = Vector3.MoveTowards(
                        currentPos,
                        targetPosition,
                        moveSpeed * Time.fixedDeltaTime);

                    currentCharacter.transform.position = currentPos;

                    if (currentPos == targetPosition)
                    {
                        currentWayPointIndex++;
                    }

                    currentCharacter.Evaluate(Time.fixedDeltaTime);

                    break;
            }
        }

        private void OnDrawGizmos()
        {
            if (wayPoints == null || wayPoints.Count < 2)
            {
                return;
            }

            Gizmos.color = Color.yellow;
            for (var i = 0; i < wayPoints.Count - 1; i++)
            {
                Gizmos.DrawLine(wayPoints[i].position, wayPoints[i + 1].position);
            }

            foreach (Transform wayPoint in wayPoints)
            {
                Gizmos.DrawSphere(wayPoint.position, 0.25f);
            }
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