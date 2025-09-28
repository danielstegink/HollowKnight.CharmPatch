using DanielSteginkUtils.Utilities;
using GlobalEnums;
using HKMirror.Reflection.SingletonClasses;
using HutongGames.PlayMaker;
using Modding;
using Modding.Utils;
using SFCore.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CharmPatch.Charm_Patches
{
    /// <summary>
    /// MOP Restored is a compatibility patch allowing Pale Court and PC Charms (Mark of Purity specifically) to work alongside Charm Changer
    /// </summary>
    public class MopRestored : Patch
    {
        public bool IsActive => SharedData.globalSettings.mopRestoredOn;

        public void Start()
        {
            if (!AreModsInstalled())
            {
                return;
            }

            GetCharmId();

            On.HealthManager.TakeDamage += IncrementSpeed;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += CustomizeDummy;
            On.HeroController.DoAttack += CooldownPatch;
            On.HeroController.Attack += DurationPatch;

            On.HeroController.Update += ResetMop;
            ModHooks.AfterTakeDamageHook += ResetMop;
        }

        #region Initialization
        /// <summary>
        /// Checks if Charm Changer and either PC or PC Charms are installed
        /// </summary>
        /// <returns></returns>
        private bool AreModsInstalled()
        {
            return SharedData.charmChangerMod != null &&
                    (SharedData.paleCourtMod != null ||
                        SharedData.pcCharmsMod != null);
        }

        /// <summary>
        /// Gets MOP's numeric ID
        /// </summary>
        private void GetCharmId()
        {
            if (SharedData.paleCourtMod != null)
            {
                List<int> charmIds = ClassIntegrations.GetField<IMod, List<int>>(SharedData.paleCourtMod, "charmIDs");
                charmId = charmIds[0];
            }
            else if (SharedData.pcCharmsMod != null)
            {
                List<int> charmIds = ClassIntegrations.GetField<IMod, List<int>>(SharedData.pcCharmsMod, "CharmIDs");
                charmId = charmIds[0];
            }

            CharmPatch.Instance.Log($"MOP Restored - Charm ID: {charmId}");
        }

        /// <summary>
        /// Gets a list of all the regular nail attacks
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        private List<NailSlash> GetNailSlashes(HeroController self)
        {
            return new List<NailSlash>()
            {
                self.normalSlash,
                self.alternateSlash,
                self.downSlash,
                self.upSlash,
                self.wallSlash,
            };
        }
        #endregion

        #region Properties
        /// <summary>
        /// Numeric ID of Mark of Purity
        /// </summary>
        private int charmId = -1;

        /// <summary>
        /// Tracks time between attacks
        /// </summary>
        private Stopwatch timer;

        /// <summary>
        /// Counts successful consecutive nail strikes
        /// </summary>
        private int hits = 0;

        /// <summary>
        /// Tracks the maximum number of hits that will apply the buff 
        /// </summary>
        /// <returns></returns>
        private int GetMaxHits()
        {
            // The default number of max hits is 9
            // With quick slash, its 11
            if (PlayerData.instance.GetBool("equippedCharm_32"))
            {
                return 11;
            }
            else
            {
                return 9;
            }
        }

        /// <summary>
        /// How long the player can go without a nail strike before the charm resets
        /// </summary>
        /// <returns></returns>
        private float GetMaxDuration()
        {
            // By default, MOP will wait 3.4 seconds
            // With Nailmaster's Glory equipped, it will wait 4 seconds
            if (PlayerData.instance.GetBool("equippedCharm_26"))
            {
                return 3400f;
            }
            else
            {
                return 4000f;
            }
        }

        /// <summary>
        /// Gets how much to reduce the attack cooldown (and duration) by per nail attack performed
        /// </summary>
        /// <param name="hits"></param>
        /// <returns></returns>
        private float GetModifier(int hits)
        {
            // By default, the min speed is 0.17
            // With Quickslash, this is 0.13
            if (PlayerData.instance.GetBool("equippedCharm_32"))
            {
                float max = 0.25f;
                float min = 0.13f;
                float diff = max - min;
                float diffPerHit = diff / GetMaxHits();
                float newValue = max - (diffPerHit * hits);
                return newValue / max;
            }
            else
            {
                float max = 0.41f;
                float min = 0.17f;
                float diff = max - min;
                float diffPerHit = diff / GetMaxHits();
                float newValue = max - (diffPerHit * hits);
                return newValue / max;
            }
        }
        #endregion

        #region Runtime Checks
        /// <summary>
        /// Determines if this patch should be used
        /// </summary>
        /// <returns></returns>
        private bool IsPatchEnabled()
        {
            return IsActive &&
                    PlayerData.instance.GetBool($"equippedCharm_{charmId}");
        }

        /// <summary>
        /// Checks if FOTF is active, which would make the nail attacks red
        /// </summary>
        /// <returns></returns>
        private bool IsFotfActive()
        {
            GameObject charmEffectsObject = HeroController.instance.gameObject.transform.Find("Charm Effects").gameObject;
            PlayMakerFSM fury = charmEffectsObject.LocateMyFSM("Fury");
            return fury.ActiveStateName.Equals("Activate") ||
                    fury.ActiveStateName.Equals("Stay Furied");
        }
        #endregion

        #region Buff Application
        /// <summary>
        /// Every time the player gets a successful nail attack, MOP applies its special buff
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="hitInstance"></param>
        private void IncrementSpeed(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitInstance)
        {
            orig(self, hitInstance);

            if (IsPatchEnabled())
            {
                // Regular nail attacks and Elegy beams reset the timer
                if (Logic.IsNailAttack(hitInstance))
                {
                    timer = Stopwatch.StartNew();

                    // But only nail attacks actually apply the buff
                    if (Logic.IsNailAttack(hitInstance, false, false))
                    {
                        hits++;
                        //CharmPatch.Instance.Log($"MOP Restored - Hits incremented: {hits}");
                    }

                    if (hits >= GetMaxHits())
                    {
                        // Once we reach max hits, set this field (I dunno why)
                        ReflectionHelper.SetField<HealthManager, float>(self, "evasionByHitRemaining", 0.1f);

                        // We also need to change the nail attacks' color to a special shade of light grey, unless FOTF is active
                        if (!IsFotfActive())
                        {
                            foreach (NailSlash nailSlash in GetNailSlashes(HeroController.instance))
                            {
                                tk2dSprite nailColor = nailSlash.GetComponent<tk2dSprite>();
                                if (nailColor.color != Color.black)
                                {
                                    nailColor.color = new Color(0.619f, 0.798f, 0.881f);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The training dummy in Kingdom's Edge has special rules about how hitting it works
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void CustomizeDummy(Scene from, Scene to)
        {
            if (to.name == "Deepnest_East_16")
            {
                GameObject dummy = to.FindGameObject("Training Dummy");
                PlayMakerFSM attackedFsm = dummy.LocateMyFSM("Hit");
                FsmState hitState = attackedFsm.GetState("Light Dir");
                hitState.InsertMethod(() => IncrementSpeedDummy(), 0);
            }
        }

        /// <summary>
        /// Ensures that hitting the training dummy resets the timer and increments hits
        /// </summary>
        private void IncrementSpeedDummy()
        {
            timer = Stopwatch.StartNew();
            hits++;
            //CharmPatch.Instance.Log($"MOP Restored - Dummy Hits incremented: {hits}");

            if (hits >= GetMaxHits())
            {
                // We also need to change the nail attacks' color to a special shade of light grey, unless FOTF is active
                if (!IsFotfActive())
                {
                    foreach (NailSlash nailSlash in GetNailSlashes(HeroController.instance))
                    {
                        tk2dSprite nailColor = nailSlash.GetComponent<tk2dSprite>();
                        if (nailColor.color != Color.black)
                        {
                            nailColor.color = new Color(0.619f, 0.798f, 0.881f);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Because Charm Changer hard-codes the cooldown, we have to adjust the cooldown after its set
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void CooldownPatch(On.HeroController.orig_DoAttack orig, HeroController self)
        {
            orig(self);

            if (IsPatchEnabled() &&
                hits > 0)
            {
                int hitBonus = Math.Min(hits, GetMaxHits());
                float modifier = GetModifier(hitBonus);
                HeroControllerR.attack_cooldown *= modifier;
                //CharmPatch.Instance.Log($"MOP Restored - Attack cooldown: {HeroControllerR.attack_cooldown}");
            }
        }

        /// <summary>
        /// Attack duration is in the same boat as attack cooldown
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="attackDir"></param>
        private void DurationPatch(On.HeroController.orig_Attack orig, HeroController self, AttackDirection attackDir)
        {
            orig(self, attackDir);

            if (IsPatchEnabled() &&
                hits > 0)
            {
                int hitBonus = Math.Min(hits, GetMaxHits());
                float modifier = GetModifier(hitBonus);
                HeroControllerR.attackDuration *= modifier;
                //CharmPatch.Instance.Log($"MOP Restored - Attack duration: {HeroControllerR.attackDuration}");
            }
        }
        #endregion

        #region Reset
        /// <summary>
        /// MOP should be reset if the player goes too long without a successful nail attack
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        private void ResetMop(On.HeroController.orig_Update orig, HeroController self)
        {
            orig(self);

            if (timer == null)
            {
                timer = Stopwatch.StartNew();
            }

            if (timer.ElapsedMilliseconds >= GetMaxDuration())
            {
                //CharmPatch.Instance.Log($"Max time passed: {timer.ElapsedMilliseconds}");
                ResetMop();
            }
        }

        /// <summary>
        /// MOP should be reset upon taking damage
        /// </summary>
        /// <param name="hazardType"></param>
        /// <param name="damageAmount"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private int ResetMop(int hazardType, int damageAmount)
        {
            //CharmPatch.Instance.Log($"Damage taken");
            ResetMop();

            return damageAmount;
        }

        /// <summary>
        /// Resets MOP's effects
        /// </summary>
        private void ResetMop()
        {
            if (!IsFotfActive())
            {
                // Reset the nail slashes to their default color (unless Abyssal Bloom as turned them black)
                foreach (NailSlash nailSlash in GetNailSlashes(HeroController.instance))
                {
                    tk2dSprite nailColor = nailSlash.GetComponent<tk2dSprite>();
                    if (nailColor.color != Color.black)
                    {
                        nailColor.color = Color.white;
                    }
                }
            }

            hits = 0;
            timer = Stopwatch.StartNew();
            //CharmPatch.Instance.Log($"MOP Restored - Buff reset");
        }
        #endregion
    }
}