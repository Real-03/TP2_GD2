using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class OxygenShop : MonoBehaviour
{
    public int oxygenToRestore = 100;
    public TextMeshPro shopText;

    private string requiredMineral;
    private int requiredAmount = 1;

    private string[] minerals = { "Emerald", "Diamond", "iron" };

    private void Start()
    {
        ChooseRandomRequirement();
        UpdateShopUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TileDestroyer inventory = other.GetComponent<TileDestroyer>();
            OxygenSystem playerOxygen = other.GetComponent<OxygenSystem>();

            if (inventory != null && playerOxygen != null)
            {
                if (inventory.HasItem(requiredMineral, requiredAmount))
                {
                    inventory.RemoveItem(requiredMineral, requiredAmount);
                    playerOxygen.RestoreOxygen(oxygenToRestore);

                    requiredAmount++; // Aumenta dificuldade
                    ChooseRandomRequirement();
                    UpdateShopUI();

                    Debug.Log($"Compra bem-sucedida! Oxigênio restaurado. Nova exigência: {requiredAmount} {requiredMineral}");
                }
                else
                {
                    inventory.FlashMineralUI(requiredMineral);
                    Debug.Log("Você não tem os minérios necessários!");
                }
            }
        }
    }

    void ChooseRandomRequirement()
    {
        int index = Random.Range(0, minerals.Length);
        requiredMineral = minerals[index];
    }

    void UpdateShopUI()
    {
        if (shopText != null)
        {
            shopText.text = $"Loja de Oxigênio\nPrecisa de: {requiredAmount} {requiredMineral}(s)";
        }
    }
}