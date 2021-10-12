using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFocus : MonoBehaviour
{
    private TilePreviewComponent PreviewComponent;

    void Start()
    {
        PreviewComponent = FindObjectOfType<TilePreviewComponent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !PreviewComponent.HasSelection)
        {
            Debug.Log("clicked grid");

            // check if previewcomponent has a tileselector. in that case, do nothing
            var gridcellClickedOn = GeneralUtility.GetGridLocationOfMouse();
            if (SimulationCore.Instance.AllWorkplaces.ContainsKey(gridcellClickedOn))
            {
                var workplaceClickedIn = SimulationCore.Instance.AllWorkplaces[gridcellClickedOn];
                FindObjectOfType<DetailsPane>(true).SetFocus(new WorkplaceDetails(workplaceClickedIn));
            }
        }
    }
}
