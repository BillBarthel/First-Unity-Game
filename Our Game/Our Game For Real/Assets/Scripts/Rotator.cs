using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	//rotateSpeed allows you to change the speed an object rotates from within
	//the game making screen
	public float rotateSpeed = 30;

	// Update is called once per frame
	void Update () 
	{
		transform.Rotate (new Vector3 (15, rotateSpeed, 45) * Time.deltaTime);
	}
}