using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameBehaviour : MonoBehaviour
{
    public float _timeStart = 60f;

    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _coins;
    [SerializeField] private TextMeshProUGUI _items;
    [SerializeField] private GameObject _restartButton;

    private int _balance = 0;
    public int Balance
    {
        get
        {
            return _balance;
        }
        set
        {
            _balance = value;
            _coins.text = _balance.ToString();
        }
    }

    private int _itemsCollected = 0;
    public int Items
    {
        get {
            return _itemsCollected;
        }
        set {
            _itemsCollected = value;
            _items.text = _itemsCollected.ToString();
        }
    }

    private void Start()
    {
        Time.timeScale = 1.0f;
        _restartButton.SetActive(false);
        _timerText.text = _timeStart.ToString();
        _coins.text = _balance.ToString();
        _items.text = _itemsCollected.ToString();
    }

    private void Update()
    {
        _timeStart -= Time.deltaTime;
        _timerText.text = Mathf.Round(_timeStart).ToString();
        if (Mathf.Round(_timeStart) <= 0) {
            Time.timeScale = 0.0f;
            _restartButton.SetActive(true);
        }
    }

    public void Restart() {
        SceneManager.LoadScene(0);
    }
}
