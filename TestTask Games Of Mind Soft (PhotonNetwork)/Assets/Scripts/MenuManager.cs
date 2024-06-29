using Photon.Pun;
using Photon.Realtime;

public class MenuManager : MonoBehaviourPunCallbacks
{
    public TMPro.TMP_InputField createRoomInput;
    public TMPro.TMP_InputField joinRoomInput;

    public void CreateRoom()
    {
        RoomOptions _roomOptions = new RoomOptions();
        _roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(createRoomInput.text, _roomOptions);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinRoomInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}