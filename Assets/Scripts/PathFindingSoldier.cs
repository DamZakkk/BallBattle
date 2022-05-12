using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFindingSoldier : MonoBehaviour
{
    public PlayerMazeManager manager;
    NavMeshAgent agent;
    [SerializeField] private Transform ballTarget;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(FieldMazeManager.instance.ball.position);
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("Run", true);
    }

    // Update is called once per frame
    void Update()
    {
        var lookPos = agent.steeringTarget - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 20);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") && !FieldMazeManager.instance.isComplete)
        {
            FieldMazeManager.instance.Complete(manager);
        }
    }
}
