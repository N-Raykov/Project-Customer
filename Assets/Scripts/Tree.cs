using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tree : MonoBehaviourWithPause {

    [Header("Gameplay")]
    [SerializeField] int hp;
    [SerializeField] int value;
    [SerializeField] int healValue;
    PlayerHealth playerHp;

    [Header("Physics Stuff")]

    [SerializeField] float pushForce;
    [SerializeField] float duration;
    [SerializeField] float timeDelay;
    [SerializeField] float directionDivisioFactor;
    Rigidbody rb;
    public bool hasStarterFalling { get; private set; }
    public bool hasFallen { get; private set; }
    public int _value { get; private set; }
    public int _hp { get { return hp; } }

    public int _maxHP{ get; private set; }

    void Start(){
        playerHp = GameObject.FindAnyObjectByType<PlayerHealth>();
        _value = value;
        rb = GetComponent<Rigidbody>();
        hasFallen = false;
        _maxHP = hp;
    }

    public void TakeDamage(Vector3 pNormal) {
        hp--;
        if (hp == 0) {
            hasStarterFalling = true;
            playerHp.AddHp(healValue);
            rb.constraints = RigidbodyConstraints.None;
            rb.AddForce(pNormal*pushForce,ForceMode.Force);
            StartCoroutine(Move(pNormal));
        }
    }

    private void OnCollisionStay(Collision collision){
        if (collision.gameObject.CompareTag("Ground") && hasFallen == false) {
            hasFallen = true;
            GameManager.fallenTrees++;
            Debug.Log("tree has fallen");
        }
    }

    IEnumerator Move(Vector3 pNormal) {

        int i = 0;
        while (i < duration) {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.AddForce(pNormal/directionDivisioFactor, ForceMode.VelocityChange);
            rb.constraints = RigidbodyConstraints.None;
            i++;
            yield return new WaitForSeconds(timeDelay);//0.125

        }
    }


}
