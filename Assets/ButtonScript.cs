using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

    public int col;
    [FMODUnity.EventRef]
    public string inputSound = "event:/Click Arrow";

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        GameObject.Find("GameManager").GetComponent<GameManagerScript>().shiftColUp(col);
        FMODUnity.RuntimeManager.PlayOneShot(inputSound, transform.position);

    }
}
