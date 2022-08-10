using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WheatBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _gatheringWheatPrefab;

    private GameObject _click;
    private bool _isGathering = false;

    private float _timeOfAlive = 3f;
    private float _timeOfReincornation = 10f;
    private bool _isReincornation = true;

    private void Start()
    {
        _click = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        CheckIsGathering();
    }

    private void CheckIsGathering() {
        if (_isGathering && _click.GetComponent<PlayerController>().CheckClick())
        {
            _timeOfAlive -= Time.deltaTime;
            if (Mathf.Round(_timeOfAlive) <= 0)
            {
                this.gameObject.SetActive(false);
                GameObject _gatheredWheat = Instantiate(_gatheringWheatPrefab, new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z),
                   Quaternion.Euler(new Vector3(0, 90, 0)));
                _gatheredWheat.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0f, 3f), 7f, Random.Range(-3f, 3f)), ForceMode.Impulse);
                _timeOfAlive = 3f;
                this.OffGathering();
            }
        }
    }

    public void OnGathering() {
        _isGathering = true;
    }

    public void OffGathering() {
        _isGathering = false;
    }

    public bool IsReincornation()
    {
        return _isReincornation;
    }

    public void StartReincornation()
    {
        StartCoroutine(Reincornation());
        _isReincornation = false;
    }

    private IEnumerator Reincornation()
    {
        this.transform.Find("Wheat_Mesh").gameObject.SetActive(false);

        yield return new WaitForSeconds(_timeOfReincornation);

        this.transform.Find("Wheat_Mesh").gameObject.SetActive(true);
    }
}
