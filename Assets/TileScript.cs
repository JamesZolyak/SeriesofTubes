using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class TileScript : MonoBehaviour {

    public GameObject[] anchors;
    public Vector2 position;
    float animTime;
    Vector3 end;
   
    int animCount = 0;
    public List<GameObject> packets = new List<GameObject>();

    [FMODUnity.EventRef]
    public string conveyerAnim = "event:/Move Conveyer";
    
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    public void startAnim( Vector3 end)
    {
        Vector3 offset = end - gameObject.transform.position;
        foreach(GameObject p in packets)
        {
            p.GetComponent<PacketScript>().moveUp(offset);
        }
        animCount++;
        this.end = end;
        FMODUnity.RuntimeManager.PlayOneShot(conveyerAnim, transform.position);
        StartCoroutine("moveTile");
        Invoke("stopAnim", 0.5f);
    }

    public void stopAnim()
    {
        animCount--;
        if(animCount == 0)
        {
            animTime = 0;
            StopCoroutine("moveTile");
        }
    }

    IEnumerator moveTile()
    {
        //lerp
        while (true)
        {
            animTime += Time.deltaTime*2;
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, end, animTime);
            yield return null;
        }
    }



    public void removePacket(GameObject packet)
    {
        packets.Remove(packet);
    }

    public  void GetPacket(int anchor, GameObject packet)
    {
        packets.Add(packet);
        packet.transform.position = anchors[anchor].transform.position;
        packet.transform.parent = gameObject.transform;
        int outPutAnchor = getOutputAnchor(anchor);
        if (outPutAnchor == -1)
        {
            GameObject.Find("GameManager").GetComponent<GameManagerScript>().resetPacket(packet);
            return;
        }
        Vector3 startPoint = anchors[anchor].transform.position;
        Vector3 endPoint = anchors[outPutAnchor].transform.position;
        Vector2 nextTile = getOutputTile(anchor);
        int nextTileInputAnchor = getNextInputAnchor(outPutAnchor);
        Debug.Log("position: " + position.x + "," + position.y + " next tile: " + nextTile + " " + nextTileInputAnchor);
        packet.GetComponent<PacketScript>().move(startPoint, endPoint, nextTile, nextTileInputAnchor, this);
    }
    public abstract int getOutputAnchor(int inputAnchor);
    public abstract Vector2 getOutputTile(int inputAnchor);

    public void setPosition(Vector2 position)
    {
        this.position = position;
    }

    public int getNextInputAnchor(int outputAnchor)
    {
        switch (outputAnchor)
        {
            case 0:
                return 2;
            case 1:
                return 3;
            case 2:
                return 0;
            case 3:
                return 1;
            default:
                return -1;
        }
    }

    public void clearPackets()
    {
        packets.Clear();
    }
    
}
