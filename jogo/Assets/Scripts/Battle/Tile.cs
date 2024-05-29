using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public enum ConnectionType { Floor, Gap }
public enum Direction { None, Up, Down, Left, Right, Interact }

[System.Serializable]
public class Connection {

    [SerializeField]
    public Tile tile;

    [SerializeField]
    public ConnectionType connectionType = ConnectionType.Floor;

    [SerializeField]
    public Direction direction;

    public Connection(Tile tile, ConnectionType connectionType, Direction direction) {
        this.tile = tile;
        this.connectionType = connectionType;
        this.direction = direction;
    }
}


public class Tile : MonoBehaviour {

    public List<Connection> connections = new List<Connection>();

    public virtual Tile GetConnection(Direction direction) {
        for (int i = 0; i < connections.Count; i++) {
            if (connections[i].direction == direction) {
                return connections[i].tile as Tile;
            }
        }

        return null;
    }

    public Tile GetConnectionRaw(Direction direction) {
        for (int i = 0; i < connections.Count; i++) {
            if (connections[i].direction == direction) {
                return connections[i].tile as Tile;
            }
        }

        return null;
    }

    void OnDrawGizmos() {
        if (connections == null) return;

        for (int i = 0; i < connections.Count; i++) {
            if (connections[i] == null) continue;
            if (connections[i].tile == null) continue;

            Vector3 direction = connections[i].tile.transform.position - transform.position;
            Vector3 position = transform.position + direction / 2;

            Gizmos.color = GetGizmoColor((int) connections[i].direction);

            Gizmos.DrawLine(transform.position, position);
        }
    }

    Color GetGizmoColor(int i) {
        Color color = Color.white;
        Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow };

        if (i < colors.Length) {
            color = colors[i];
        }

        return color;
    }


    #if UNITY_EDITOR

    Tile[] GetTiles() {
        List<Tile> connectables = new List<Tile>();
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject selectedObject in selectedObjects) {
            Tile tile = selectedObject.GetComponent<Tile>();
            if (tile != null) {
                connectables.Add(tile);
            }
        }

        return connectables.ToArray();
    }

    [ContextMenu("Gerar conexões")]
    void CreateConnections() {
        // Get selected objects
        List<Tile> connectables = new List<Tile>();
        
        foreach (Tile tile in GetTiles()) {
            connectables.Add(tile);
        }


        for (int i = 0; i < connectables.Count; i++) {
            Tile tile = connectables[i];
            Vector3 direction = tile.transform.position - transform.position;
            Direction directionEnum = Direction.None;

            if (direction.x > 0) {
                directionEnum = Direction.Right;
            } else if (direction.x < 0) {
                directionEnum = Direction.Left;
            } else if (direction.y > 0) {
                directionEnum = Direction.Up;
            } else if (direction.y < 0) {
                directionEnum = Direction.Down;
            }

            Connection connection = new Connection(tile, ConnectionType.Floor, directionEnum);
            connections.Add(connection);
        }
    }

    [ContextMenu("Gerar conexões int. horizontais")]
    void CreateConnectionsHorizontal() {
        Tile[] tiles = GetTiles();

        // order tiles by gameobject x
        System.Array.Sort(tiles, delegate(Tile tile1, Tile tile2) {
            return tile1.transform.position.x.CompareTo(tile2.transform.position.x);
        });

        Debug.Log(tiles.Length);

        for (int i = 0; i < tiles.Length - 1; i++) {
            Tile tile1 = tiles[i];
            Tile tile2 = tiles[i + 1];
            
            if (tile1.GetConnectionRaw(Direction.Right) == null) 
                ConnectTiles(tile1, tile2, ConnectionType.Floor, Direction.Right);
            if (tile2.GetConnectionRaw(Direction.Left) == null) 
                ConnectTiles(tile2, tile1, ConnectionType.Floor, Direction.Left);

            // Set dirty
            EditorUtility.SetDirty(tile1);
            EditorUtility.SetDirty(tile2);
        }
    }

    [ContextMenu("Gerar conexões int. verticais")]
    void CreateConnectionsVertical() {
        Tile[] tiles = GetTiles();

        // order tiles by gameobject y
        System.Array.Sort(tiles, delegate(Tile tile1, Tile tile2) {
            return tile1.transform.position.y.CompareTo(tile2.transform.position.y);
        });

        for (int i = 0; i < tiles.Length - 1; i++) {
            Tile tile1 = tiles[i];
            Tile tile2 = tiles[i + 1];
            
            if (tile1.GetConnectionRaw(Direction.Up) == null) 
                ConnectTiles(tile1, tile2, ConnectionType.Floor, Direction.Up);
            if (tile2.GetConnectionRaw(Direction.Down) == null) 
                ConnectTiles(tile2, tile1, ConnectionType.Floor, Direction.Down);

            // Set dirty
            EditorUtility.SetDirty(tile1);
            EditorUtility.SetDirty(tile2);
        }
    }

    [ContextMenu("Resetar conexões internas")]
    void ResetConnections() {
        Tile[] tiles = GetTiles();

        foreach (Tile tile in tiles) {
            tile.connections.Clear();

            EditorUtility.SetDirty(tile);
        }
    }

    void ConnectTiles(Tile receiving, Tile connector, ConnectionType connectionType, Direction direction) {
        Connection connection = new Connection(connector, connectionType, direction); 
        receiving.connections.Add(connection);
    }

    #endif
}
