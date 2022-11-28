using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;

    private int highScore;
    
    private bool m_Started = false;
    private int m_Points;
    private bool highestScore = false;
    
    private bool m_GameOver = false;

    [System.Serializable]
    public class HighScores{
        public string highname;
        public int highscore;
    }
    // Start is called before the first frame update
    void Start()
    {
        highScore = NameManager.Instance.bestScore;
        SetHighScoreText(NameManager.Instance.bestName,highScore);
        ScoreText.text = NameManager.Instance.name + " Score : 0";
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void SetHighScoreText(string name,int score){
        HighScoreText.text = "Best Score: "+ name + ": " + score;
    }

    void AddPoint(int point)
    {
        m_Points += point;
        if (m_Points>highScore){
            highestScore = true;
            highScore = m_Points;
            SetHighScoreText(NameManager.Instance.name,m_Points);
        }
        ScoreText.text = NameManager.Instance.name + $" Score : {m_Points}";
    }

    public void GameOver()
    {
        if(highestScore){
            WriteHighScore(NameManager.Instance.name,highScore);
            NameManager.Instance.bestName = NameManager.Instance.name;
            NameManager.Instance.bestScore = highScore;
        }
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    private void WriteHighScore(string name, int highScore){
        string path = Application.persistentDataPath + "/savefile.json";

        HighScores playerData = new HighScores();
        playerData.highname = name;
        playerData.highscore = highScore;
        string json = JsonUtility.ToJson(playerData);
        Debug.Log(json);
        File.WriteAllText(path,json);
    }
}
