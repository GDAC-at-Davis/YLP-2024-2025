using CharacterScripts;
using EditorUtils.BoldHeader;
using LevelScripts;
using Managers;
using Menus;
using TMPro;
using UnityEngine;

namespace Input_Scripts
{
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
        }

        private void Update()
        {
            if (selected || input == Vector3.zero)
            {
                return;
            }

            transform.position += input * Time.deltaTime * speed;

            ClampPositionToCanvas();
        }

        private void OnDestroy()
        {
            UnsubscribeToInputEvents();
            gameDataSO.OnPlayerDataChanged -= HandlePlayerDataChanged;
            CharacterSelect.Instance.AllPlayersReady -= LockIn;
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

            CharacterSelect.Instance.AllPlayersReady += LockIn;
            CharacterSelect.Instance.ReadyUp(playerID, null);
        }

        private void HandlePlayerDataChanged(int priorid, PlayerDataChange changeType,
            GameDataSO.PlayerData postchangedata)
        {
            if (priorid == playerID)
            {
                if (changeType == PlayerDataChange.PlayerRemoved)
                {
                    playerID = -1;
                    Destroy(gameObject);
                }

                if (changeType == PlayerDataChange.IdChanged)
                {
                    UnsubscribeToInputEvents();
                    playerID = postchangedata.PlayerId;
                    text.text = (playerID + 1).ToString();

                    events = playerInputSO.TryGetPlayerInputEvents(playerID);
                    SubscribeToInputEvents();
                }
            }
        }

        private void SubscribeToInputEvents()
        {
            events.JumpEvent += TrySelectCharacter;
            events.HeavyAttackEvent += UnselectCharacter;
            events.MoveEvent += MoveCursor;
        }

        private void UnsubscribeToInputEvents()
        {
            events.JumpEvent -= TrySelectCharacter;
            events.HeavyAttackEvent -= UnselectCharacter;
            events.JumpEvent -= TrySelectLevel;
            events.HeavyAttackEvent -= UnselectLevel;
            events.MoveEvent -= MoveCursor;
        }

        private void MoveCursor(Vector2 input)
        {
            this.input = input;
        }

        private void TrySelectCharacter(bool pressed)
        {
            if (!pressed)
            {
                return;
            }

            Collider2D button = Physics2D.OverlapPoint(transform.position);

            if (button == null)
            {
                return;
            }

            transform.position = button.transform.position;
            text.text = "";
            CharacterSO character = button.GetComponent<CharacterSelectButton>().Character;
            QueueCharacter(character);
        }

        private void UnselectCharacter(bool pressed)
        {
            if (!pressed)
            {
                return;
            }

            gameDataSO.RemovePlayer(playerID);

            text.text = (playerID + 1).ToString();
            QueueCharacter(null);
        }

        private void TrySelectLevel(bool pressed)
        {
            if (!pressed)
            {
                return;
            }

            Collider2D button = Physics2D.OverlapPoint(transform.position);

            if (button == null)
            {
                return;
            }

            transform.position = button.transform.position;
            text.text = "";
            LevelSO level = button.GetComponent<LevelSelectButton>().Level;
            QueueLevel(level);
        }

        private void UnselectLevel(bool pressed)
        {
            if (!pressed)
            {
                return;
            }

            QueueLevel(null);
        }

        private void QueueCharacter(CharacterSO character)
        {
            selected = character != null;
            CharacterSelect.Instance.ReadyUp(playerID, character);
        }

        private void QueueLevel(LevelSO level)
        {
            selected = level != null;
            if (!level)
            {
                CharacterSelect.Instance.ReturnToCharacterSelect();
                return;
            }

            CharacterSelect.Instance.TryStartGame(level);
        }

        // This is a mess and I should not have done it like this I'm sorry
        private void LockIn(bool toggle)
        {
            if (toggle)
            {
                if (playerID == 0)
                {
                    events.JumpEvent += TrySelectLevel;
                    events.HeavyAttackEvent += UnselectLevel;

                    text.text = (playerID + 1).ToString();
                    selected = false;
                }

                events.HeavyAttackEvent -= UnselectCharacter;
                events.JumpEvent -= TrySelectCharacter;
            }
            else
            {
                if (playerID == 0)
                {
                    events.JumpEvent -= TrySelectLevel;
                    events.HeavyAttackEvent -= UnselectLevel;
                }

                events.JumpEvent += TrySelectCharacter;
                events.HeavyAttackEvent += UnselectCharacter;

                text.text = (playerID + 1).ToString();
                selected = false;
            }
        }
    }
}