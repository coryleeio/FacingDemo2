using System;
using System.Collections.Generic;

namespace Gamepackage
{
    public class ItemFactory
    {
        public static void SetDefaults(Item item)
        {
            item.DisplayName = "item.no.name".Localize();
            item.Description = "item.no.description".Localize();
            item.SlotsWearable = new List<ItemSlot>(0);
            item.SlotsOccupiedByWearing = new List<ItemSlot>(0);
            item.NumberOfItems = 1;
            item.MaxStackSize = 1;
            item.Attributes = new Dictionary<Attributes, int>(0);
            item.EffectsGlobal = new List<Effect>();
            item.AttackTypeParameters = new Dictionary<AttackType, AttackTypeParameters>()
            {
                {     AttackType.Melee, new AttackTypeParameters()
                      {
                          Range = 1,
                          AttackParameters = new List<AttackParameters>(0),
                          NumberOfTargetsToPierce = 1,
                      }
                },
                {     AttackType.Ranged, new AttackTypeParameters()
                      {
                          Range = 5,
                          AttackParameters = new List<AttackParameters>(0),
                          NumberOfTargetsToPierce = 1,
                      }
                },
                {     AttackType.Thrown, new AttackTypeParameters()
                      {
                          Range = 5,
                          AttackParameters =new List<AttackParameters>() {
                                new AttackParameters()
                                 {
                                   AttackMessage = "attacks.throw.useless.1".Localize(),
                                   Bonus = 0,
                                    DyeNumber = 1,
                                     DyeSize = 1,
                                    DamageType = DamageTypes.PIERCING,
                                          ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_NONE,
                                   }
                            },
                          NumberOfTargetsToPierce = 1,
                      }
                },
                {     AttackType.Zapped, new AttackTypeParameters()
                      {
                          Range = 5,
                          AttackParameters = new List<AttackParameters>(0),
                          NumberOfTargetsToPierce = 1,
                      }
                },
                {     AttackType.ApplyToSelf, new AttackTypeParameters()
                      {
                          Range = 1,
                          AttackParameters = new List<AttackParameters>(0),
                          NumberOfTargetsToPierce = 1,
                          AttackTargetingType = AttackTargetingType.SelectTarget,
                      }
                },
                {     AttackType.ApplyToOther, new AttackTypeParameters()
                      {
                          Range = 1,
                          AttackParameters = new List<AttackParameters>(0),
                          NumberOfTargetsToPierce = 1,
                          AttackTargetingType = AttackTargetingType.SelectTarget,
                      }
                },
            };
            item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_ARROW;
            item.ExactNumberOfChargesRemaining = 0;
            item.HasUnlimitedCharges = false;
            item.DestroyWhenAllChargesAreConsumed = false;
            item.ChanceToSurviveLaunch = 100;
            item.AmmoType = AmmoType.None;
        }

