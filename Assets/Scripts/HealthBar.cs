using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public GameObject currentHealthBar;
	public GameObject currentHealthText;
	public GameObject healthLoss;

	private float width;
	private float lossWidth;
	private float lossWait = 0.6f;
	private float lossStep = 0.1f;
	private bool losingHealth = false;

	// Use this for initialization
	void Start () {
		width = 1.0f;
		lossWidth = width;
	}
	
	// Update is called once per frame
	void Update () {
		float currentHealth = PlayerManager.Instance.currentHealth;
		float maxHealth = PlayerManager.Instance.maxHealth;
		width = currentHealth / maxHealth;
		Vector3 newScale = new Vector3 (width, currentHealthBar.transform.localScale.y, currentHealthBar.transform.localScale.z);
		currentHealthBar.transform.localScale = newScale;

		if (width != lossWidth && !losingHealth) {
			losingHealth = true;
			StartCoroutine (HealthLoss());
			healthLoss.GetComponent<Image> ().enabled = true;
		}

		currentHealthText.GetComponent<Text> ().text = ((int)PlayerManager.Instance.currentHealth).ToString();
	}

	//waits for a little bit then gradually shows health being lost
	private IEnumerator HealthLoss(){
		yield return new WaitForSeconds(lossWait);
		while(lossWidth >= width){
			Vector3 newScale = new Vector3 (lossWidth, healthLoss.transform.localScale.y, healthLoss.transform.localScale.z);
			healthLoss.transform.localScale = newScale;
			yield return new WaitForSeconds(lossStep);
			lossWidth -= 0.01f;
		}
		lossWidth = width;
		losingHealth = false;
		healthLoss.GetComponent<Image> ().enabled = false;
	}
}
