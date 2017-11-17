using UnityEngine;
using System.Collections;

public class SpectraCS : MonoBehaviour {
	public GameObject cubePrefab;
	private GameObject parent;
	
	
	private static Vector3 barPosition = new Vector3(8,0,30);

	[Range(0, 2)]
	public int vizualisationType = 0;

	private int lastVizualisationType;
	const int maxSpectrumSize = 1024;
    
    public static float currentLow = 0;
	public static float currentMiddle = 0;
	public static float currentHigh = 0;

	private float stepSize;
    private float minimalistStepSize;

    private float[] spectrum = new float[maxSpectrumSize];

    private bool epilepsyMode;
    private bool blackAndWhite;
    private bool minimalist;
    private bool darken;

    // Use this for initialization
    void Start () {
		RenderSettings.ambientIntensity = 6f;
		DynamicGI.UpdateEnvironment();

        epilepsyMode = false;
        blackAndWhite = false;
        minimalist = false;
        darken = false;

        parent = new GameObject();
		parent.tag = "parent";
		parent.name = "Bars";
		parent.transform.parent = Camera.main.transform;

		stepSize = cubePrefab.transform.localScale.y;
		minimalistStepSize = stepSize + 0.5f;

		if(vizualisationType == 0)
			createHorizontalBars();

		lastVizualisationType = vizualisationType;

    }

    // Update is called once per frame
    void Update() {

		//Collect the audio spectrum data from the current audio frame into our spectrum array.
        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Hamming);
        
        /*
        The channels that we look at fall withing the following frequencies.
        [64 hz, 128 hz, 256 hz, 512 hz, 1024 hz ... and so on]

        We then group these into the low, middle and high frequencies.

        The low frequencies is calculated as the sum of channels 0-7.

        The middle frequencies is caluclated by the max of the sum of channels 12-16, 17-21, 22-26 27-31 or 32-35.

        The high frequnecies follow the same pattern, max of 28-31, 32-35 and 40-43
        */

		currentLow = vectorMax (spectrum, 0, 7);
	
		currentMiddle = Mathf.Max(vectorSum(spectrum, 12, 15),
						Mathf.Max(vectorSum(spectrum, 16, 19),
						Mathf.Max(vectorSum(spectrum, 20, 23),
						Mathf.Max(vectorSum(spectrum, 24, 27),
						vectorSum(spectrum, 28, 31)))));

		currentHigh = 	Mathf.Max (vectorSum(spectrum, 32, 35),
						Mathf.Max (vectorSum(spectrum, 36, 39),
						vectorSum(spectrum, 40, 43)));


		//Handle all types of key-presses
		//The keys Z and P (for showing the menu and for pausing are found in PanelGUI.cs and Play.cs respectively.)

		//Scroll through vizualition types [0 -> 1 -> 2 -> 0]
		if (Input.GetKeyDown (KeyCode.RightArrow)) 
			vizualisationType++;
			vizualisationType = vizualisationType > 2 ? 0 : vizualisationType;

		//Reverse scroll through vizualition types [0 -> 2 -> 1 -> 0]
		if (Input.GetKeyDown (KeyCode.LeftArrow))
			vizualisationType--; 
			vizualisationType = vizualisationType < 0 ? 2 : vizualisationType;

        //Disable or enable epilepsy-mode
        if (Input.GetKeyDown(KeyCode.E)) {
        	bool current = epilepsyMode;
        	resetVisualizationMode();
            epilepsyMode = !current;
        }

        //Disable or enable minimalist-mode
        if (Input.GetKeyDown(KeyCode.M)) {
            bool current = minimalist;
        	resetVisualizationMode();
            minimalist = !current;
        }

        //Disable or enable minimalist-mode
        if (Input.GetKeyDown(KeyCode.D)) {
            bool current = darken;
        	resetVisualizationMode();
            darken = !current;
            Debug.Log("Darken changed, is now " + darken);
        }

        //Disable or enable white bars
        if (Input.GetKeyDown(KeyCode.B)) {
            bool current = blackAndWhite;
        	resetVisualizationMode();
            blackAndWhite = !current;
        }

        //Reset to standard vizualisation
        if (Input.GetKeyDown(KeyCode.R)) {
            resetVisualizationMode();
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

	public Color darkenColor(Color32 color)
	{

		byte R = (byte) (color.r * 0.3);
		byte G = (byte) (color.g * 0.3);
		byte B = (byte) (color.b * 0.3);
		return new Color32(R, B, G, 30);
	}

	public void createHorizontalBars()
	{
		//The horizontal-mode bars do not have a minimalist option. Hence, the stepsize is always the base value.
		stepSize = cubePrefab.transform.localScale.y;
		
		for (int i = 0; i < spectrum.Length; i++)
		{
			GameObject bar = (GameObject) Instantiate(cubePrefab, new Vector3(0, -8.0f + stepSize * i, barPosition.z), Quaternion.identity);
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
			cubes[i].transform.localScale = new Vector3(5 + 350 * spectrum[i], cubes[i].transform.localScale.y,  cubes[i].transform.localScale.z);

            if (blackAndWhite) {
                cubes[i].GetComponent<Renderer>().material.color = Color.black;
            }
            else {

            	if (darken)
            		cubes[i].GetComponent<Renderer>().material.color = darkenColor(NoiseBall.NoiseBallRenderer.barColor);

            	else if (epilepsyMode)
                	cubes[i].GetComponent<Renderer>().material.color = ToColor(0xAAAAAA ^ NoiseBall.NoiseBallRenderer.currColor.GetHashCode());

            	else
                	cubes[i].GetComponent<Renderer>().material.color = NoiseBall.NoiseBallRenderer.barColor;
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

        if (blackAndWhite)
            bar.GetComponent<Renderer>().material.color = Color.black;
        else
            bar.GetComponent<Renderer>().material.color = NoiseBall.NoiseBallRenderer.barColor;

        bar.transform.parent = parent.transform;
		bar.transform.localScale = new Vector3(bar.transform.localScale.x, 1 + vectorSum(spectrum, 0, maxSpectrumSize-1), bar.transform.localScale.z);
		bar.GetComponent<SelfDestruct>().enabled = true;

	}

	public void deleteBars(string tagname)
	{
		GameObject[] cubes = GameObject.FindGameObjectsWithTag (tagname);
		foreach (GameObject item in cubes) {
			Destroy (item);
		}
	}

	private void resetVisualizationMode() {
		epilepsyMode = false;
        blackAndWhite = false;
        minimalist = false;
        darken = false;	
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
				deleteBars ("horizontalBar");
				deleteBars ("verticalBar");
			}
		}
		lastVizualisationType =  vizualisationType;
	}


}