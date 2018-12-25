using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static FollowTheWhiteRabbit.Program;

namespace FollowTheWhiteRabbit.Tests
{
    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void CreateMD5_PassSampleString_ExpectedHash()
        {
            // Arrange
            string expectedHash = "e4820b45d2277f3844eac66c903e84be";
            string input = "printout stout yawls";

            // Act
            string actualHash = Program.CreateMD5(input);

            // Assert
            Assert.AreEqual(expectedHash, actualHash);
        }

        [DataRow("printout")]
        [DataRow("stout")]
        [DataRow("yawls")]
        [DataRow("ty")]
        [DataRow("outlaws")]
        [DataRow("printouts")]
        [DataRow("printoutsoutlaws")]
        [DataTestMethod]
        public void IsValidWord_WordContainsEveryChar_True(string word)
        {
            // Arrange
            string anagram = "poultry outwits ants";

            // Act
            var anagramFreq = Program.GetCharFrequencyDic(anagram);
            var isValid = Program.IsValidWord(anagram, anagramFreq, word);

            // Assert
            Assert.IsTrue(isValid);
        }

        [DataRow("das's")]
        [DataRow("adz")]
        [DataRow("asd ")]
        [DataTestMethod]
        public void IsValidWord_WordContainsToManyChars_False(string word)
        {
            // Arrange
            string anagram = "poultry outwits ants";

            // Act
            var anagramFreq = Program.GetCharFrequencyDic(anagram);
            var isValid = Program.IsValidWord(anagram, anagramFreq, word);

            // Assert
            Assert.IsFalse(isValid);
        }

        [DataRow("das's")]
        [DataRow("adz")]
        [DataRow("printouts")]
        [DataRow("ailnooprssttttuuwy")]
        [DataTestMethod]
        public void GetCharFrequencyDic_CalculatesAllChars(string text)
        {
            // Act
            var textFreq = Program.GetCharFrequencyDic(text);

            // Assert
            Assert.AreEqual(text.Count(), textFreq.Sum(x => x.Value));
        }

        [TestMethod]
        public void GetCharFrequencyDic_CheckIfFreqIsCorrect()
        {

            var text = "ailnooprssttttuuwy";
            // Act
            var textFreq = Program.GetCharFrequencyDic(text);

            // Assert
            Assert.AreEqual(12, textFreq.Count());
            Assert.AreEqual(18, textFreq.Sum(x => x.Value));
            Assert.AreEqual(4, textFreq['t']);
        }

        [TestMethod]
        public void GetPermutationsNotRep_ReturnsGoodPermutationCount()
        {
            // Arrange
            var anagramEntity = new AnagramEntity();
            anagramEntity.Anagram = "poultryoutwitsants";
            anagramEntity.AnagramLen = anagramEntity.Anagram.Count();
            anagramEntity.AnagramFreq = Program.GetCharFrequencyDic(anagramEntity.Anagram);
            var words = new List<string>
            {
                "ads", "printout", "asd", "stout", "yawls", "yawsl", "ywasl"
            };

            // Act
            var permutation = Program.GetPermutationsNotRep(words, anagramEntity, 3);

            // Assert
            Assert.AreEqual(3, permutation.Count());
        }

        [TestMethod]
        public void GetPermutationsNotRep_CheckIfPermutationIsCorrect()
        {
            // Arrange
            var anagramEntity = new AnagramEntity();
            anagramEntity.Anagram = "poultryoutwitsants";
            anagramEntity.AnagramLen = anagramEntity.Anagram.Count();
            anagramEntity.AnagramFreq = Program.GetCharFrequencyDic(anagramEntity.Anagram);
            var words = new List<string>
            {
                "printout", "stout", "yawls"
            };

            // Act
            var permutation = Program.GetPermutationsNotRep(words, anagramEntity, 3);

            // Assert
            Assert.AreEqual(1, permutation.Count());
            Assert.AreEqual("printoutstoutyawls", 
                string.Join("", permutation.First()));
        }

        [TestMethod]
        public void GetPermutationsNotRep_WithTwoWords()
        {
            // Arrange
            var anagramEntity = new AnagramEntity();
            anagramEntity.Anagram = "poultryoutwitsants";
            anagramEntity.AnagramLen = anagramEntity.Anagram.Count();
            anagramEntity.AnagramFreq = Program.GetCharFrequencyDic(anagramEntity.Anagram);
            var words = new List<string>
            {
                "poultryou", "twitsants"
            };

            // Act
            var permutation = Program.GetPermutationsNotRep(words, anagramEntity, 2);

            // Assert
            Assert.AreEqual(1, permutation.Count());
        }

        [TestMethod]
        public void GetPermutationsNotRep_WithThreeWords()
        {
            // Arrange
            var anagramEntity = new AnagramEntity();
            anagramEntity.Anagram = "poultryoutwitsants";
            anagramEntity.AnagramLen = anagramEntity.Anagram.Count();
            anagramEntity.AnagramFreq = Program.GetCharFrequencyDic(anagramEntity.Anagram);
            var words = new List<string>
            {
                "poultryou", "twitsants", "a"
            };

            // Act
            var permutation = Program.GetPermutationsNotRep(words, anagramEntity, 3);

            // Assert
            Assert.AreEqual(1, permutation.Count());
        }

        [TestMethod]
        public void GetPermutations_ReturnsAllCombinations()
        {
            // Arrange
            var words = new List<string>
            {
                "prin", "tout", "st"
            };

            // Act
            var permutation = Program.GetPermutations(words, 3).ToList();

            // Assert
            Assert.AreEqual(6, permutation.Count);
        }
    }
}
