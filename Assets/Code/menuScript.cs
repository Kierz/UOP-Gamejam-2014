﻿using UnityEngine;
using System.Collections;

public class menuScript : MonoBehaviour {

	public string itemName;
	public GameObject menuOutline;
	public AudioClip mouseOver;
	public AudioClip mouseClick;
	public AudioClip mouseBack;

	// Use this for initialization
	void Start () {
		if ( PlayerPrefs.GetFloat("volume") < 0.0f )
		{
			GameManager.setVolume( 1.0f );
			Debug.Log ("Volume was reset");
		}

		AudioListener.volume = GameManager.getVolume();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseUp()
	{
		if ( itemName == "start game" )
		{
			Debug.Log("Start Game");
			AudioSource.PlayClipAtPoint(mouseClick, transform.position);
			Application.LoadLevel("dansTest");

		}

		else if ( itemName == "options" )
		{
			Debug.Log("Options");
			AudioSource.PlayClipAtPoint(mouseClick, transform.position);
			Application.LoadLevel("optionsMenu");
		}

		else if ( itemName == "credits" )
		{
			Debug.Log("Credits");
			AudioSource.PlayClipAtPoint(mouseClick, transform.position);
			Application.LoadLevel("creditsMenu");
		}

		else if ( itemName == "exit" )
		{
			Debug.Log("Exit");
			AudioSource.PlayClipAtPoint(mouseClick, transform.position);
			Application.Quit();
		}

		else if ( itemName == "back" )
		{
			Debug.Log("back");
			AudioSource.PlayClipAtPoint(mouseClick, transform.position);
			Application.LoadLevel("mainMenu");
		}
	}

	//Mouse enter function
	void OnMouseEnter()
	{
		if ( itemName == "start game" )
		{
			Instantiate( menuOutline, transform.position, transform.rotation );
		}
		
		else if ( itemName == "options" )
		{
			Instantiate( menuOutline, transform.position, transform.rotation );
		}
		
		else if ( itemName == "credits" )
		{
			Instantiate( menuOutline, transform.position, transform.rotation );
		}
		
		else if ( itemName == "exit" )
		{
			Instantiate( menuOutline, transform.position, transform.rotation );
		}

		else if ( itemName == "back" )
		{
			Instantiate( menuOutline, transform.position, transform.rotation );
		}

		AudioSource.PlayClipAtPoint(mouseOver, transform.position);
	}

	void OnMouseExit()
	{
		Destroy( GameObject.FindWithTag("menuOutline") );
	}	
}
