using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class sceneController : MonoBehaviour
{
    [SerializeField] private GameObject BGMGO;
    [SerializeField] private GameObject cameraControllerGO;
    [SerializeField] private GameObject playerGO;
    [SerializeField] private GameObject roomGO;

    private roomController room;
    private playerCharacter player;
    private AudioSource BGM;
    private cameraController cameraController;
    private bool alive = true;
    private Notifiable[] notifiables;
    private bool firstFrame = true;
    private bool changingRoom = false;

     public enum SceneColor
    {
        red = 0,
        blue = 1
    }

    public SceneColor currentColor = SceneColor.blue;

    private void Start()
    {
        BGM = BGMGO.GetComponent<AudioSource>();
        cameraController = cameraControllerGO.GetComponent<cameraController>();
        player = playerGO.GetComponent<playerCharacter>();
        roomGO = Instantiate(roomGO);
        room = roomGO.GetComponent<roomController>();
        BGM.Play();
        alive = true;

        playerGO.transform.localPosition = new Vector3 (room.initialPositionX, room.initialPositionY, playerGO.transform.localPosition.z);
        getNotifiables();
        OrderNotifiables();
    }

    private void getNotifiables()
    {
        GameObject[] notGO = GameObject.FindGameObjectsWithTag("ColorNotified");
        notifiables = new Notifiable[notGO.Length];

        int i = 0;

        foreach (GameObject notGOItem in notGO)
        {   notifiables[i] = notGOItem.GetComponent<Notifiable>();
            i++;
        }
    }
    private void OrderNotifiables()
    {
        Array.Sort(notifiables, delegate (Notifiable a, Notifiable b)
        {
            return a.priority.CompareTo(b.priority);
        }); 
    }

    private void updateNotifiableState(bool active)
    {
        foreach (Notifiable n in notifiables)
        {
            n.ActiveNotifier = active;
        }
    }

   

    public void initializeNotifiers(bool isNewColorRed)
    {
        foreach (Notifiable n in notifiables)
        {
            n.initializeNotifiable(isNewColorRed);
        }
        player.initializeNotifiable(isNewColorRed); //El jugador es un caso especial
    }

    private void notifyColorChange(bool isNewColorRed)
    {
        foreach (Notifiable n in notifiables)
        {
            n.ChangeColour(isNewColorRed);
        }
        player.ChangeColour(isNewColorRed); //El jugador es un caso especial
        currentColor = isNewColorRed ? SceneColor.red : SceneColor.blue;
    }
    public void Update()
    {
        if (firstFrame)
        {
            initializeNotifiers(currentColor == SceneColor.red);
            firstFrame = false;
        }
        if (alive)
        {
            if (Input.GetButtonDown("changeColourBlue") && currentColor != SceneColor.blue)
            {
                notifyColorChange(false);
            }
            else if (Input.GetButtonDown("changeColourRed") && currentColor != SceneColor.red)
            {
                notifyColorChange(true);
            }
        }
    }

    private IEnumerator givePlayerControl(GameObject roomGO)
    {
        yield return new WaitForSeconds(1); //MISMO QUE ANIMATION SECONDS. LO SIENTO OS JURO QUE PROGRAMARIA BIEN PERO NO ME APETECE, MEJOR PONER UN COMENTARIO DE 3 MESES
        player.modifyCollisions(true);
        changingRoom = false;
        Destroy(roomGO);
    }


    private IEnumerator reloadScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void killPlayer()
    {
        player.die();
        alive = false;
        playerGO.transform.localPosition = playerGO.transform.localPosition + new Vector3 (300, 300, 0); //Aleja de la camara al jugador
        playerGO.SetActive(false);
        StartCoroutine(respawnPlayer());
     
        //alive = false;
        //StartCoroutine(reloadScene());
    }

    public IEnumerator respawnPlayer()
    {
        yield return new WaitForSeconds(2);
        alive = true;
        playerGO.SetActive(true);
        playerGO.transform.localPosition = new Vector3(room.initialPositionX, room.initialPositionY, playerGO.transform.localPosition.z);


    }

    public void edgeReached(CustomEnums.directions direction)
    {
        GameObject nextRoom = room.getNextRoom(direction);
        if (nextRoom != null && !changingRoom)
        {
            //PREPARATION FOR THE ANIMATION
            float xOffset = 0;
            float yOffset = 0;
            float animationMovementX = 0;
            float animationMovementY = 0;
            if (direction == CustomEnums.directions.right)
            {
                xOffset = 36.25f;
                animationMovementX = -player.transform.localPosition.x * 2 + 0.25f;
            }
            else if (direction == CustomEnums.directions.left)
            {
                xOffset = -36.25f;
                animationMovementX = -player.transform.localPosition.x * 2 - 0.25f;
            } else if (direction == CustomEnums.directions.up)
            {
                yOffset = 20.25f;
                animationMovementY = -player.transform.localPosition.y * 2 + 1f;
            } else
            {
                yOffset = -20.25f;
                animationMovementY = -player.transform.localPosition.y * 2 - 2f;
            }


            //CREO HABITACION
            changingRoom = true;
            GameObject nextRoomInstance = Instantiate(nextRoom);
            nextRoomInstance.transform.position = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z);

            //ACTUALIZO LOS NOTIFIERS
            updateNotifiableState(false); //desactivo los notifiers antiguos
            getNotifiables();
            OrderNotifiables();
            initializeNotifiers(currentColor == SceneColor.red);

            //ANIMACION DEL JUGADOR
            player.modifyCollisions(false);
            moveSmoothly moveSmoothlyPlayer = player.GetComponent<moveSmoothly>();
            moveSmoothlyPlayer.moveObjectiveX = animationMovementX;
            moveSmoothlyPlayer.moveObjectiveY = animationMovementY;
            moveSmoothlyPlayer.animationSeconds = 1;
            moveSmoothlyPlayer.UpdatePath();
            moveSmoothlyPlayer.move();

            //ANIMACION CAMARA
            cameraController.move(xOffset, yOffset, 1);
            StartCoroutine(givePlayerControl(roomGO));

            roomGO = nextRoomInstance;
            room = roomGO.GetComponent<roomController>();

        }
    }
}
