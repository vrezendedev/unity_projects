using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScrolling : MonoBehaviour
{
    [SerializeField] float parallaxSpeed;

    MeshRenderer myMR;

    private void Awake()
    {
        myMR = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
 
    }

    private void Update()
    {
        myMR.material.mainTextureOffset = myMR.material.mainTextureOffset + new Vector2(parallaxSpeed * Time.deltaTime, 0f);
    }

}
