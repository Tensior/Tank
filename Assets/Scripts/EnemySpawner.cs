using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Enemy[] enemyPrefabs_;
    private Queue<Enemy>[] enemyPools_;

    [SerializeField]
    private int maxNumberOfEnemies_ = 10;
    private int currentNumberOfEnemies_ = 0;

    [SerializeField]
    private Rect spawnRect_; // x -> x, y -> z for world positioning
    [SerializeField]
    Vector2Int nSpawns_;
    private int numberOfSpawnPoints_ = 10;
    private List<Transform> spawnTransforms_;

    // Start is called before the first frame update
    private void Awake()
    {
        CalculateSpawnTransforms();
        CreateEnemyPools();
    }

    private void CreateEnemyPools()
    {
        enemyPools_ = new Queue<Enemy>[enemyPrefabs_.Length];
        for ( int i = 0; i < enemyPools_.Length; i++ )
        {
            enemyPools_[i] = new Queue<Enemy>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Z ) )
        {
            CalculateSpawnTransforms();
        }

        if( currentNumberOfEnemies_ < maxNumberOfEnemies_)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {

    }

    private void CalculateSpawnTransforms()
    {
        numberOfSpawnPoints_ = 2 * ( nSpawns_.x + nSpawns_.y );

        if ( spawnTransforms_ != null )
        {
            if ( spawnTransforms_.Count == numberOfSpawnPoints_ )
            {
                return;
            }

            //destroy old spawn points
            foreach ( var spawnTransform in spawnTransforms_ )
            {
                if ( spawnTransform )
                {
                    Destroy( spawnTransform.gameObject );
                }
            }
            spawnTransforms_.Clear();
        }

        //create new
        spawnTransforms_ = new List<Transform>( numberOfSpawnPoints_ );

        var spawnSteps = new Vector2Int( (int) spawnRect_.size.x / (nSpawns_.x + 1), (int) spawnRect_.size.y / (nSpawns_.y + 1) );

        for (int i = 0; i < nSpawns_.x; i++ )
        {
            //points on bottom side
            var currentX = spawnRect_.xMin + spawnSteps.x * (i + 1);
            var currentY = spawnRect_.yMin;
            var currentRotation = Quaternion.LookRotation( Vector3.forward, Vector3.up );
            AddSpawnPoint( currentX, currentY, currentRotation );

            //points on top side
            currentY = spawnRect_.yMax;
            currentRotation = Quaternion.LookRotation( Vector3.forward, Vector3.up );
            AddSpawnPoint( currentX, currentY, currentRotation );
        }

        for ( int i = 0; i < nSpawns_.y; i++ )
        {
            //points on left side
            var currentX = spawnRect_.xMin ;
            var currentY = spawnRect_.yMin + spawnSteps.y * (i + 1);
            var currentRotation = Quaternion.LookRotation( Vector3.right, Vector3.up );
            AddSpawnPoint( currentX, currentY, currentRotation );

            //points on right side
            currentX = spawnRect_.xMax;
            currentRotation = Quaternion.LookRotation( Vector3.left, Vector3.up );
            AddSpawnPoint( currentX, currentY, currentRotation );
        }
    }

    private void AddSpawnPoint( float currentX, float currentY, Quaternion currentRotation )
    {
        var currentSpawn = new GameObject().transform;
        currentSpawn.position = new Vector3( currentX, 0f, currentY );
        currentSpawn.rotation = currentRotation;

        currentSpawn.SetParent( gameObject.transform );
        spawnTransforms_.Add( currentSpawn );
    }

    private void OnDrawGizmosSelected()
    {
        foreach ( var spawnTransform in spawnTransforms_ )
        {
            Gizmos.DrawLine( spawnTransform.position, spawnTransform.position + spawnTransform.forward );
        }
    }
}
