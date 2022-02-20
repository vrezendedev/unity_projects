using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentPoints;
    [SerializeField] Button replayButton;
    [SerializeField] AudioClip replayAudio;
    public TextMeshProUGUI highestPoints;
    Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        highestPoints.text = PlayerPrefs.GetInt("Highscore", 0).ToString();
        replayButton.interactable = true;
    }

    
    void Update()
    {
        currentPoints.text = player.GetPoints().ToString();
        if (player.GetPoints() > PlayerPrefs.GetInt("Highscore", 0))
        {
            PlayerPrefs.SetInt("Highscore", player.GetPoints());
            highestPoints.text = PlayerPrefs.GetInt("Highscore", 0).ToString();
        }
    }

    public void Replay()
    {
        AudioSource.PlayClipAtPoint(replayAudio, Camera.main.transform.position, 0.01f);
        replayButton.interactable = false;
        StartCoroutine(WaitBeforeLoad());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator WaitBeforeLoad()
    {
        yield return new WaitForSeconds(1.5f);
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }
}
