using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaitUntilBigCutTrees : MonoBehaviour
{

    GameObject[] bigTreesTemp;
    List<Tree> bigTrees;

    int treesCut;
    bool activated = false;

    [SerializeField] int RequiredBigTrees = 1;
    public UnityEvent OnRequirementMet;

    void Start()
    {
        bigTrees = new List<Tree>();

        bigTreesTemp = GameObject.FindGameObjectsWithTag("BigTree");
        foreach (GameObject g in bigTreesTemp)
        {
            bigTrees.Add(g.GetComponent<Tree>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < bigTrees.Count; i++)
        {
            if (bigTrees[i].hasFallen)
            {
                //SpawnWave();
                bigTrees.RemoveAt(i);
                treesCut++;
                i--;
            }
        }

        if (treesCut >= RequiredBigTrees && !activated)
        {
            activated = true;
            OnRequirementMet?.Invoke();
        }
    }
}
