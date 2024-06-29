using Photon.Pun;
using UnityEngine;

public class SpawnerCubesOnZone3 : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject cubeWhite, cubeGray;
    [SerializeField] private GameObject _listCubes;

    private void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            GameObject _spawnedWhiteCube = PhotonNetwork.Instantiate(cubeWhite.name, transform.position, Quaternion.identity);
            _spawnedWhiteCube.transform.SetParent(_listCubes.transform);
            _spawnedWhiteCube.transform.position = _listCubes.transform.GetChild(i).transform.position;
        }

        for (int i = 9; i < 18; i++)
        {
            GameObject _spawnedGrayCube = PhotonNetwork.Instantiate(cubeGray.name, transform.position, Quaternion.identity);
            _spawnedGrayCube.transform.SetParent(_listCubes.transform);
            _spawnedGrayCube.transform.position = _listCubes.transform.GetChild(i).transform.position;
        }
    }
}