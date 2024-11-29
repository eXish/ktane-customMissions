using System;
using System.Reflection;
using UnityEngine;

public class EXMissionServiceScript : MonoBehaviour {

	public KMGameInfo game;

    KeyCode[] eToExplodeKeys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9, KeyCode.Keypad0, KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow };
	string currentMission;

    bool dontDoAgain;

    void Awake()
    {
        game.OnStateChange += state => {
            currentMission = GetMissionID();
        };
    }

	void Update () {
        if (currentMission == "mod_eXishMissions_etoexplode")
        {
            foreach (KeyCode k in eToExplodeKeys)
            {
                if (Input.GetKeyDown(k))
                    Detonate();
            }
        }
        else if (currentMission == "mod_eXishMissions_elevatorbomb")
        {
            if (dontDoAgain)
                return;
            if (GameObject.Find("ElevatorBomb(Clone)") == null)
                Application.Quit();
            dontDoAgain = true;
        }
        else
            dontDoAgain = false;
    }

    void Detonate()
    {
        var bomb = ReflectionHelper.FindGameType("Bomb");
        ReflectionHelper.CallMethod(bomb, "Detonate", FindObjectOfType(bomb));
    }

    string GetMissionID()
    {
        try
        {
            Component gameplayState = GameObject.Find("GameplayState(Clone)").GetComponent("GameplayState");
            Type type = gameplayState.GetType();
            FieldInfo fieldMission = type.GetField("MissionToLoad", BindingFlags.Public | BindingFlags.Static);
            return fieldMission.GetValue(gameplayState).ToString();
        }

        catch (NullReferenceException)
        {
            return "undefined";
        }
    }
}
