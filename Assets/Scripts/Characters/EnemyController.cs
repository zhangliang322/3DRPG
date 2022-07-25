using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { GUARD, PATROL, CHASE, DEAD }
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    private EnemyStates enemyStates;
    private NavMeshAgent agent;
    private Animator anim;

    private CharacterStats characterStats;


    [Header("Basic Settings")]

    public float sightRadius;
    public bool isGuard;

    private float speed;


    private GameObject attackTarget;
    public float lookAtTime;
    private float remainLookAtTime;

    private float lastAttackTime;

    [Header("Patrol State")]
    public float patrolRange;
    
    private Vector3 wayPoint;
    private Vector3 guardPos;
    //原始坐标
    //bool 配合动画
    bool isWalk;
    bool isChase;
    bool isFollow;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //初始速度
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();

        speed = agent.speed;
        guardPos = transform.position;
        remainLookAtTime = lookAtTime;

    }
     void Start()
    {
        if(isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            GetNewWayPoint();//初始化巡逻点
        }
    }

    void Update()
    {
        SwitchStates();
        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;

     }

    void SwitchAnimation()
    {
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Follow", isFollow);
        anim.SetBool("Critical", characterStats.isCritical);
    }
    void SwitchStates()
    {
        if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
            //Debug.Log("找到player");
        }

            switch (enemyStates)
        {
            //发现player开始追击 CHASE
            

            case EnemyStates.GUARD:
                break;
            case EnemyStates.PATROL:
                //巡逻
                isChase = false;
                agent.speed = speed * 0.5f;
                //是否到了随机巡逻点
                if(Vector3.Distance(wayPoint,transform.position)<= agent.stoppingDistance)
                {
                    isWalk = false;
                    if(remainLookAtTime > 0)
                        remainLookAtTime -= Time.deltaTime;
                    else
                    GetNewWayPoint();
                }
                else
                {
                    isWalk = true;
                    agent.destination = wayPoint;
                }
                break;
            case EnemyStates.CHASE:
                //TODO:追击player
                //TODO：超出范围回到上个状态
                
                //TODO:配合攻击动画
                isWalk = false;
                isChase = true;
                agent.speed = speed;

                if(!FoundPlayer())
                {
                    //拉脱
                    isFollow = false;
                    if (remainLookAtTime > 0)
                    {
                        agent.destination = transform.position;
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else if (isGuard)
                        enemyStates = EnemyStates.GUARD;
                    else
                        enemyStates = EnemyStates.PATROL;
                }
                else
                {                   
                    isFollow = true;
                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position;
                }
                //TODO:攻击范围内攻击
                if (TargetInAttackRange() || TargetInSkillRange())
                {

                    isFollow = false;
                    agent.isStopped = true;

                    if(lastAttackTime<0)
                    {
                        lastAttackTime = characterStats.attackData.coolDown;

                        //暴击判断,一行判断，随机赋值，小于就是true ,暴击
                        characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;
                        //攻击
                        Attack();

                    }

                }    

                    


                break;
            case EnemyStates.DEAD:
                break;
        }
    }
    void Attack()
    {
        transform.LookAt(attackTarget.transform);
        if (TargetInAttackRange())
        {
            //近身动画
            anim.SetTrigger("Attack");

        }
        if(TargetInSkillRange())
        {
            //远程动画
            anim.SetTrigger("Skill");
        }
    }


    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);

        foreach(var target in colliders)
        {
            if(target.CompareTag("Player"))
            {
                attackTarget =target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }

    bool TargetInAttackRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.attackRange;
        else
            return false;
    }

    bool TargetInSkillRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
        else
            return false;
    }
    void GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;

        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        Vector3 randomPoint = new Vector3(guardPos.x + randomX,transform.position.y, guardPos.z + randomZ);
        //FIXME:可能出现的问题
        //wayPoint = randomPoint;会卡住
        NavMeshHit hit;
        //返回布尔值，判断目标点是否可走,可走，返回hit,不可走，返回当前坐标（不动，再次获得新的点）
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;


    }
     void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
        
    }

}
