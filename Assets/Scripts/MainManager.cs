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

    public Text ScoreText, bestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    public int Score;
    int bestScore;
    public string PlayerName;
    string bestPlayerName;

    public static MainManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Inside Start");
        BuildBrickWall();
    }

    void BuildBrickWall()
    {
        Debug.Log("Inside BuildBrickWall");
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                // changing colors of the bricks
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    void RunGame()
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

    private void Update()
    {
        if (!m_Started)
        {
            RunGame();
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_GameOver = false;
                m_Started = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Debug.Log("Scene Reloaded");
                //BuildBrickWall();
                //RunGame();
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points} ({PlayerName})";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SaveStats();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadStats();
    }

    [System.Serializable]
    class SaveData
    {
        public int Score;
        public string PlayerName;
    }

    public void SaveStats()
    {
        if (m_Points > bestScore)
        {
            SaveData data = new SaveData();
            data.Score = m_Points;
            data.PlayerName = PlayerName;

            string json = JsonUtility.ToJson(data);

            File.WriteAllText(Application.persistentDataPath + "/brick_savefile.json", json);
        }
    }

    public void LoadStats()
    {
        string path = Application.persistentDataPath + "/brick_savefile.json";
        Debug.Log(path);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestScore = data.Score;
            string bestPlayerName = data.PlayerName != "" ? data.PlayerName : "Name";
            //Debug.Log("Name: " + bestPlayerName + " Score: " + bestScore);

            bestScoreText = GameObject.Find("BestScoreText").GetComponent<Text>();
            bestScoreText.text = $"Best Score : {bestPlayerName} : {bestScore}";
        }
    }
}
