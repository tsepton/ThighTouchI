using System.Collections;
using UnityEngine;

public class GridCircleLocker : MonoBehaviour
{
    public GameObject Canvas_lock1;
    public GameObject Canvas;

    public GameObject CubeLock;
    public GameObject informationGuide;

    // Références aux sphères
    public GameObject sphereLock1;
    public GameObject sphereLock2;
    public GameObject sphereLock3;
    public GameObject sphereLock4;
    public GameObject sphereLock5;
    public GameObject sphereLock6;
    public GameObject sphereLock7;
    public GameObject sphereLock8;

    public bool locker = true; 
    

    public 
    //tets
    // Start is called before the first frame update
    void Start()
    {
         

        // Initialisation des canvas
         // Activer le canvas_lock par défaut
        Canvas.SetActive(false); // Désactiver le canvas par défaut
        CubeLock.SetActive(false);
        // Lancement de la coroutine pour vérifier l'état des sphères et changer les canvas si nécessaire
        StartCoroutine(CheckSphereStates());
    }

    IEnumerator CheckSphereStates()
    {
        
        while (locker == true) // Utilisation de "==" pour comparer les valeurs
        {
            
            // Vérifier l'état des sphères
            bool sphere1IsGreen = sphereLock1.GetComponent<LockSphere1>().isGreen;
            bool sphere2IsGreen = sphereLock2.GetComponent<LockSphere1>().isGreen;
            bool sphere3IsGreen = sphereLock3.GetComponent<LockSphere1>().isGreen;
            bool sphere4IsGreen = sphereLock4.GetComponent<LockSphere1>().isGreen;
            bool sphere5IsGreen = sphereLock5.GetComponent<LockSphere1>().isGreen;
            bool sphere6IsGreen = sphereLock6.GetComponent<LockSphere1>().isGreen;
            bool sphere7IsGreen = sphereLock7.GetComponent<LockSphere1>().isGreen;
            bool sphere8IsGreen = sphereLock8.GetComponent<LockSphere1>().isGreen;
           

            // Si toutes les sphères sont vertes, changer les canvas
            if (sphere1IsGreen && sphere2IsGreen && sphere3IsGreen && sphere4IsGreen && sphere5IsGreen && sphere6IsGreen && sphere7IsGreen && sphere8IsGreen)
            {
                // Changer les canvas
                
                informationGuide.SetActive(false);
                Canvas_lock1.SetActive(false);
                Canvas.SetActive(true);
                locker = false; 
                GetComponent<GridLockerObjectMover>().enabled = false;
                CubeLock.SetActive(true);
                GetComponent<ReLock>().locker = false;
                Debug.Log(GetComponent<ReLock>().locker);
                

            }

            // Attendre une frame avant de vérifier à nouveau l'état des sphères
            yield return null;
        }
    }
}
