using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPTile : Tile {
    public override Tile GetConnection(Direction direction) {
        if (direction == Direction.Left || direction == Direction.Right) {
            RotatePlayer(direction);
        } else if (direction == Direction.Up) {
            FPPlayer player = BattleManager.instance.player.GetComponent<FPPlayer>();
            Direction facingDirection = player.facingDirection;
            return base.GetConnection(facingDirection);
        } else if (direction == Direction.Interact) {
            TileInteragivel tileInteragivel = GetComponent<TileInteragivel>();
            if (tileInteragivel == null || !tileInteragivel.Interact())
                Controller7Erros.instance.GetInteraction(this);
        }

        return null;
    }

    protected void RotatePlayer(Direction direction) {
        FPPlayer player = BattleManager.instance.player.GetComponent<FPPlayer>();

        Direction[] order = new Direction[] { Direction.Up, Direction.Right, Direction.Down, Direction.Left };
        Direction facingDirection = player.facingDirection;

        // Get the index of the current facing direction
        int currentIndex = 0;
        for (int i = 0; i < order.Length; i++) {
            if (order[i] == facingDirection) {
                currentIndex = i;
                break;
            }
        }

        int offset = direction == Direction.Left ? -1 : 1;
        int newIndex = (currentIndex + offset) % order.Length;
        newIndex = newIndex < 0 ? order.Length + newIndex : newIndex;

        player.Rotate(order[newIndex]);
    }
}
