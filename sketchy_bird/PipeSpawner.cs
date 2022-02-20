using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] GameObject pipe;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float spawnRate;

    bool spawn;
    Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        spawn = true;
    }

    private void Update()
    {
        SpawnPipe();
        IncreaseSpawnRate();
    }

    IEnumerator SpawnPipeTime()
    {
        yield return new WaitForSeconds(spawnRate);
        spawn = true;
    }

    private void SpawnPipe()
    {
        if (spawn == true && player.isPlayerAlive() == true && player.isPlayerReady() == true)
        {
            GameObject instance = pipe;
            Instantiate(instance, spawnPoint);
            spawn = false;
            StartCoroutine("SpawnPipeTime");
        }
    }

    private void IncreaseSpawnRate()
    {
        int pPoints = player.GetPoints();

        if (pPoints < 5)
        {
            spawnRate = 1.75f;
        }
        else if (pPoints >= 5 && pPoints < 25)
        {
            spawnRate = 1.50f;
        }
        else if (pPoints >= 25 && pPoints < 50)
        {
            spawnRate = 1.25f;
        }
        else if (pPoints >= 50 && pPoints < 100)
        {
            spawnRate = 1.15f;
        }
        else if (pPoints >= 100)
        {
            spawnRate = 1.10f;
        }
    }
}
