using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public enum GameState{
        loading,
        inGame,
        gameOver
    }
    public GameState gameState;

    public List<GameObject> targetPrefabs;

    private float spawnRate = 1.0f;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;

    public Button restartButton;


    //Variable Autocomputada
    private int _score;
    private int score{
        set{
            _score = Mathf.Clamp(value,0,99999);

        }
        get{
            return _score;
        }
    }

    public GameObject startMenu;

    public GameObject swordFollow;

    /// <summary>
    /// Método que inicia la partida cambiando el valor del estado del juego
    /// </summary>`
    /// <param name="difficulty"> Número entero que indica el grado de dificultad del juego </param>
    public void StartGame(float difficulty){
        gameState = GameState.inGame;
        startMenu.SetActive(false);

        spawnRate /=difficulty;

        StartCoroutine(SpawnTarget());
        score=0;
        UpdateScore(0);
    }

    IEnumerator SpawnTarget(){
        while (gameState == GameState.inGame){
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targetPrefabs.Count);
            Instantiate(targetPrefabs[index]);
        }
    }

    /// <summary>
    /// Actualiza la puntación y lo muestra por pantalla
    /// </summary>
    /// <param name="scoreToAdd"> Número de puntos a añadir a la puntuación global</param>
    public void UpdateScore(int scoreToAdd){
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    /// <summary>
    /// Muestra el mensaje de Game Over, muestra el botón de Restart y Pausa el tiempo de juego
    /// </summary>
    public void GameOver(){
        gameState = GameState.gameOver;
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Carga de nuevo la escena para empezar nueva partida
    /// </summary>
    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GitHubUpdate(){

    }

}
