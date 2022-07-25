using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    private CharacterStats characterStats;
    //target function
    private GameObject attackTarget;
    private float lastAttackTime;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();//��ñ�����ֵ
        characterStats = GetComponent<CharacterStats>();

    }
    void Start()
    {
        //�ѷ�����ӽ�on Mouse clicked�����а��������з�������ִ��
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
        characterStats.MaxHealth = 2;
    }



    void Update()
    {
        SwtichAnimation();

        lastAttackTime -= Time.deltaTime;
    }

    private void SwtichAnimation()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    public void MoveToTarget(Vector3 target)
    {
        StopAllCoroutines();
        agent.isStopped = false;
        agent.destination = target;
    }

    private void EventAttack(GameObject target)
    {
        if(target != null)
        {
            attackTarget = target;
            StartCoroutine(MoveToAttackTarget());
        }
    }
    //����ѭ���жϣ������ƶ���ʹ��Э��
    IEnumerator MoveToAttackTarget()
    {
        agent.isStopped = false;
        transform.LookAt(attackTarget.transform);

        //FIXME:�޸Ĺ�����Χ
        //FIXME��ע�⹥����Χ���ܵ���ģ�Ͱ뾶
        while (Vector3.Distance(attackTarget.transform.position,transform.position)>characterStats.attackData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }

        agent.isStopped = true;
        //Attack

        if(lastAttackTime <0)
        {
            anim.SetTrigger("Attack");
            //������ȴʱ��
            lastAttackTime = 0.5f;
        }

    }


}
