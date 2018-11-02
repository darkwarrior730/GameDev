using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineController : MonoBehaviour {

    Cinemachine.CinemachineVirtualCamera followCam;
    CharacterController charScript;
    private GameObject UI;
    private LevelManager level;

    void Start () {
        followCam = gameObject.GetComponent<Cinemachine.CinemachineVirtualCamera>();
        UI = GameObject.Find("UI");
    }
	
	void Update () {

        var characters = GameObject.FindGameObjectsWithTag("Player");
        level = UI.GetComponent<LevelManager>();

        foreach (var character in characters)
        {
            charScript = character.GetComponent<CharacterController>();
            if (charScript.thisCharId == level.currentCharId)
            {
                followCam.Follow = character.transform;
            }
        }
    }
}
