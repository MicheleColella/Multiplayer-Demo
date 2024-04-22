using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class CreateAndJoinsRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "camera1Available", true }, { "camera2Available", true } };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "camera1Available", "camera2Available" };

        PhotonNetwork.CreateRoom(createInput.text, roomOptions);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room successfully.");
        CheckRoomPropertiesAndLoadScene();
    }

    private void CheckRoomPropertiesAndLoadScene()
    {
        var roomProps = PhotonNetwork.CurrentRoom.CustomProperties;
        if (!(bool)roomProps["camera1Available"] && !(bool)roomProps["camera2Available"])
        {
            Debug.LogError("Camera setup failed: No cameras available.");
        }
        else
        {
            PhotonNetwork.LoadLevel("Game");
        }
    }
}
