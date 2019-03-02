﻿using UnityEngine;
using System.Collections;

namespace Invector
{
    using vCharacterController;
    using vItemManager;

    [vClassHeader("Draw/Hide Melee Weapons", "This component works with vItemManager, vWeaponHolderManager and vMeleeCombatInput", useHelpBox = true)]
    public class vDrawHideMeleeWeapons : vMonoBehaviour
    {
        public bool hideWeaponsAutomatically = true;
        [vHideInInspector("hideWeaponsAutomatically")]
        public float hideWeaponsTimer = 5f;
        [vHelpBox("Set Lock input to Inventory when Lock method is called")]
        public bool lockInventoryInputOnLock;
        [vReadOnly]
        public bool isLocked;
        public GenericInput hideAndDrawWeaponsInput = new GenericInput("H", "LB", "LB");
        vWeaponHolderManager holderManager;
        vMeleeCombatInput melee;
        protected float currentTimer;
        protected bool forceHide;

        protected virtual void Start()
        {
            holderManager = GetComponent<vWeaponHolderManager>();
            var tpInput = GetComponent<vThirdPersonInput>();

            if (holderManager && tpInput)
            {
                tpInput.onUpdateInput.AddListener(ControlWeapons);
                melee = tpInput as vMeleeCombatInput;
                if (melee == null) Debug.LogWarning("Nee to be an vMeleeCombatInput", gameObject);
            }
        }

        protected virtual void ControlWeapons(vThirdPersonInput tpInput)
        {
            if (isLocked)
                return;

            HandleInput();
            DrawWeaponsImmediateHandle();
            HideWeaponsAutomatically();
        }

        protected virtual GameObject RightWeaponObject(bool checkIsActve = false)
        {

            return melee && melee.meleeManager && melee.meleeManager.rightWeapon && (!checkIsActve||melee.meleeManager.rightWeapon.gameObject.activeInHierarchy) ? melee.meleeManager.rightWeapon.gameObject : null;
        }

        protected virtual GameObject LeftWeaponObject(bool checkIsActve = false)
        {
            return melee && melee.meleeManager && melee.meleeManager.leftWeapon && (!checkIsActve || melee.meleeManager.leftWeapon.gameObject.activeInHierarchy) ? melee.meleeManager.leftWeapon.gameObject : null;
        }

        public virtual void LockDrawHideInput(bool value)
        {
            isLocked = value;
            if (lockInventoryInputOnLock && holderManager.itemManager && holderManager.itemManager.inventory)
                holderManager.itemManager.inventory.lockInput = value;
        }

        public virtual void HideWeapons(bool immediate = false)
        {
            if (CanHideRightWeapon())
                HideRightWeapon(immediate);
            else if (CanHideLeftWeapon())
                HideLeftWeapon(immediate);
        }

        public virtual void ForceHideWeapons(bool immediate = false)
        {
            forceHide = true;
            HideWeapons(immediate);
            Invoke("ResetForceHide", 1);
        }

        protected virtual void ResetForceHide()
        {
            forceHide = false;
        }

        public virtual void DrawWeapons(bool immediate = false)
        {
            if (CanDrawRightWeapon())
                DrawRightWeapon(immediate);
            else if (CanDrawLeftWeapon())
            {
                DrawLeftWeapon(immediate);
            }
                
        }

        protected virtual void HideWeaponsAutomatically()
        {
            if (hideWeaponsAutomatically)
            {
                if (HideTimerConditions())
                {
                    currentTimer += Time.deltaTime;
                }
                else currentTimer = 0;

                if (currentTimer >= hideWeaponsTimer && !IsEquipping)
                {
                    currentTimer = 0;
                    HideWeapons();
                }
            }
            else if (currentTimer > 0) currentTimer = 0;
        }

        protected virtual bool HideTimerConditions()
        {
            return CanHideWeapons() && (CanHideRightWeapon() || CanHideLeftWeapon());
        }

        protected virtual bool CanHideWeapons()
        {
            return melee && melee.meleeManager && (forceHide || (!melee.isAttacking && !melee.isBlocking && (melee.meleeManager.rightWeapon || melee.meleeManager.leftWeapon)));
        }

        protected virtual bool CanDrawWeapons()
        {
            return melee && melee.meleeManager;
        }

        protected virtual bool CanHideRightWeapon()
        {
            return ((CanHideWeapons()) && RightWeaponObject() && RightWeaponObject().activeInHierarchy);
        }

        protected virtual bool CanHideLeftWeapon()
        {
            return (CanHideWeapons() && LeftWeaponObject() && LeftWeaponObject().activeInHierarchy);
        }

        protected virtual bool CanDrawRightWeapon()
        {
            return (CanDrawWeapons() && RightWeaponObject() && !RightWeaponObject().activeInHierarchy);
        }

        protected virtual bool CanDrawLeftWeapon()
        {
            return (CanDrawWeapons() && LeftWeaponObject() && !LeftWeaponObject().activeInHierarchy);
        }

        protected virtual bool IsEquipping
        {
            get
            {
                return melee != null && melee.cc && melee.cc.IsAnimatorTag("IsEquipping");
            }
        }

