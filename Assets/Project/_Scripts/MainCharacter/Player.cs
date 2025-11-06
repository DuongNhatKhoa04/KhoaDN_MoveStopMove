using System.IO;
using MoveStopMove.Core;
using MoveStopMove.Core.CoreComponents;
using MoveStopMove.Extensions.FSM;
using MoveStopMove.Extensions.FSM.States;
using MoveStopMove.Extensions.Helpers;
using MoveStopMove.Interfaces;
using MoveStopMove.MainCharacter.Data;
using MoveStopMove.Manager;
using UnityEngine;

namespace MoveStopMove.MainCharacter
{
    public class Player : Character
    {
        #region -- Fields --

        private Vector3 m_direction;
        private bool m_isMoving;
        private bool m_isGrounded;

        [Header("Pants Skin Test")]
        [SerializeField] private SkinnedMeshRenderer pantsRenderer;
        [SerializeField] private Texture2D pantsAlbedoTexture;

        private IDecoratable m_decoratorChain;

        #endregion

        #region -- Methods --

        private void Awake()
        {
            base.Initialize();
            m_decoratorChain = new WeaponDecorator(new FullSetSkinDecorator(new PantDecorator(new NullDecoratable())))
            {
                PantsRenderer = pantsRenderer,
                PantTexture = pantsAlbedoTexture
            };
        }

        private void Start()
        {
            StateMachine.Initialize(CharacterIdleState);
            m_decoratorChain.EquipPant();
        }

        private void Update()
        {
            StateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }

        [ContextMenu("Print full save JSON to Console")]
        public void PrintFullJson()
        {
            if (SaveManager.Instance == null) { Debug.LogWarning("SaveManager not initialized"); return; }
            string json = SaveManager.Instance.ExportToJson();
            Debug.Log($"=== Save JSON ===\n{json}");
        }

        [ContextMenu("Print summary (coins + equipped)")]
        public void PrintSummary()
        {
            if (SaveManager.Instance == null) { Debug.LogWarning("SaveManager not initialized"); return; }
            var d = SaveManager.Instance.Data;
            Debug.Log($"Coins: {d.coins}\nEquippedPant: {d.equippedPant}\nEquippedSkin: {d.equippedSkin}\nEquippedWeapon: {d.equippedWeapon}");
        }

        [ContextMenu("Print unlocked lists")]
        public void PrintUnlockedLists()
        {
            if (SaveManager.Instance == null) { Debug.LogWarning("SaveManager not initialized"); return; }
            var d = SaveManager.Instance.Data;
            Debug.Log($"Unlocked Pants ({d.unlockedPant?.Count ?? 0}): {string.Join(", ", d.unlockedPant ?? new System.Collections.Generic.List<string>())}");
            Debug.Log($"Unlocked Skins ({d.unlockedSkin?.Count ?? 0}): {string.Join(", ", d.unlockedSkin ?? new System.Collections.Generic.List<string>())}");
            Debug.Log($"Unlocked Weapons ({d.unlockedWeapon?.Count ?? 0}): {string.Join(", ", d.unlockedWeapon ?? new System.Collections.Generic.List<string>())}");
        }

        [ContextMenu("Export save.json to Desktop")]
        public void ExportToDesktop()
        {
            if (SaveManager.Instance == null) { Debug.LogWarning("SaveManager not initialized"); return; }
            string desktop = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
            string path = Path.Combine(desktop, "exported_save.json");
            File.WriteAllText(path, SaveManager.Instance.ExportToJson());
            Debug.Log("Exported save to " + path);
        }

        [ContextMenu("Open persistent save folder (Editor)")]
        public void OpenPersistentFolder()
        {
            string path = SaveManager.Instance?.GetPersistentSavePath();
            if (string.IsNullOrEmpty(path)) { Debug.LogWarning("No save path available"); return; }

        #if UNITY_EDITOR
            UnityEditor.EditorUtility.RevealInFinder(path);
        #else
            Application.OpenURL("file://" + System.IO.Path.GetDirectoryName(path));
        #endif
        }

        #endregion
    }
}