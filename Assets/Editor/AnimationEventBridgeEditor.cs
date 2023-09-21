using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(AnimationEventBridge))]
public class AnimationEventBridgeEditor : Editor
{
    private SerializedProperty targetObjectProperty;
    private SerializedProperty targetComponentProperty;
    private SerializedProperty selectedMethodNameProperty;

    private void OnEnable()
    {
        targetObjectProperty = serializedObject.FindProperty("targetObject");
        targetComponentProperty = serializedObject.FindProperty("targetComponent");
        selectedMethodNameProperty = serializedObject.FindProperty("selectedMethodName");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(targetObjectProperty);

        if (targetObjectProperty.objectReferenceValue != null)
        {
            MonoBehaviour[] components = ((GameObject)targetObjectProperty.objectReferenceValue).GetComponents<MonoBehaviour>();
            string[] componentNames = components
                .Where(c => c != null)
                .Select(c => c.GetType().Name)
                .ToArray();

            int selectedComponentIndex = GetSelectedComponentIndex(components, targetComponentProperty.objectReferenceValue as MonoBehaviour);
            selectedComponentIndex = EditorGUILayout.Popup("Target Component", selectedComponentIndex, componentNames);

            targetComponentProperty.objectReferenceValue = selectedComponentIndex >= 0 && selectedComponentIndex < components.Length ? components[selectedComponentIndex] : null;

            if (components.Length > 0)
            {
                List<string> methodNames = GetPublicMethodNames(targetComponentProperty.objectReferenceValue as MonoBehaviour);
                int selectedMethodIndex = methodNames.IndexOf(selectedMethodNameProperty.stringValue);

                selectedMethodIndex = EditorGUILayout.Popup("Target Method", selectedMethodIndex, methodNames.ToArray());
                selectedMethodNameProperty.stringValue = selectedMethodIndex >= 0 ? methodNames[selectedMethodIndex] : "";

                // Add a button to invoke the method immediately during runtime.
                if (GUILayout.Button("Invoke Method"))
                {
                    AnimationEventBridge targetScript = (AnimationEventBridge)target;
                    if (targetScript != null)
                    {
                        targetScript.ForwardEvent();
                    }
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private int GetSelectedComponentIndex(MonoBehaviour[] components, MonoBehaviour selectedComponent)
    {
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] == selectedComponent)
            {
                return i;
            }
        }
        return -1;
    }

    private List<string> GetPublicMethodNames(MonoBehaviour component)
    {
        List<string> methodNames = new List<string>();
        if (component != null)
        {
            MethodInfo[] methods = component.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);

            foreach (MethodInfo method in methods)
            {
                if (method.DeclaringType == component.GetType() && !method.IsSpecialName)
                {
                    methodNames.Add(method.Name);
                }
            }
        }
        return methodNames;
    }
}