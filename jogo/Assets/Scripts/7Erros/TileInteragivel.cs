using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInteragivel : MonoBehaviour {
    public static TileInteragivel ultimoTileInteragivel;
    public Direction direction;
    public System.Action onInteract;

    public bool isFP = true;

    public bool Interact() {
        if (isFP) {
            FPPlayer player = BattleManager.instance.player.GetComponent<FPPlayer>();
            if (player.facingDirection != direction) {
                return false;
            }
        }
        

        if (onInteract != null) {
            ultimoTileInteragivel = this;
            onInteract();
            return true;
        }

        return false;
    }
}
