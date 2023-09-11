using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviourWithPause
{

    [SerializeField] int hp;
    Rigidbody rb;
    public bool hasFallen { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hasFallen = false;
    }

    public void TakeDamage(Vector3 pNormal)
    {
        hp--;
        if (hp == 0)
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.AddForce(pNormal * 30, ForceMode.Force);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            hasFallen = true;
            GameManager.fallenTrees++;
        }
    }

}
