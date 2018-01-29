using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketScript : MonoBehaviour
{
    public TileScript currentTile;
    int id;
    Material color;
    GameObject tile;

    Vector3 start, end;
    Vector2 destination;
    int nextTileInput;
    float time = 0;

    bool isReset = false;
    [FMODUnity.EventRef]
    public string dataTransfering = "event:/Data Transfering";
    FMOD.Studio.EventInstance transferEv;
    [FMODUnity.EventRef]
    public string recieveData = "event:/Recieve Data Packet";
    [FMODUnity.EventRef]
    public string spawnPacket = "event:/Spawn Data Packet";
    
    void Start()
    {
        transferEv = FMODUnity.RuntimeManager.CreateInstance(dataTransfering);
        FMODUnity.RuntimeManager.PlayOneShot(spawnPacket, transform.position);
    }
    public void getDestroyed()
    {
        
        Destroy(gameObject);
    }

    public void move(Vector3 start, Vector3 end, Vector2 destinationTile, int nextTileInput, TileScript currentTile)
    {
        this.currentTile = currentTile;
        time = 0;
        this.start = start;
        this.end = end;
        this.nextTileInput = nextTileInput;
        destination = destinationTile;
        transferEv.start();
        StartCoroutine("movePacket");
        Invoke("goToNextTile", 1f);
    }

    IEnumerator movePacket()
    {
        //lerp
        while (true)
        {
            time += Time.deltaTime;
            gameObject.transform.position =  Vector3.Lerp(transform.position, end, time);
            yield return null;
        }
    }

    public void moveUp(Vector3 offset) 
    {
        start += offset;
        end += offset;
        destination.y--;
        Debug.Log(destination.y);
        if (destination.y < 0)
        {
            destination.y = 4;
        }
    }

    private void goToNextTile()
    {
        StopCoroutine("movePacket");
        transferEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        time = 0;
        currentTile.removePacket(gameObject);
        if (isReset)
        {
            isReset = false;
            GameObject.Find("GameManager").GetComponent<GameManagerScript>().resetPacket(gameObject);
        }
       // Debug.Log("GO TO NEXT TILE: " + destination + " " + gameObject + " " + nextTileInput);
        GameObject.Find("GameManager").GetComponent<GameManagerScript>().placeInTile(destination, gameObject, nextTileInput);
        //go to tile
    }


}
