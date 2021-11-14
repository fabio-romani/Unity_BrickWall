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
        MainManager.Instance.LoadStats();
        playerNameInput = GameObject.Find("PlayerName").GetComponent<InputField>();
        playerNameInput.onEndEdit.AddListener(delegate { GetPlayerName(playerNameInput); });
        bestScoreText = GameObject.Find("BestScore").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
