using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hitAnything = Physics.Raycast(ray, out hit);

            // Check to see if we hit a HarvestableObject
            if (hitAnything)
            {
                HarvestableObject harvestable = hit.collider.GetComponent<HarvestableObject>();
                if (harvestable != null)
                {
                    harvestable.OnClick();
                }
            }
        }
    }
}
