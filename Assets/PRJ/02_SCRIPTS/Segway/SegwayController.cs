using UnityEngine;
using UnityEngine.AI;

// Use physics raycast hit from mouse click to set agent destination
[RequireComponent(typeof(NavMeshAgent))]
public class SegwayController : MonoBehaviour
{
    public float maxSpeed = 1.0f;
    public float rotationSmooth;
    public float brakingDistance;

    NavMeshAgent m_Agent;
    NavMeshPath m_Path;
    int pathIter = 1;
    Vector3 AgentPosition;
    Vector3 destination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
    Vector3 endDestination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
    RaycastHit m_HitInfo = new RaycastHit();

    [HideInInspector]
    public float speed, targetSpeed;
    Vector3 rotVel = Vector3.zero;
    float linearVel = 0;
    float speedSmooth = 0.99f;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if(m_Path != null && m_Path.corners != null && m_Path.corners.Length > 0)
        {
            var prev = AgentPosition;
            for(int i = pathIter; i < m_Path.corners.Length; ++i)
            {
                Gizmos.DrawLine(prev, m_Path.corners[i]);
                prev = m_Path.corners[i];
            }
        }
    }


    void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.isStopped = true;
        m_Path = new NavMeshPath();
    }


    void Update()
    {
        SetAgentPosition();
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
            {
                //m_Agent.destination = m_HitInfo.point;
                SetDestination(m_HitInfo.point);          
            }
        }

        if (m_Path.corners == null || m_Path.corners.Length == 0)
            return;


        if (pathIter >= m_Path.corners.Length)
        {
            destination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
            m_Agent.isStopped = true;
            return;
        }
        else
        {
            destination = m_Path.corners[pathIter];
        }

        if (destination.x < float.PositiveInfinity)
        {
            Vector3 direction = destination - AgentPosition;
            //var newDir = Vector3.RotateTowards(transform.forward, direction, 50 * Time.deltaTime, 0.0f);
            var newDir = Vector3.SmoothDamp(transform.forward, direction, ref rotVel, rotationSmooth);
            var newRot = Quaternion.LookRotation(newDir);
            //transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 0.03f);
            transform.rotation = newRot;
                
            float distance = Vector3.Distance(AgentPosition, destination);

            if (distance > m_Agent.radius + 0.1)
            {
                speed = Mathf.SmoothDamp(speed, targetSpeed, ref linearVel, speedSmooth);
                //Vector3 movement = transform.forward * Time.deltaTime * 2f;

                Vector3 movement = transform.forward * Time.deltaTime * Mathf.Max(speed, 0) * maxSpeed;

                m_Agent.Move(movement);
            }
            else
            {
                ++pathIter;
                if (pathIter >= m_Path.corners.Length)
                {
                    destination = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
                    m_Agent.isStopped = true;
                }
            }


            // Manage targert speed
            float totalDist = Vector3.Distance(AgentPosition, m_Path.corners[m_Path.corners.Length - 1]);
            if (distance < brakingDistance) {
                targetSpeed = Mathf.Min(totalDist / brakingDistance, 1.0f);
                speedSmooth = 0f;
            } else {
                targetSpeed = 1.0f;
                speedSmooth = 0.8f;
            }
        }
    }


    void SetAgentPosition()
    {
        NavMeshHit hit;
        if(NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            AgentPosition = hit.position;
        }
    }


    public void SetDestination(Vector3 _point) {
        m_Path = new NavMeshPath();
        m_Agent.CalculatePath(_point, m_Path);
        pathIter = 1;
        m_Agent.isStopped = false;
    }
}
