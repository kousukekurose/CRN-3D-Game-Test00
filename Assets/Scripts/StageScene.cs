using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using R3;

public class StageScene : MonoBehaviour
{
    [Header("Player")]
    public GameObject playerPrefab;
    private Transform currentPlayer;
    private Rigidbody playerRb;
    private PlayerInput playerInput;

    [Header("Camera")]
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(0, 10, -10);

    [Header("UI")]
    public TextMeshProUGUI countdownText;

    private bool isGameStart = false;

    void Awake()
    {
        // Mazeからスタート位置を受け取る
        MazeCreator3D.OnStartPosition
            .Subscribe(pos =>
            {
                SpawnPlayer(pos);
            });
    }

    void Update()
    {
        // クリックで開始
        if (!isGameStart && Mouse.current.leftButton.wasPressedThisFrame)
        {
            isGameStart = true;

            if (currentPlayer != null)
                StartCoroutine(GameStartSequence());
        }
    }

    void LateUpdate()
    {
        if (currentPlayer == null || cameraTransform == null) return;

        Vector3 target = currentPlayer.position + cameraOffset;

        cameraTransform.position = new Vector3(
            target.x,
            cameraTransform.position.y,
            target.z
        );
    }

    // ■ Mazeから呼ばれるスポーン
    void SpawnPlayer(Vector3 pos)
    {
        Debug.Log("Spawn呼ばれた: " + pos);
        GameObject player = Instantiate(playerPrefab, pos, Quaternion.identity);

        currentPlayer = player.transform;
        playerRb = player.GetComponent<Rigidbody>();
        playerInput = player.GetComponent<PlayerInput>();
    }

    // ■ ゲーム開始演出
    IEnumerator GameStartSequence()
    {
        // 一旦停止
        if (playerInput) playerInput.enabled = false;
        if (playerRb) playerRb.isKinematic = true;

        // カウントダウン
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false);

        // 再開
        if (playerInput) playerInput.enabled = true;
        if (playerRb) playerRb.isKinematic = false;
    }
}