        protected virtual void HandleInput()
        {
            if (hideAndDrawWeaponsInput.GetButtonDown() && !IsEquipping)
            {
                if (CanHideRightWeapon() || CanHideLeftWeapon())
                    HideWeapons();
                else if (CanDrawRightWeapon() || CanDrawLeftWeapon())
                    DrawWeapons();
            }
        }

        protected virtual void DrawWeaponsImmediateHandle()
        {
            if (DrawWeaponsImmediateConditions())
            {
                DrawWeapons(true);
            }
        }

        protected virtual bool DrawWeaponsImmediateConditions()
        {
            if (!melee || melee.cc.customAction || melee.meleeManager.CurrentAttackWeapon == null)
                return false;
            else
                return melee.weakAttackInput.GetButtonDown() || melee.strongAttackInput.GetButtonDown() || melee.blockInput.GetButton();
        }

        protected virtual void HideRightWeapon(bool immediate = false)
        {
            var weapon = RightWeaponObject(true);
            if (weapon)
            {
               
                var equipment = weapon.GetComponent<vIEquipment>();
                if (equipment == null)
                {
                    Debug.LogWarning(weapon.name + " need to have an  vIEquipment Component", weapon.gameObject);
                    return;
                }
                var holder = holderManager.GetHolder(weapon.gameObject, equipment.referenceItem.id);
                HideWeaponsHandle(melee, equipment,
                null,
                () =>
                {
                    if (holder) holder.SetActiveWeapon(true);
                    weapon.gameObject.SetActive(false);
                    if (CanHideLeftWeapon()) HideLeftWeapon(immediate);
                }, immediate);
            }
        }

        protected virtual void HideLeftWeapon(bool immediate = false)
        {
            var weapon = LeftWeaponObject(true);
            if (weapon)
            {
                var equipment = weapon.GetComponent<vIEquipment>();
                if (equipment == null)
                {
                    Debug.LogWarning(weapon.name + " need to have an  vIEquipment Component", weapon.gameObject);
                    return;
                }
                var holder = holderManager.GetHolder(weapon.gameObject, equipment.referenceItem.id);
                HideWeaponsHandle(melee, equipment,
                null,
                () =>
                {
                    if (holder) holder.SetActiveWeapon(true);
                    weapon.gameObject.SetActive(false);
                }, immediate);
            }
        }

        protected virtual void DrawRightWeapon(bool immediate = false)
        {
            var weapon = RightWeaponObject();
            if (weapon)
            {                
                var equipment = weapon.GetComponent<vIEquipment>();
                if (equipment == null)
                {
                    Debug.LogWarning(weapon.name + " need to have an  vIEquipment Component", weapon.gameObject);
                    return;
                }
                var holder = holderManager.GetHolder(weapon.gameObject, equipment.referenceItem.id);
                DrawWeaponsHandle(melee, equipment, null,
                                                            () =>
                                                            {
                                                                if (holder) holder.SetActiveWeapon(false);
                                                                weapon.gameObject.SetActive(true);
                                                                if (CanDrawLeftWeapon())
                                                                    DrawLeftWeapon(immediate);
                                                            }, immediate);
            }
        }

        protected virtual void DrawLeftWeapon(bool immediate = false)
        {
            var weapon = LeftWeaponObject();
            if (weapon)
            {
                var equipment = weapon.GetComponent<vIEquipment>();
                if (equipment == null)
                {
                    Debug.LogWarning(weapon.name + " need to have an  vIEquipment Component", weapon.gameObject);
                    return;
                }
                var holder = holderManager.GetHolder(weapon.gameObject, equipment.referenceItem.id);
                DrawWeaponsHandle(melee, equipment, null,
                                                            () =>
                                                            {
                                                                if (holder) holder.SetActiveWeapon(false);
                                                                weapon.gameObject.SetActive(true);
                                                            }, immediate);
            }
        }

        protected virtual void DrawWeaponsHandle(vThirdPersonInput tpInput, vIEquipment equipment, UnityEngine.Events.UnityAction onStart, UnityEngine.Events.UnityAction onFinish, bool immediate = false)
        {
            if (holderManager.inEquip) return;
            if (!immediate)
            {
                tpInput.animator.SetInteger("EquipItemID", equipment.equipPoint.equipPointName.Contains("Right") ? equipment.referenceItem.EquipID : -equipment.referenceItem.EquipID);
                tpInput.animator.SetTrigger("EquipItem");
            }
            StartCoroutine(holderManager.EquipRoutine(equipment.referenceItem.equipDelayTime, immediate, onStart, onFinish));
        }

        protected virtual void HideWeaponsHandle(vThirdPersonInput tpInput, vIEquipment equipment, UnityEngine.Events.UnityAction onStart, UnityEngine.Events.UnityAction onFinish, bool immediate = false)
        {
            if (holderManager.inUnequip) return;
            if (!immediate)
            {
                tpInput.animator.SetInteger("EquipItemID", equipment.equipPoint.equipPointName.Contains("Right") ? equipment.referenceItem.EquipID : -equipment.referenceItem.EquipID);
                tpInput.animator.SetTrigger("EquipItem");
            }
            StartCoroutine(holderManager.UnequipRoutine(equipment.referenceItem.unequipDelayTime, immediate, onStart, onFinish));
        }
    }
}
