using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitMarkerBehavior : MonoBehaviourWithPause{
    
    public static bool hitEnemy { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void UpdateWithPause() {
        if (hitEnemy) {
            StartCoroutine(EnableHitMarker());
        }
    }
    IEnumerator EnableHitMarker() {
        hitEnemy = false;
        GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(0.05f);
        GetComponent<Image>().enabled = false;
    }
}
