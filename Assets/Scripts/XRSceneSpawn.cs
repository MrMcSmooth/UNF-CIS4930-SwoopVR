
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;

public class XRSceneSpawn : MonoBehaviour
{
    private XROrigin xrOrigin;

    void Awake()
    {
        xrOrigin = GetComponent<XROrigin>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode != LoadSceneMode.Single)
            return;

        foreach (GameObject root in scene.GetRootGameObjects())
        {
            SceneSpawnPoint spawn =
                root.GetComponentInChildren<SceneSpawnPoint>(true);

            if (spawn != null)
            {
                StartCoroutine(TeleportPlayer(spawn.transform));
                return;
            }
        }

        Debug.LogWarning("No spawn point found in " + scene.name);
    }

    IEnumerator TeleportPlayer(Transform spawn)
    {
        yield return null;

        if (xrOrigin == null || xrOrigin.Camera == null)
            yield break;

        CharacterController controller =
            GetComponent<CharacterController>();

        bool wasEnabled =
            controller != null && controller.enabled;

        if (wasEnabled)
            controller.enabled = false;

        float headHeight =
            xrOrigin.Camera.transform.position.y -
            xrOrigin.transform.position.y;

        xrOrigin.MatchOriginUpCameraForward(
            Vector3.up,
            spawn.forward
        );

        xrOrigin.MoveCameraToWorldLocation(
            spawn.position + Vector3.up * headHeight
        );

        if (wasEnabled)
            controller.enabled = true;

        Debug.Log("Teleported player to " + spawn.name);
    }
}
