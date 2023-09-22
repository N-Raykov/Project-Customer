using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterfallDegrade : MonoBehaviour
{

    [SerializeField] MoodHandler moodHandler;

    Vector3 startSize;

    // Start is called before the first frame update
    void Start()
    {
        startSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(startSize, new Vector3(startSize.x, 0, startSize.z), moodHandler.globalDegradation);
    }
}
