using UnityEngine;
using System.Collections.Generic;
public class RadialObjectDisplayer : MonoBehaviour
{
    public List<GameObject> objects;
    public GameObject centerObject;

    public float radius = 2f;
    public float nbrButton;
    void Start(){
    
    foreach (GameObject obj in objects){
        obj.SetActive(false); 
    }

    }
    void OnTriggerEnter(Collider other)
    {

        int angle = 360/objects.Count;
        int i = 1;
        

        foreach (GameObject obj in objects)
        {
        Debug.Log(i); 
        obj.SetActive(true); 
        Vector3 relativePosition1 = new Vector3(centerObject.transform.position.x, centerObject.transform.position.y + 0.02f, centerObject.transform.position.z);
        
        
        Vector3 relativePosition2 = relativePosition1 - centerObject.transform.position;
        // Créer une rotation de 45 degrés autour de l'axe Y
        Quaternion rotation = Quaternion.Euler(0, 45*i, 0);
        Debug.Log(rotation); 
        // Appliquer la rotation aux coordonnées relatives
        Vector3 rotatedRelativePosition = rotation * relativePosition2;
        Debug.Log(relativePosition2); 
        Debug.Log(rotatedRelativePosition); 
        // Ajouter les coordonnées pivotées au point fixe pour obtenir les nouvelles coordonnées de l'objet
        Vector3 newPosition = relativePosition1 + rotatedRelativePosition;
        Debug.Log(newPosition); 
        // Déplacer l'objet vers les nouvelles coordonnées
        obj.transform.position = newPosition;
        i++;
        }
        

        

    }

  


    
}
