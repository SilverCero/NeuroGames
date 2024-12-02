using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatedGaze : MonoBehaviour
{
    public CubeGameController gameController;  // Reference to CubeGameController
    private Renderer _currentTargetRenderer;
    private Renderer _previousTargetRenderer; // To keep track of the previous target
    private Color _originalColor = Color.blue;
    private Color _targetColor = Color.magenta;
    private Vector3 _originalScale = new Vector3(2f, 2f, 1f);
    private Vector3 _targetScale = new Vector3(3f, 3f, 1f);
    private float _transitionSpeed = .2f;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Check if the hit object has the tag "testo" (not selected yet)
            if (hit.collider.CompareTag("testo") || hit.collider.CompareTag("reseto"))
            {
                Renderer targetRenderer = hit.collider.GetComponent<Renderer>();

                // If we have a new target
                if (_currentTargetRenderer != targetRenderer)
                {
                    // Keep track of the previous target
                    _previousTargetRenderer = _currentTargetRenderer;
                    _currentTargetRenderer = targetRenderer;
                    _originalColor = _currentTargetRenderer.material.color; // Store the original color
                }

                // Gradually scale and change the color of the new gazed object
                _currentTargetRenderer.transform.localScale = Vector3.Lerp(_currentTargetRenderer.transform.localScale, _targetScale, Time.deltaTime * _transitionSpeed);
                _currentTargetRenderer.material.color = Color.Lerp(_currentTargetRenderer.material.color, _targetColor, Time.deltaTime * _transitionSpeed);

                // Call the game controller when the size threshold is met
                if (Vector3.Distance(_currentTargetRenderer.transform.localScale, _targetScale) < 0.05f)  // Threshold 0.05f
                {
                    gameController.OnCubeSelected(hit.collider.gameObject);
                }
            }
        }

        // Smoothly reset the previous target if there was one
        if (_previousTargetRenderer != null)
        {
            // Only reset if the previous cube hasn't been selected (tagged with "X" or "O")
            if (_previousTargetRenderer.gameObject.CompareTag("testo"))
            {
                _previousTargetRenderer.transform.localScale = Vector3.Lerp(_previousTargetRenderer.transform.localScale, _originalScale, Time.deltaTime * _transitionSpeed);
                _previousTargetRenderer.material.color = Color.Lerp(_previousTargetRenderer.material.color, _originalColor, Time.deltaTime * _transitionSpeed);

                // If the previous target has returned to its original state, stop modifying it
                if (Vector3.Distance(_previousTargetRenderer.transform.localScale, _originalScale) < 0.01f && _previousTargetRenderer.material.color == _originalColor)
                {
                    _previousTargetRenderer = null; // Stop tracking once it's reset
                }
            }
        }
    }
}
