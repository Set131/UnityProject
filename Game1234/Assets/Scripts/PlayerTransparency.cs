using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransparency : MonoBehaviour
{
    public List<Renderer> bodyParts; // Список рендереров частей тела
    public float transparency = 0.5f; // Уровень прозрачности (0 = полностью прозрачный, 1 = полностью непрозрачный)

    private List<Material> originalMaterials = new List<Material>(); // Оригинальные материалы частей тела
    private List<Color> originalColors = new List<Color>(); // Оригинальные цвета частей тела

    void Start()
    {
        // Сохраняем оригинальные материалы и цвета частей тела
        foreach (Renderer renderer in bodyParts)
        {
            foreach (var material in renderer.materials)
            {
                originalMaterials.Add(material);
                originalColors.Add(material.color);
            }
        }
    }

    void Update()
    {
        // Проверка нажатия левой клавиши мыши
        if (Input.GetMouseButton(0))
        {
            SetTransparency(transparency);
        }
        else
        {
            ResetTransparency();
        }
    }

    void SetTransparency(float alpha)
    {
        // Устанавливаем прозрачность для всех частей тела
        foreach (Renderer renderer in bodyParts)
        {
            foreach (var material in renderer.materials)
            {
                Color color = material.color;
                color.a = alpha;
                material.color = color;
            }
        }
    }

    void ResetTransparency()
    {
        // Восстанавливаем оригинальные цвета для всех частей тела
        int index = 0;
        foreach (Renderer renderer in bodyParts)
        {
            foreach (var material in renderer.materials)
            {
                Color color = originalColors[index];
                material.color = color;
                index++;
            }
        }
    }
}