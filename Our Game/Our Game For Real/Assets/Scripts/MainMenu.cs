using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	//public string startLevel;
	//public string levelSelect;

	public void newGame()
	{
		Application.LoadLevel ("1-1");
	}

	public void levelSelect()
	{
		Application.LoadLevel ("Level Select");
	}

	public void quitGame()
	{
		Application.Quit ();
	}
}
