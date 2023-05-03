using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour
{
    [SerializeField] AudioSource TickButtonClicked;
    [SerializeField] AudioSource TickMouseEnter;

    public void PlayerSoundMouseEnter()
    {
        TickMouseEnter.Play();
    }
    public void PlayerSoundMouseClick()
    {
        TickButtonClicked.Play();
    }
}
