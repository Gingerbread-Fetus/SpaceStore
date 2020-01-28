using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Random = UnityEngine.Random;

namespace Tests
{
    public class PathfindingTests
    {
        #region Node Access Shortcuts
        private int GetParentIndex(int index) => (index - 1) / 2;
        private int GetLeftChildIndex(int index) => (2 * index) + 1;
        private int GetRightChildIndex(int index) => (2 * index) + 2;
        #endregion

        Cell cell_0,
             cell_1,
             cell_2,
             cell_3,
             cell_4,
             cell_5,
             cell_6;

        List<Cell> testPath;

        [SetUp]
        public void Setup()
        {
            Cell cell_0 = new Cell(new Vector3(5, 9.5f, 0), new Vector3(7, 6.5f, 0));
            Cell cell_1 = new Cell(cell_0, new Vector3(5.5f, 9, 0), new Vector3(7, 6.5f, 0));
            Cell cell_2 = new Cell(cell_1, new Vector3(6, 8.5f, 0), new Vector3(7, 6.5f, 0));
            Cell cell_3 = new Cell(cell_2, new Vector3(6, 8, 0), new Vector3(7, 6.5f, 0));
            Cell cell_4 = new Cell(cell_3, new Vector3(6, 7.5f, 0), new Vector3(7, 6.5f, 0));
            Cell cell_5 = new Cell(cell_4, new Vector3(6, 7, 0), new Vector3(7, 6.5f, 0));
            Cell cell_6 = new Cell(cell_5, new Vector3(7, 6.5f, 0), new Vector3(7, 6.5f, 0));

            testPath.Add(cell_0);
            testPath.Add(cell_1);
            testPath.Add(cell_2);
            testPath.Add(cell_3);
            testPath.Add(cell_4);
            testPath.Add(cell_5);
            testPath.Add(cell_6);
        }



