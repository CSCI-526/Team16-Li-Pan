using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int maxEnemies = 6;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            int aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (aliveEnemies < maxEnemies && spawnPoints.Length > 0)
            {
                int index = Random.Range(0, spawnPoints.Length);

                Instantiate(
                    enemyPrefab,
                    spawnPoints[index].position,
                    Quaternion.identity
                );
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
