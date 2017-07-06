using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraFollow : MonoBehaviour {

	public GameObject player;
	public int offsetH = 12;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = new Vector3 (0, offsetH, 0);//transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = player.transform.position + (new Vector3 (0, offsetH, 0));
	}
}
