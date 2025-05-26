using UnityEngine;

public class FollowPlayerWhenMoving : MonoBehaviour
{
    public Transform player;           // Referência ao jogador
    public float followSpeed = 10f;    // Velocidade de seguir
    private Vector3 offset;            // Distância fixa entre câmera e jogador
    private Vector3 lastPlayerPosition;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Jogador não atribuído!");
            enabled = false;
            return;
        }

        // Calcula o offset inicial entre a câmera e o jogador
        offset = transform.position - player.position;
        lastPlayerPosition = player.position;
    }

    void LateUpdate()
    {
        if (player.position != lastPlayerPosition)
        {
            transform.position = player.position + offset;
        }

        lastPlayerPosition = player.position;
    }
}