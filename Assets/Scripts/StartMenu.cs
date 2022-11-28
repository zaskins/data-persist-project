using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;
using System.IO;

public class StartMenu : MonoBehaviour
{
    public TextMeshProUGUI highscore;
    public TextMeshProUGUI name;

    [System.Serializable]
    public class HighScores{
        public string highname;
        public int highscore;
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadHighScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighScores data = JsonUtility.FromJson<HighScores>(json);

            highscore.text = data.highname+" "+data.highscore;
            NameManager.Instance.bestName = data.highname;
            NameManager.Instance.bestScore = data.highscore;
        }
        else{
            highscore.text = "";
            NameManager.Instance.bestName = "";
            NameManager.Instance.bestScore = 0;
        }
    }

    public void StartGame()
    {
        NameManager.Instance.name = name.text;
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
