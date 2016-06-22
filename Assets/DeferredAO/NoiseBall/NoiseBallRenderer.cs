using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

namespace NoiseBall
{
    [ExecuteInEditMode]
    public class NoiseBallRenderer : MonoBehaviour
    {
        #region Exposed Parameters

        [SerializeField]
        NoiseBallMesh _mesh;

        [Space]
        [SerializeField]
        float _radius = 0.6f;

        [SerializeField]
        float _noiseAmplitude = 0.05f;

        [SerializeField]
        float _noiseFrequency = 1.0f;

        [SerializeField]
        float _noiseMotion = 0.2f;

        [Space]
        [SerializeField, ColorUsage(false, true, 0, 8, 0.125f, 3)]
        Color _lineColor = Color.white;

        [SerializeField, ColorUsage(false)]
        Color _surfaceColor = Color.white;

        [SerializeField, Range(0, 1)]
        float _metallic = 0.5f;

        [SerializeField, Range(0, 1)]
        float _smoothness = 0.5f;

        [Space]
        [SerializeField]
        ShadowCastingMode _castShadows;

        [SerializeField]
        bool _receiveShadows;

        #endregion

        #region Private Resources

        [SerializeField, HideInInspector]
        Shader _surfaceShader;

        [SerializeField, HideInInspector]
        Shader _lineShader;

        #endregion

        #region Private Variables

        Material _surfaceMaterial;
        Material _lineMaterial;
        MaterialPropertyBlock _materialProperties;
        Vector3 _noiseOffset;
		float radiusOffset = 0;
		float polygonOffset = 0;
		float lastOffset = 0;


        #endregion
		public static Color currColor;
        #region MonoBehaviour Functions

        void Start()
        {
            Update();
            StartCoroutine("ColorShift");
			StartCoroutine ("RadiusShift");
			StartCoroutine ("PolygonShift");
			//StartCoroutine ("MotionShift");
        }

        void Update()
        {
            

            if (_surfaceMaterial == null)
            {
                _surfaceMaterial = new Material(_surfaceShader);
                _surfaceMaterial.hideFlags = HideFlags.DontSave;
            }

            if (_lineMaterial == null)
            {
                _lineMaterial = new Material(_lineShader);
                _lineMaterial.hideFlags = HideFlags.DontSave;
            }

            if (_materialProperties == null)
                _materialProperties = new MaterialPropertyBlock();


            _noiseOffset += new Vector3(0.13f, 0.82f, 0.11f) * _noiseMotion * Time.deltaTime;


            _surfaceMaterial.color = _surfaceColor;
            _lineMaterial.color = _lineColor;
			_lineMaterial.SetFloat("_Radius", _radius * 1.05f + 1 *radiusOffset);

            _surfaceMaterial.SetFloat("_Metallic", _metallic);
            _surfaceMaterial.SetFloat("_Glossiness", _smoothness);
			_surfaceMaterial.SetFloat("_Radius", _radius + 1 * polygonOffset);

            _materialProperties.SetFloat("_NoiseAmplitude", _noiseAmplitude);
            _materialProperties.SetFloat("_NoiseFrequency", _noiseFrequency);
            _materialProperties.SetVector("_NoiseOffset", _noiseOffset);

            Graphics.DrawMesh(
                _mesh.sharedMesh, transform.localToWorldMatrix,
                _surfaceMaterial, 0, null, 0, _materialProperties,
                _castShadows, _receiveShadows, transform
            );


            Graphics.DrawMesh(
                _mesh.sharedMesh, transform.localToWorldMatrix,
                _lineMaterial, 0, null, 1, _materialProperties,
                _castShadows, _receiveShadows, transform
            );
        }

        IEnumerator ColorShift()
        {
            float h = 0f;
            float s = 0f;
            float v = 0f;
            while (true)
            {
				currColor = Color.HSVToRGB(h, 1, SpectraCS.currentSpec * 0.5f + 0.2f, true);
				_surfaceMaterial.color = currColor;
                //_surfaceMaterial.color = Color.HSVToRGB(h, Mathf.Abs(Mathf.Sin(s)*0.5f + 0.5f), Mathf.Abs(Mathf.Cos(v)*0.5f + 0.5f), true);
                h += 0.005f;
                h = h % 1;
                s += 0.0001f;
                //    s = s % 1 * Mathf.PI;
                v += 0.0002f;
                //    v = v % 1 * Mathf.PI;
                yield return null;
            }
        }

		IEnumerator RadiusShift()
		{
			while (true) 
			{
				if (SpectraCS.currentSpec > radiusOffset) {
					radiusOffset = SpectraCS.currentSpec;
					//lastOffset = SpectraCS.currentSpec;
				} else {
					if (radiusOffset > 0) {
						radiusOffset -= 0.012f;
					}
				}
				yield return null;
			}
		}

		IEnumerator PolygonShift()
		{
			while (true) 
			{
				
				if (SpectraCS.currentHigh > polygonOffset  && Mathf.Abs(polygonOffset - SpectraCS.currentHigh) > 0.1f) {
					if (SpectraCS.currentHigh > radiusOffset) 
					{
						polygonOffset = radiusOffset;
					} else 
					{
						polygonOffset = SpectraCS.currentHigh;
					}
					//lastOffset = SpectraCS.currentSpec;
				} 
				else 
				{
					if (polygonOffset > 0) 
					{
						polygonOffset -= 0.02f;
					}
				}
				//print (polygonOffset);
				yield return null;
			}
		}

		IEnumerator MotionShift()
		{
			while (true) 
			{
				if (SpectraCS.currentMiddle > _noiseMotion && SpectraCS.currentMiddle > 0.4f) {
					_noiseMotion = SpectraCS.currentMiddle;
				} else if (_noiseMotion > 0.4f){
					_noiseMotion -= 0.1f;
				}
				_noiseFrequency = SpectraCS.currentSpec;
				print ("_NoiseFrequency: " + _noiseFrequency);
				yield return null;
			}
		}

        #endregion
    }


}

