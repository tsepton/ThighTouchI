using System;
using UnityEngine;

public class ObjectMoverCondCoord : MonoBehaviour
{
    // GameObject à déplacer
    public GameObject objectToMove;
    
    public RectTransform canvas;

    public GameObject implementation;

    public GameObject informationGuide;

    // Booléen pour suivre si le déplacement a déjà eu lieu
    private bool hasMoved = false;

    // Start is called before the first frame update
    void Start()
    {
        // Désactiver l'objet initialement
        objectToMove.SetActive(false);
    
        // S'abonner à l'événement OnTouch du Touchpad
        Touchpad.OnTouch += HandleTouch;
    }

    void HandleTouch(TouchPoint touchPoint)
    {
        // Vérifier si le déplacement a déjà eu lieu
        if (!implementation.activeSelf){
        if(!touchPoint.isLast){
        if (!hasMoved )
        { 
            // Activer l'objet à déplacer
            
            informationGuide.SetActive(false);
            Vector2 canvasSize = canvas.sizeDelta;
            
            float touchX = (-0.5f + touchPoint.coordinates.x) * canvasSize.x / 5f;
            float touchY = (-0.5f + touchPoint.coordinates.y) * canvasSize.y / 5f;
            var coord = Rotation90Degrees(touchX,-touchY);
            
            float z = objectToMove.transform.localPosition.z;
            if (-touchY < -36f){
            objectToMove.SetActive(true);
            Vector3 touchPosition = new Vector3(coord.Item1, coord.Item2, z);

            // Déplacer l'objet à l'emplacement de l'entrée tactile
            objectToMove.transform.localPosition = touchPosition;

            // Marquer que le déplacement a eu lieu
            hasMoved = true;
            }
        }} else {
            LockSphere1[] lockSpheres = objectToMove.GetComponentsInChildren<LockSphere1>();
            foreach (LockSphere1 lockSphere in lockSpheres)
            {
                lockSphere.isGreen = false;
            }
            informationGuide.SetActive(true);
            objectToMove.SetActive(false);
            hasMoved = false;
              
            
        }
        }
        
        
    }
        public static Tuple<float, float> Rotation90Degrees(float x, float y)
    {
        float newX = -y;
        float newY = x;
        return Tuple.Create(newX, newY);
    }

}