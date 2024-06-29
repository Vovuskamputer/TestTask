using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private void Start()
    {
        // Only the client will create the player
        if (!PhotonNetwork.IsMasterClient) PhotonNetwork.Instantiate(_player.name, new Vector3(0f, 1f, 0f), Quaternion.identity);
    }
}