using UnityEngine;
using UnityEngine.UI; // necessário para Image

public class OxygenSystem : MonoBehaviour
{
    [Header("Configuração de Oxigênio")]
    public float maxOxygen = 100f;
    public float currentOxygen;
    public float oxygenDecreaseRate = 5f;
    public float dangerZoneY = -1f;

    [Header("UI")]
    [SerializeField] private Image oxygenBar; // arraste aqui a Image do tipo Filled no Inspector

    private void Start()
    {
        currentOxygen = maxOxygen;

        UpdateUI();
    }

    void Update()
    {
        if (transform.position.y < dangerZoneY)
        {
            currentOxygen -= oxygenDecreaseRate * Time.deltaTime;
            currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);
        }

        UpdateUI();

        if (currentOxygen <= 0)
        {
            Debug.Log("Ficou sem oxigênio!");
        }
    }

    public void RefillOxygen(float amount)
    {
        currentOxygen = Mathf.Clamp(currentOxygen + amount, 0, maxOxygen);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (oxygenBar != null)
        {
            // Atualiza o fillAmount com base no percentual de oxigênio restante
            oxygenBar.fillAmount = currentOxygen / maxOxygen;
        }
    }

    public void RestoreOxygen(int amount)
    {
        currentOxygen = Mathf.Min(currentOxygen + amount, maxOxygen);
        Debug.Log("Oxigênio restaurado: " + currentOxygen);
    }
}