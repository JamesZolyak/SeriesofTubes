using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {
    public GameObject[,] tiles;
    public GameObject straightTilePrefab, diagonalDownTilePrefab;
    public GameObject packetPrefab;
    const int COLS = 5;
    const int ROWS = 5;

    [FMODUnity.EventRef]
    public string musicReference = "event:/Music";
    FMOD.Studio.EventInstance musicEv;
    [FMODUnity.EventRef]
    public string recieveData = "event:/Recieve Data Packet";
    // Use this for initialization
    void Start () {
        tiles = new GameObject[COLS, ROWS];
        for (int col = 0; col < COLS; col++)
        {
            for(int row = 0; row < ROWS; row++)
            {
                int rand = Mathf.FloorToInt(Random.value * 2);

                GameObject tile = (GameObject)Instantiate((rand == 0?diagonalDownTilePrefab: straightTilePrefab), new Vector3(col * 0.35f, row * -0.35f, 0), Quaternion.identity);
                tiles[col, row] = tile;
                tiles[col, row].GetComponent<TileScript>().setPosition(new Vector2(col, row));
            }
        }
        
       
        GameObject packet = Instantiate(packetPrefab, new Vector3(-1 - 1, 0), Quaternion.identity);
        tiles[0, 0].GetComponent<TileScript>().GetPacket(3, packet);
        GameObject packet2 = Instantiate(packetPrefab, new Vector3(-1 - 1, 0), Quaternion.identity);
        tiles[0, 2].GetComponent<TileScript>().GetPacket(3, packet2);
        GameObject packe3 = Instantiate(packetPrefab, new Vector3(-1 - 1, 0), Quaternion.identity);
        tiles[0, 3].GetComponent<TileScript>().GetPacket(3, packe3);
        musicEv = FMODUnity.RuntimeManager.CreateInstance(musicReference);
        musicEv.start();
    }
	
	public void shiftColUp(int col)
    {
        GameObject[] tempArr = new GameObject[ROWS];
        for(int i = 0; i < ROWS; i++)
        {
            if (i == ROWS -1)
            {
                
                tempArr[ROWS - 1] = tiles[col, 0];
            }else
            {
                tempArr[i] = tiles[col,i + 1];
            }
        }
        for(int i = 0; i < ROWS; i++)
        {
            tiles[col, i] = tempArr[i];
            tiles[col, i].GetComponent<TileScript>().setPosition(new Vector2(col, i));
            tiles[col, i].GetComponent<TileScript>().startAnim(calculatePosition(col, i));
            //tiles[col, i].transform.position = calculatePosition(col, i);
        }
    }

    private Vector3 calculatePosition(int col, int row)
    {
        return new Vector3(col * 0.35f, row * -0.35f, 0);
    }

    public void placeInTile(Vector2 tile, GameObject packet, int input)
    {
        //Debug.Log(((int)tile.x).ToString() +"," + ((int)tile.y).ToString());

        foreach(GameObject t in tiles)
        {
            t.GetComponent<TileScript>().clearPackets();
        }

        if(tile.x >= COLS || tile.y >= ROWS)
        {
            resetPacket(packet);
        }else
        {
            tiles[(int)tile.x, (int)tile.y].GetComponent<TileScript>().GetPacket(input, packet);
        }
    }

    public void resetPacket(GameObject packet)
    {
        FMODUnity.RuntimeManager.PlayOneShot(recieveData, transform.position);
        tiles[0, Mathf.FloorToInt(Random.Range(0,4))].GetComponent<TileScript>().GetPacket(3, packet);
    }
}
