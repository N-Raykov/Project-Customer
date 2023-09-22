using UnityEngine;

public class AnimationEventBridge : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObject;

    [SerializeField]
    private Object targetComponent;

    [SerializeField]
    private string selectedMethodName;

    public void ForwardEvent(string methodName = null)
    {
        if (targetObject != null && targetComponent != null && !string.IsNullOrEmpty(selectedMethodName))
        {
            System.Reflection.MethodInfo method = targetComponent.GetType().GetMethod(selectedMethodName);

            if (method != null && selectedMethodName == methodName)
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