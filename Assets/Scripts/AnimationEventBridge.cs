using UnityEngine;

public class AnimationEventBridge : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObject;

    [SerializeField]
    private Object targetComponent;

    [SerializeField]
    private string selectedMethodName;

    public void ForwardEvent()
    {
        if (targetObject != null && targetComponent != null && !string.IsNullOrEmpty(selectedMethodName))
        {
            // Get the method info using the selected method name.
            System.Reflection.MethodInfo method = targetComponent.GetType().GetMethod(selectedMethodName);

            if (method != null)
            {
                method.Invoke(targetComponent, null);
            }
            else
            {
                Debug.LogError("Method " + selectedMethodName + " not found on the target component.");
            }
        }
        else
        {
            Debug.LogError("Target object, target component, or target method is not set.");
        }
    }
}