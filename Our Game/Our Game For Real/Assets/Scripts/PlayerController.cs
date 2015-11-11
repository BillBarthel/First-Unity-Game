using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb; //This is our player
	private Vector3 checkpoint;
	private Quaternion q = new Quaternion(0, 0, 0, .5f);//stops our player from rotateing
	//private bool isPaused = false;
	//private PauseMenu pauseMenu;

	private string worldNumber;
	private string levelNumber;
	
	public float speed;
	public float momentum = 0;
	public float jumpForce = 100.0f;
	public GameObject groundDetector;
	public GameObject wallDetector;
	public GameObject ceilingDetector;

	private GameObject[] TripleJumpPowerUps;
	private GameObject[] CeilingJumpPowerUps;
	private GameObject[] WallJumpPowerUps;
	
	//jumpType == 0 - Only basic jump
	//jumpType == 1 - Triple jump upgrade is activated
	//jumpType == 2 - Wall jump upgrade is activated
	//jumpType == 3 - Ceiling jump upgrade is activated
	private int jumpType = 0;
	private int jumpsRemaining = 1;
	
	void Start ()
	{
		getWorldAndLevelNumber(); //Breaks the game if not used in a scene titled with the convenction "1-1"
		//need setters and getters to update the pause menu with the correct level
		//pauseMenu = new PauseMenu();
		
		rb = GetComponent<Rigidbody>();
		checkpoint = rb.position;
		wallDetector.SetActive (false);
		ceilingDetector.SetActive(false);
		
		//Creates arrays containing every power up of a specific type
		TripleJumpPowerUps = GameObject.FindGameObjectsWithTag("Triple Jump");
		CeilingJumpPowerUps = GameObject.FindGameObjectsWithTag("Ceiling Jump");
		WallJumpPowerUps = GameObject.FindGameObjectsWithTag("Wall Jump");
	}

	//Only works with scenes with the nameing convention "worldNumber-levelNumber".  Ex. "1-1"
	void getWorldAndLevelNumber()
	{
		string[] level = Application.loadedLevelName.Split ('-');
		worldNumber = level[0];
		levelNumber = level[1];
	}
	
	//Sets different stats of the player depending on the jump type
	//If we don't want manditory wall alternations for the wall jump
	void OnTriggerEnter(Collider other)
	{
		print ("trigger entered");
		//If we continue to build this game and add extra power ups
		//use this function to more efficiently toggle multiple power ups.
		//Probably use a HashTable
		//togglePowerUps (other.gameObject.tag);

		//If the player collects a Triple Jump power up
		if (other.gameObject.tag.Equals("Triple Jump")) 
		{
			jumpType = 1;
			wallDetector.SetActive(false);
			ceilingDetector.SetActive(false);
			rb.GetComponent<Renderer>().material.color = Color.green;
			//Disable all other Triple Jump Power Ups and enable all others
			toggleTripleJumpPowerUps(false);
			toggleCeilingJumpPowerUps(true);
			toggleWallJumpPowerUps(true);
		}

		//If the player collects a Wall Jump power up
		if (other.gameObject.tag.Equals("Wall Jump")) 
		{
			jumpType = 2;
			wallDetector.SetActive(true);
			ceilingDetector.SetActive(false);
			rb.GetComponent<Renderer>().material.color = Color.red;
			//Disable all other Triple Jump Power Ups and enable all others
			toggleTripleJumpPowerUps(true);
			toggleCeilingJumpPowerUps(true);
			toggleWallJumpPowerUps(false);
		}

		//If the player collects a Ceiling Jump power up
		if (other.gameObject.tag.Equals("Ceiling Jump")) 
		{
			jumpType = 3;
			wallDetector.SetActive(false);
			ceilingDetector.SetActive(true);
			rb.GetComponent<Renderer>().material.color = Color.yellow;
			//Disable all other Triple Jump Power Ups and enable all others
			toggleTripleJumpPowerUps(true);
			toggleCeilingJumpPowerUps(false);
			toggleWallJumpPowerUps(true);
		}

		//If the player comes into contact with an object that will kill them or if they fall off the map
		//Any aquired power ups are disabled
		if (other.gameObject.tag.Equals ("Player Dies"))
		{
			rb.position = checkpoint;
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			rb.GetComponent<Renderer>().material.color = Color.blue;
			jumpType = 0;
			jumpsRemaining = 1;
			toggleTripleJumpPowerUps(true);
			toggleCeilingJumpPowerUps(true);
			toggleWallJumpPowerUps(true);
			wallDetector.SetActive(false);
			ceilingDetector.SetActive(false);
		}

		//If the player collects an item that completes the level
		if(other.gameObject.tag.Equals("End Level"))
		{
			if(int.Parse (levelNumber) != 20) //Assuming we have 20 levels per world
			{
				int nextLevel = int.Parse (levelNumber);
				nextLevel++;
				Application.LoadLevel(worldNumber + "-" + nextLevel.ToString());
			}
			else if(int.Parse (levelNumber) != 2) //Assuming we have 2 worlds
			{
				int nextWorld = int.Parse (worldNumber);
				nextWorld++;
				Application.LoadLevel(nextWorld.ToString() + "-1");
			}
			else
			{
				Application.Quit (); //Game complete!  Should add in some GUI for this
			}
		}

		if(other.gameObject.tag.Equals ("GoToMainMenu"))
		{
			Application.LoadLevel("Main Menu");
		}
		changeBasicJumps(other);
	}
	
	//	void togglePowerUps(string powerUpType)
	//	{
	//		if(powerUpType.Equals ("Triple Jump")
	//		   {
	//
	//		   }
	//	}
	
	//Determines how many basic jumps the player has
	void changeBasicJumps(Collider other)
	{
		//Fixes a bug where the player would currently have triple jump activated
		//and only used one of their jumps while jumping into a different power up.
		//The user should lose the ability to jump more than once after loseing 
		//triple jump but that doesn't take affect until the player touches the ground.
		if(jumpType == 1 && jumpsRemaining < 3)
		{
			jumpsRemaining = 0;
		}
		
		//The if statements fix a bug where the user would be able to reset their basic
		//jumps by colliding with a power up when they should only be able to reset their
		//basic jumps by touching the ground.
		if(jumpType == 1 && !other.gameObject.CompareTag ("Triple Jump")
		   && !other.gameObject.CompareTag ("Wall Jump")
		   && !other.gameObject.CompareTag ("Ceiling Jump")
		   && !other.gameObject.CompareTag ("End Level"))
		{
			jumpsRemaining = 3; //Triple jump max jumps without touching the ground
		}
		else if(!other.gameObject.CompareTag ("Triple Jump")
		        && !other.gameObject.CompareTag ("Wall Jump")
		        && !other.gameObject.CompareTag ("Ceiling Jump")
		        && !other.gameObject.CompareTag ("End Level"))
		{
			jumpsRemaining = 1; //Every other jump types max jump
		}
	}
	
	//Turns the Triple Jump Power Ups in the level off or on
	void toggleTripleJumpPowerUps(bool onOrOff)
	{
		for(int i = 0; TripleJumpPowerUps.Length > i; i++)
			TripleJumpPowerUps[i].SetActive (onOrOff);
	}
	
	//Turns the Ceiling Jump Power Ups in the level off or on
	void toggleCeilingJumpPowerUps(bool onOrOff)
	{
		for(int i = 0; CeilingJumpPowerUps.Length > i; i++)
			CeilingJumpPowerUps[i].SetActive (onOrOff);
	}
	
	//Turns the Wall Jump Power Ups in the level off or on
	void toggleWallJumpPowerUps(bool onOrOff)
	{
		for(int i = 0; WallJumpPowerUps.Length > i; i++)
			WallJumpPowerUps[i].SetActive (onOrOff);
	}
	
	void OnTriggerExit(Collider other)
	{
		
	}
	
	//Is called once per frame
	void Update()
	{
		//if(Input.GetButtonDown ("escape") && isPaused == false)
		//{
		//	Time.timeScale = 0;
		//	isPaused = true;
		//}
		//else
		//{
		//	Time.timeScale = 1;
		//	isPaused = false;
		//}

		if (Input.GetButtonDown ("Jump")) 
		{
			jump ();
		}
	}
	
	void jump()
	{
		if (jumpsRemaining > 0)
		{
			rb.velocity = new Vector3(0, jumpForce, 0);
			jumpsRemaining--;
		}
	}
	
	void FixedUpdate ()
	{
		//baisic player movement
		//momentum = momentum + 0.01f;
		if(momentum > .06f)
		{
			momentum = 0.06f;
		}
		float moveHorizontal = Input.GetAxis ("Horizontal");
		Vector2 movement = new Vector2 (moveHorizontal, 0.0f);
		rb.AddForce (movement * (speed + momentum));
		
		//stop our player from rotateing 
		rb.transform.eulerAngles = q.eulerAngles;
	}
}