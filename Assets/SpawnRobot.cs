using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnRobot : MonoBehaviourWithPause
{
    bool isAvailable = true;

    Rigidbody rb;
    [SerializeField] PlayerInput input;
    [SerializeField] GameObject robot;

    [SerializeField] float dropRobotRangeMax;
    [SerializeField] float dropRobotRangeMin;
    [SerializeField] LayerMask mask;
    [SerializeField] LayerMask ground;

    private void Start(){
        rb = input.GetComponent<Rigidbody>();
    }

    protected override void UpdateWithPause()
    {
        if (input.spawnBot && isAvailable == true)
        {
            isAvailable = false;
            Spawn();
        }
    }

    void Spawn()
    {
        NavMeshAgent robotAgent = robot.GetComponent<NavMeshAgent>();

        Vector3 spawnPoint = new Vector3(0, transform.position.y - robotAgent.height * 0.5f, 0);

        int randomXOrientation = UnityEngine.Random.Range(0, 2);
        int randomZOrientation = UnityEngine.Random.Range(0, 2);

        spawnPoint.x = rb.position.x + UnityEngine.Random.Range(dropRobotRangeMin, dropRobotRangeMax) * ((randomXOrientation == 0) ? -1 : 1);
        spawnPoint.z = rb.position.z + UnityEngine.Random.Range(dropRobotRangeMin, dropRobotRangeMax) * ((randomZOrientation == 0) ? -1 : 1);

        RaycastHit hit;
        Physics.SphereCast(spawnPoint + new Vector3(0, 10, 0), robotAgent.radius, Vector3.down, out hit, 200, mask, QueryTriggerInteraction.UseGlobal);

        if (hit.collider == null)
        {
            Robot robotScript = Instantiate(robot, spawnPoint, Quaternion.identity).GetComponentInChildren<Robot>();
            RaycastHit groundCheck;
            Physics.Raycast(spawnPoint, Vector3.down, out groundCheck, 10000, ground);
            robotScript.distanceToGround = groundCheck.distance;
        }
        else
        {
            Spawn();
        }
    }
}