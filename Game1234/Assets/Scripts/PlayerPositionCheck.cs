using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerPositionCheck : MonoBehaviour
{
    public Vector3 teleportLocation; // Место для телепортации

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Teleport(other.gameObject);
        }
    }

    void Teleport(GameObject player)
    {
        // Находим компонент тела игрока
        Rigidbody playerRigidbody = player.GetComponentInChildren<Rigidbody>();
        
        // Проверяем, найден ли компонент тела игрока
        if (playerRigidbody != null)
        {
            // Телепортируем игрока в заданное место
            playerRigidbody.transform.position = teleportLocation;
            Debug.Log("Player teleported to: " + teleportLocation);
        }
        else
        {
            Debug.LogError("Player body not found for teleportation!");
        }
    }
}