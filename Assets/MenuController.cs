using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void OnClick()
    {
        Debug.Log("Start!");
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    private void OnPlayButtonClick(object obj, EventArgs args)
    {
        Debug.Log("Start!");
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }
}
