using System.Collections;

using UnityEngine;

public class ReLock : MonoBehaviour
{

    public GameObject Canvas_lock1;
    public GameObject Canvas;
    public GameObject CubeLock;
    public GameObject informationGuide;
    public bool locker = false; 

    // Références aux sphères
    public GameObject LockIsGreen;
    // Start is called before the first frame update


    void onEnable()
    {
        // Initialisation des canvas
         // Activer le canvas_lock par défaut
         // Désactiver le canvas par défaut
         locker = false;
        // Lancement de la coroutine pour vérifier l'état des sphères et changer les canvas si nécessaire
        StartCoroutine(CheckSphereStates());
        
    }


    // Update is called once per frame
  

    IEnumerator CheckSphereStates()
    {
         

        while (locker == false) // Utilisation de "==" pour comparer les valeurs
        {


        bool lockIsGreen = LockIsGreen.GetComponent<LockSphere1>().isGreen;
           

            // Si toutes les sphères sont vertes, changer les canvas
            if (lockIsGreen)
            {
                // Changer les canvas
                Debug.Log("test");
                informationGuide.SetActive(true);
                Canvas_lock1.SetActive(true);
                Canvas.SetActive(false);
                CubeLock.SetActive(false);
                locker = true;
                GetComponent<GridLockerObjectMover>().enabled = true;
                GetComponent<GridCircleLocker>().locker = true;

            }
            yield return null;

        }
    }
}
