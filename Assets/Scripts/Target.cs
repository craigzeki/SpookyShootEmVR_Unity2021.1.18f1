using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//handling of emission based on https://blog.terresquall.com/2020/01/getting-your-emission-maps-to-work-in-unity/


public class Target : MonoBehaviour, IShootable
{
	[SerializeField] private TextMeshPro myPointsText;
	[SerializeField] private int points = 10;
	[SerializeField] private float damageDuration = 0.5f;
	[SerializeField] private float floatUpDistance = 0.5f;
	[SerializeField] private int emissiveMaterialIndex = 0;
	
	
	private Material targetEmissiveMaterial;

	private bool isHit = false;

	private Color defaultFaceColour;
	private Color lerpFaceColour;
	private Color emissionColour;

	private Renderer myRenderer;
	private Vector3 startPos;

	// Start is called before the first frame update
    void Start()
    {
		if (myPointsText == null) return;

		myRenderer = GetComponent<Renderer>();
		targetEmissiveMaterial = GetComponent<Renderer>().materials[emissiveMaterialIndex];
		emissionColour = targetEmissiveMaterial.GetColor("_EmissionColor");
		//setup colours used in the lerp - do it here for performance
		defaultFaceColour = myPointsText.faceColor;
		lerpFaceColour = defaultFaceColour;

		startPos = myPointsText.rectTransform.position;

		//disable the text
		myPointsText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	

	private IEnumerator targetHit()
	{
		float elapsedTime = 0.0f;
		float lerpPositionY = 0.0f;

		//guard
		if(myPointsText == null) yield break;
		if (targetEmissiveMaterial == null) yield break;

		//dull the target
		targetEmissiveMaterial.DisableKeyword("_EMISSION");
		targetEmissiveMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;

		targetEmissiveMaterial.SetColor("_EmissionColor", Color.black);
		RendererExtensions.UpdateGIMaterials(myRenderer);

		DynamicGI.SetEmissive(myRenderer, Color.black);
		DynamicGI.UpdateEnvironment();

		//set the points text
		myPointsText.text = "+" + myPointsText.ToString();
		myPointsText.enabled = true;

		while(elapsedTime > damageDuration)
        {
			elapsedTime += Time.deltaTime;

			lerpFaceColour.a = Mathf.Lerp(defaultFaceColour.a, 0, elapsedTime / damageDuration);
			lerpPositionY = Mathf.Lerp(myPointsText.rectTransform.position.y, (myPointsText.rectTransform.position.y + floatUpDistance), elapsedTime / damageDuration);

			myPointsText.faceColor = lerpFaceColour;
			myPointsText.rectTransform.position = new Vector3(myPointsText.rectTransform.position.x, lerpPositionY, myPointsText.rectTransform.position.z);
			yield return null;
        }

		//re light the target
		targetEmissiveMaterial.EnableKeyword("_EMISSION");
		targetEmissiveMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;

		// Update the emission color and intensity of the material.
		targetEmissiveMaterial.SetColor("_EmissionColor", emissionColour);

		// Makes the renderer update the emission and albedo maps of our material.
		RendererExtensions.UpdateGIMaterials(myRenderer);

		// Inform Unity's GI system to recalculate GI based on the new emission map.
		DynamicGI.SetEmissive(myRenderer, emissionColour);
		DynamicGI.UpdateEnvironment();

		
		myPointsText.rectTransform.position = startPos;
		myPointsText.enabled = false;
		isHit = false;

	}

    public void doDamage(int damage)
    {
		//target does not have a health
		doDamage();
    }

    public void doDamage()
    {
		//if already showing points - do not register hit
		if (isHit) return;

		StartCoroutine(targetHit());

    }
}
