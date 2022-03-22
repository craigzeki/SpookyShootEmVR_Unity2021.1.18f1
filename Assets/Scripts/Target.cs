using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//handling of switching emissive material on / off based on https://blog.terresquall.com/2020/01/getting-your-emission-maps-to-work-in-unity/


public class Target : MonoBehaviour, IShootable
{
	//settings for each target
	[SerializeField] private TextMeshPro myPointsText;
	[SerializeField] private int points = 10;
	[SerializeField] private float damageDuration = 0.5f;
	[SerializeField] private float floatUpDistance = 0.5f;
	[SerializeField] private int emissiveMaterialIndex = 0;
	[SerializeField] private iAnimatableObject animatableObject;
	

	//prevents multiple hits until the coroutine is completed
	private bool isHit = false;

	//parameters for handling the emissive matterial
	private Material targetEmissiveMaterial;
	private Renderer myRenderer;
	private Color emissionColour;

	//paramters for lerping transparency and position
	private Color defaultFaceColour;
	private Color lerpFaceColour;
	private float lerpPositionY = 0.0f;
	private Vector3 startPos;

	// Start is called before the first frame update
    void Start()
    {
		//gaurd
		if (myPointsText == null) return;

		//get the animation interface from the parent object (if it has one)
		animatableObject = GetComponentInParent<iAnimatableObject>();

		//get the target's renderer for use when switching emissive property on / off
		myRenderer = GetComponent<Renderer>();
		//get the correct material (the yellow light)
		targetEmissiveMaterial = GetComponent<Renderer>().materials[emissiveMaterialIndex];
		emissionColour = targetEmissiveMaterial.GetColor("_EmissionColor");
		//setup colours used in the lerp - do it here for performance
		defaultFaceColour = myPointsText.faceColor;
		lerpFaceColour = defaultFaceColour;
		
		//setup position used in the lerp
		startPos = myPointsText.rectTransform.position;
		lerpPositionY = startPos.y;

		//disable the points text
		myPointsText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
		//update the points text with values calculated in the coroutine
		myPointsText.faceColor = lerpFaceColour;
		myPointsText.rectTransform.position = new Vector3(myPointsText.rectTransform.position.x, lerpPositionY, myPointsText.rectTransform.position.z);
	}
	
	

	private IEnumerator targetHit()
	{
		float elapsedTime = 0.0f;
		
		//guards
		if ((myPointsText == null) || (targetEmissiveMaterial == null))
		{
			isHit = false;
			yield break;
		}

		//trigger animations if available
		if(animatableObject != null)
        {
			animatableObject.DoAnimations();
		}

		//dull the target by switching off emmissive property
		targetEmissiveMaterial.DisableKeyword("_EMISSION");
		targetEmissiveMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
		targetEmissiveMaterial.SetColor("_EmissionColor", Color.black);
		//update the renderer with the change
		RendererExtensions.UpdateGIMaterials(myRenderer);
		DynamicGI.SetEmissive(myRenderer, Color.black);
		//update the environment to handle any dynamic lighting change (shouldn't be needed by my lighting strategy, but included for completeness
		DynamicGI.UpdateEnvironment();

		//set the points text
		myPointsText.text = "+" + points.ToString();
		myPointsText.enabled = true;

		//add the points to the score
		ScoringSystem.Instance.AddPoints(points);

		//lerp the transparency and position of the points text
		while(elapsedTime < damageDuration)
        {
			elapsedTime += Time.deltaTime;

			lerpFaceColour.a = Mathf.Lerp(defaultFaceColour.a, 0, elapsedTime / damageDuration);
			lerpPositionY = Mathf.Lerp(startPos.y, (startPos.y + floatUpDistance), elapsedTime / damageDuration);

			
			yield return null;
        }

		//re light the target by switching the emissive property back on
		targetEmissiveMaterial.EnableKeyword("_EMISSION");
		targetEmissiveMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;

		// Update the emission color and intensity of the material.
		targetEmissiveMaterial.SetColor("_EmissionColor", emissionColour);

		// Makes the renderer update the emission and albedo maps of our material.
		RendererExtensions.UpdateGIMaterials(myRenderer);

		// Inform Unity's GI system to recalculate GI based on the new emission map.
		DynamicGI.SetEmissive(myRenderer, emissionColour);
		DynamicGI.UpdateEnvironment();

		//reset parameters for next time
		lerpFaceColour.a = 1;
		lerpPositionY = startPos.y;
		myPointsText.enabled = false;
		isHit = false;

	}

    public void DoDamage(int damage)
    {
		//target does not have a health so no need to handle damage parameter
		DoDamage();
    }

    public void DoDamage()
    {
		//if already running the coroutine - do not register hit
		if (isHit) return;

		StartCoroutine(targetHit());

    }
}
