using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericObjectPool<T> : MonoBehaviour where T : Component
{
    [SerializeField]
    private T prefab_;

    public static GenericObjectPool<T> Instance { get; private set; }
    private Queue<T> objects = new Queue<T>();

    private void Awake()
    {
        Instance = this;
    }

    public T GetFromPool()
    {
        if (objects.Count == 0)
        {
            AddObject();
        }
        return objects.Dequeue();
    }

    private void AddObject()
    {
        var newObject = Instantiate( prefab_ );
        newObject.gameObject.SetActive( false );
        objects.Enqueue( newObject );
    }

    public void ReturnToPool( T objectToReturn )
    {
        objectToReturn.gameObject.SetActive( false );
        objects.Enqueue( objectToReturn );
    }
}
