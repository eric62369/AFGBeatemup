using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using BattleInput;

namespace Tests
{
    public class BattleInputTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void NewTestScriptSimplePasses()
        {
            // Use the Assert class to test conditions
            Assert.AreNotEqual(1, 0);
        }

        [Test]
        public void InterpretN5History()
        {
            InputHistory history = new InputHistory(10, 4);
            IList<ButtonStatus> buttons = new List<ButtonStatus>();
            buttons.Add(ButtonStatus.Up);
            buttons.Add(ButtonStatus.Down);
            buttons.Add(ButtonStatus.Up);
            buttons.Add(ButtonStatus.Up);
            
            for (int i = 0; i < history.GetSize(); i++) {
                history.AddNewEntry(
                    Numpad.N5,
                    buttons,
                    2
                );
            }

            IList<string> list236B = new List<string> ();
            list236B.Add ("236");
            list236B.Add ("2365");
            list236B.Add ("2369");
            AttackMotionInput S236B = new AttackMotionInput (list236B, "B", 60);

            Assert.False(InterpretUtil.InterpretNormalAttackInput(history, S236B));
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}