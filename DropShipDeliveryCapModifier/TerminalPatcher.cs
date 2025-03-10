using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;

namespace DropShipDeliveryCapModifier
{
    [HarmonyPatch]
    [HarmonyPatch(typeof(Terminal))]
    public class TerminalPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        private static void OnStart(Terminal __instance)
        {
            __instance.terminalNodes.specialNodes[4].displayText = __instance.terminalNodes.specialNodes[4].displayText.Replace("12", Plugin.config.deliveryCap.Value.ToString());
        }

        [HarmonyTranspiler]
        [HarmonyPatch("LoadNewNodeIfAffordable")]
        public static IEnumerable<CodeInstruction> OnLoadNewNodeIfAffordableIL(IEnumerable<CodeInstruction> instructions)
        {
            Log.Info("Modifying Terminal.LoadNewNodeIfAffordable");
            var code = new List<CodeInstruction>(instructions);

            int index = 0;
            for(int i = 2; i < code.Count; i++)
            {
                if(code[i - 2].opcode == OpCodes.Ldarg_0 &&
                   code[i - 1].opcode == OpCodes.Ldfld && code[i - 1].operand.ToString() == "System.Int32 playerDefinedAmount" &&
                   code[i].opcode == OpCodes.Ldc_I4_S && code[i].operand.ToString() == "12")
                {
                    index = i;
                    code.RemoveAt(i);
                    code.Insert(i, new CodeInstruction(OpCodes.Ldc_I4, Plugin.config.deliveryCap.Value));
                    break;
                }
            }

            for (int i = index + 4; i < code.Count; i++)
            {
                if (code[i - 4].opcode == OpCodes.Ldarg_0 &&
                    code[i - 3].opcode == OpCodes.Ldfld && code[i - 3].operand.ToString() == "System.Int32 numberOfItemsInDropship" &&
                    code[i - 2].opcode == OpCodes.Conv_R4 &&
                    code[i - 1].opcode == OpCodes.Add &&
                    code[i].opcode == OpCodes.Ldc_R4 && code[i].operand.ToString() == "12")
                {
                    code.RemoveAt(i);
                    code.Insert(i, new CodeInstruction(OpCodes.Ldc_R4, (float)Plugin.config.deliveryCap.Value));
                    break;
                }
            }

            return code.AsEnumerable();
        }
        

        [HarmonyTranspiler]
        [HarmonyPatch("SyncBoughtItemsWithServer")]
        public static IEnumerable<CodeInstruction> OnSyncBoughtItemsWithServerIL(IEnumerable<CodeInstruction> instructions)
        {
            var code = new List<CodeInstruction>(instructions);

            for (int i = 3; i < code.Count; i++)
            {
                if (code[i - 3].opcode == OpCodes.Ldarg_1 &&
                    code[i - 2].opcode == OpCodes.Ldlen &&
                    code[i - 1].opcode == OpCodes.Conv_I4 &&
                    code[i].opcode == OpCodes.Ldc_I4_S && code[i].operand.ToString() == "12")
                {
                    code.RemoveAt(i);
                    code.Insert(i, new CodeInstruction(OpCodes.Ldc_I4, Plugin.config.deliveryCap.Value));
                    break;
                }
            }

            return code.AsEnumerable();
        }
        
        [HarmonyTranspiler]
        [HarmonyPatch("BuyItemsServerRpc")]
        public static IEnumerable<CodeInstruction> OnBuyItemsServerRpcIL(IEnumerable<CodeInstruction> instructions)
        {
            var code = new List<CodeInstruction>(instructions);

            for (int i = 4; i < code.Count; i++)
            {
                if (code[i - 4].opcode == OpCodes.Nop &&
                    code[i - 3].opcode == OpCodes.Ldarg_1 &&
                    code[i - 2].opcode == OpCodes.Ldlen &&
                    code[i - 1].opcode == OpCodes.Conv_I4 && 
                    code[i].opcode == OpCodes.Ldc_I4_S && code[i].operand.ToString() == "12")
                {
                    code.RemoveAt(i);
                    code.Insert(i, new CodeInstruction(OpCodes.Ldc_I4, Plugin.config.deliveryCap.Value));
                    break;
                }
            }

            return code.AsEnumerable();
        }
        

        [HarmonyTranspiler]
        [HarmonyPatch("ParsePlayerSentence")]
        public static IEnumerable<CodeInstruction> OnParsePlayerSentenceIL(IEnumerable<CodeInstruction> instructions)
        {
            Log.Info("Modifying Terminal.ParsePlayerSentence");
            var code = new List<CodeInstruction>(instructions);

            for (int i = 4; i < code.Count; i++)
            {
                if (code[i - 4].opcode == OpCodes.Ldarg_0 &&
                    code[i - 3].opcode == OpCodes.Ldloc_3 &&
                    code[i - 2].opcode == OpCodes.Call &&
                    code[i - 1].opcode == OpCodes.Ldc_I4_0 &&
                    code[i].opcode == OpCodes.Ldc_I4_S && code[i].operand.ToString() == "10")
                {
                    code.RemoveAt(i);
                    code.Insert(i, new CodeInstruction(OpCodes.Ldc_I4, Plugin.config.buyCap.Value));
                    break;
                }
            }

            return code.AsEnumerable();
        }
    }
}
