using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnTrigger : MonoBehaviour
{

    public UnityEvent OnTriggerEvent;
    [SerializeReference] bool doOnce;
    bool alreadyTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 6) { return; }
        if (doOnce && alreadyTriggered == false)
        {
            alreadyTriggered = true;
            OnTriggerEvent?.Invoke();
        } else if (doOnce == false)
        {
            OnTriggerEvent?.Invoke();
        }
    }
}
