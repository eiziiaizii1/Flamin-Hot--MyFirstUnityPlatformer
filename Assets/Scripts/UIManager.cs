using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject peperParent;
    private PlayerController playerController;

    public GameObject gameOverMenu;

    public Image healthBar;
    public float healthAmount = 100f;

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
        HandlePaperIcons();
        HandleHealtHUD();
    }

    private void HandlePaperIcons()
    {
        if (playerController.pepperAmount > 0 && playerController.pepperAmount <= playerController.maxPepperAmount)
        {
            peperParent.transform.GetChild(playerController.pepperAmount - 1).gameObject.SetActive(true);
        }

        if (playerController.isFired)
        {
            peperParent.transform.GetChild(playerController.pepperAmount).gameObject.SetActive(false);
        }
    }

    private void HandleHealtHUD()
    {
        healthAmount = Mathf.Clamp(playerController.currentHealth, 0, playerController.maxHealth);
        healthBar.fillAmount = healthAmount / playerController.maxHealth;
    }
}
