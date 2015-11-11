using UnityEngine;
using System.Collections;

public class GoToMainMenu : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag.Equals ("Player"))
		{
			Application.LoadLevel("Main Menu");
		}
	}
}
