using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipes : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed;
    Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, Random.Range(-3.5f, -7.5f), transform.position.z);
        Destroy(gameObject, 10f);
    }

    private void Update()
    {
        PipeMovement();
        IncreaseSpeed();
    }

    private void PipeMovement()
    {
        transform.position = transform.position + new Vector3(-1f, 0f, 0f) * speed * Time.deltaTime;
    }

    private void IncreaseSpeed()
    {

        int pPoints = player.GetPoints();


        if (pPoints < 5)
        {
            speed = 7f;
        }
        else if (pPoints >= 5 && pPoints < 25)
        {
            speed = 6.5f;
        }
        else if (pPoints >= 25 && pPoints < 50)
        {
            speed = 6f;
        }
        else if (pPoints >= 50 && pPoints < 100)
        {
            speed = 5.5f;
        }
        else if (pPoints >= 100)
        {
            speed = 4f;
        }
    }
}
