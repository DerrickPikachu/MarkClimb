using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLevelManager : MonoBehaviour
{
    public float levelOneCheckPoint = 20;
    public float levelTwoCheckPoint = 40;
    public float levelThreeCheckPoint = 60;
    public float levelFourCheckPoint = 80;
    public float levelFiveCheckPoint = 100;
    public GameObject levelUI;

    private int currentLevel = 0;
    private float bestHeight = 0;

    // Start is called before the first frame update
    void Start()
    {
        WriteLevelText();
    }

    // Update is called once per frame
    void Update()
    {
        bestHeight = Math.Max(bestHeight, transform.position.y);
        UpdateLevel();
        HandleKey();
    }

    void UpdateLevel()
    {
        if (currentLevel == 0 && bestHeight > levelOneCheckPoint) {
            currentLevel = 1;
            WriteLevelText();
        } else if (currentLevel == 1 && bestHeight > levelTwoCheckPoint) {
            currentLevel = 2;
            WriteLevelText();
        } else if (currentLevel == 2 && bestHeight > levelThreeCheckPoint) {
            currentLevel = 3;
            WriteLevelText();
        } else if (currentLevel == 3 && bestHeight > levelFourCheckPoint) {
            currentLevel = 4;
            WriteLevelText();
        } else if (currentLevel == 4 && bestHeight > levelFiveCheckPoint) {
            currentLevel = 5;
            WriteLevelText();
        }
    }

    void HandleKey()
    {
        // Level 1
        if (currentLevel >= 1 && Input.GetKeyDown(KeyCode.Z)) {
            Debug.Log("Flash");
            Flash();
        }
        // Level 2
        if (Input.GetKeyDown(KeyCode.X)) {

        }
        // Level 3
        if (Input.GetKeyDown(KeyCode.C)) {

        }
        // Level 4 
        if (Input.GetKeyDown(KeyCode.V)) {

        }
        // Level 5
        if (Input.GetKeyDown(KeyCode.B)) {

        }
    }

    void WriteLevelText()
    {
        TextMeshProUGUI text = levelUI.GetComponent<TextMeshProUGUI>();
        text.text = "Level " + currentLevel.ToString();
    }

    void Flash()
    {
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.RightArrow)) { moveDirection += new Vector3(0, 0, 1); }
        if (Input.GetKey(KeyCode.LeftArrow)) { moveDirection += new Vector3(0, 0, -1); }
        if (Input.GetKey(KeyCode.UpArrow)) { moveDirection += new Vector3(0, 1, 0); }
        if (Input.GetKey(KeyCode.DownArrow)) { moveDirection += new Vector3(0, -1, 0); }

        moveDirection.Normalize();
        if (moveDirection != Vector3.zero) {
            Vector3 target = DecideFlashDestination(moveDirection);
            transform.position = target;
        }
    }

    private Vector3 DecideFlashDestination(Vector3 moveDirection)
    {
        float moveDistance = 10;
        float blockLength = 3;
        float bonusFactor = 0.7f;
        Vector3 right = new Vector3(0, 0, 1);
        Vector3 left = new Vector3(0, 0, -1);
        Vector3 targetPos = transform.position + moveDirection * moveDistance;
        Vector3 finalPos = transform.position;
        Vector3 topPos = targetPos + new Vector3(0, blockLength * bonusFactor, 0);
        Vector3 rightPos = targetPos + new Vector3(0, 0, blockLength * bonusFactor);
        Vector3 leftPos = targetPos + new Vector3(0, 0, -blockLength * bonusFactor);

        if (!isPositionHasObject(targetPos)) {
            finalPos = targetPos;
        } else if (!isPositionHasObject(topPos)) {
            finalPos = topPos;
            Ray downRay = new Ray(finalPos, Vector3.down);
            RaycastHit downRayHit;
            if (Physics.Raycast(downRay, out downRayHit, moveDistance)) {
                if (downRayHit.distance > 0.1f) {
                    finalPos += Vector3.down * (downRayHit.distance - 0.1f);
                }
            }
        } else if (!isPositionHasObject(rightPos) && Vector3.Dot(moveDirection.normalized, right) > 0) {
            finalPos = rightPos;
        } else if (!isPositionHasObject(leftPos) && Vector3.Dot(moveDirection.normalized, left) > 0) {
            finalPos = leftPos;
        } else {
            // The raycast from player and move to the flash target
            Ray directRay = new Ray(transform.position, moveDirection);
            RaycastHit rayHit;
            if (Physics.Raycast(directRay, out rayHit, moveDistance)) {
                finalPos = transform.position + moveDirection * (rayHit.distance - blockLength / 2);
            }
        }

        return finalPos;
    }

    private bool isPositionHasObject(Vector3 position)
    {
        Vector3 rayStartPos = position + new Vector3(10, 0, 0);
        Ray cubeRay = new Ray(rayStartPos, position - rayStartPos);
        RaycastHit rayHit;
        if (Physics.Raycast(cubeRay, out rayHit)) {
            Debug.Log(rayHit.collider.gameObject.name);
            if (rayHit.collider.gameObject.name.IndexOf("BackGround") == -1) {
                return true;
            }
        }
        return false;
    }
}
