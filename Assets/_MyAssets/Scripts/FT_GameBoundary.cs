using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FT_GameBoundary : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
//        Debug.Log(other.gameObject.name + " has left the playspace");
        ResetPositionOfGamePiecesOutsideGameBoundary(other);

    }


    /*  if a player throws a game piece outside the trigger area of the collider on this component
        then reset it to it's original position.  If it is a projectile game piece do nothing and let it get
        destroyed by that code.
    */
    private static void  ResetPositionOfGamePiecesOutsideGameBoundary(Collider other)
    {
        FT_GamePiece gamePiece = other.gameObject.GetComponent<FT_GamePiece>();
        if (gamePiece != null && !gamePiece.projectileGamePiece)
        {
            gamePiece.ResetPosition();
        }
    }
}
