using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scores : MonoBehaviour
{
    private void Start()
    {
        GameController.ScoresChanged += SetScores;
    }

    private void SetScores()
    {
        GetComponent<Text>().text = GameController.Scores.ToString();
    }

    private void OnDestroy()
    {
        GameController.ScoresChanged -= SetScores;
    }
}
