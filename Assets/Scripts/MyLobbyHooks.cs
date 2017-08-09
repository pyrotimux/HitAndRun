using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.Networking;
using HitAndRun.Proto;

public class pLobbyHook : LobbyHook {

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobPlayer = lobbyPlayer.GetComponent<LobbyPlayer>();
        Player locPlayer = gamePlayer.GetComponent<Player>();

        locPlayer.pname = lobPlayer.playerName;
        locPlayer.pcolor = lobPlayer.playerColor;
    }
}
