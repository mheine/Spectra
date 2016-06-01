
var origsize;

function Start () {
    Debug.Log("HEJSAN");
    var firstCube = GameObject.FindGameObjectsWithTag("Cubes")[0];

    //origsize = firstCube.mesh.bounds.size.y;

    for (var i = 0; i < 10; i++) {
        var bar = Instantiate(firstCube, new Vector3((i * 2.0) - 10, 0, 0), Quaternion.identity);
        bar.tag = "Cubes";
        bar.name = "c" + (i + 1);
        //origsize = bar.renderer.bounds.size;
    }
    
    
}

function Update() {
    var spectrum = AudioListener.GetSpectrumData(1024, 0, FFTWindow.Hamming);

    /*
	c1 = 64hz
	c3 = 256hz
	c4 = 512hz
	c5 = 1024
	*/

    var c1 = spectrum[0] + spectrum[1] + spectrum[2] + spectrum[3];
    var c2 = spectrum[4] + spectrum[5] + spectrum[6] + spectrum[7];
    var c3 = spectrum[8] + spectrum[9] + spectrum[10] + spectrum[11];
    var c4 = spectrum[12] + spectrum[13] + spectrum[14] + spectrum[15];
    var c5 = spectrum[16] + spectrum[17] + spectrum[18] + spectrum[19];
    var c6 = spectrum[20] + spectrum[21] + spectrum[22] + spectrum[23];
    var c7 = spectrum[24] + spectrum[25] + spectrum[26] + spectrum[27];
    var c8 = spectrum[28] + spectrum[29] + spectrum[30] + spectrum[31];
    var c9 = spectrum[32] + spectrum[33] + spectrum[34] + spectrum[35];
    var c10 = spectrum[36] + spectrum[37] + spectrum[38] + spectrum[39];

    var specs = [c1, c2, c3, c4, c5, c6, c7, c8, c9, c10];

    /* var c2 = spectrum[11] + spectrum[12] + spectrum[13];
    
   
    var c4 = spectrum[22] + spectrum[23] + spectrum[24];
    var c5 = spectrum[44] + spectrum[45] + spectrum[46] + spectrum[47] + spectrum[48] + spectrum[49]; */

    var cubes = GameObject.FindGameObjectsWithTag("Cubes");

    Debug.Log("Size of spectrum: " + spectrum.length);

    Debug.Log("c1: " + c1 + "  c2: " + c2 + "  c3: " + c3 + "  c4: " + c4 + "  c5: " + c5 + "  c6: " + c6 + "  c6: " + c6 + "  ");

    //cubes[0].transform.localScale += new Vector3(0.1  , 0, 0);

    for (var i = 0; i < cubes.length; i++) {
        cubes[i].transform.localScale.y = 1 + 20 * specs[i]; cubes[i].transform.position.y = 0 + (20 * specs[i]) / 2;
    }

 /*   for (var i = 0; i < cubes.length; i++) {
        Debug.Log("Cube name: " + cubes[i].name);
        switch (cubes[i].name) {
            case "c1": cubes[i].transform.localScale.y = 1 + 20 * c1; cubes[i].transform.position.y = 0 + (20 * c1) / 2;
            case "c2": cubes[i].transform.localScale.y = 1 + 20 * c2; cubes[i].transform.position.y = 0 + (20 * c2) / 2;
            case "c3": cubes[i].transform.localScale.y = 1 + 20 * c3; cubes[i].transform.position.y = 0 + (20 * c3) / 2;
            case "c4": cubes[i].transform.localScale.y = 1 + 20 * c4; cubes[i].transform.position.y = 0 + (20 * c4) / 2;
            case "c5": cubes[i].transform.localScale.y = 1 + 20 * c5; cubes[i].transform.position.y = 0 + (20 * c5) / 2;
            case "c6": cubes[i].transform.localScale.y = 1 + 20 * c6; cubes[i].transform.position.y = 0 + (20 * c6) / 2;
            case "c7": cubes[i].transform.localScale.y = 1 + 20 * c7; cubes[i].transform.position.y = 0 + (20 * c7) / 2;
            case "c8": cubes[i].transform.localScale.y = 1 + 20 * c8; cubes[i].transform.position.y = 0 + (20 * c8) / 2;
            case "c9": cubes[i].transform.localScale.y = 1 + 20 * c9; cubes[i].transform.position.y = 0 + (20 * c9) / 2;
            case "c10": cubes[i].transform.localScale.y = 1 + 20 * c10; cubes[i].transform.position.y = 0 + (20 * c10) / 2;

        }
    } */


}