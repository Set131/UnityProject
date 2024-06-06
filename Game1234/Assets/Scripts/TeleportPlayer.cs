using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public GameObject player; // Ссылка на объект игрока, к которому прикреплены все части тела и камера
    public Vector3 teleportLocation; // Место для телепортации
    private float checkRadius = 3.0f; // Радиус проверки вокруг объекта

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F key pressed.");
            if (IsPlayerInZone())
            {
                Debug.Log("Player is in the zone. Teleporting...");
                Teleport();
            }
            else
            {
                Debug.Log("Player is not in the zone.");
            }
        }
    }

    bool IsPlayerInZone()
    {
        // Проверка, находится ли игрок в радиусе checkRadius относительно объекта
        float distance = Vector3.Distance(transform.position, player.transform.position) + 1;
        Debug.Log("Distance to player: " + distance);
        return distance <= checkRadius;
    }

    void Teleport()
    {
        // Телепортируем игрока в заданное место
        player.transform.position = teleportLocation;
        Debug.Log("Player teleported to: " + teleportLocation);
    }

    void OnDrawGizmosSelected()
    {
        // Рисуем сферу в редакторе Unity для визуализации радиуса проверки
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}