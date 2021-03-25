using System;
using System.IO;
using System.Text;
using NSubstitute;
using NSubstitute.Exceptions;
using NUnit.Framework;
using PhoneLockerClassLibrary;

namespace UsbSimulator.Test
{
    [TestFixture]
    public class UTLogFileDAL
    {
        string path;
        ILogging _uut;

        [SetUp]
        public void Setup()
        {
            path = "PhoneLockerLog.txt";

            _uut = new LogFileDAL();

        }

        [TestCase("Test streng: 10"),
         TestCase(""),
         TestCase("Dette er en lang test for at finde ud af, om vores klasse kan udskrive lange," +
                  "lange, og enddog ENORMT lange strenge. Vi er meget spændte på, om det går godt. Det er " +
                  "godt nok en laaaaaang streng"),]
        public void Write_WriteTextToFil_TextIsWrittenToFile(string expected)
        {
            //Act
            _uut.Write(expected);
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var sr = new StreamReader(fs, Encoding.UTF8);

            string result = sr.ReadToEnd();
            //Assert
            Assert.That(result, Is.EqualTo(expected));

        }


        //Could not make the method throw exception, but we would have tested it like this
        //[TestCase("Test")]
        //public void Write_WriteToWrongPath_ThrowsException(string expected)
        //{

        //    //Arrange
        //    File.Create(path).Close();
        //    //Act
        //    _uut.Write(expected);


        //    //Assert
        //    Assert.Catch<Exception>((() => _uut.Write(expected)));
        //}

    }
}