using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public string currentPlayerName;
    public InputField playerNameInput;
    public static MainMenu Instance;
    int BestScore;
    string bestPlayerName;
    TMP_Text bestScoreText;

    // Start is called before the first frame update
    void Start()
    {
        playerNameInput = GameObject.Find("PlayerName").GetComponent<InputField>(); //gameObject.GetComponent<InputField>();
        playerNameInput.onEndEdit.AddListener(delegate { GetPlayerName(playerNameInput); });
        bestScoreText = GameObject.Find("BestScore").GetComponent<TMP_Text>();
        LoadStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);
        //LoadStats();
    }

    public void GetPlayerName(InputField name)
    {
        currentPlayerName = name.text;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    [System.Serializable]
    class SaveData
    {
        public int Score;
        public string PlayerName;
    }

    public void SaveStats(int m_Points)
    {
        if (m_Points > BestScore)
        {
            SaveData data = new SaveData();
            data.Score = m_Points;
            data.PlayerName = Instance.currentPlayerName;

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

            BestScore = data.Score;
            bestPlayerName = data.PlayerName;

            bestScoreText.text = $"Best Score : {bestPlayerName} : {BestScore}";
        }
    }
}
