using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class stuff : MonoBehaviour {
	
	public float speed;
	public float momentum = 0;
	public float jumpForce = 100.0f;
	public GameObject groundDetector;
	public GameObject wallDetector;
	public GameObject ceilingDetector;
	
	private GameObject[] TripleJumpPowerUps;
	private GameObject[] CeilingJumpPowerUps;
	private GameObject[] WallJumpPowerUps;
	
	
	private int jumpType = 0; //depending on which item the player is holding their jump type will change (0-3)
	private int jumpsRemaining = 1;
	
	public Quaternion q = new Quaternion(0, 0, 0, .5f);//stops our player from rotateing
	
	private Rigidbody rb; //This is our player
	
	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		wallDetector.SetActive (false);
		ceilingDetector.SetActive(false);
		
		//Creates arrays containing every power up of a specific type
		TripleJumpPowerUps = GameObject.FindGameObjectsWithTag("Triple Jump");
		CeilingJumpPowerUps = GameObject.FindGameObjectsWithTag("Ceiling Jump");
		WallJumpPowerUps = GameObject.FindGameObjectsWithTag("Wall Jump");
	}
	
	//Sets different stats of the player depending on the jump type
	//If we don't want manditory wall alternations for the wall jump
	void OnTriggerEnter(Collider other)
	{
		if(jumpType == 1)// && other.gameObject.CompareTag == "Ground") 
		{
			jumpsRemaining = 3; //Triple jump max jumps without touching the ground
		}
		else
		{
			jumpsRemaining = 1; //Every other jump types max jump
		}
		print ("Grounded");
		
		if (other.gameObject.CompareTag ("Triple Jump")) 
		{
			jumpType = 1;
			//other.gameObject.SetActive (false); //destoys the rotating power up
			wallDetector.SetActive(false);
			ceilingDetector.SetActive(false);
			rb.GetComponent<Renderer>().material.color = Color.green;
			//Disable all other Triple Jump Power Ups and enable all others
			toggleTripleJumpPowerUps(false);
			toggleCeilingJumpPowerUps(true);
			toggleWallJumpPowerUps(true);
		}
		
		if (other.gameObject.CompareTag ("Wall Jump")) 
		{
			jumpType = 2;
			//other.gameObject.SetActive (false);
			wallDetector.SetActive(true);
			ceilingDetector.SetActive(false);
			rb.GetComponent<Renderer>().material.color = Color.red;
			//Disable all other Triple Jump Power Ups and enable all others
			toggleTripleJumpPowerUps(true);
			toggleCeilingJumpPowerUps(true);
			toggleWallJumpPowerUps(false);
		}
		
		if (other.gameObject.CompareTag ("Ceiling Jump")) 
		{
			jumpType = 3;
			//other.gameObject.SetActive (false);
			wallDetector.SetActive(false);
			ceilingDetector.SetActive(true);
			rb.GetComponent<Renderer>().material.color = Color.yellow;
			//Disable all other Triple Jump Power Ups and enable all others
			toggleTripleJumpPowerUps(true);
			toggleCeilingJumpPowerUps(false);
			toggleWallJumpPowerUps(true);
		}
	}
	
	void toggleTripleJumpPowerUps(bool onOrOff)
	{
		for(int i = 0; TripleJumpPowerUps.Length > i; i++)
			TripleJumpPowerUps[i].SetActive (onOrOff);
	}
	
	void toggleCeilingJumpPowerUps(bool onOrOff)
	{
		for(int i = 0; CeilingJumpPowerUps.Length > i; i++)
			CeilingJumpPowerUps[i].SetActive (onOrOff);
	}
	
	void toggleWallJumpPowerUps(bool onOrOff)
	{
		for(int i = 0; WallJumpPowerUps.Length > i; i++)
			WallJumpPowerUps[i].SetActive (onOrOff);
	}
	
	void OnTriggerExit(Collider other)
	{
		print ("In the air");
	}
	
	void Update()
	{
		if (Input.GetButtonDown ("Jump")) 
		{
			whichJump(jumpType);
		}
	}
	
	//Holds the mechanics for each jump type
	//0 - Single jump
	//1 - tripple jump
	//2 - ceiling jump
	//3 - wall jump
	//Make two private GameObjects for the wall and ceiling jump detectors and make them false
	//When the correct power up is held set the additional detectors to true
	//Will probably put that in the item pick up code and change this to just one if/else block
	void whichJump(int jumpType)
	{
		if(jumpType == 0 && jumpsRemaining > 0)
		{
			jump ();
			jumpsRemaining--;
		}
		else if (jumpType == 1 && jumpsRemaining > 0)
		{
			jump ();
			jumpsRemaining--;
		}
		else if (jumpType == 2 && jumpsRemaining > 0)
		{
			jump ();
			jumpsRemaining--;
		}
		else if (jumpType == 3 && jumpsRemaining > 0)
		{
			jump ();
			jumpsRemaining--;
		}
	}
	
	void jump()
	{
		rb.velocity = new Vector3(0, jumpForce, 0);
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
