﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public GameObject stoneDeposit;

	void Awake() {
	}
	
	void Update() {	
	}

	public enum eTeam {
		TEAM_1,
		TEAM_2
	}

	private static GameManager gm;
	public static GameManager Singleton() {
		if ( !gm ) {
			gm = FindObjectOfType<GameManager>();
		}

		return gm;
	}
}