        // A Test behaves as an ordinary method
        [Test]
        public void TestCellEquals()
        {
            Vector3 position1 = new Vector3(1, 1, 0);
            Vector3 position2 = new Vector3(1, 1, 0);
            Vector3 goalPosition = new Vector3(0, 0, 0);

            Cell cell1 = new Cell(position1, goalPosition);
            Cell cell2 = new Cell(position2, goalPosition);

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

        #region HeapTests
        [Test]
        public void TestConstructor1()
        {
            MinHeap<int> intHeap = new MinHeap<int>();
            Assert.IsFalse(intHeap == null);
        }

        [Test]
        public void TestConstructor2()
        {
            int[] testArray = { 5, 4, 7, 8, 4, 5 };
            MinHeap<int> intHeap = new MinHeap<int>(testArray);
            Debug.Log(intHeap.ToString());
            Assert.IsTrue(intHeap.min == 4);
        }

        [Test]
        public void TestConstructor2Array()
        {
            int[] testArray = { 5, 4, 7, 8, 4, 5 };
            MinHeap<int> intHeap = new MinHeap<int>(testArray);
            Debug.Log(intHeap.ToString());
            Assert.IsTrue(ValidMinHeap(intHeap));
        }

        [Test]
        public void TestConstructor2WithCells()
        {
            MinHeap<Cell> cellHeap = new MinHeap<Cell>(testPath);
            Debug.Log(cellHeap.ToString());
            Cell minCell = new Cell(new Vector3(10, 10, 0), new Vector3(10, 10, 0));
            Assert.IsTrue(cellHeap.min.Equals(minCell));
        }

        [Test]
        public void TestConstructor2WithCells2()
        {
            MinHeap<Cell> cellHeap = new MinHeap<Cell>(testPath);
            Debug.Log(cellHeap.ToString());
            Assert.IsTrue(ValidMinHeap(cellHeap));
        }

        [Test]
        public void TestConstructor3()
        {
            int[] testArray2 = { 19, 30, 44, 88, 17, 5 };
            List<int> intList = new List<int>(testArray2);
            MinHeap<int> intHeap = new MinHeap<int>(intList);
            Assert.IsTrue(ValidMinHeap(intHeap));
        }


        [Test]
        public void TestConstructor3WithCells()
        {
            List<Cell> cellList = new List<Cell>(testPath);
            MinHeap<Cell> cellHeap = new MinHeap<Cell>(cellList);
            Assert.IsTrue(ValidMinHeap(cellHeap));
        }

        [Test]
        public void TestAdd()
        {
            List<Cell> cellList = new List<Cell>(testPath);
            MinHeap<Cell> cellHeap = new MinHeap<Cell>(cellList);
            Vector3 cellVector = new Vector3(12, 7, 0);
            Cell newCell = new Cell(cellVector, new Vector3(10, 10, 0));
            cellHeap.Add(newCell);
            Assert.IsTrue(ValidMinHeap(cellHeap));
        }

        [Test]
        public void TestAddMultipleRandom()
        {
            List<Cell> cellList = new List<Cell>(testPath);
            MinHeap<Cell> cellHeap = new MinHeap<Cell>(cellList);
            for (int i = 0; i < 10; i++)
            {
                int xRandom = Random.Range(0, 100);
                int yRandom = Random.Range(0, 100);
                Vector3 cellVector = new Vector3(xRandom, yRandom, 0);
                Cell newCell = new Cell(cellVector, new Vector3(10, 10, 0));
                cellHeap.Add(newCell);
            }

            Assert.IsTrue(ValidMinHeap(cellHeap));
        }

        [Test]
        public void TestRemove()
        {
            List<Cell> cellList = new List<Cell>(testPath);
            MinHeap<Cell> cellHeap = new MinHeap<Cell>(cellList);
            Vector3 cellVector = new Vector3(12, 7, 0);
            Cell newCell = new Cell(cellVector, new Vector3(10, 10, 0));
            cellHeap.Add(newCell);
            cellHeap.Remove(newCell);
            Assert.IsTrue(ValidMinHeap(cellHeap));
        }

        [Test]
        public void TestRemoveNonExistant()
        {
            List<Cell> cellList = new List<Cell>(testPath);
            MinHeap<Cell> cellHeap = new MinHeap<Cell>(cellList);
            Vector3 cellVector = new Vector3(12, 7, 0);
            Cell newCell = new Cell(cellVector, new Vector3(10, 10, 0));

            Assert.IsFalse(cellHeap.Remove(newCell));
        }

        [Test]
        public void TestRemoveMin()
        {
            Cell expectedMinCell = new Cell(new Vector3(10, 10, 0), new Vector3(10, 10, 0));
            List<Cell> cellList = new List<Cell>(testPath);
            MinHeap<Cell> cellHeap = new MinHeap<Cell>(cellList);
            Cell minCell = cellHeap.RemoveMin();
            Assert.IsTrue(minCell.Equals(expectedMinCell));
        }

        [Test]
        public void TestRemoveMinLeavesValidHeap()
        {
            List<Cell> cellList = new List<Cell>(testPath);
            MinHeap<Cell> cellHeap = new MinHeap<Cell>(cellList);
            Debug.Log(cellHeap.ToString());
            cellHeap.RemoveMin();
            Debug.Log(cellHeap.ToString());
            Assert.IsTrue(ValidMinHeap(cellHeap));
        }

        [Test]
        public void TestRemoveMinMultipleTimes()
        {
            Cell expectedMinCell = new Cell(new Vector3(9, 8, 0), new Vector3(10, 10, 0));
            List<Cell> cellList = new List<Cell>(testPath);
            MinHeap<Cell> cellHeap = new MinHeap<Cell>(cellList);
            Debug.Log("Start heap: " + cellHeap.ToString());
            cellHeap.RemoveMin();
            Debug.Log("First RemoveMin: " + cellHeap.ToString());
            Cell minCell = cellHeap.RemoveMin();
            Debug.Log("Second RemoveMin: " + cellHeap.ToString());
            Assert.IsTrue(expectedMinCell.Equals(minCell));
        }

        [Test]
        public void TestRemoveMinOnEmpty()
        {
            MinHeap<Cell> newHeap = new MinHeap<Cell>();
            Assert.IsNull(newHeap.RemoveMin());
        }



        #endregion

        #region Helpers
        private bool ValidMinHeap<T>(MinHeap<T> testHeap) where T : IComparable
        {
            T[] comparables = testHeap.ToArray();
            for (int i = 0; i < (comparables.Length - 2) / 2; i++)
            {
                //If left child is lesser, return false
                if (comparables[2 * i + 1].CompareTo(comparables[i]) < 0)
                {
                    return true;
                }
                //If right child is lesser, return false
                if (2 * i + 2 < comparables.Length && comparables[2 * i + 2].CompareTo(comparables[i]) < 0)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
    }
}
