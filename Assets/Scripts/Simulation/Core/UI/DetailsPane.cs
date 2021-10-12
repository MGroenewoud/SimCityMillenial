using TMPro;
using UnityEngine;

public class DetailsPane : MonoBehaviour
{
    private IFocus Focus;
    private TextMeshProUGUI DetailsText;

    void Start()
    {
        DetailsText = GetComponentInChildren<TextMeshProUGUI>(true);
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            RemoveFocus();
        } else if (Focus != null)
        {
            DetailsText.text = Focus.GetDetailsString();
        }
    }

    public void SetFocus(IFocus focus)
    {
        Focus = focus;
        gameObject.SetActive(true);
    }

    public void RemoveFocus()
    {
        RemoveDetails();
        Focus = null;
        gameObject.SetActive(false);
    }

    private void RemoveDetails()
    {
        DetailsText.text = "";
    }

}