        public static Item Build(UniqueIdentifier uniqueIdentifier)
        {
            var item = new Item();
            SetDefaults(item);
            item.UniqueIdentifier = uniqueIdentifier;
            if (uniqueIdentifier == UniqueIdentifier.ITEM_LONGSWORD)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_LONGSWORD;
                item.DisplayName = "item.longsword.name".Localize();
                item.Description = "item.longsword.description".Localize();
                item.AttackTypeParameters[AttackType.Melee].AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.slashing.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 8,
                        DamageType = DamageTypes.SLASHING,
                    }
                };
                item.AttackTypeParameters[AttackType.Thrown].AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.throw.useless.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 3,
                        DamageType = DamageTypes.SLASHING,
                        ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_LONGSWORD
                    }
                };
                item.SlotsWearable.Add(ItemSlot.MainHand);
                item.SlotsOccupiedByWearing.Add(ItemSlot.MainHand);
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_POISON_DAGGER)
            {
                item = ItemFactory.Build(UniqueIdentifier.ITEM_DAGGER);
                item.EffectsGlobal.Add(EffectFactory.Build(UniqueIdentifier.EFFECT_APPLIED_WEAK_POISON, new List<AttackType>() { AttackType.Melee, AttackType.Thrown }));
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_DAGGER)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_DAGGER;
                item.DisplayName = "item.dagger.name".Localize();
                item.Description = "item.dagger.description".Localize();
                item.AttackTypeParameters[AttackType.Melee].AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.piercing.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 8,
                        DamageType = DamageTypes.PIERCING,
                    }
                };
                item.AttackTypeParameters[AttackType.Thrown].AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.throw.useless.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 3,
                        DamageType = DamageTypes.PIERCING,
                        ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_DAGGER,
                    }
                };
                item.SlotsWearable.Add(ItemSlot.MainHand);
                item.SlotsOccupiedByWearing.Add(ItemSlot.MainHand);
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_MACE)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_MACE;
                item.DisplayName = "item.mace.name".Localize();
                item.Description = "item.mace.description".Localize();
                item.AttackTypeParameters[AttackType.Melee].AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.bludgeoning.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 8,
                        DamageType = DamageTypes.BLUDGEONING,
                    }
                };
                item.AttackTypeParameters[AttackType.Thrown].AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.throw.useless.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 3,
                        DamageType = DamageTypes.BLUDGEONING,
                        ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_MACE,
                    }
                };
                item.SlotsWearable.Add(ItemSlot.MainHand);
                item.SlotsOccupiedByWearing.Add(ItemSlot.MainHand);
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_STAFF_OF_FIREBALLS)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_ACTION_STAFF;
                item.DisplayName = "item.staff.of.fireballs.name".Localize();
                item.Description = "item.staff.of.fireballs.description".Localize();
                item.AttackTypeParameters[AttackType.Melee].AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.bludgeoning.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 8,
                        DamageType = DamageTypes.BLUDGEONING,
                    }
                };
                item.AttackTypeParameters[AttackType.Thrown].AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.throw.useless.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 3,
                        DamageType = DamageTypes.BLUDGEONING,
                        ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_ACTION_STAFF,
                    }
                };
                item.AttackTypeParameters[AttackType.Zapped].AttackParameters = new List<AttackParameters>()
                {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.fire.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 0,
                        DyeSize = 0,
                        DamageType = DamageTypes.FIRE,
                        ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_FIREBALL,
                        ExplosionParameters = new ExplosionParameters()
                        {
                            AttackMessage = "attacks.fire.1".Localize(),
                            Radius = 4,
                            Bonus = 0,
                            DyeNumber = 1,
                            DyeSize = 3,
                            DamageType = DamageTypes.FIRE,
                            ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_FIRE_EXPLOSION,
                        }
                    }
                };
                item.HasUnlimitedCharges = true;
                item.AttackTypeParameters[AttackType.Zapped].NumberOfTargetsToPierce = 999;
                item.SlotsWearable.Add(ItemSlot.MainHand);
                item.SlotsOccupiedByWearing.Add(ItemSlot.MainHand);
            }

            else if (uniqueIdentifier == UniqueIdentifier.ITEM_SHORTBOW)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_BOW;
                item.DisplayName = "item.shortbow.name".Localize();
                item.Description = "item.shortbow.description".Localize();
                item.AttackTypeParameters[AttackType.Melee].AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.bludgeoning.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 3,
                        DamageType = DamageTypes.BLUDGEONING,
                    }
                };
                item.AttackTypeParameters[AttackType.Thrown].AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.throw.useless.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 3,
                        DamageType = DamageTypes.BLUDGEONING,
                        ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_BOW,
                    }
                };
                item.AttackTypeParameters[AttackType.Ranged].AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.piercing.1".Localize(),
                        Bonus = 4,
                        DamageType = DamageTypes.PIERCING,
                    }
                };
                item.AmmoType = AmmoType.Arrow;
                item.AttackTypeParameters[AttackType.Ranged].Range = 5;
                item.SlotsWearable.Add(ItemSlot.MainHand);
                item.SlotsOccupiedByWearing.Add(ItemSlot.MainHand);
                item.SlotsOccupiedByWearing.Add(ItemSlot.OffHand);
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_WAND_OF_LIGHTNING)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_SWIRL_STAFF;
                item.DisplayName = "item.wand.of.lightning.name".Localize();
                item.Description = "item.wand.of.lightning.description".Localize();
                item.AttackTypeParameters[AttackType.Melee].AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.bludgeoning.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 8,
                        DamageType = DamageTypes.BLUDGEONING,
                    }
                };
                item.AttackTypeParameters[AttackType.Thrown].AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.throw.useless.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 3,
                        DamageType = DamageTypes.BLUDGEONING,
                        ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_SWIRL_STAFF,
                    }
                };
                item.AttackTypeParameters[AttackType.Zapped].AttackParameters = new List<AttackParameters>()
                {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.fire.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 8,
                        DamageType = DamageTypes.FIRE,
                        ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_LIGHTNING_JET,
                    }
                };
                item.HasUnlimitedCharges = true;
                item.AttackTypeParameters[AttackType.Zapped].NumberOfTargetsToPierce = 999;
                item.SlotsWearable.Add(ItemSlot.MainHand);
                item.SlotsOccupiedByWearing.Add(ItemSlot.MainHand);
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_WAND_OF_MADNESS)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_ORB_SCEPTER;
                item.DisplayName = "item.wand.of.madness.name".Localize();
                item.Description = "item.wand.of.madness.description".Localize();
                item.AttackTypeParameters[AttackType.Melee].AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.bludgeoning.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 8,
                        DamageType = DamageTypes.BLUDGEONING,
                    }
                };
                item.AttackTypeParameters[AttackType.Thrown].AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.throw.useless.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 3,
                        DamageType = DamageTypes.BLUDGEONING,
                        ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_ORB_SCEPTER,
                    }
                };
                item.AttackTypeParameters[AttackType.Zapped].AttackParameters = new List<AttackParameters>()
                {
                    new AttackParameters()
                    {
                        Bonus = 0,
                        DyeNumber = 0,
                        DyeSize = 0,
                        DamageType = DamageTypes.NEGATIVE,
                        ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_PURPLE_BALL,
                    }
                };
                item.AttackTypeParameters[AttackType.Zapped].AttackTargetingType = AttackTargetingType.SelectTarget;
                item.AttackTypeParameters[AttackType.Zapped].NumberOfTargetsToPierce = 1;
                item.EffectsGlobal.Add(EffectFactory.Build(UniqueIdentifier.EFFECT_APPLIED_MADNESS, new List<AttackType>() { AttackType.Zapped }));
                item.HasUnlimitedCharges = true;
                item.SlotsWearable.Add(ItemSlot.MainHand);
                item.SlotsOccupiedByWearing.Add(ItemSlot.MainHand);
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_WAND_OF_CHARM)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_ORB_SCEPTER;
                item.DisplayName = "item.wand.of.charm.name".Localize();
                item.Description = "item.wand.of.charm.description".Localize();
                item.AttackTypeParameters[AttackType.Melee].AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.bludgeoning.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 8,
                        DamageType = DamageTypes.BLUDGEONING,
                    }
                };
                item.AttackTypeParameters[AttackType.Thrown].AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.throw.useless.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 3,
                        DamageType = DamageTypes.BLUDGEONING,
                        ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_ORB_SCEPTER,
                    }
                };
                item.AttackTypeParameters[AttackType.Zapped].AttackParameters = new List<AttackParameters>()
                {
                    new AttackParameters()
                    {
                        Bonus = 0,
                        DyeNumber = 0,
                        DyeSize = 0,
                        DamageType = DamageTypes.NEGATIVE,
                        ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_PURPLE_BALL,
                    }
                };
                item.AttackTypeParameters[AttackType.Zapped].AttackTargetingType = AttackTargetingType.SelectTarget;
                item.AttackTypeParameters[AttackType.Zapped].NumberOfTargetsToPierce = 1;
                item.EffectsGlobal.Add(EffectFactory.Build(UniqueIdentifier.EFFECT_APPLIED_CHARM, new List<AttackType>() { AttackType.Zapped }));
                item.HasUnlimitedCharges = true;
                item.SlotsWearable.Add(ItemSlot.MainHand);
                item.SlotsOccupiedByWearing.Add(ItemSlot.MainHand);
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_WAND_OF_DOMINATION)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_ORB_SCEPTER;
                item.DisplayName = "item.wand.of.domination.name".Localize();
                item.Description = "item.wand.of.domination.description".Localize();
                item.AttackTypeParameters[AttackType.Melee].AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.bludgeoning.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 8,
                        DamageType = DamageTypes.BLUDGEONING,
                    }
                };
                item.AttackTypeParameters[AttackType.Thrown].AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.throw.useless.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 3,
                        DamageType = DamageTypes.BLUDGEONING,
                        ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_ORB_SCEPTER,
                    }
                };
                item.AttackTypeParameters[AttackType.Zapped].AttackParameters = new List<AttackParameters>()
                {
                    new AttackParameters()
                    {
                        Bonus = 0,
                        DyeNumber = 0,
                        DyeSize = 0,
                        DamageType = DamageTypes.NEGATIVE,
                        ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_PURPLE_BALL,
                    }
                };
                item.AttackTypeParameters[AttackType.Zapped].NumberOfTargetsToPierce = 1;
                item.AttackTypeParameters[AttackType.Zapped].AttackTargetingType = AttackTargetingType.SelectTarget;
                item.EffectsGlobal.Add(EffectFactory.Build(UniqueIdentifier.EFFECT_APPLIED_DOMINATION, new List<AttackType>() { AttackType.Zapped }));
                item.HasUnlimitedCharges = true;
                item.SlotsWearable.Add(ItemSlot.MainHand);
                item.SlotsOccupiedByWearing.Add(ItemSlot.MainHand);
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_ANTIDOTE)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_PURPLE_POTION;
                item.DisplayName = "item.antidote.name".Localize();
                item.Description = "item.antidote.description".Localize();
                item.CustomOnUseText = "item.antidote.action".Localize();
                item.ExactNumberOfChargesRemaining = 1;
                item.DestroyWhenAllChargesAreConsumed = true;

                item.AttackTypeParameters[AttackType.Thrown].AttackParameters = new List<AttackParameters>()
                {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.throw.useless.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 0,
                        DyeSize = 0,
                        DamageType = DamageTypes.BLUDGEONING,
                        ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_PURPLE_POTION,
                    },
                };

                item.AttackTypeParameters[AttackType.ApplyToSelf].AttackParameters = new List<AttackParameters>()
                {
                    new AttackParameters()
                    {
                        AttackMessage = "item.antidote.on.use".Localize(),
                        Bonus = 0,
                        DyeNumber = 0,
                        DyeSize = 0,
                        DamageType = DamageTypes.HEALING,
                        ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_PURPLE_POTION,
                    },
                };

                item.ChanceToSurviveLaunch = 0;
                item.SlotsWearable.Add(ItemSlot.MainHand);
                item.SlotsOccupiedByWearing.Add(ItemSlot.MainHand);
                item.EffectsGlobal.Add(EffectFactory.Build(UniqueIdentifier.EFFECT_APPLIED_POISON_IMMUNITY, new List<AttackType>() { AttackType.ApplyToSelf, AttackType.Thrown }));
                item.EffectsGlobal.Add(EffectFactory.Build(UniqueIdentifier.EFFECT_APPLIED_WEAK_REGENERATION, new List<AttackType>() { AttackType.ApplyToSelf, AttackType.Thrown }));
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_LUCKY_COIN)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_LUCKY_COIN;
                item.DisplayName = "item.lucky.coin.name".Localize();
                item.Description = "item.lucky.coin.description".Localize();
                item.AttackTypeParameters[AttackType.Thrown].AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.throw.useless.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 0,
                        DyeSize = 0,
                        DamageType = DamageTypes.BLUDGEONING,
                        ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_COIN,
                    }
                };

                item.EffectsGlobal.Add(EffectFactory.Build(UniqueIdentifier.EFFECT_LUCKY_COIN_LIFE_SAVE));
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_ROBE_OF_WONDERS)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_ROBE_OF_WONDERS;
                item.DisplayName = "item.robe.of.wonders.name".Localize();
                item.Description = "item.robe.of.wonders.description".Localize();
                item.SlotsOccupiedByWearing = new List<ItemSlot>()
                {
                    ItemSlot.Helmet,
                    ItemSlot.Chest,
                };
                item.SlotsWearable = new List<ItemSlot>()
                {
                    ItemSlot.Chest,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_SANDALS)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_SANDALS;
                item.DisplayName = "item.sandals.name".Localize();
                item.Description = "item.sandals.description".Localize();
                item.SlotsOccupiedByWearing = new List<ItemSlot>()
                {
                    ItemSlot.Shoes,
                };
                item.SlotsWearable = new List<ItemSlot>()
                {
                    ItemSlot.Shoes,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_SHIELD_OF_AMALURE)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_SHIELD_OF_AMALURE;
                item.DisplayName = "item.shield.of.amalure.name".Localize();
                item.Description = "item.shield.of.amalure.description".Localize();
                item.SlotsOccupiedByWearing = new List<ItemSlot>()
                {
                    ItemSlot.OffHand,
                };
                item.SlotsWearable = new List<ItemSlot>()
                {
                    ItemSlot.OffHand,
                };
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_ARROW)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_ARROW;
                item.DisplayName = "item.arrow.name".Localize();
                item.Description = "item.arrow.description".Localize();
                item.MaxStackSize = 60;
                item.ChanceToSurviveLaunch = 50;
                item.AttackTypeParameters[AttackType.Thrown].AttackParameters = new List<AttackParameters>()
                {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.throw.useless.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 1,
                        DamageType = DamageTypes.PIERCING,
                        ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_ARROW_SPIN,
                    },
                };

                item.AttackTypeParameters[AttackType.Ranged].AttackParameters = new List<AttackParameters>()
                {
                    new AttackParameters()
                    {
                        AttackMessage = "attacks.piercing.1".Localize(),
                        Bonus = 0,
                        DyeNumber = 1,
                        DyeSize = 8,
                        DamageType = DamageTypes.PIERCING,
                        ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_ARROW,
                    },
                };

                item.AmmoType = AmmoType.Arrow;
                item.NumberOfItems = MathUtil.ChooseRandomIntInRange(5, item.MaxStackSize / 2 - 1);
                item.SlotsWearable.Add(ItemSlot.Ammo);
                item.SlotsOccupiedByWearing.Add(ItemSlot.Ammo);
            }
            else
            {
                throw new NotImplementedException("Not implemented: " + uniqueIdentifier);
            }

            return item;
        }
    }
}
