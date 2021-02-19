using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class UIFork : MonoBehaviour
{
    [SerializeField]
    private Image _uiFork;
    [SerializeField]
    private Vector3 _bottomInstantiatePoint;
    private int _scoresToComplete;
    private List<Image> _uiForks;

    void Start()
    {
        _scoresToComplete = GameController.ScoresToWin;
        _uiForks = new List<Image>();

        for (int i = 0; i < _scoresToComplete; i++)
        {
            var instantiatePoint = _bottomInstantiatePoint + new Vector3(0, i * 0.5f, 0);
            var uiFork = Instantiate(_uiFork, instantiatePoint, Quaternion.identity);
            uiFork.transform.Rotate(0, 0, -45);
            uiFork.transform.SetParent(GetComponent<Canvas>().transform);
            _uiForks.Add(uiFork);
        }

        Fork.ForkCollision += OnForkCollision;
    }

    private void OnForkCollision(Fork fork)
    {
        if (fork.State == ForkState.HitTarget)
        {
            _scoresToComplete--;
            var currElem = _uiForks[_scoresToComplete];
            var newColor = new Color(currElem.color.r, currElem.color.g, currElem.color.b, 0.3f);
            currElem.color = newColor;
        }
    }

    public void Reset()
    {
        foreach (var fork in _uiForks)
        {
            Destroy(fork.gameObject);
        }

        _scoresToComplete = GameController.ScoresToWin;
        _uiForks = new List<Image>();

        for (int i = 0; i < _scoresToComplete; i++)
        {
            var instantiatePoint = _bottomInstantiatePoint + new Vector3(0, i * 0.5f, 0);
            var uiFork = Instantiate(_uiFork, instantiatePoint, Quaternion.identity);
            uiFork.transform.Rotate(0, 0, -45);
            uiFork.transform.SetParent(GetComponent<Canvas>().transform);
            _uiForks.Add(uiFork);
        }
    }

    private void OnDestroy()
    {
        Fork.ForkCollision -= OnForkCollision;
        GameController.LevelComplete -= Reset;
    }
}
