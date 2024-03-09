using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnsHandlerNetworked : TurnsHandler
{
    protected override void FillMovesList()
    {
        base.FillMovesList();
        RPCGenerateMoves(piecesHandler);
    }
    [ClientRpc]
    void RPCGenerateMoves(PlayerPiecesHandler playerPiecesHandler)
    {
        if(NetworkServer.active)
        {
            return;
        }
        GenerateMoves(playerPiecesHandler.PiecesParent);
    }
}
