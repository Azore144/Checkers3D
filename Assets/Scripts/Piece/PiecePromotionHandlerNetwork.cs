using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecePromotionHandlerNetwork : PiecePromotionHandler
{
    public override void OnStartServer()
    {
        PieceMovementHandlerNetwork.OnPieceReachedBackline += TryPromotePiece;
    }
    public override void OnStopServer()
    {
        PieceMovementHandlerNetwork.OnPieceReachedBackline -= TryPromotePiece;
    }
    protected override bool TryPromotePiece(PiecePromotionHandler promotedPiece, int x, int z)
    {
        if(!base.TryPromotePiece(promotedPiece, x, z))
        {
            return false;
        }
        RPCPromotePiece();
        return true;
    }
    [ClientRpc]
    void RPCPromotePiece()
    {
        if(NetworkServer.active)
        {
            return;
        }
        PromotePiece();
    }
}
