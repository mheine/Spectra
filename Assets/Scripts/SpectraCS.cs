using UnityEngine;
using System.Collections;

public class SpectraCS : MonoBehaviour {
	public GameObject cubePrefab;
	[Range(0, 1)] public int vizualisationType = 0;

	const int max = 1024;
    float[] spectrum = new float[max];
    public static float currentSpec = 0;
	public static float currentMiddle = 0;
	public static float currentHigh = 0;
	private GameObject parent;
	private float stepSize;
	public static Vector3 barPosition = new Vector3(8,0,28);

    // Use this for initialization
    void Start () {
		//Uppdatera så att det blir lite ljusare
		RenderSettings.ambientIntensity = 6f;
		DynamicGI.UpdateEnvironment();

		parent = new GameObject();
		parent.tag = "parent";
		parent.name = "Bars";
		parent.transform.parent = Camera.main.transform;
		stepSize = cubePrefab.transform.localScale.y;
		if(vizualisationType == 0)
			createHorizontalBars ();

    }

    // Update is called once per frame
    void Update() {
        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Hamming);
        
        /*
        c1 = 64hz
        c3 = 256hz
        c4 = 512hz
        c5 = 1024
        */

		currentSpec = vectorMax (spectrum, 0, 7);
		//print (currentSpec);
		currentMiddle = Mathf.Max (vectorSum(spectrum, 12, 15), Mathf.Max (vectorSum(spectrum, 16, 19), Mathf.Max (vectorSum(spectrum, 20, 23), vectorSum(spectrum, 24, 27))));
		currentMiddle = vectorSum(spectrum, 32, 35);
		currentHigh = Mathf.Max (vectorSum(spectrum, 28, 31), Mathf.Max (vectorSum(spectrum, 32, 35), vectorSum(spectrum, 36, 39)));

		//HERE IS THE BACKGROUND STUFF
		if(vizualisationType == 0)
			updateHorizontalBars();
		
		else if(vizualisationType == 1)
			updateVericalBars ();
    }

	//min and max are inclusive
	private float vectorMax(float[] vec, int min, int max)
	{
		if (min >= vec.Length || max >= vec.Length)
			return -1;
		
		float maxVal = 0;
		for (int i = min; i <= max; i++)
		{
			maxVal = Mathf.Max(maxVal, vec[i]);
		}

		return maxVal;
	}

	//min and max are inclusive
	private float vectorSum(float[] vec, int min, int max)
	{
		if (min >= vec.Length || max >= vec.Length)
			return -1;
		
		float sumVal = 0;
		for (int i = min; i <= max; i++)
		{
			sumVal += vec[i];
		}

		return sumVal;
	}

	public Color ToColor(int HexVal)
	{
		byte R = (byte)((HexVal >> 16) & 0xFF);
		byte G = (byte)((HexVal >> 8) & 0xFF);
		byte B = (byte)((HexVal) & 0xFF);
		return new Color32(R, G, B, 100);
	}

	public void createHorizontalBars()
	{
		for (int i = 0; i < spectrum.Length; i++)
		{
			GameObject bar = (GameObject) Instantiate(cubePrefab, new Vector3(0, -6.5f + stepSize * i, barPosition.z), Quaternion.identity);
			bar.tag = "cube";
			bar.transform.parent = parent.transform;
			bar.name = "c" + (i + 1);
			bar.transform.localScale = new Vector3(bar.transform.localScale.x, bar.transform.localScale.y, bar.transform.localScale.z);
			bar.GetComponent<SelfDestruct>().enabled = false;

		}
	}

	public void updateHorizontalBars()
	{
		GameObject[] cubes = GameObject.FindGameObjectsWithTag("cube");
		for (var i = 0; i < cubes.Length; i++)
		{
			cubes[i].transform.localScale = new Vector3(5 + 400 * spectrum[i], cubes[i].transform.localScale.y,  cubes[i].transform.localScale.z);
			cubes[i].GetComponent<Renderer> ().material.color = ToColor(0xffffff ^ NoiseBall.NoiseBallRenderer.currColor.GetHashCode()) ;
		}
	}

	public void updateVericalBars()
	{
		GameObject[] cubes = GameObject.FindGameObjectsWithTag("cube");
		foreach (GameObject item in cubes) {
			item.transform.Translate (-Camera.main.transform.right * stepSize);
		}

		GameObject bar = (GameObject) Instantiate(cubePrefab, barPosition, Quaternion.identity);
		bar.GetComponent<Renderer> ().material.color = NoiseBall.NoiseBallRenderer.currColor;
		bar.transform.parent = parent.transform;
		bar.transform.localScale = new Vector3(bar.transform.localScale.x, 1 + vectorSum(spectrum, 0, max-1), bar.transform.localScale.z);
		bar.GetComponent<SelfDestruct>().enabled = true;

	}
}