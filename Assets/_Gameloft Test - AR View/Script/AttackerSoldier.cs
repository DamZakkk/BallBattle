using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerSoldier : Soldier
{
    Rigidbody m_Rigidbody;
    PlayerManager manager;
    public bool activated = false;
    public Transform ballPos;

    public SkinnedMeshRenderer mesh;
    public bool haveBall = false;
    public Animator animator;
    Outline outline;


    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.isPlaying || FieldManager.instance.isComplete)
        {
            animator.SetBool("Run", false);
            m_Rigidbody.velocity = Vector3.zero;
            return;
        }

        if (activated)
        {
            animator.SetBool("Run", true);
            m_Rigidbody.velocity = transform.forward * (manager.ballCarrier == this ? manager.carryingSpeed : manager.normalSpeed);

            if (!manager.ballCarrier)
            {
                var lookPos = manager.ball.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 100);
            }
            else if (manager.ballCarrier == this)
            {
                var lookPos = manager.gate.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 100);
            }
            else
            {

                transform.eulerAngles = Vector3.zero;
            }

        }
        else
        {
            animator.SetBool("Run", false);
            m_Rigidbody.velocity = Vector3.zero;
        }
    }

    public override void Activate(PlayerManager _manager)
    {
        manager = _manager;
        StartCoroutine(ActivateCoroutine());
    }

    IEnumerator ActivateCoroutine()
    {
        yield return new WaitForSeconds(manager.spawnTime);
        mesh.material = manager.activeMat;
        activated = true;
    }

    IEnumerator DeactivateCoroutine()
    {
        outline.enabled = false;
        haveBall = false;
        activated = false;
        mesh.material = manager.inactiveMat;
        manager.DeactiveSoldier(this);
        yield return new WaitForSeconds(manager.reactivateTime);
        mesh.material = manager.activeMat;
        manager.soldierList.Add(this);
        activated = true;
        animator.SetTrigger("RunAgain");
    }

    IEnumerator DestroyCoroutine()
    {
        activated = false;
        m_Rigidbody.velocity = Vector3.zero;
        manager.DeactiveSoldier(this);
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(1.6f);
        Destroy(gameObject);
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            if (manager.ballCarrier == this)
            {
                animator.SetTrigger("Goal");
                manager.isPlaying = false;
                FieldManager.instance.Complete(manager);
            }
        }
        if (other.CompareTag("Fence") && manager.isPlaying)
        {
            animator.SetTrigger("Death");
            StartCoroutine(DestroyCoroutine());
        }
        if (other.CompareTag("Player") &&
            other.GetComponent<DefenderSoldier>() &&
            other.GetComponent<DefenderSoldier>().activated &&
            other.GetComponent<Soldier>() != lastCollide &&
            manager.ballCarrier == this)
        {
            animator.SetTrigger("Death");
            lastCollide = other.GetComponent<Soldier>();
            StartCoroutine(DeactivateCoroutine());
            other.GetComponent<DefenderSoldier>().Deactivate();
        }

        if (other.CompareTag("Ball") && activated)
        {
            outline.enabled = true;
            haveBall = true;
            manager.ballCarrier = this;
            other.transform.parent = ballPos;
            other.transform.localPosition = Vector3.zero;
            other.GetComponent<Ball>().passing = false;
        }
    }
}
