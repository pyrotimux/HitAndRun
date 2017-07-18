using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class pLobbyHook : LobbyHook {

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobPlayer = lobbyPlayer.GetComponent<LobbyPlayer>();
        pPlayer locPlayer = gamePlayer.GetComponent<pPlayer>();

        locPlayer.pname = lobPlayer.playerName;
        locPlayer.pcolor = lobPlayer.playerColor;
    }
}
