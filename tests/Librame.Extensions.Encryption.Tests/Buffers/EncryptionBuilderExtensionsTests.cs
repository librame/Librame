﻿using Xunit;

namespace Librame.Extensions.Encryption.Tests
{
    public class EncryptionBuilderExtensionsTests
    {
        [Fact]
        public void JointTest()
        {
            var str = nameof(EncryptionBuilderExtensionsTests);
            var plaintextBuffer = str.AsPlaintextBuffer(TestServiceProvider.Current);
            
            var hashString = plaintextBuffer.ApplyServiceProvider(TestServiceProvider.Current)
                .Md5()
                .Sha1()
                .Sha256()
                .Sha384()
                .Sha512()
                .HmacMd5()
                .HmacSha1()
                .HmacSha256()
                .HmacSha384()
                .HmacSha512()
                .AsCiphertextString();

            var plaintextBufferCopy = plaintextBuffer.Copy();
            var ciphertextString = plaintextBufferCopy
                .AsDes()
                .AsTripleDes()
                .AsAes()
                .AsRsa()
                .AsCiphertextString();
            Assert.NotEmpty(ciphertextString);

            var ciphertextBuffer = ciphertextString.AsCiphertextBuffer(TestServiceProvider.Current)
                .FromRsa()
                .FromAes()
                .FromTripleDes()
                .FromDes();

            // 值虽相同但引用地址不同，不适合用 Equals 比较
            //Assert.Equal(plaintextBufferCopy.Memory, ciphertextBuffer.Memory);
            Assert.Equal(hashString, ciphertextBuffer.AsCiphertextString());
        }

    }
}