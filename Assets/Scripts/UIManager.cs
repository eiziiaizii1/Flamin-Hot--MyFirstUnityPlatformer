using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject peperParent;
    private PlayerController playerController;

    public GameObject gameOverMenu;

    private void OnEnable()
    {
        PlayerController.OnPlayerDeath += EnableGameOverMenu;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDeath -= EnableGameOverMenu;
    }

    public void EnableGameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (playerController.pepperAmount > 0 && playerController.pepperAmount <= playerController.maxPepperAmount)
        {
            peperParent.transform.GetChild(playerController.pepperAmount-1).gameObject.SetActive(true);
        }

        if (playerController.isFired)
        {
            peperParent.transform.GetChild(playerController.pepperAmount).gameObject.SetActive(false);
        }
    }
}
