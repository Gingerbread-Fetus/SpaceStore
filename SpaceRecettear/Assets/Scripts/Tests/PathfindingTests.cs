using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PathfindingTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestCellEquals()
        {
            Vector3 position1 = new Vector3(1, 1, 0);
            Vector3 position2 = new Vector3(1, 1, 0);
            Vector3 goalPosition = new Vector3(0, 0, 0);

            Cell cell1 = new Cell(0, position1, goalPosition);
            Cell cell2 = new Cell(0, position2, goalPosition);

            Assert.True(cell1.Equals(cell2));
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator PathfindingTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
