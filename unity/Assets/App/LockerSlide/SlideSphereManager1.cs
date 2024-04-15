using System;
using UnityEngine;
public class SlideObjectMover : MonoBehaviour
{
    // GameObject à déplacer
    public GameObject objectToMove1;
    public GameObject objectToMove2;
    public GameObject objectToMove3;

    
    public RectTransform canvas;

    public GameObject implementation;

    // Booléen pour suivre si le déplacement a déjà eu lieu
    private bool hasMoved1 = false;
    private bool hasMoved2 = false;
    private bool hasMoved3 = false;

    // Start is called before the first frame update
    void Start()
    {
        // Désactiver l'objet initialement
        objectToMove1.SetActive(false);
                    objectToMove2.SetActive(false);
                    objectToMove3.SetActive(false);
    
        // S'abonner à l'événement OnTouch du Touchpad
        Touchpad.OnTouch += HandleTouch;
    }

    void HandleTouch(TouchPoint touchPoint)
    {
        Debug.Log(touchPoint.id);
        // Vérifier si le déplacement a déjà eu lieu
        if (!implementation.activeSelf){
            if(!touchPoint.isLast){
                if (!hasMoved1 & touchPoint.id == 1)
                    { 
                        // Activer l'objet à déplacer
                        objectToMove1.SetActive(true);
                        
                        Vector2 canvasSize = canvas.sizeDelta;
                        
                        float touchX = (-0.5f + touchPoint.coordinates.x) * canvasSize.x / 5f;
                        float touchY = (-0.5f + touchPoint.coordinates.y) * canvasSize.y / 5f;
                        var coord = Rotation90Degrees(touchX,-touchY);
                        float z = objectToMove1.transform.localPosition.z;
                        
                        Vector3 touchPosition = new Vector3(coord.Item1, coord.Item2, z);

                        // Déplacer l'objet à l'emplacement de l'entrée tactile
                        objectToMove1.transform.localPosition = touchPosition;

                        // Marquer que le déplacement a eu lieu
                        hasMoved1 = true;
                    } else if (!hasMoved2 & touchPoint.id == 2){
                         objectToMove2.SetActive(true);
                        
                        Vector2 canvasSize = canvas.sizeDelta;
                        
                        float touchX = (-0.5f + touchPoint.coordinates.x) * canvasSize.x / 5f;
                        float touchY = (-0.5f + touchPoint.coordinates.y) * canvasSize.y / 5f;
                        var coord = Rotation90Degrees(touchX,-touchY);
                        float z = objectToMove2.transform.localPosition.z;
                        
                        Vector3 touchPosition = new Vector3(coord.Item1, coord.Item2, z);

                        // Déplacer l'objet à l'emplacement de l'entrée tactile
                        objectToMove2.transform.localPosition = touchPosition;

                        // Marquer que le déplacement a eu lieu
                        hasMoved2 = true;
                    } else if (!hasMoved3 & touchPoint.id == 3){
                         objectToMove3.SetActive(true);
                        
                        Vector2 canvasSize = canvas.sizeDelta;
                        
                        float touchX = (-0.5f + touchPoint.coordinates.x) * canvasSize.x / 5f;
                        float touchY = (-0.5f + touchPoint.coordinates.y) * canvasSize.y / 5f;
                        var coord = Rotation90Degrees(touchX,-touchY);
                        float z = objectToMove3.transform.localPosition.z;
                        
                        Vector3 touchPosition = new Vector3(coord.Item1, coord.Item2, z);

                        // Déplacer l'objet à l'emplacement de l'entrée tactile
                        objectToMove3.transform.localPosition = touchPosition;

                        // Marquer que le déplacement a eu lieu
                        hasMoved3 = true;

                    }
            } else {
                    objectToMove1.SetActive(false);
                    objectToMove2.SetActive(false);
                    objectToMove3.SetActive(false);
                    hasMoved1 = false;
                    hasMoved2= false;
                    hasMoved3 = false;
                    
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