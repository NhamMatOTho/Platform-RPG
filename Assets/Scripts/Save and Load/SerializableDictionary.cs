using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<IKey, IValue> : Dictionary<IKey, IValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<IKey> keys = new List<IKey>();
    [SerializeField] private List<IValue> values = new List<IValue>();


    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach(KeyValuePair<IKey, IValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }
    public void OnAfterDeserialize()
    {
        this.Clear();

        if(keys.Count != values.Count)
        {
            Debug.Log("keys not equal to values");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }

}
