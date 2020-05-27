using System.Collections.Generic;
using UnityEngine;

public interface IGenericPoolableObject<T> where T : Component, IGenericPoolableObject<T>
{
    GenericObjectPool<T> Pool { get; set; }
}
public abstract class GenericObjectPool<T> : MonoBehaviour where T : Component, IGenericPoolableObject<T>
{
    [SerializeField]
    private T prefab_;

    protected Queue<T> objects = new Queue<T>();

    private void Awake()
    {
        prefab_.gameObject.SetActive( false ); //so that Instantiate doesn't call OnEnable(), but it will only be called by SetActive(true) when needed
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
        newObject.Pool = this;
        objects.Enqueue( newObject );
    }

    public void ReturnToPool( T objectToReturn )
    {
        objectToReturn.gameObject.SetActive( false );
        objects.Enqueue( objectToReturn );
    }
}
