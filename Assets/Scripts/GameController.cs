using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private FirstPersonController player;
    private CanvasGroup gameOverScreen;

    private void Awake()
    {
        gameOverScreen = mainCanvas.transform.GetChild(2).GetComponent<CanvasGroup>();

        gameOverScreen.alpha = 0;
        gameOverScreen.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        StartCoroutine(IEGameOver());
    }

    IEnumerator IEGameOver()
    {
        player.AllowMove = false;
        gameOverScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        gameOverScreen.DOFade(1, 0.5f);
        
    }
}
