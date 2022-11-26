using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AI_Movement : MonoBehaviour
{
    [SerializeField] Transform target;
    UnityEngine.AI.NavMeshAgent agent;
    public LightDetection LightDetection;
    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
    void Update()
    {
        if(LightDetection.s_fLightValue >= .3)
        {
            SetAgentPosition();
        }
        
    }
    void SetAgentPosition()
    {
        agent.SetDestination(target.position);
    }
}
