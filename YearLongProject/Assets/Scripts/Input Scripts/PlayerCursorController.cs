using EditorUtils.BoldHeader;
using Managers;
using Menus;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

        [SerializeField]
        [Tooltip("How often should cursor check for OnHover")]
        private float onHoverRate = 0.5f;

        public int PlayerID => playerID;

        public Image Cursor
        {
            get => cursor;
            set => cursor = value;
        }

        public Transform Container => container;

        public UnityAction<PlayerCursorController> BackAction = null;
        private float lastOnHover;
        private ButtonBehavior currentHoveredButton;

        private PlayerInputSo.PlayerInputEvents events;

        private int playerID;

        private Vector3 input;

        private Image cursor;

        private TextMeshProUGUI text;
        private Image image;

        private RectTransform cursorBottomLeft;
        private RectTransform cursorTopRight;

        private RectTransform rectTransform;
        private Transform container;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            cursor = GetComponentInChildren<Image>();

            // Center

            Vector2 bottomLeftPos = cursorBottomLeft.position;
            Vector2 topRightPos = cursorTopRight.position;

            rectTransform.position = (bottomLeftPos + topRightPos) / 2;
        }

        private void Update()
        {
            if (lastOnHover + onHoverRate < Time.time)
            {
                ButtonBehavior button = null;
                button = Physics2D.OverlapPoint(transform.position)?.GetComponent<ButtonBehavior>();
                if (button == null)
                {
                    if (currentHoveredButton != null)
                    {
                        currentHoveredButton.OnHoverExit(this);
                        currentHoveredButton = null;
                    }
                }
                else
                {
                    if (currentHoveredButton != null && currentHoveredButton != button)
                    {
                        currentHoveredButton.OnHoverExit(this);
                    }

                    button.OnHoverEnter(this);
                    currentHoveredButton = button;
                }

                lastOnHover = Time.time;
            }

            if (input == Vector3.zero)
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

        public void Initialize(int playerId, RectTransform bottomLeft, RectTransform topRight, Transform container)
        {
            Debug.Log($"Player {playerId} cursor initialized");
            cursorBottomLeft = bottomLeft;
            cursorTopRight = topRight;
            this.container = container;

            playerID = playerId;
            text = GetComponentInChildren<TextMeshProUGUI>();
            image = GetComponentInChildren<Image>();
            text.text = (playerId + 1).ToString();
            text.color = gameDataSO.PlayerColors[playerID];
            image.color = gameDataSO.PlayerColors[playerID];

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
            events.JumpEvent += TryOnClick;
            events.HeavyAttackEvent += TryBackAction;
            events.MoveEvent += MoveCursor;
        }

        private void UnsubscribeToInputEvents()
        {
            events.JumpEvent -= TryOnClick;
            events.HeavyAttackEvent -= TryBackAction;
            events.MoveEvent -= MoveCursor;
        }

        private void MoveCursor(Vector2 input)
        {
            this.input = input;
        }

        public void SetText(string text)
        {
            this.text.text = text;
        }

        private void TryOnClick(bool pressed)
        {
            if (!pressed)
            {
                return;
            }

            ButtonBehavior button = null;
            button = Physics2D.OverlapPoint(transform.position)?.GetComponent<ButtonBehavior>();
            if (button == null)
            {
                return;
            }

            button.OnClick(this);
        }

        private void TryBackAction(bool pressed)
        {
            if (!pressed)
            {
                return;
            }

            BackAction(this);
        }

        private void LockIn()
        {
            gameObject.SetActive(false);
        }
    }
}