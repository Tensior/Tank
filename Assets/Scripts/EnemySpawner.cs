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
    private int numberOfSpawnPoints_ = 10;
    private List<Transform> spawnTransforms_;

    private EnemyPool[] enemyPools_;

    // Start is called before the first frame update
    private void Awake()
    {
        CalculateSpawnTransforms();
        GetEnemyPools();
    }

    private void GetEnemyPools()
    {
        enemyPools_ = GetComponents<EnemyPool>();
    }

    // Update is called once per frame
    private void Update()
    {
        if( currentNumberOfEnemies_ < maxNumberOfEnemies_)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        int enemyType = Random.Range( 0, enemyPools_.Length );
        int enemyTransform = Random.Range( 0, spawnTransforms_.Count );

        var newEnemy = enemyPools_[enemyType].GetFromPool();
        newEnemy.gameObject.SetActive( true );
        newEnemy.transform.SetPositionAndRotation( spawnTransforms_[enemyTransform].position, spawnTransforms_[enemyTransform].rotation );
        ++currentNumberOfEnemies_;

        newEnemy.OnDeath += DecreaseCurrentNumberOfEnemies;
    }

    private void DecreaseCurrentNumberOfEnemies()
    {
        --currentNumberOfEnemies_;
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

        var spawnSteps = new Vector2( spawnRect_.size.x / (nSpawns_.x + 1), spawnRect_.size.y / (nSpawns_.y + 1) );

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
