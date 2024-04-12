using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;

public class SurviourBehaviour : MonoBehaviour
{
    private Blackboard blackboard;
    private Root behaviorTree;
    float moveSpeed = 0.5f;
    float avoidanceRadius = 1.0f;
    Color myColor = new Color(200f / 255f, 101f / 255f, 0f, 0f);
    ZombieBehaviour[] Zombies;

    void Start()
    {
        Zombies = FindObjectsOfType<ZombieBehaviour>();
        behaviorTree = CreateBehaviourTree();
        blackboard = behaviorTree.Blackboard;
        behaviorTree.Start();
    }

    private Root CreateBehaviourTree()
    {
        return new Root(
            new Service(0.125f, UpdatePlayerDistance,
                new Selector(
                    new BlackboardCondition("playerDistance", Operator.IS_SMALLER, 7.5f, Stops.IMMEDIATE_RESTART,
                        new Sequence(
                            new Action(() => SetColor(Color.blue)),
                            new Action((bool _shouldCancel) =>
                            {
                                if (!_shouldCancel)
                                {
                                    MoveTowards(blackboard.Get<Vector3>("playerLocalPos"));
                                    return Action.Result.PROGRESS;
                                }
                                else
                                {
                                    return Action.Result.FAILED;
                                }
                            })
                        )
                    ),
                    new Sequence(
                        new WaitUntilStopped()
                    )
                )
            )
        );
    }

    private void UpdatePlayerDistance()
    {
        float minDistance = Mathf.Infinity;
        ZombieBehaviour nearestZombie = null;

        foreach (var zombie in Zombies)
        {
            float distance = Vector3.Distance(transform.position, zombie.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestZombie = zombie;
            }
        }

        if (nearestZombie != null)
        {
            Vector3 playerLocalPos = this.transform.InverseTransformPoint(nearestZombie.transform.position);
            behaviorTree.Blackboard["playerLocalPos"] = playerLocalPos;
            behaviorTree.Blackboard["playerDistance"] = playerLocalPos.magnitude;
        }
    }

    private void MoveTowards(Vector3 localPosition)
    {
        transform.localPosition -= localPosition * moveSpeed * Time.deltaTime;
    }

    private void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().material.SetColor("_Color", color);
    }
}