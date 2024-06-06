using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThroughManager : MonoBehaviour
{
    public string tagToIgnore = "PassThrough"; // Тег для объектов, через которые нужно проходить

    private List<Collider> colliders;

    void Start()
    {
        colliders = new List<Collider>();

        // Находим все объекты с заданным тегом и сохраняем их коллайдеры
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tagToIgnore);
        foreach (GameObject obj in objects)
        {
            Collider col = obj.GetComponent<Collider>();
            if (col != null)
            {
                colliders.Add(col);
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // Отключаем все коллайдеры
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }
        }
        else
        {
            // Включаем все коллайдеры
            foreach (Collider col in colliders)
            {
                col.enabled = true;
            }
        }
    }
}