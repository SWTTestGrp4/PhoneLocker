using System;
using System.IO;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using NSubstitute;
using NUnit.Framework;
using PhoneLockerClassLibrary;

namespace UTPhoneLocker
{
    [TestFixture]
    public class UTDisplay
    {
        private IDisplay _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new Display();
        }

        [TestCase("Tilslut telefon"),
         TestCase("Telefon Ikke Tilstluttet"),
         TestCase(""),
         TestCase("Dette er en lang test for at finde ud af, om vores klasse kan udskrive lange," +
                  "lange, og enddog ENORMT lange strenge. Vi er meget spændte på, om det går godt. Det er " +
                  "godt nok en laaaaaang streng")]
        public void DisplayText_InsertText_DisplayText(string message)
        {
            //ARRANGE

            var output = new StringWriter();
            Console.SetOut(output);

            //ACT
            _uut.DisplayText(message);
            //var currentConsoleOut = Console.Out.ToString();

            //ASSERT

            Assert.That(output.ToString(), Is.EqualTo(message+"\r\n"));
     
        }


        [TestCase("Tilslut telefon"),
         TestCase("Telefon Ikke Tilstluttet"),
         TestCase(""),
         TestCase("Dette er en lang test for at finde ud af, om vores klasse kan udskrive lange," +
                  "lange, og enddog ENORMT lange strenge. Vi er meget spændte på, om det går godt. Det er " +
                  "godt nok en laaaaaang streng")]
        public void DisplayCharge_InsertText_DisplayTextToTheRight(string message)
        {
            //ARRANGE

            var output = new StringWriter();
            Console.SetOut(output);

            //ACT
            _uut.DisplayCharge(message);
            //var currentConsoleOut = Console.Out.ToString();

            //ASSERT

            Assert.That(output.ToString(), Is.EqualTo("\t\t\t"+message + "\r\n"));

        }

    }
}