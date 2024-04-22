using System.Collections;
using UnityEngine;

public class DiagLocker : MonoBehaviour
{
    public GameObject Canvas_lock1;
    public GameObject Canvas_lock2;
   
    public GameObject Canvas;

    // Références aux sphères
    public GameObject sphereLock1;
    public GameObject sphereLock2;
   

    public GameObject informationGuide;

     
    //tets
    // Start is called before the first frame update
    void Start()
    {
        // Initialisation des canvas
         // Activer le canvas_lock par défaut
        Canvas.SetActive(false); // Désactiver le canvas par défaut

        // Lancement de la coroutine pour vérifier l'état des sphères et changer les canvas si nécessaire
        StartCoroutine(CheckSphereStates());
    }

    IEnumerator CheckSphereStates()
    {
        bool locker = true; 

        while (locker == true) // Utilisation de "==" pour comparer les valeurs
        {
            
            // Vérifier l'état des sphères
            bool sphere1IsGreen = sphereLock1.GetComponent<LockSphere1>().isGreen;
            bool sphere2IsGreen = sphereLock2.GetComponent<LockSphere1>().isGreen;
            

            // Si toutes les sphères sont vertes, changer les canvas
            if (sphere1IsGreen && sphere2IsGreen)
            {
                // Changer les canvas
                informationGuide.SetActive(false);
                Canvas_lock1.SetActive(false);
                Canvas_lock2.SetActive(false);
            
                Canvas.SetActive(true);
                locker = false; 
                GetComponent<SlideObjectMover>().enabled = false;

            }

            // Attendre une frame avant de vérifier à nouveau l'état des sphères
            yield return null;
        }
    }
}
