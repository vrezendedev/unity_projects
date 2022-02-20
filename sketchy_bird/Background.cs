using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] GameObject title;

    private void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Background");
        
        if(objects.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Destroy(title, 3f);
    }
}
