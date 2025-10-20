using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseShipMenuManager : MonoBehaviour
{
    public GameObject chooseShipContainer;
    public GameObject shipSelectionPrefab;

    private Action m_onMenuClosed;
    private Action<Buff> m_onBuffSelected;
    private Action<ShipEntry> m_onShipSelected;

    public void Init(Action onMenuClosed, Action<Buff> onBuffSelected, Action<ShipEntry> onShipSelected)
    {
        m_onMenuClosed = onMenuClosed;
        m_onBuffSelected = onBuffSelected;
        m_onShipSelected = onShipSelected;
    }

    // Start is called before the first frame update
    public void Set(ShipDatabase shipDatabase)
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
                ssm.Set(shipEntry, OnBuffSelected, OnShipSelected);
        }
    }

    void OnBuffSelected(Buff buff)
    {
        // propagate the event
        m_onBuffSelected?.Invoke(buff);
    }

    // Update is called once per frame
    void OnDisable()
    {
        // remove all existing children
        foreach (Transform child in chooseShipContainer.transform)
            Destroy(child.gameObject);
    }

    public void CloseMenu()
    {
        m_onMenuClosed?.Invoke();
    }

    public void OnShipSelected(ShipEntry shipEntry)
    {
        m_onShipSelected?.Invoke(shipEntry);
    }
}
