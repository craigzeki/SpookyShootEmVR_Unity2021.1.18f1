using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CheckBounds))]
public class MaterialTiler : MonoBehaviour
{
    [SerializeField] private Vector2 oneByOneTileScale = new Vector2(1,1);

    private new Renderer renderer;
    void Start()
    {

        renderer = GetComponent<Renderer>();
        Vector3 bounds = GetComponent<CheckBounds>().MyBoundsSize;
        Vector2 newScale = new Vector2(oneByOneTileScale.x * bounds.x, oneByOneTileScale.y * bounds.y);
        renderer.material.mainTextureScale = newScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
