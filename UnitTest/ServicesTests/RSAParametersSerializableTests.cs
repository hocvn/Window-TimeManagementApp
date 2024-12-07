using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.IO;
using TimeManagementApp.Services;

namespace UnitTest
{
    [TestClass]
    public class RSAParametersSerializableTests
    {
        private RSAParameters rsaParameters;
        private RSAParametersSerializable rsaParametersSerializable;

        [TestInitialize]
        public void TestInitialize()
        {
            using (var rsa = RSA.Create())
            {
                rsa.KeySize = 2048;
                rsaParameters = rsa.ExportParameters(true);
                rsaParametersSerializable = new RSAParametersSerializable(rsaParameters);
            }
        }

        [TestMethod]
        public void Constructor_ShouldInitializeWithRSAParameters()
        {
            // Act
            var result = new RSAParametersSerializable(rsaParameters);

            // Assert
            Assert.AreEqual(rsaParameters.D, result.D);
            Assert.AreEqual(rsaParameters.DP, result.DP);
            Assert.AreEqual(rsaParameters.DQ, result.DQ);
            Assert.AreEqual(rsaParameters.Exponent, result.Exponent);
            Assert.AreEqual(rsaParameters.InverseQ, result.InverseQ);
            Assert.AreEqual(rsaParameters.Modulus, result.Modulus);
            Assert.AreEqual(rsaParameters.P, result.P);
            Assert.AreEqual(rsaParameters.Q, result.Q);
        }

        [TestMethod]
        public void GetObjectData_ShouldSerializeAllFields()
        {
            // Arrange
            var info = new SerializationInfo(typeof(RSAParametersSerializable), new FormatterConverter());
            var context = new StreamingContext();

            // Act
            rsaParametersSerializable.GetObjectData(info, context);

            // Assert
            Assert.AreEqual(rsaParameters.D, info.GetValue("D", typeof(byte[])));
            Assert.AreEqual(rsaParameters.DP, info.GetValue("DP", typeof(byte[])));
            Assert.AreEqual(rsaParameters.DQ, info.GetValue("DQ", typeof(byte[])));
            Assert.AreEqual(rsaParameters.Exponent, info.GetValue("Exponent", typeof(byte[])));
            Assert.AreEqual(rsaParameters.InverseQ, info.GetValue("InverseQ", typeof(byte[])));
            Assert.AreEqual(rsaParameters.Modulus, info.GetValue("Modulus", typeof(byte[])));
            Assert.AreEqual(rsaParameters.P, info.GetValue("P", typeof(byte[])));
            Assert.AreEqual(rsaParameters.Q, info.GetValue("Q", typeof(byte[])));
        }

        [TestMethod]
        public void RSAParametersSerializableConstructor_ShouldDeserializeCorrectly()
        {
            // Arrange
            var info = new SerializationInfo(typeof(RSAParametersSerializable), new FormatterConverter());
            var context = new StreamingContext();
            rsaParametersSerializable.GetObjectData(info, context);

            // Act
            var deserialized = new RSAParametersSerializable(info, context);

            // Assert
            Assert.AreEqual(rsaParameters.D, deserialized.D);
            Assert.AreEqual(rsaParameters.DP, deserialized.DP);
            Assert.AreEqual(rsaParameters.DQ, deserialized.DQ);
            Assert.AreEqual(rsaParameters.Exponent, deserialized.Exponent);
            Assert.AreEqual(rsaParameters.InverseQ, deserialized.InverseQ);
            Assert.AreEqual(rsaParameters.Modulus, deserialized.Modulus);
            Assert.AreEqual(rsaParameters.P, deserialized.P);
            Assert.AreEqual(rsaParameters.Q, deserialized.Q);
        }
    }
}
