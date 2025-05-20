using UnityEngine;
using UnityEngine.AI;

public class EnemyMovment : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position);
    }
}
