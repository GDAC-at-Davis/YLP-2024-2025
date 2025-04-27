using CharacterScripts;
using EditorUtils.BoldHeader;
using LevelScripts;
using Managers;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Input_Scripts
{
    internal enum CursorType
    {
        CharacterSelect,
        StageSelect
    }

    /// <summary>
    ///     Temporary Character Select cursor thing its 2:30 am and I can't figure out how to combine
    ///     the virtual mouse and multiplayer event system so we're gonna do whatever the fuck this is for now
    /// </summary>
    public class PlayerCursorController : MonoBehaviour
    {
        [BoldHeader("Player Virtual Cursor")]
        [Header("Depends")]

        [SerializeField]
        private PlayerInputSo playerInputSO;

        [SerializeField]
        private GameDataSO gameDataSO;

        [Header("Config")]

        [SerializeField]
        private float speed = 500;

        [SerializeField]
        private CursorType cursorType;

        [SerializeField]
        [Scene]
        private string gameplayScene;

        [SerializeField]
        [Scene]
        private string charSelectScene;

        private PlayerInputSo.PlayerInputEvents events;

        private int playerID;
        private Vector3 input;
        private bool selected;

        private TextMeshProUGUI text;

        private RectTransform cursorBottomLeft;
        private RectTransform cursorTopRight;

        private RectTransform rectTransform;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();

            // Center

            Vector2 bottomLeftPos = cursorBottomLeft.position;
            Vector2 topRightPos = cursorTopRight.position;

            rectTransform.position = (bottomLeftPos + topRightPos) / 2;
        }

        private void Update()
        {
            if (selected || input == Vector3.zero)
            {
                return;
            }

            transform.position += input.normalized * (Time.deltaTime * speed);

            ClampPositionToCanvas();
        }

        private void OnDestroy()
        {
            UnsubscribeToInputEvents();
            gameDataSO.OnPlayerDataChanged -= HandlePlayerDataChanged;
            gameDataSO.OnAllPlayersReady -= LockIn;
        }

        private void ClampPositionToCanvas()
        {
            Vector2 pos = rectTransform.position;

            Vector2 bottomLeftPos = cursorBottomLeft.position;
            Vector2 topRightPos = cursorTopRight.position;

            pos.x = Mathf.Clamp(pos.x, bottomLeftPos.x, topRightPos.x);
            pos.y = Mathf.Clamp(pos.y, bottomLeftPos.y, topRightPos.y);

            rectTransform.position = pos;
        }

        public void Initialize(int playerId, RectTransform bottomLeft, RectTransform topRight)
        {
            Debug.Log($"Player {playerId} cursor initialized");
            cursorBottomLeft = bottomLeft;
            cursorTopRight = topRight;

            playerID = playerId;
            text = GetComponentInChildren<TextMeshProUGUI>();
            text.text = (playerId + 1).ToString();

            events = playerInputSO.TryGetPlayerInputEvents(playerID);
            SubscribeToInputEvents();

            gameDataSO.OnPlayerDataChanged += HandlePlayerDataChanged;
        }

        private void HandlePlayerDataChanged(int priorid, PlayerDataChange changeType,
            GameDataSO.PlayerData postchangedata)
        {
            if (priorid != playerID)
            {
                return;
            }

            if (changeType == PlayerDataChange.PlayerRemoved)
            {
                playerID = -1;
                Destroy(gameObject);
            }
        }

        private void SubscribeToInputEvents()
        {
            events.JumpEvent += TrySelectCharacter;
            events.HeavyAttackEvent += RemovePlayer;
            events.MoveEvent += MoveCursor;
            events.JumpEvent += TrySelectLevel;
            events.HeavyAttackEvent += UnselectLevel;
        }

        private void UnsubscribeToInputEvents()
        {
            events.JumpEvent -= TrySelectCharacter;
            events.HeavyAttackEvent -= RemovePlayer;
            events.MoveEvent -= MoveCursor;
            events.JumpEvent -= TrySelectLevel;
            events.HeavyAttackEvent -= UnselectLevel;
        }

        private void MoveCursor(Vector2 input)
        {
            this.input = input;
        }

        private void TrySelectCharacter(bool pressed)
        {
            if (!pressed || cursorType != CursorType.CharacterSelect)
            {
                return;
            }

            // Unselect if pressed again
            if (gameDataSO.GetPlayerData(playerID).SelectedCharacter != null)
            {
                Debug.Log($"player {playerID} unselected character");

                text.text = (playerID + 1).ToString();
                QueueCharacter(null);
                return;
            }

            Collider2D button = Physics2D.OverlapPoint(transform.position);
            if (button == null)
            {
                return;
            }

            CharacterSO character = button.GetComponent<CharacterSelectButton>()?.Character;
            if (character == null)
            {
                return;
            }

            transform.position = button.transform.position;
            text.text = "";

            Debug.Log($"player {playerID} selected character {character.name}");
            QueueCharacter(character);
        }

        private void RemovePlayer(bool pressed)
        {
            if (!pressed || cursorType != CursorType.CharacterSelect)
            {
                return;
            }

            gameDataSO.RemovePlayer(playerID);
        }

        private void TrySelectLevel(bool pressed)
        {
            if (!pressed || playerID != 0 || cursorType != CursorType.StageSelect)
            {
                return;
            }

            Collider2D button = Physics2D.OverlapPoint(transform.position);
            if (button == null)
            {
                return;
            }

            LevelSO level = button.GetComponent<LevelSelectButton>()?.Level;
            if (level == null)
            {
                return;
            }

            transform.position = button.transform.position;
            text.text = "";
            QueueLevel(level);
        }

        private void UnselectLevel(bool pressed)
        {
            if (!pressed || playerID != 0 || cursorType != CursorType.StageSelect)
            {
                return;
            }

            QueueLevel(null);
        }

        private void QueueCharacter(CharacterSO character)
        {
            selected = character != null;
            gameDataSO.SetPlayerSelectedCharacter(playerID, character);
        }

        private void QueueLevel(LevelSO level)
        {
            selected = level != null;
            if (!level)
            {
                gameDataSO.ClearPlayerData();
                gameDataSO.LoadScene(charSelectScene);
                return;
            }

            gameDataSO.SetSelectedLevel(level);
            gameDataSO.LoadScene(gameplayScene);
        }

        private void LockIn()
        {
            gameObject.SetActive(false);
        }
    }
}