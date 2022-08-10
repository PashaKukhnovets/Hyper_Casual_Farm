using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _body;
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameBehaviour _gameManager;
    [SerializeField] private GameObject _gatheringButton;
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private Transform _canvasTransform;

    private float _moveSpeed = 4f;
    private float _heightDistance = 0.21f;
    private float _wideDistance = -0.5f;
    private bool _isClicked = false;

    private List<GameObject> _wheatList = new List<GameObject>();

    private void Start()
    {
        _gatheringButton.SetActive(false);
    }

    private void Update()
    {
        GatheringLogic();
    }

    void FixedUpdate()
    {
        MoveLogic();
    }

    void MoveLogic() {
        _body.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, 0, _joystick.Vertical * _moveSpeed);

        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(_body.velocity);
            _animator.SetBool("IsRunning", true);
        }
        else
        {
            _animator.SetBool("IsRunning", false);
        }
    }

    void GatheringLogic() {
        if (_isClicked)
        {
            _animator.SetBool("IsGathering", true);

        }
        else
        {
            _animator.SetBool("IsGathering", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<GatheredWheatBehaviour>() && 
            collision.gameObject.GetComponent<GatheredWheatBehaviour>().GetComponent<Rigidbody>().isKinematic) {
            if (_wheatList.Count < 40)
            {
                if (_wheatList.Count == 0) {
                    _heightDistance = 0.21f;
                }
                if (_wheatList.Count >= 20)
                {
                    _wideDistance = -1.03f;
                    _heightDistance = 0.21f * (_wheatList.Count - 19);
                }
                else if (_wheatList.Count < 20 && _wheatList.Count != 0)
                {
                    _wideDistance = -0.5f;
                    _heightDistance = (_wheatList.Count + 1) * 0.21f;
                }
                _wheatList.Add(collision.gameObject);
                collision.gameObject.transform.parent = transform;
                collision.gameObject.transform.localPosition = new Vector3(-0.03f, _heightDistance, _wideDistance);
                collision.gameObject.transform.localRotation = Quaternion.Euler(0, -90, 0);
                _gameManager.Items++;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<WheatBehaviour>()) 
        {
            other.gameObject.GetComponent<WheatBehaviour>().OnGathering();
        }
        if (other.gameObject.GetComponent<GroundBehaviour>())
        {
            _gatheringButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<GroundBehaviour>())
        {
            _gatheringButton.SetActive(false);
        }
    }

    public void StateOnDown() {
        _isClicked = true;
    }

    public void StateOnUp()
    {
        _isClicked = false;
    }

    public bool CheckClick() {
        return _isClicked;
    }

    public void Sale() {
        if (_wheatList.Count >= 1)
        {
            StartCoroutine(SaleWheat());
        }
    }

    private IEnumerator SaleWheat() {
        Instantiate(_coinPrefab, _canvasTransform);
        Destroy(_wheatList[_wheatList.Count - 1]);
        _gameManager.Items--;
        _heightDistance -= 0.21f;
        _wheatList.RemoveAt(_wheatList.Count - 1);

        yield return new WaitForSeconds(0.75f);

        _gameManager.Balance += 15;
    }
}
