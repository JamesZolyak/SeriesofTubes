using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagonalDownScript : TileScript {

    


    public override Vector2 getOutputTile(int inputAnchor)
    {
        switch (inputAnchor)
        {
            case 0:
                return new Vector2(position.x + 1, position.y);
            case 1:
                return new Vector2(position.x , position.y -1);
            case 2:
                return new Vector2(position.x-1, position.y );
            case 3:
                return new Vector2(position.x , position.y+1);
            default:
                return new Vector2(-1, -1);
        }
    }

    public override int getOutputAnchor(int inputAnchor)
    {
        switch (inputAnchor)
        {
            case 0:
                return 1;
            case 1:
                return 0;
            case 2:
                return 3;
            case 3:
                return 2;
            default:
                return -1;
        }
    }


}
