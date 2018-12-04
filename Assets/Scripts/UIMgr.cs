using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour
{

    public void ReGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void GoMain()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LobyScene");
    }

    public void GameEnd()
    {
        Application.Quit();
    }
}