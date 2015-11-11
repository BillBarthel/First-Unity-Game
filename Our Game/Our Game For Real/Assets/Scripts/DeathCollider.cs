using UnityEngine;
using System.Collections;

public class DeathCollider : MonoBehaviour {

	private string worldNumber;
	private string levelNumber;

	// Use this for initialization
	void Start () 
	{
		getWorldAndLevelNumber();
	}

	//Destroy(this.gameObject); destroys the object this scipt is attatched to upon a collider overlapping it
	//Destroy(other.gameObject); destroys the object that is coming into contact with the object that uses this script
	void OnTriggerEnter(Collider other)
	{
		//Currently the level gets reset when the player comes in contact with an object with this script
		//We could have the game to reset to a check point or bring up a menu screen
		if(other.gameObject.tag.Equals ("Player"))
		{
			Application.LoadLevel(worldNumber + "-" + levelNumber);
		}
	}

	//Only works with scenes with the nameing convention "worldNumber-levelNumber".  Ex. "1-1"
	void getWorldAndLevelNumber()
	{
		string[] level = Application.loadedLevelName.Split ('-');
		worldNumber = level[0];
		levelNumber = level[1];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
