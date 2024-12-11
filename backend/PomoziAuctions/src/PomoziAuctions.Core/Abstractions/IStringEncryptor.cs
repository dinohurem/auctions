namespace PomoziAuctions.Core.Abstractions;

public interface IStringEncryptor
{
	string Encrpyt(string plainText, string purpose);

	string Decrpyt(string cipherText, string purpose);
}
