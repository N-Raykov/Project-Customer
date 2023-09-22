using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnRobot : MonoBehaviourWithPause
{
    Rigidbody rb;
    [SerializeField] PlayerInput input;
    [SerializeField] GameObject robot;

    [SerializeField] float dropRobotRangeMax;
    [SerializeField] float dropRobotRangeMin;
    [SerializeField] LayerMask mask;
    [SerializeField] LayerMask ground;

    public float spawnCooldown; 
    public float cooldownTimer { get; set; }     
    public bool isCooldownActive { get; set; }

    private void Start(){
        rb = input.GetComponent<Rigidbody>();
    }

    protected override void UpdateWithPause()
    {
        if (isCooldownActive)
        {
            cooldownTimer -= Time.deltaTime;

            // Check if the cooldown timer has reached zero or less
            if (cooldownTimer <= 0f)
            {
                isCooldownActive = false;
            }
        }

        if (input.spawnBot && GameManager.robot == null && isCooldownActive == false)
        {
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
        Physics.SphereCast(spawnPoint + new Vector3(0, 110, 0), robotAgent.radius, Vector3.down, out hit, 200, mask, QueryTriggerInteraction.UseGlobal);

        if (hit.collider == null)
        {
            Robot robotScript = Instantiate(robot, spawnPoint, Quaternion.identity).GetComponentInChildren<Robot>();
            RaycastHit groundCheck;
            Physics.Raycast(spawnPoint, Vector3.down, out groundCheck, 10000, ground);
            robotScript.distanceToGround = groundCheck.distance;

            cooldownTimer = spawnCooldown;
            isCooldownActive = true;
            FindObjectOfType<AbilityUI>().SpawnRobotUI();
        }
        else
        {
            Spawn();
        }
    }
}