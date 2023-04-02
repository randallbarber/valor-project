using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerRoomInfo : MonoBehaviour
{
    [SerializeField] TMP_Text killCounter;
    int killCount;
    
    public void KilledPlayer()
    {
        killCount += 1;
        killCounter.text = killCount.ToString();
    }
}
