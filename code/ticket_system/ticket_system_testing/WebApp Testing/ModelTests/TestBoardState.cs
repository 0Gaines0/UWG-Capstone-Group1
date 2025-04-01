using System;
using NUnit.Framework;
using ticket_system_web_app.Models;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace ticket_system_testing.WebApp_Testing.ModelTests
{
    [TestFixture]
    public class TestBoardState
    {
        [Test]
        public void TestDefaultConstructor()
        {
            var boardState = new BoardState();

            Assert.That(boardState.BoardId, Is.EqualTo(0));
            Assert.That(boardState.StateId, Is.EqualTo(0));
            Assert.That(boardState.StateName, Is.Null);
            Assert.That(boardState.ProjectBoard, Is.Null);
            Assert.That(boardState.Position, Is.EqualTo(0));
            Assert.That(boardState.Tasks, Is.Empty);
        }
    }
}