using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Fork _fork;
    [SerializeField]
    private PreFork _preFork;
    [SerializeField]
    private Target _target;
    [SerializeField]
    private Vector3 _forkInstantiatePoint;
    [SerializeField]
    private Vector3 _targetInstantiatePoint;
    [SerializeField]
    private Canvas _menuCanvas;
    [SerializeField]
    private Canvas _inGameCanvas;
    [SerializeField]
    [Range(1, 3)]
    private int _forkPreSpawn;

    private List<Fork> _forks;
    public static int ScoresToWin = 1;

    private Target TargetInstance;
    [SerializeField]
    private float _preForkInstantiateY;
    public static int Scores { get; private set; } = 0;
    public static int LevelScores;

    public delegate void GameControllerEvent();
    public static event GameControllerEvent LevelComplete;
    public static event GameControllerEvent LevelFailed;
    public static event GameControllerEvent ScoresChanged;

    void Start()
    {
        Vibration.Init();
        Reset();
    }

    private void Reset()
    {
        TargetInstance = Instantiate(_target, _targetInstantiatePoint, Quaternion.identity);
        _forks = new List<Fork>();
        _menuCanvas.enabled = false;
        _inGameCanvas.enabled = true;

        for (int i = 0; i < _forkPreSpawn; i++)
        {
            var preFork = Instantiate(_preFork, new Vector3(0, _preForkInstantiateY, 0), Quaternion.identity);
            _forks.Add(preFork);
            TargetInstance.transform.Rotate(new Vector3(0, 0, 90));
        }

        Fork.ForkCollision += OnForkCollision;
        LevelScores = 0;

        InstantiateFork(_forkInstantiatePoint);
    }

    private void OnForkCollision(Fork fork)
    {
        Vibration.VibratePeek();
        if (fork.State == ForkState.HitTarget)
        {
            Scores++;
            ScoresChanged.Invoke();
            LevelScores++;
            if (LevelScores >= ScoresToWin)
            {
                OnLevelComplete();
            }
            else
            {
                InstantiateFork(_forkInstantiatePoint);
            }
        }
        else if (fork.State == ForkState.HitKnife)
        {
            OnLevelFailed();
        }
    }

    private Fork InstantiateFork(Vector3 position)
    {
        var fork = Instantiate(_fork, position, Quaternion.identity);

        _forks.Add(fork);
        return fork;
    }

    private IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(2);

        foreach (var fork in _forks)
        {
            Destroy(fork.gameObject);
        }

        Destroy(TargetInstance.gameObject);

        _forks.Clear();
        Reset();
        _inGameCanvas.GetComponent<UIFork>().Reset();
    }

    private IEnumerator ViewScoreBoard()
    {
        yield return new WaitForSeconds(0.5f);

        SwitchCanvas();
        Fork.ForkCollision -= OnForkCollision;
        foreach (var fork in _forks)
        {
            Destroy(fork.gameObject);
        }

        Destroy(TargetInstance.gameObject);

        _forks.Clear();
    }

    public void OnLevelComplete()
    {
        Fork.ForkCollision -= OnForkCollision;
        ScoresToWin++;
        LevelComplete.Invoke();
        StartCoroutine(NextLevel());
    }

    public void OnLevelFailed()
    {
        if (PlayerPrefs.GetInt("HighScore", 0) < Scores)
        {
            PlayerPrefs.SetInt("HighScore", Scores);
        }
        LevelFailed.Invoke();
        StartCoroutine(ViewScoreBoard());
    }

    public void OnRestartButtonClick()
    {
        SwitchCanvas();
        Reset();
        Scores = 0;
        ScoresChanged.Invoke();
        ScoresToWin = 1;

        _inGameCanvas.GetComponent<UIFork>().Reset();
    }

    public void OnExitButtonClick()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    private void SwitchCanvas()
    {
        _inGameCanvas.enabled = !_inGameCanvas.enabled;
        _menuCanvas.enabled = !_menuCanvas.enabled;
    }
}
