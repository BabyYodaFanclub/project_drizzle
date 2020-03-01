using UnityEditor;
using UnityEngine;
 
/// <summary>
/// Changes the scrollwheel behaviour from setting the movementspeed to jumping forward/backwards
/// </summary>
[InitializeOnLoad]
public class CameraScrollFix
{
    private const float CameraSpeed = -0.25f;
    private static bool _rmbDown = false;
 
    static CameraScrollFix()
    {
        SceneView.beforeSceneGui -= OnScene;
        SceneView.beforeSceneGui += OnScene;
    }
 
    private static void OnScene( SceneView scene )
    {
        Event e = Event.current;
        if( e.isMouse && e.button == 1 )
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    _rmbDown = true;
                    break;
                case EventType.MouseUp:
                    _rmbDown = false;
                    break;
            }
        }
       
        if( e.isScrollWheel && _rmbDown )
        {
            Vector3 pivot = scene.pivot;
            pivot += scene.camera.transform.forward * ( e.delta.y * CameraSpeed );
            scene.pivot = pivot;
 
            e.Use();
        }
    }
}