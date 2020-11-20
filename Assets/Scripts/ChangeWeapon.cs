using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWeapon : MonoBehaviour {

	public GameObject currentWep;
	private GameObject currentWepCopy;
	public GameObject[] allWeapons;

	public Transform hand;

	private int index;
	private int lastIndex;

	// Use this for initialization
	void Start () {
		index = 0;
		currentWep = allWeapons [index];
		//PlayerManager.Instance.currentWep = currentWep;
		currentWepCopy = GameObject.Instantiate (currentWep, hand);
        PlayerManager.Instance.currentWep = currentWepCopy;
	}
	
	// Update is called once per frame
	void Update () {

		if (!PauseMenu.isPaused)
		{
			if (Input.GetButtonDown("NextWep"))
			{
				index++;
				if (index >= allWeapons.Length)
					index = 0;
			}
			else if (Input.GetButtonDown("PrevWep"))
			{
				index--;
				if (index < 0)
					index = allWeapons.Length - 1;
			}

			if (index != lastIndex)
			{
				GameObject.Destroy(currentWepCopy);
				currentWep = allWeapons[index];
				//PlayerManager.Instance.currentWep = currentWep;
				SpawnWeapon();
			}

			lastIndex = index;
		}
	}

	public void SpawnWeapon()
    {
		currentWepCopy = GameObject.Instantiate(currentWep, hand);
		PlayerManager.Instance.currentWep = currentWepCopy;
	}
}
