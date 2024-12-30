using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ChainComponent : MonoBehaviour
{
    public GameObject referenceGameObject = null;
    public float margin = 1.5f;
    public int countOfChain = 1;

    [SerializeField]
    List<GameObject> boneList_Activate = new List<GameObject>();
    [SerializeField]
    List<GameObject> boneList_Rested = new List<GameObject>();
    [SerializeField]
    int countOfChildren = 0;


    void Reset()
    {
        if (GetComponent<Rigidbody>() == null) gameObject.AddComponent<Rigidbody>();

        countOfChildren = transform.childCount;
        for (int i = 0; i < countOfChildren; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        StopAllCoroutines();
        StartCoroutine(Co_Update());
    }

    GameObject InstantiateBone()
    {
        GameObject child = Instantiate(referenceGameObject);
        child.transform.parent = transform;

        if (child.GetComponent<Rigidbody>() == null) child.AddComponent<Rigidbody>();
        if (child.GetComponent<HingeJoint>() == null) child.AddComponent<HingeJoint>();

        return child;
    }

    void UpdateChain()
    {
        if (referenceGameObject == null)
        {
            Debug.LogError("Null property (ReferenceGameObject)");
        }
        else if (countOfChain != boneList_Activate.Count)
        {
            string boneName = referenceGameObject.name;

            int dist_FromActivateList = countOfChain - boneList_Activate.Count;
            if (dist_FromActivateList > 0)
            {
                int dist_fromRestList = dist_FromActivateList - boneList_Rested.Count;
                if (dist_fromRestList > 0)
                {
                    for (int i = 0; i < dist_fromRestList; i++)
                    {
                        GameObject restedBone = InstantiateBone();
                        if (restedBone.GetComponent<HingeJoint>() == null) restedBone.AddComponent<HingeJoint>();
                        boneList_Rested.Add(restedBone);
                        restedBone.SetActive(false);
                    }
                    new WaitForSeconds(0.1f);
                }
            }
            else
            {
                int countOfRemaining = dist_FromActivateList * -1;
                for (int i = 0; i < countOfRemaining; i++)
                {
                    GameObject lastBone = boneList_Activate[boneList_Activate.Count - 1];
                    Debug.Log($"boneList_Activate [{lastBone.name}]");

                    boneList_Rested.Add(lastBone);
                    boneList_Activate.RemoveAt(boneList_Activate.Count - 1);
                    lastBone.GetComponent<HingeJoint>().connectedBody = null;
                    lastBone.SetActive(false);
                }
            }

            for (int i = 0; i < dist_FromActivateList; i++)
            {
                GameObject lastRestedBone = boneList_Rested[boneList_Rested.Count - 1];
                lastRestedBone.SetActive(true);
                boneList_Activate.Add(lastRestedBone);
                boneList_Rested.RemoveAt(boneList_Rested.Count - 1);
            }

            for (int i = 0; i < boneList_Activate.Count; i++)
            {
                boneList_Activate[i].name = boneName + "_" + i.ToString();
                boneList_Activate[i].GetComponent<Rigidbody>().isKinematic = i == 0 ? true : false;
                boneList_Activate[i].GetComponent<HingeJoint>().connectedBody = i == 0 ? null : boneList_Activate[i - 1].GetComponent<Rigidbody>();
                boneList_Activate[i].transform.position = new Vector3(
                    0,
                    -i * margin,
                    0
                );
                boneList_Activate[i].transform.localEulerAngles = new Vector3(
                    0,
                    i % 2 == 0 ? 0 : 90f,
                    0
                );

            }
        }

        countOfChildren = transform.childCount;
    }

    IEnumerator Co_Update()
    {
#if UNITY_EDITOR
        while (true)
        {
            UpdateChain();
            yield return new WaitForSeconds(0.4f);
        }
#else
        UpdateChain();
        yield return new WaitForSeconds(0.1f);
#endif
    }
}
