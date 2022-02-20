using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] Canvas inGameCanvas;
    [SerializeField] Canvas gameOverCanvas;
    Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        inGameCanvas.gameObject.SetActive(true);
        gameOverCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (player.isPlayerAlive() == true)
        {
            inGameCanvas.gameObject.SetActive(true);
        }
        else
        {
            inGameCanvas.gameObject.SetActive(false);
            gameOverCanvas.gameObject.SetActive(true);
        }
    }
}
