using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private const int COIN_SCORE_AMOUNT = 5;
    public static GameManager Instance { get; set; }
    public bool IsDead { set; get; }
    private bool isGameStarted = false;
    private PlayerMotor motor;

    public Animator gameCanvas;
    public Animator menuAnim;
    public TextMeshProUGUI scoreText, coinText, modifierText, highscoreText;
    private float score, coinScore, modifierScore;
    private int lastScore;

    public Animator deathMenuAnim;
    public TextMeshProUGUI deadScoreText, deadCoinText;

    private void Awake() {
        modifierScore = 1;
        Instance = this;
        motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        modifierText.text = "x" + modifierScore.ToString("0.0");
        scoreText.text = score.ToString("0");
        coinText.text = coinScore.ToString("0");

        highscoreText.text = PlayerPrefs.GetInt("Highscore").ToString();
    }

    private void Update() {
        if(MobileInput.Instance.Tap && !isGameStarted) {
            isGameStarted = true;
            FindObjectOfType<GlacierSpawner>().IsScrolling = true;
            motor.StartRunning();
            FindObjectOfType<CameraMotor>().IsMoving = true;
            gameCanvas.SetTrigger("Show");
            menuAnim.SetTrigger("Hide");
        }
        
        if(isGameStarted && !IsDead)
        {
            //Bump up the score
            score += (Time.deltaTime * modifierScore);
            if(lastScore != (int)score) {
            lastScore = (int)score;
            scoreText.text = score.ToString("0");    
            }
        }
    }

    public void GetCoin() 
    {
        coinScore++;
        coinText.text = coinText.text = coinScore.ToString("0");
        score += COIN_SCORE_AMOUNT;
        scoreText.text = score.ToString("0");
    }

    public void UpdateModifier(float modifierAmount) 
    {
        modifierScore = 1.0f + modifierAmount;
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }

    public void OnPlayButton ()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnDeath()
    {
        IsDead = true;
        FindObjectOfType<GlacierSpawner>().IsScrolling = false;
        deadScoreText.text = score.ToString("0");
        deadCoinText.text = coinScore.ToString("0");
        deathMenuAnim.SetTrigger("Dead");
        gameCanvas.SetTrigger("Hide");
        
        //check if this is a highscore
        if(score > PlayerPrefs.GetInt("Highscore"))
        {
        float s = score;
        if(s % 1 == 0)
        s += 1;
        PlayerPrefs.SetInt("Highscore", (int)s);
        }
    }

}
