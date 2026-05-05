using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class StageScene : MonoBehaviour
{
    //public Timer timer;
    //public RankingManager rankingManager;

    //public GameObject gameClearUI;
    //public GameObject gameOverUI;
    public GameObject gameClearArea;

    public GameObject playerPrefab;
    public Transform playerSpawnP;

    public GameObject playerHpUI;

    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI timeText;

    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0, 10, -10);

    private Transform currentPlayer;
    private Rigidbody playerRb;

    private bool isGameStart = false;

    public static bool startcheck = false;

    void Awake()
    {
        startcheck = false;
    }

    void Update()
    {
        if (!isGameStart && Mouse.current.leftButton.wasPressedThisFrame)
        {
            isGameStart = true;
            StartCoroutine(StartGameSequence());
        }
    }

    void LateUpdate()
    {
        if (currentPlayer == null || cameraTransform == null) return;

        Vector3 targetPos = currentPlayer.position + cameraOffset;

        cameraTransform.position = new Vector3(
            targetPos.x,
            cameraTransform.position.y,
            targetPos.z
        );
    }

    IEnumerator StartGameSequence()
    {
        GameObject player = Instantiate(playerPrefab, playerSpawnP.position, Quaternion.identity);
        currentPlayer = player.transform;

        playerHpUI.SetActive(true);

        playerRb = player.GetComponent<Rigidbody>();
        PlayerInput input = player.GetComponent<PlayerInput>();
        //Player p = player.GetComponent<Player>();

        // í‚é~Åi3Dî≈Åj
        if (input) input.enabled = false;
       // if (p) p.enabled = false;
        if (playerRb) playerRb.isKinematic = true;

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);

        startcheck = true;

        // çƒäJ
        if (input) input.enabled = true;
        //if (p) p.enabled = true;
        if (playerRb) playerRb.isKinematic = false;

        //timer.StratTimer();
    }
}