using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPod : MonoBehaviourWithPause{
    Rigidbody rb;

    [SerializeField] float startingVelocity;
    
    public string item { get; set; }
    public int amount { get; set; }
    public float distanceToGround { get; set; }

    float startPosition;
    float currentPosition;
    float timeToWaitUntilStart;


    void Start(){
        startPosition = transform.position.y;
        rb = GetComponent<Rigidbody>();
        float velocity = startingVelocity + UnityEngine.Random.Range(5,10)* ((UnityEngine.Random.Range(0, 2) == 0) ? -1 : 1);
        startingVelocity = 0;
        timeToWaitUntilStart = UnityEngine.Random.Range(1,5);
        StartCoroutine(WaitToStart(timeToWaitUntilStart, velocity));
    }

    protected override void UpdateWithPause(){
        currentPosition = transform.position.y;
        float t = Mathf.Abs(currentPosition-startPosition) / distanceToGround;
        rb.velocity = new Vector3(0,-Mathf.Lerp(startingVelocity,0f,t),0);
    }

    IEnumerator WaitToStart(float pTime,float pVelocity) {
        yield return new WaitForSeconds(pTime);
        startingVelocity = pVelocity;
        rb.AddForce(Vector3.down * startingVelocity, ForceMode.VelocityChange);

    }
}
