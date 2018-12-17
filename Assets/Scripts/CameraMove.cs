using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    public Transform target;
	public Vector3 offset;
	public bool invertY = true;

	public Transform pivot;

	public float rotateSpeed = 1.0f;

	public float maxViewAngle = 45f;
	public float minViewAngle = -45f;

	private float cameracorrect = 0.5f;

	// Use this for initialization
	void Start () {
		offset = new Vector3 (0f, -2.5f, 5f);

		pivot.transform.position = target.transform.position;
		pivot.transform.parent = null;

		Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void LateUpdate () {

		pivot.transform.position = target.transform.position;

		float deltaX = 0.0f;
		float deltaY = 0.0f;

		//gets x position of mouse to rotate pivot, unless joystick is connected in which it'll use that
		if (Input.GetJoystickNames ().Length == 0) {
			deltaX = Input.GetAxis ("Mouse X") * rotateSpeed;
			deltaY = Input.GetAxis ("Mouse Y") * rotateSpeed;
		} else if (Input.GetJoystickNames ().Length > 0) {
			deltaX = Input.GetAxis ("HorizontalLook") * rotateSpeed;
			deltaY = Input.GetAxis ("VerticalLook") * rotateSpeed;
		}
		pivot.Rotate (0f, deltaX, 0f);
		if (invertY) {
			deltaY *= -1;
		}
		pivot.Rotate (deltaY, 0f, 0f);

		//limits the up and down movement of camera
		if (pivot.rotation.eulerAngles.x > maxViewAngle && pivot.rotation.eulerAngles.x < 180f)
			pivot.rotation = Quaternion.Euler (maxViewAngle, pivot.rotation.eulerAngles.y, pivot.rotation.eulerAngles.z);
		if (pivot.rotation.eulerAngles.x > 180f && pivot.rotation.eulerAngles.x < 360f + minViewAngle)
			pivot.rotation = Quaternion.Euler (360f + minViewAngle, pivot.rotation.eulerAngles.y, pivot.rotation.eulerAngles.z);

		float desiredYAngle = pivot.eulerAngles.y;
		float desiredXAngle = pivot.eulerAngles.x;
		Quaternion rot = Quaternion.Euler (desiredXAngle, desiredYAngle, 0f);
		transform.position = target.position - (rot * offset);

		if (transform.position.y < target.position.y) {
			transform.position = new Vector3 (transform.position.x, target.position.y - 0.5f, transform.position.z);
		}

		checkGround ();

		transform.LookAt (target);
	}

	//checks if the camera would be pushing through geometry on the ground layer
	void checkGround() {
		Vector3 between = transform.position - target.position;
		LayerMask groundMask = LayerMask.GetMask ("Ground");
		RaycastHit hit;
		Debug.DrawRay (target.position, between, Color.blue);
		if (Physics.Raycast (target.position, between, out hit, between.magnitude, groundMask)) {
			transform.position = hit.point + (between.normalized * -cameracorrect);
		}
	}
}
