using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    public int fairCount = 0;

    [SerializeField]
    public List<TKey> keys = new List<TKey>();

    [SerializeField]
    public List<TValue> values = new List<TValue>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }

        //for(int i=0; i<fairCount; i++)
        //{
        //    keyList.Add(this.Keys[0]);
        //    valueList.Add(pair.Value);
        //}
    }

    public void OnAfterDeserialize()
    {
        int dist = fairCount - keys.Count;
        for(int i=0; i<dist; i++)
        {
            //keyList.Add()
        }
        Debug.Log("After Serialize 01");

        this.Clear();
        if (keys.Count != values.Count)
        {
            throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));
        }
        else
        {
            for (int i = 0; i < keys.Count; i++)
            {
                this.Add(keys[i], values[i]);
            }
        }
        Debug.Log("After Serialize 02");
    }
}
