using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static RapaxFructus.ControlSystem;


namespace RapaxFructus
{
    internal class CheatCodeManager : MonoBehaviour
    {
        private enum Cheat
        {
            GiveEnergy
        }
        private Dictionary<Action[], Cheat > CheatCodes => new Dictionary<Action[], Cheat>()
        {
            {new Action[] {Action.MoveIn, Action.MoveIn, Action.MoveIn, Action.MoveLeft, Action.MoveDown, Action.BuildMode, Action.TimeStop, Action.TimeStop, Action.TimeStop }, Cheat.GiveEnergy }
        };
        private List<Action> currentInput = new List<Action>();

        private void Start()
        {
            GetComponent<ControlSystem>().ActionNotify += OnAction;
        }

        private void OnAction(Action action)
        {
            currentInput.Add(action);
            bool itsCode = true;
            foreach (var cheatCode in CheatCodes)
            {
                Action[] code = cheatCode.Key;
                if (currentInput.Count > code.Length)
                    itsCode = false;
                for (int i  = 0; i < currentInput.Count && itsCode; i++)
                {
                    if (code[i] != currentInput[i])
                    {
                        itsCode = false;
                        break;
                    }
                }
                if (!itsCode)
                {
                    currentInput.Clear();
                    if (action == code[0])
                    {
                        currentInput.Add(action);
                    }
                }
                else
                {
                    if (code.Length == currentInput.Count)
                    {
                        UseCheat(cheatCode.Value);
                    }
                }
            }
        }

        private void UseCheat(Cheat cheat)
        {
            switch (cheat)
            {
                case Cheat.GiveEnergy:
                    {
                        DataManager.ChangeEnergyPoints(10000, false);
                        break;
                    }
            }
        }
    }
}