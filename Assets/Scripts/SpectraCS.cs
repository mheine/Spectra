using UnityEngine;
using System.Collections;

public class SpectraCS : MonoBehaviour {
	public GameObject cubePrefab;
	private GameObject parent;
	
	
	private static Vector3 barPosition = new Vector3(8,0,30);

	[Range(0, 1)] public int vizualisationType = 0;

	private int lastVizualisationType;
	const int max = 1024;
    
    public static float currentSpec = 0;
	public static float currentMiddle = 0;
	public static float currentHigh = 0;

	private float stepSize;
    private float minimalistStepSize;

    private float[] spectrum = new float[max];

    private bool epilepsy_mode;
    private bool black_and_white;
    private bool minimalist;
    private bool darken;

    // Use this for initialization
    void Start () {
		RenderSettings.ambientIntensity = 6f;
		DynamicGI.UpdateEnvironment();

        epilepsy_mode = false;
        black_and_white = false;
        minimalist = false;
        darken = false;

        parent = new GameObject();
		parent.tag = "parent";
		parent.name = "Bars";
		parent.transform.parent = Camera.main.transform;
		stepSize = cubePrefab.transform.localScale.y;
		minimalistStepSize = stepSize + 0.4f;

		if(vizualisationType == 0)
			createHorizontalBars ();

		lastVizualisationType = vizualisationType;

    }

    // Update is called once per frame
    void Update() {
		//Audio data collecting
        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Hamming);
        
        /*
        c1 = 64hz
        c3 = 256hz
        c4 = 512hz
        c5 = 1024
        */

		currentSpec = vectorMax (spectrum, 0, 7);
	
		currentMiddle = Mathf.Max (vectorSum(spectrum, 12, 15), Mathf.Max (vectorSum(spectrum, 16, 19), Mathf.Max (vectorSum(spectrum, 20, 23), vectorSum(spectrum, 24, 27))));
		currentMiddle = vectorSum(spectrum, 32, 35);
		currentHigh = Mathf.Max (vectorSum(spectrum, 28, 31), Mathf.Max (vectorSum(spectrum, 32, 35), vectorSum(spectrum, 36, 39)));

		//HERE IS THE BACKGROUND STUFF
		if (Input.GetKeyDown (KeyCode.RightArrow)) 
		{
			vizualisationType++;
			if (vizualisationType > 2)
				vizualisationType = 0;
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) 
		{
			vizualisationType--;
			if (vizualisationType < 0)
				vizualisationType = 2;
		}

        //Disable or enable epilepsy-mode
        if (Input.GetKeyDown(KeyCode.E)) {
            epilepsy_mode = !epilepsy_mode;
        }

        //Disable or enable minimalist-mode
        if (Input.GetKeyDown(KeyCode.M)) {
            minimalist = !minimalist;
        }

        //Disable or enable minimalist-mode
        if (Input.GetKeyDown(KeyCode.M)) {
            darken = !darken;
        }

        //Disable or enable white bars
        if (Input.GetKeyDown(KeyCode.B))
        {
            black_and_white = !black_and_white;
        }

        checkChange();
		if(vizualisationType == 0)
			updateHorizontalBars();
		
		else if(vizualisationType == 1)
			updateVerticalBars();
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
		//The horizontal-mode bars do not have a minimalist option. Hence, the stepsize is always the base value.
		stepSize = cubePrefab.transform.localScale.y;
		
		for (int i = 0; i < spectrum.Length; i++)
		{
			GameObject bar = (GameObject) Instantiate(cubePrefab, new Vector3(0, -6.5f + stepSize * i, barPosition.z), Quaternion.identity);
			bar.tag = "horizontalBar";
			bar.transform.parent = parent.transform;
			bar.name = "c" + (i + 1);
			bar.transform.localScale = new Vector3(bar.transform.localScale.x, bar.transform.localScale.y, bar.transform.localScale.z);
			bar.GetComponent<SelfDestruct>().enabled = false;

		}
	}

	public void updateHorizontalBars()
	{
		GameObject[] cubes = GameObject.FindGameObjectsWithTag("horizontalBar");
		for (var i = 0; i < cubes.Length; i++)
		{
			cubes[i].transform.localScale = new Vector3(5 + 400 * spectrum[i], cubes[i].transform.localScale.y,  cubes[i].transform.localScale.z);

            if (black_and_white) {
                cubes[i].GetComponent<Renderer>().material.color = Color.black;
            }
            else {
            	if (epilepsy_mode)
                	cubes[i].GetComponent<Renderer>().material.color = NoiseBall.NoiseBallRenderer.barColor;
            	else
                	cubes[i].GetComponent<Renderer> ().material.color = ToColor(0xffffff ^ NoiseBall.NoiseBallRenderer.currColor.GetHashCode()) ;
			}
		}
	}



	public void updateVerticalBars()
	{

		if(minimalist)
			stepSize = minimalistStepSize;
		else
			stepSize = cubePrefab.transform.localScale.y;


		GameObject[] cubes = GameObject.FindGameObjectsWithTag("verticalBar");
		foreach (GameObject item in cubes) {
			item.transform.Translate (-Camera.main.transform.right * stepSize);
		}

		GameObject bar = (GameObject) Instantiate(cubePrefab, barPosition, Quaternion.identity);
		bar.tag = "verticalBar";

        if (black_and_white)
            bar.GetComponent<Renderer>().material.color = Color.black;
        else
            bar.GetComponent<Renderer>().material.color = NoiseBall.NoiseBallRenderer.barColor;

        bar.transform.parent = parent.transform;
		bar.transform.localScale = new Vector3(bar.transform.localScale.x, 1 + vectorSum(spectrum, 0, max-1), bar.transform.localScale.z);
		bar.GetComponent<SelfDestruct>().enabled = true;

	}

	public void deleteBars(string var)
	{
		GameObject[] cubes = GameObject.FindGameObjectsWithTag (var);
		foreach (GameObject item in cubes) {
			Destroy (item);
		}
	}

	public void checkChange()
	{
		if ( lastVizualisationType !=  vizualisationType)
		{
			if (vizualisationType == 0) {
				deleteBars ("verticalBar");
				createHorizontalBars ();
			} else if (vizualisationType == 1) {
				deleteBars ("horizontalBar");
			} else if (vizualisationType == 2) {
				if (lastVizualisationType == 0)
					deleteBars ("horizontalBar");
				else if (lastVizualisationType == 1)
					deleteBars ("verticalBar");
			}
		}
		lastVizualisationType =  vizualisationType;
	}


}