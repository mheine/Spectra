using UnityEngine;
using System.Collections;

public class SpectraCS : MonoBehaviour {

    float[] spectrum = new float[1024];
    public static float currentSpec = 0;
	public static float currentMiddle = 0;
	public static float currentHigh = 0;

    // Use this for initialization
    void Start () {

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

        //TODO - Look up specifics for which range is for which bar
        float c1 = spectrum[0] + spectrum[1] + spectrum[2] + spectrum[3];
        float c2 = spectrum[4] + spectrum[5] + spectrum[6] + spectrum[7];
        float c3 = spectrum[8] + spectrum[9] + spectrum[10] + spectrum[11];
        float c4 = spectrum[12] + spectrum[13] + spectrum[14] + spectrum[15];
        float c5 = spectrum[16] + spectrum[17] + spectrum[18] + spectrum[19];
        float c6 = spectrum[20] + spectrum[21] + spectrum[22] + spectrum[23];
        float c7 = spectrum[24] + spectrum[25] + spectrum[26] + spectrum[27];
        float c8 = spectrum[28] + spectrum[29] + spectrum[30] + spectrum[31];
        float c9 = spectrum[32] + spectrum[33] + spectrum[34] + spectrum[35];
        float c10 = spectrum[36] + spectrum[37] + spectrum[38] + spectrum[39];

		float curr1 = Mathf.Max (spectrum [0], Mathf.Max (spectrum [1], Mathf.Max (spectrum [2], spectrum [3])));
		float curr2 = Mathf.Max (spectrum [4], Mathf.Max (spectrum [5], Mathf.Max (spectrum [6], spectrum [7])));
		currentSpec = Mathf.Max (curr1, curr2);
		//currentMiddle = Mathf.Max (c4, Mathf.Max (c5, Mathf.Max (c6, c7)));
		currentMiddle = c9;
		currentHigh = Mathf.Max (c8, Mathf.Max (c9, c10));

		//print ("currentHigh: " + currentHigh + " currentMiddle: " + currentMiddle);
        //currentSpec = c2;

		//print ("0:" + spectrum[0] + " 1: " + spectrum[1] + " 2: " + spectrum[2] + " 3: " + spectrum[3]);
		//print ("4:" + spectrum[4] + " 5: " + spectrum[5] + " 6: " + spectrum[6] + " 7: " + spectrum[7]);


        float[] specs = { c1, c2, c3, c4, c5, c6, c7, c8, c9, c10 };

        /* var c3 = spectrum[11] + spectrum[12] + spectrum[13];
        var c4 = spectrum[22] + spectrum[23] + spectrum[24];
        var c5 = spectrum[44] + spectrum[45] + spectrum[46] + spectrum[47] + spectrum[48] + spectrum[49];
        */
    }
}
