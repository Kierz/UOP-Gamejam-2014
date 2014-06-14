/*
	File:			GameManager.cs
	Author:			Krz, Dan, Zack
	Project:		Curling Game
	Soundtrack:		Station 90 Show 13: Simon Heartfield and Manni Dee
	Description:	Manages the state of the game.
*/

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static float volume = CheckVolume();
    private static int team1score = 0, team2score = 0;
    public GameObject stonesDeposit;
    public Camera playerCam;
    public Camera rockCam;
    public Camera bullseyeCam;
    private eGameState mGameState;
    private Player player;
	private static int roundCounter;
    public Vector3 BACK_OF_HOUSE_POSITION;
    public Vector3 GUARD_LINE_POSITION;
    public GUIText hudDisqualified;
    public GUIText hudResetPosition;
    public GUIText hudBrushNow;

    public enum eGameState {
        ePlayer = 0,
        eRock,
        eBullseye
    }

    void Awake() {
        Screen.showCursor =         false;
		ChangeState                 (eGameState.ePlayer);
        player =                    FindObjectOfType<Player>();
        roundCounter =              1;

        BACK_OF_HOUSE_POSITION =    GameObject.FindGameObjectWithTag("BackOfHouse").transform.position;
        GUARD_LINE_POSITION =       GameObject.FindGameObjectWithTag("GuardLine").transform.position;
	}

    public void ChangeState(eGameState state) {
        mGameState = state;

        // disable all cameras
        rockCam.enabled =       false;
        playerCam.enabled =     false;
        bullseyeCam.enabled =   false;
        HUDBrushNow(false);

        if (state == eGameState.ePlayer) {
            playerCam.enabled = true;
        }

        if (state == eGameState.eRock) {
            rockCam.enabled =   true;
            HUDBrushNow(true);
        }

        if (state == eGameState.eBullseye) {
            // i dont think this ever gets used =[ -Krz
            EndOfRound();
            bullseyeCam.enabled = true;
        }
    }

	private static float CheckVolume() {
		if ( PlayerPrefs.HasKey("volume") ) {
			volume = PlayerPrefs.GetFloat( "volume" );
			return volume;
		} else {
			volume = 1.0f;
		}

		return volume;
	}

	void Update() {
		if ( Input.GetKeyUp( KeyCode.Escape ) ) {
			Application.LoadLevel( "mainMenu" );
		}
	}

	public enum eTeam {
		TEAM_RED,
		TEAM_BLUE
	}

	private static GameManager gm;
	public static GameManager Singleton() {
		if ( !gm ) {
			gm = FindObjectOfType<GameManager>();
		}

		return gm;
	}

    public void UpdateScores() {
        eTeam winningTeam = GetRoundWinner();
        GivePoints(winningTeam, GetEnemyClosestToBullseye(winningTeam) );
        Debug.Log("Game Over");
        Debug.Log(winningTeam + "won the game");
    }

    private void EndOfRound() {
        UpdateScores();
		roundCounter++;
		print ("Round number: " + roundCounter.ToString ());
		Application.LoadLevel ("EndOfRound");
    }

    private eTeam GetRoundWinner() {
        eTeam winningTeam = 0;
        float winningDistance = 99999.9f;

        foreach ( Rock rock in FindObjectsOfType<Rock>() ) {
            if ( rock.DistanceFromBullseye() < winningDistance ) {
                winningTeam = rock.team;
                winningDistance = rock.DistanceFromBullseye();
            }
        }

        return winningTeam;
    }

    private float GetEnemyClosestToBullseye( eTeam winningTeam ) {
        float closestToBullseye = 99999.9f;
        foreach ( Rock rock in FindObjectsOfType<Rock>() ) {
            if ( rock.team != winningTeam ) {
                if ( rock.DistanceFromBullseye() < closestToBullseye ) {
                	closestToBullseye = rock.DistanceFromBullseye();
                }
            }
        }

        return closestToBullseye;
    }

	private void GivePoints( eTeam winningTeam, float enemyDistanceFromBullseye ) {
        int points = 0;

        foreach ( Rock rock in FindObjectsOfType<Rock>() ) {
            if ( rock.team == winningTeam ) {
                if ( rock.DistanceFromBullseye() < enemyDistanceFromBullseye ) {
                    points++;
                }
            }
        }

        GiveWinningTeamPoints( winningTeam, points );
    }

    private void GiveWinningTeamPoints( eTeam team, int points ) {
        switch ( team ) {
            case eTeam.TEAM_RED:
            {
                team1score += points;
                break;
            }

            case eTeam.TEAM_BLUE:
            {
                team2score += points;
                break;
            }
        }
    }

	public static float GetVolume() {
		return volume;
	}

    public static void SetVolume(float newVolume) {
		volume = newVolume / 100;
		PlayerPrefs.SetFloat( "volume", volume );
		PlayerPrefs.Save();
	}

	public static int[] GetScore() {
		int[] scores = {team1score, team2score};
		return scores;
	}

    public bool IsTeamOne() {
        return (FindObjectOfType<Player>().team == eTeam.TEAM_RED);
    }

    public bool IsTeamTwo() {
        return (FindObjectOfType<Player>().team == eTeam.TEAM_BLUE);
    }

	public static int TeamOneStonesLeft() {
        int i = 0;

        foreach (Rock stone in FindObjectsOfType<Rock>()) {
			if ((stone.IsPickedUp() || stone.InSupply()) && stone.team == GameManager.eTeam.TEAM_RED) {
				i++;
			}
        }

        return i;
    }

	public static int TeamTwoStonesLeft() {
        int i = 0;

        foreach (Rock stone in FindObjectsOfType<Rock>()) {
			if ((stone.IsPickedUp() || stone.InSupply()) && stone.team == GameManager.eTeam.TEAM_BLUE) {
                i++;
            }
        }

        return i;
    }

    public void SetFriction(float friction) {
        player.SetFriction(friction);
    }

    public eGameState GetGameState() {
        return mGameState;
    }

	//Added by Aidan
	public static int GetRoundNumber() {
		return roundCounter;
	}

    public void HUDResetPosition() {
        //turn hud reset position on for half a second
        if (!hudResetPosition.guiText.enabled) {
            hudResetPosition.guiText.enabled = true;
            StartCoroutine(HUDResetPositionCont());
        }
    }

    public IEnumerator HUDResetPositionCont() {
        yield return new WaitForSeconds(1);

        hudResetPosition.guiText.enabled = false;
    }

    public void HUDDisqualified() {
        //tell player disqualified they passed the hogline
        if (!hudDisqualified.guiText.enabled) {
            hudDisqualified.guiText.enabled = true;
            StartCoroutine(HUDDisqualifiedCont());
        }
    }

    public IEnumerator HUDDisqualifiedCont() {
        yield return new WaitForSeconds(1);

        hudDisqualified.guiText.enabled = false;
    }

    private void HUDBrushNow(bool setting) {
        //tell player to brush now by moving the mouse up and down fast
        hudBrushNow.guiText.enabled = setting;
    }
}