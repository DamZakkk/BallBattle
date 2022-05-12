using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderSoldier : Soldier
{
    Rigidbody m_Rigidbody;
    PlayerManager manager;
    public bool activated = false;

    public SkinnedMeshRenderer mesh;

    public Soldier target;

    public Vector3 initPos;
    public Animator animator;

    public MeshRenderer detectAreaIndicator;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

        detectAreaIndicator.transform.localScale = Vector3.one;
        detectAreaIndicator.transform.localScale = new Vector3(manager.detectionRangeDistance() / transform.lossyScale.x, transform.lossyScale.y, manager.detectionRangeDistance() / transform.lossyScale.z);
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
            Collider[] colliderArray = Physics.OverlapSphere(initPos, manager.detectionRangeDistance() / 2);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<AttackerSoldier>(out AttackerSoldier attacker))
                {
                    if (attacker.haveBall)
                    {
                        target = attacker;
                    }
                }
            }

            if (target)
            {
                animator.SetBool("Run", true);
                var lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 100);
                m_Rigidbody.velocity = transform.forward * manager.normalSpeed;
            }
            else if (Vector3.Distance(transform.position, initPos) > 0.2f)
            {
                animator.SetBool("Run", true);
                var lookPos = initPos - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 100);
                m_Rigidbody.velocity = transform.forward * manager.returnSpeed;
            }
        }

    }

    public override void Activate(PlayerManager _manager)
    {
        initPos = transform.position;
        manager = _manager;
        StartCoroutine(ActivateCoroutine());
    }

    IEnumerator ActivateCoroutine()
    {
        yield return new WaitForSeconds(manager.spawnTime);
        mesh.material = manager.activeMat;
        activated = true;
    }

    public void Deactivate()
    {
        StartCoroutine(DeactivateCoroutine());
    }
    IEnumerator DeactivateCoroutine()
    {
        animator.SetBool("Run", false);
        m_Rigidbody.velocity = Vector3.zero;
        activated = false;
        mesh.material = manager.inactiveMat;
        yield return new WaitForSeconds(manager.reactivateTime);
        mesh.material = manager.activeMat;
        activated = true;
    }
}
