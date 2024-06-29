using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviourPunCallbacks
{
    private PhotonView _photonView;
    private Vector3 _directionMovement;

    // CAMERA DATA FIELD //

    [SerializeField] private float _speed = 3f;

    private float _sensitivityHorizontal = 3f;
    private float _sensitivityVertical = 3f;
    private float _minVertical = -75f;
    private float _maxVertical = 90f;
    private float _rotationX;
    private float _rotationY;

    [SerializeField] private Camera _camera;

    [SerializeField] private GameObject _helpPanelCatch;
    [SerializeField] private GameObject _rayPoint;
    [Range(0f, 10f)] [SerializeField] private float _rayLength;

    [SerializeField] private GameObject cubeWhite, cubeGray;
    private GameObject _inventoryItem = null;
    private string _namePlate = "";

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();

        HideCursor();

        _helpPanelCatch.SetActive(false);
    }

    private void CameraRotation()
    {
        _rotationX -= Input.GetAxis("Mouse Y") * _sensitivityVertical;
        _rotationX = Mathf.Clamp(_rotationX, _minVertical, _maxVertical);
        _rotationY = Input.GetAxis("Mouse X") * _sensitivityHorizontal + transform.localEulerAngles.y;

        transform.localEulerAngles = new Vector3(0f, _rotationY, 0f);
        _camera.transform.localEulerAngles = new Vector3(_rotationX, 0f, 0f);
    }

    private void Movement()
    {
        _directionMovement.x = Input.GetAxis("Horizontal");
        _directionMovement.z = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(_directionMovement.x, 0f, _directionMovement.z) * _speed * Time.deltaTime);
    }

    [PunRPC]
    private void DestroyCube() => PhotonNetwork.Destroy(_inventoryItem.gameObject);

    private void RayDetection()
    {
        Ray _ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit;

        _rayPoint.SetActive(_helpPanelCatch.activeSelf);

        if (Physics.Raycast(_ray, out _hit, _rayLength))
        {
            _rayPoint.transform.position = _hit.point;

            if (_hit.collider.gameObject.GetComponent<Rigidbody>() != null)
            {
                _helpPanelCatch.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    if (_hit.transform.name.Contains("Cube") && _inventoryItem == null)
                    {
                        if (_namePlate != "") GameObject.Find(_namePlate).GetComponent<Detector>().SetTag("Untagged");

                        _inventoryItem = _hit.collider.gameObject;
                        // Mistake with destroy cubes which was placed on the scene already
                        _photonView.RPC("DestroyCube", RpcTarget.AllBuffered);

                        // Visible state of cube which hide when player push on the left mouse button and show too with this system
                        //_inventoryItem.SetActive(false);
                    }
                    else if (_inventoryItem != null)
                    {
                        if (_hit.transform.name == "DestroyPlate")
                        {
                            Destroy(_inventoryItem);
                            _inventoryItem = null;
                        }
                        else if (_hit.transform.name == "ResetPlate")
                        {
                            Win();
                        }
                        else if(_hit.transform.name.Contains("Plate"))
                        {
                            _inventoryItem.transform.position = new Vector3(_hit.transform.position.x, _inventoryItem.transform.position.y + 0.1f, _hit.transform.position.z);
                            _inventoryItem.transform.rotation = Quaternion.identity;

                            if (!_hit.transform.name.Contains("Free")) CheckCode(_hit);

                            if (_inventoryItem.transform.name.Contains("White")) PhotonNetwork.Instantiate(cubeWhite.name, _inventoryItem.transform.position, Quaternion.identity);
                            else PhotonNetwork.Instantiate(cubeGray.name, _inventoryItem.transform.position, Quaternion.identity);
                            //_inventoryItem.SetActive(true);
                            _inventoryItem = null;

                            if (GameManager.code == GameManager.winCode) Win();
                        }
                    }
                }
            }
            else _helpPanelCatch.SetActive(false);
        }
        else _helpPanelCatch.SetActive(false);
    }

    private void CheckCode(RaycastHit _hit)
    {
        _hit.transform.GetComponent<Detector>().SetTag(_inventoryItem.transform.tag);
        _namePlate = _hit.transform.name;

        int _selectedIndex = Convert.ToInt32(_namePlate.Substring(_namePlate.Length - 1));
        char _symbol;

        if (_inventoryItem.transform.tag == "Filling") _symbol = '1';
        else _symbol = '0';

        string _inCode = GameManager.code;
        string _outCode = "";

        for (int i = 0; i < _inCode.Length; i++)
        {
            if (i == _selectedIndex - 1) _outCode += _symbol;
            else _outCode += GameManager.code[i];
        }

        GameManager.code = _outCode;
    }

    private void Win()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Menu");

        ShowCursor();
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                CameraRotation();
                Movement();
                RayDetection();
            }
        }
    }
}