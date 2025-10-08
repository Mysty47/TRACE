using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightManager : MonoBehaviour
{
    [Header("References")]
    private Transform highlightedObj;
    private Transform selectedObj;
    public LayerMask selectableLayer;
    
    private RaycastHit hit;

    void Start()
    {
        // Disable all Outlines
        Outline[] outlines = FindObjectsOfType<Outline>();
        foreach (Outline outline in outlines)
        {
            outline.enabled = false;
        }
        
    }

    void Update()
    {
        HoverHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            CheckClick();
        }
    }


    public void HoverHighlight()
    {
        // Remove all highlights
        if (highlightedObj != null)
        {
            Outline prevOutline = highlightedObj.GetComponent<Outline>();
            if (prevOutline != null && highlightedObj != selectedObj)
                prevOutline.enabled = false;

            highlightedObj = null;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Raycast
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit, Mathf.Infinity, selectableLayer))
        {
            highlightedObj = hit.transform;

            if (highlightedObj.CompareTag("InteractionnalButton") && highlightedObj != selectedObj)
            {
                Outline outline = highlightedObj.GetComponent<Outline>();
                if (outline != null)
                    outline.enabled = true;
            }
            else
            {
                highlightedObj = null;
            }
        }
    }

    public void SelectedHighlight()
    {
        if (highlightedObj != null && highlightedObj.CompareTag("InteractionnalButton"))
        {
            if (selectedObj != null)
            {
                selectedObj.GetComponent<Outline>().enabled = false;
            }

            selectedObj = hit.transform;
            selectedObj.GetComponent<Outline>().enabled = true;

            highlightedObj = null;
        }
    }

    public void DeselectHighlight()
    {
        if (selectedObj != null)
        {
            selectedObj.GetComponent<Outline>().enabled = false;
            selectedObj = null;
        }
    }
    
    void CheckClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit, Mathf.Infinity, selectableLayer))
        {
            Transform clickedObj = hit.transform;

            if (clickedObj.CompareTag("InteractionnalButton"))
            {
                Button3D button = clickedObj.GetComponent<Button3D>();
                if (button != null)
                {
                    button.OnClick();
                }
            }
        }
    }

}
