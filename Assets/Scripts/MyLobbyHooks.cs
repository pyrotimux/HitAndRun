using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.Networking;
using HitAndRun.Proto;

public class MyLobbyHooks : LobbyHook {

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobPlayer = lobbyPlayer.GetComponent<LobbyPlayer>();
        IPlayer locPlayer = gamePlayer.GetComponent<Survivor>();

        locPlayer.pname = lobPlayer.playerName;
        locPlayer.pcolor = lobPlayer.playerColor;
    }
}
