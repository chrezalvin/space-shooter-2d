using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseShipMenuManager : MonoBehaviour
{
    public GameObject chooseShipContainer;
    public GameObject shipSelectionPrefab;
    public ShipDatabase shipDatabase;

    // Start is called before the first frame update
    void OnEnable()
    {
        // remove all existing children
        foreach (Transform child in chooseShipContainer.transform)
            Destroy(child.gameObject);

        // instantiate ship selection prefabs for each ship in the database
        foreach (var shipEntry in shipDatabase.AllShips())
        {
            GameObject shipSelectionObject = Instantiate(shipSelectionPrefab, chooseShipContainer.transform);
            ShipSelectionManager ssm = shipSelectionObject.GetComponent<ShipSelectionManager>();
            if (ssm)
                ssm.Set(shipEntry);
        }

        ShipSelectionManager.OnShipSelected += OnShipSelected;
    }

    // Update is called once per frame
    void OnDisable()
    {
        // remove all existing children
        foreach (Transform child in chooseShipContainer.transform)
            Destroy(child.gameObject);

        ShipSelectionManager.OnShipSelected -= OnShipSelected;
    }

    public void CloseMenu()
    {
        this.gameObject.SetActive(false);
    }

    public void OnShipSelected(string _)
    {
        // updates the ship selection UI, assuming index is the same like database
        for (int iii = 0; iii < shipDatabase.AllShips().Count; ++iii)
        {
            var entry = shipDatabase.AllShips()[iii];
            GameObject shipSelectionObject = chooseShipContainer.transform.GetChild(iii).gameObject;
            ShipSelectionManager ssm = shipSelectionObject.GetComponent<ShipSelectionManager>();
            if (ssm)
                ssm.Set(entry);
        }
    }
}
