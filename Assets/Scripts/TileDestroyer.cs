using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Collections;
public class TileDestroyer : MonoBehaviour
{
    public float destroyRadius = 1.5f;
    private TextMeshProUGUI inventoryText;
    // Inventário simples usando dicionário
    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    public bool HasItem(string itemName, int amount)
    {
        return inventory.ContainsKey(itemName) && inventory[itemName] >= amount;
    }

    public void RemoveItem(string itemName, int amount)
    {
        if (HasItem(itemName, amount))
        {
            inventory[itemName] -= amount;

            UpdateUI(itemName.ToUpper(), inventory[itemName]);
        }
    }

    public void AddItem(string itemName, int amount)
    {
        if (!inventory.ContainsKey(itemName))
            inventory[itemName] = 0;

        inventory[itemName] += amount;
        UpdateUI(itemName.ToUpper(), inventory[itemName]);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clique esquerdo
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                Tilemap tilemap = hit.collider.GetComponent<Tilemap>();

                if (tilemap != null)
                {
                    Vector3Int cellPos = tilemap.WorldToCell(mouseWorldPos);
                    Vector3 cellCenter = tilemap.GetCellCenterWorld(cellPos);
                    float distance = Vector3.Distance(transform.position, cellCenter);

                    if (distance <= destroyRadius && tilemap.HasTile(cellPos))
                    {
                        TileBase tile = tilemap.GetTile(cellPos);
                        if (tile != null)
                        {
                            string tileName = tile.name;

                            // Adiciona ao inventário
                            if (!inventory.ContainsKey(tileName))
                            {
                                inventory[tileName] = 0;
                            }

                            inventory[tileName]++;
                            Debug.Log($"Coletado: {tileName} (Total: {inventory[tileName]})");
                            UpdateUI(tileName,inventory[tileName]);

                        }

                        // Remove o tile
                        tilemap.SetTile(cellPos, null);
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, destroyRadius);
    }

    private void UpdateUI(string mineral, int amount)
    {
        GameObject uiObject = GameObject.Find(mineral.ToUpper());

        if (uiObject != null)
        {
            // Pegar o componente TextMeshProUGUI
            inventoryText = uiObject.GetComponent<TextMeshProUGUI>();

            if (inventoryText != null)
            {
                inventoryText.text = amount.ToString();
            }
        }
    }
    public void FlashMineralUI(string mineral)
    {
        GameObject uiObject = GameObject.Find(mineral.ToLower());

        if (uiObject != null)
        {
            Image image = uiObject.GetComponent<Image>();
            if (image != null)
            {
                StartCoroutine(FlashImage(image));
            }
            else
            {
                Debug.LogWarning("O objeto encontrado não tem um componente Image.");
            }
        }
        else
        {
            Debug.LogWarning($"UI com nome '{mineral}' não encontrada.");
        }
    }

    private IEnumerator FlashImage(Image image)
    {
        Color originalColor = image.color;
        Color flashColor = Color.red;

        for (int i = 0; i < 2; i++)
        {
            image.color = flashColor;
            yield return new WaitForSeconds(0.1f);
            image.color = originalColor;
            yield return new WaitForSeconds(0.2f);
        }
    }
}