using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( EnemyPool ) )]
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private int maxNumberOfEnemies_ = 10;
    private int currentNumberOfEnemies_ = 0;

    [SerializeField]
    private Rect spawnRect_; // x -> x, y -> z for world positioning
    [SerializeField]
    Vector2Int nSpawns_;
    private List<Transform> spawnTransforms_;

    private EnemyPool[] enemyPools_;

    private void Awake()
    {
        CalculateSpawnTransforms();
        GetEnemyPools();
    }

    private void GetEnemyPools()
    {
        enemyPools_ = GetComponents<EnemyPool>();
    }

    private void Update()
    {
        if( currentNumberOfEnemies_ < maxNumberOfEnemies_)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        int enemyTypeIndex = Random.Range( 0, enemyPools_.Length );
        int enemyTransformIndex = Random.Range( 0, spawnTransforms_.Count );

        var newEnemy = enemyPools_[enemyTypeIndex].GetFromPool();
        newEnemy.gameObject.SetActive( true );
        newEnemy.transform.SetPositionAndRotation( spawnTransforms_[enemyTransformIndex].position, spawnTransforms_[enemyTransformIndex].rotation );
        ++currentNumberOfEnemies_;

        newEnemy.OnDeath += DecreaseCurrentNumberOfEnemies;
    }

    private void DecreaseCurrentNumberOfEnemies()
    {
        --currentNumberOfEnemies_;
    }

    private void CalculateSpawnTransforms()
    {
        int numberOfSpawnPoints = 2 * ( nSpawns_.x + nSpawns_.y );

        spawnTransforms_ = new List<Transform>( numberOfSpawnPoints );

        var spawnSteps = new Vector2( spawnRect_.size.x / (nSpawns_.x + 1), spawnRect_.size.y / (nSpawns_.y + 1) );

        //set positions and rotations for all spawn points
        for (int i = 0; i < nSpawns_.x; i++ )
        {
            //points on bottom side
            var currentX = spawnRect_.xMin + spawnSteps.x * (i + 1);
            var currentY = spawnRect_.yMin;
            var currentRotation = Quaternion.LookRotation( Vector3.forward, Vector3.up );
            AddSpawnPoint( currentX, currentY, currentRotation );

            //points on top side
            currentY = spawnRect_.yMax;
            currentRotation = Quaternion.LookRotation( Vector3.back, Vector3.up );
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
        if ( spawnTransforms_ != null )
        {
            foreach ( var spawnTransform in spawnTransforms_ )
            {
                Gizmos.DrawLine( spawnTransform.position, spawnTransform.position + spawnTransform.forward );
            }
        }
    }
}
