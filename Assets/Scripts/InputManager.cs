using UnityEngine;

public class InputManager : MonoBehaviour
{
    private IClickable currentlyHovered;

    // Update is called once per frame
    void Update()
    {
        HandleHover();
        HandleClick();
    }

    // For hovering over clickable objects
    void HandleHover()
    {
        // Raycast from camera to the mouse pos, then sort it based on distance, so it's closest first.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        // If ray hits something 
        if (hits.Length > 0)
        {
            System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance)); //Sort based on distance, closest first
            IClickable newHovered = null;

            foreach (RaycastHit hit in hits) //Then check if any of the objects are clickable
            {
                IClickable clickable = hit.collider.GetComponent<IClickable>();

                if (clickable != null) // if yes, set it to newly hovered object
                {
                    newHovered = clickable;
                    break;
                }
            }

            // if the newly hovered one is different from the old one
            if (newHovered != currentlyHovered)
            {
                //Unhover the old hovered object
                if (currentlyHovered != null)
                {
                    currentlyHovered.OnUnhover();
                }

                // Change what's being hovered currently to the new one
                currentlyHovered = newHovered;

                if (newHovered != null)
                {
                    currentlyHovered.OnHover();
                }
            }
        }
        else //if ray hits nothing
        {
            // Unhover the currently hovered object
            if (currentlyHovered != null)
            {
                currentlyHovered.OnUnhover();
                currentlyHovered = null;
            }
        }
    }

    // For handling clicking clickable objects
    void HandleClick()
    {
        if (Input.GetMouseButtonDown(0) && currentlyHovered != null)
        {
            currentlyHovered.OnClick();
        }
    }
}
