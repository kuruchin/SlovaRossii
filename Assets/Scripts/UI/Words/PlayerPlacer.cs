using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlacer : MonoBehaviour
{
    public GameObject[] PlayerIndicator;

    [SerializeField]
    private List<PlayerListing> _listings = new List<PlayerListing>();

    [SerializeField]
    private PlayerListing _playerListing;

    private void Update()
    {
        
    }
}
