using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class HighScore : MonoBehaviour
    {
        [SerializeField]
        private string _highScoreString;

        private void Start()
        {
            GameController.LevelFailed += SetScores;
        }

        private void SetScores()
        {
            GetComponent<Text>().text = $"{_highScoreString} {PlayerPrefs.GetInt("HighScore", 0).ToString()}";
        }

        private void OnDestroy()
        {
            GameController.LevelFailed -= SetScores;
        }
    }
}