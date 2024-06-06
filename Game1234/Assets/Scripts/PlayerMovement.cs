using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public Transform[] playerBodyParts; // Массив частей тела персонажа
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;

    public float leanAmount = 10f; // максимальный угол наклона вперед/назад
    public float leanSpeed = 2f; // скорость наклона
    public float cameraDistance = 5f; // отдаление камеры при движении
    public float cameraMoveSpeed = 2f; // скорость отдаления камеры
    public float swayAmount = 0.1f; // амплитуда колыхания
    public float swayFrequency = 2f; // частота колыхания
    public float sideLeanAmount = 1f; // максимальный угол наклона по сторонам
    public float bodyPartOffset = 0.1f; // Смещение частей тела для предотвращения наложения

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;
    private float targetLean = 0; // целевой угол наклона вперед/назад
    private float currentLean = 0; // текущий угол наклона вперед/назад
    private Vector3 originalCameraPosition; // исходное положение камеры
    private Vector3[] originalBodyPositions; // исходные положения частей тела
    private float sway = 0; // текущее значение колебания

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalCameraPosition = playerCamera.transform.localPosition; // сохраняем исходное положение камеры
        originalBodyPositions = new Vector3[playerBodyParts.Length]; // инициализируем массив

        // Сохраняем исходные положения всех частей тела и устанавливаем начальный поворот на -90 градусов по оси X
        for (int i = 0; i < playerBodyParts.Length; i++)
        {
            originalBodyPositions[i] = playerBodyParts[i].localPosition;
            playerBodyParts[i].localRotation = Quaternion.Euler(-90, 0, 0);
        }
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.R) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        // Управление наклоном тела персонажа вперед/назад
        if (curSpeedX != 0)
        {
            targetLean = Mathf.Sign(curSpeedX) * leanAmount;
        }
        else
        {
            targetLean = 0;
        }

        currentLean = Mathf.Lerp(currentLean, targetLean, Time.deltaTime * leanSpeed);

        // Добавление колыхания тела из стороны в сторону и наклона в зависимости от колыхания
        if (curSpeedX != 0 || curSpeedY != 0)
        {
            sway = Mathf.Sin(Time.time * swayFrequency) * swayAmount;
            float sideLean = Mathf.Sin(Time.time * swayFrequency) * sideLeanAmount;
            for (int i = 0; i < playerBodyParts.Length; i++)
            {
                float offset = i * bodyPartOffset; // Смещение каждой части тела
                playerBodyParts[i].localPosition = originalBodyPositions[i] + new Vector3(sway, offset, 0);
                playerBodyParts[i].localRotation = Quaternion.Euler(-90 + currentLean, 0, sideLean);
            }
        }
        else
        {
            sway = 0;
            for (int i = 0; i < playerBodyParts.Length; i++)
            {
                playerBodyParts[i].localPosition = Vector3.Lerp(playerBodyParts[i].localPosition, originalBodyPositions[i], Time.deltaTime * cameraMoveSpeed);
                playerBodyParts[i].localRotation = Quaternion.Euler(-90, 0, 0); // Возвращаем в исходное положение
            }
        }

        // Отдаление камеры при движении вперед
        Vector3 targetCameraPosition = originalCameraPosition;
        if (curSpeedX > 0)
        {
            targetCameraPosition -= new Vector3(0, 0, cameraDistance);
        }
        playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, targetCameraPosition, Time.deltaTime * cameraMoveSpeed);
    }
}