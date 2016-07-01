using UnityEngine;
using System.Collections;

public class PolyRotate : MonoBehaviour {

	public GameObject target;//the target object
	private float speedMod = 10.0f;//a speed modifier
	private Vector3 point;//the coord to the point where the camera looks at
	GameObject parent;
	void Start () {
		point = target.transform.position;//get target's coords
		transform.LookAt(point);//makes the camera look to it
		parent = GameObject.FindGameObjectWithTag("parent");
	}
	
	// Update is called once per frame
	void Update () {
		target.transform.Rotate (new Vector3 (-60 * Time.deltaTime * SpectraCS.currentMiddle * speedMod, -40 * Time.deltaTime * SpectraCS.currentHigh * speedMod, 0));
	}
}