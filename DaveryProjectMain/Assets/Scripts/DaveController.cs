using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaveController : MonoBehaviour {

    public Transform FirePoint;
    public GameObject bulletPrefab;

    private int thisCharId;
    private GameObject UI;
    private LevelManager level;
    private int currentCharId;

    private void Start()
    {
        UI = GameObject.Find("UI");
        thisCharId = GetComponent<CharacterController>().thisCharId;
    }
    // Update is called once per frame
    void Update () {
        level = UI.GetComponent<LevelManager>();
        currentCharId = level.currentCharId;

        if (currentCharId == thisCharId)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
	}

    void Shoot()
    {
        Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);
    }
}
