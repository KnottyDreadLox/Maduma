using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu (fileName = "AiNode", menuName = "TileGenerator/AiNode")]
public class AiNode : ScriptableObject {


    public string NodeName;

    public TileBase tile;

    public TileConnection Top;
    public TileConnection Bottom;
    public TileConnection Left;
    public TileConnection Right;

}

[System.Serializable]
public class TileConnection
{
    public List<AiNode> CompatibleNodes = new List<AiNode>();
}