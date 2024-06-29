using System.Collections.Generic;
using UnityEngine;

public class PictureCreator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _plates = new List<GameObject>();
    [SerializeField] private GameObject _prefabCubeGray, _prefabCubeWhite;

    private void Start()
    {
        GameObject _listOfCubes = new GameObject("ListOfCubes");
        _listOfCubes.transform.position = transform.position;

        foreach (GameObject _plate in _plates)
        {
            int _rand = Random.Range(0, 2);
            GameObject _prefab;

            if (_rand == 1) _prefab = _prefabCubeGray;
            else _prefab = _prefabCubeWhite;

            GameManager.winCode += _rand;

            GameObject _spawnedCube = Instantiate(_prefab, _plate.transform.position, Quaternion.identity);

            _spawnedCube.transform.SetParent(_listOfCubes.transform);
            Destroy(_spawnedCube.GetComponent<Rigidbody>());
            Destroy(_plate);
        }

        _listOfCubes.transform.SetParent(transform);
    }
}