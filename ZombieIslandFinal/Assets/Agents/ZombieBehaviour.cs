using NPBehave;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ZombieBehaviour : MonoBehaviour
{
    private Blackboard blackboard;
    private Root behaviorTree;

    public float moveSpeed = 1.0f;
    public float chaseRange = 8.0f; // Chase range for thieves
    public float attackRange = 5.0f; // Attack range for instant attack
    public Color transparent = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0f);
    public Color white = new Color(255f / 255f, 255f / 255f, 255f / 255f);
    internal bool attacking;

    float raycastDistance = 10f;
    Islandgen Island1;
    Vector3 randomPosition;

    public GameObject ZombiePrefab;
    public int numberOfzombies = 3;

    private void Start()
    {
        Island1 = GameObject.Find("GameController").GetComponent<Islandgen>();
        randomPosition = new Vector3(UnityEngine.Random.Range(0, Island1.width), UnityEngine.Random.Range(0, Island1.height), 0);
        behaviorTree = CreateBehaviourTree();
        blackboard = behaviorTree.Blackboard;
        behaviorTree.Start();

        for (int i = 0; i < numberOfzombies; i++)
        {
            GameObject Zombie = Instantiate(ZombiePrefab, transform.position, Quaternion.identity);
            ZombieBehaviour trollBehaviour = Zombie.GetComponent<ZombieBehaviour>();



            trollBehaviour.enabled = true;
        }

    }


    private Root CreateBehaviourTree()
    {
        return new Root(
            new Service(0.1f, UpdateBlackboard,
                new Selector(
                    new BlackboardCondition("thievesInRange", Operator.IS_EQUAL, true, Stops.IMMEDIATE_RESTART,
                        new Sequence(
                            new Action(() => ChaseSurvivor()),
                            new Action(() => Attack())
                        )
                    ),
                    new Action(() => MoveRandomly())
                )
            )
        );
    }

    private void UpdateBlackboard()
    {
        GameObject[] survivors = GameObject.FindGameObjectsWithTag("survivor");
        bool survivorsInRange = false;

        foreach (GameObject survivor in survivors)
        {
            float distance = Vector3.Distance(transform.position, survivor.transform.position);
            if (distance <= chaseRange)
            {
                 survivorsInRange = true;
                break;
            }
        }

        blackboard["thievesInRange"] = survivorsInRange;
    }

    private void ChaseSurvivor()
    {
        GameObject[] survivors = GameObject.FindGameObjectsWithTag("survivor");
        GameObject nearestsurvivors = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject survivor in survivors)
        {
            float distance = Vector3.Distance(transform.position, survivor.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestsurvivors = survivor;
            }
        }

        if (nearestsurvivors != null)
        {
            Vector3 direction = (nearestsurvivors.transform.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);//*3)
        }
    }

    private void MoveRandomly()
    {
        if (Vector2.Distance(transform.position, randomPosition) <= 0.1f)
        {
            randomPosition = new Vector3(UnityEngine.Random.Range(0, Island1.width), UnityEngine.Random.Range(0, Island1.height), 0);
        }

        Vector3 moveDirection = (randomPosition - transform.position).normalized;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, moveDirection, out hit, raycastDistance))
        {
            randomPosition = new Vector3(UnityEngine.Random.Range(0, Island1.width), UnityEngine.Random.Range(0, Island1.height), 0);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, randomPosition, moveSpeed * Time.deltaTime);
    }

    void Attack()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("survivor");

        foreach (GameObject enemy in enemies)
        {

            float distance = Vector3.Distance(transform.position, enemy.transform.position);


            if (distance <= attackRange)
            {
                //Destroy(enemy.GetComponent<EnemyBehaviour>());
                enemy.transform.position = new Vector3(-1.0f, -1.0f, -1.0f);
                //Destroy(enemy);

                print("attack");
            }
        }

    }


}