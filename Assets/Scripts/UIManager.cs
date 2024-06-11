using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    private PlayerController playerController;



    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (playerController.pepperAmount > 0 && playerController.pepperAmount <= playerController.maxPepperAmount)
        {
            transform.GetChild(playerController.pepperAmount-1).gameObject.SetActive(true);
        }

        if (playerController.isFired)
        {
            transform.GetChild(playerController.pepperAmount).gameObject.SetActive(false);
        }
    }
}
