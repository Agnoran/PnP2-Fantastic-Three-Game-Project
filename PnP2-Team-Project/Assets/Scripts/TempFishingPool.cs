using UnityEngine;

/*
 * TEMPORARY / PROTOTYPE CODE
 * -------------------------
 * This script was generated with AI assistance and exists ONLY to support
 * early integration testing (movement, triggers, and basic flow).
 *
 * This is NOT intended to be used in the final project.
 * This code is expected to be replaced, refactored, or deleted.
 *
 * Do not build gameplay systems, tuning, or architecture on top of this.
 * Final player control logic will be authored by the assigned developer.
 * 
 * * If you are reading this in the future and it's still here — something went wrong
 */

[RequireComponent(typeof(Collider))]
public class TempFishingPool : MonoBehaviour
{
    [Header("TEMP / STUB")]
    [Tooltip("Temporary fish definition used to allow end-to-end flow. Replace with real pool logic.")]
    [SerializeField] FishDefinition stubFishDefinition;

    [Header("Trigger Settings")]
    [Tooltip("Tag used to identify the player boat")]
    [SerializeField] string playerTag = "Player";

    private void Reset()
    {
        // Ensure trigger is correctly set up
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
        {
            return;
        }

        if (WorldController.instance != null)
        {
            WorldController.instance.EnterPool(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag))
        {
            return;
        }

        if (WorldController.instance != null)
        {
            WorldController.instance.ExitPool();
        }
    }

    /// <summary>
    /// TEMP: Generates a single FishInstance so the game can function end-to-end.
    /// Jose will replace this entire method with real pool logic.
    /// </summary>
    public FishInstance GenerateFishToAttempt()
    {
        if (stubFishDefinition == null)
        {
            return null;
        }

        float rolledSize = Random.Range(
            stubFishDefinition.SizeMin,
            stubFishDefinition.SizeMax
        );

        int rolledQuality = Random.Range(0, 101);
        int rolledValue = stubFishDefinition.BaseValue;
        float rolledSpoilTime = stubFishDefinition.BaseSpoilTime;

        return new FishInstance(
            stubFishDefinition,
            rolledSize,
            rolledQuality,
            rolledValue,
            rolledSpoilTime
        );
    }
}