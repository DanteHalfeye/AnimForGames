using UnityEngine;

public class PlayerHitboxController : MonoBehaviour
{
    [SerializeField] private GameObject[] hitboxes;
    
    public void TogglePlayerHitboxes(int attackId)
    {
        for (int hitboxId = 0; hitboxId < hitboxes.Length; hitboxId++)
        {
            GameObject hitbox = hitboxes[hitboxId];
            hitbox.SetActive(!hitbox.activeSelf);
            print(hitbox.activeSelf);
        }
    }

    public void CleanupPlayerHitboxes()
    {
        foreach (GameObject colliders in hitboxes)
        {
            colliders.SetActive(true);
        }
    }
}
