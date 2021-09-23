using System;
using System.Text;
using TMPro;
using UnityEngine;

public class PersonDetails : MonoBehaviour
{
    private Person FocusedPerson;
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
        } else
        {
            BuildDetails();
        }
    }

    public void Focus(Person focus)
    {
        FocusedPerson = focus;
        gameObject.SetActive(true);
    }

    public void RemoveFocus()
    {
        RemoveDetails();
        gameObject.SetActive(false);
    }

    private void RemoveDetails()
    {
        DetailsText.text = "";
    }

    private void BuildDetails()
    {
        DetailsText.text = BuildNeeds();
    }

    private string BuildNeeds()
    {
        var needs = new StringBuilder();

        needs.AppendLine("State: " + FocusedPerson.StateMachine.ActiveState);

        foreach(var need in FocusedPerson.Needs.Needs)
        {
            needs.AppendLine(string.Format("{0}: {1}", need.Key, need.Value.Weight));
        }

        return needs.ToString();
    }
}