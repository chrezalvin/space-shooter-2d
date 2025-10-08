using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipDatabase", menuName = "Game/Ship Database")]
public class ShipDatabase : ScriptableObject
{
    [SerializeField] private List<ShipEntry> allShips;

    public List<ShipEntry> AllShips()
    {
        List<ShipEntry> allShipsCopy = new List<ShipEntry>();
        foreach (var entry in this.allShips)
            allShipsCopy.Add(CopyShipEntry(entry)); 

        return allShipsCopy;
    }

    public ShipEntry GetShipByName(string shipName = "Default")
    {
        foreach (var entry in allShips)
        {
            if (entry.shipName == shipName)
                return CopyShipEntry(entry);
        }
        return null;
    }

    public ShipEntry CopyShipEntry(ShipEntry entry)
    {
        
        ShipEntry newEntry = new ShipEntry();
        newEntry.ship = Instantiate(entry.ship);
        newEntry.shipName = entry.shipName;
        newEntry.shipPrefab = entry.shipPrefab;
        newEntry.shipIcon = entry.shipIcon;
        return newEntry;
    }
}
