using UnityEngine;
using System.Collections;

public class CameraRotate : MonoBehaviour {

	public GameObject target;//the target object
	private float speedMod = 10.0f;//a speed modifier
	private Vector3 point;//the coord to the point where the camera looks at

	void Start () {
		point = target.transform.position;//get target's coords
		transform.LookAt(point);//makes the camera look to it
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround (point, Vector3.up ,40 * Time.deltaTime * SpectraCS.currentHigh * speedMod);
		transform.RotateAround (point, Vector3.right ,60 * Time.deltaTime * SpectraCS.currentMiddle * speedMod);
		print (SpectraCS.currentHigh);
	}
}