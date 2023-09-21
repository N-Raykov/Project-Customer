using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviourWithPause{

    [SerializeField] Transform transformToShake;
    [SerializeField] Vector3 magnitude;
    [SerializeField] float duration;
    Gun target;

    private void Awake(){
        target = GetComponent<Gun>();
        target.OnShoot += StartScreenShake;
    }


    public IEnumerator ShakeScreen() {

        Vector3 originalPosition = transform.localPosition;
        float elapsedTime = 0f;
        while (elapsedTime < duration) {
            Vector3 newPos = new Vector3(Random.Range(-1f, 1f) * magnitude.x, Random.Range(-1f, 1f) * magnitude.y, Random.Range(-1f, 1f) * magnitude.z);
            transform.localPosition = originalPosition + newPos;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }


    private void OnDestroy(){
        
    }

    void StartScreenShake() {
        StartCoroutine(ShakeScreen());
    }
}
