using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pointsText;
    Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();

    }

    private void Start()
    {
        pointsText.text = "0";
    }

    private void Update()
    {
        pointsText.text = player.GetPoints().ToString();
    }
}
