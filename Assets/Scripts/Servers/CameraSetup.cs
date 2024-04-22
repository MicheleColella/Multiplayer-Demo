using UnityEngine;
using Cinemachine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CameraSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] private CinemachineVirtualCamera player1Camera;
    [SerializeField] private CinemachineVirtualCamera player2Camera;

    void Start()
    {
        SetupPlayerCamera();
    }

    private void SetupPlayerCamera()
    {
        var roomProps = PhotonNetwork.CurrentRoom.CustomProperties;

        if ((bool)roomProps["camera1Available"])
        {
            player1Camera.enabled = true;
            player2Camera.enabled = false;
            roomProps["camera1Available"] = false;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
            EnableCharacterControl(true);
        }
        else if ((bool)roomProps["camera2Available"])
        {
            player1Camera.enabled = false;
            player2Camera.enabled = true;
            roomProps["camera2Available"] = false;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProps);
            EnableCharacterControl(false);
        }
        else
        {
            Debug.LogError("No cameras available.");
        }
    }

    private void EnableCharacterControl(bool enable)
    {
        GameObject playerCharacter = GameObject.FindGameObjectWithTag("Player");
        if (playerCharacter != null)
        {
            playerCharacter.GetComponent<PlayerMovement>().enabled = enable;
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("Master client left, redirecting to lobby.");
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel("Lobby");
        }
    }
}
