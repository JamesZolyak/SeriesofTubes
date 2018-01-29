using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightTileScript : TileScript {


   


    public override Vector2 getOutputTile(int inputAnchor)
    {
        switch (inputAnchor)
        {
            case 3:
                return new Vector2(position.x + 1, position.y);
            case 1:
                return new Vector2(position.x - 1, position.y);
            default:
                return new Vector2(-1,-1);
        }
    }

    public override int getOutputAnchor(int inputAnchor)
    {
        switch (inputAnchor)
        {
            case 3:
                return 1;
            case 1:
                return 3;
            default:
                return -1;
        }
    }


}
