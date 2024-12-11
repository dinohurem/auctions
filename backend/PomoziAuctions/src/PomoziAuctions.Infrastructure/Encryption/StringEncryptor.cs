
using Microsoft.AspNetCore.DataProtection;
using PomoziAuctions.Core.Abstractions;

namespace PomoziAuctions.Infrastructure.Encryption;

public class StringEncryptor : IStringEncryptor
{
	private readonly IDataProtectionProvider _dataProtectionProvider;

	public StringEncryptor(IDataProtectionProvider dataProtectionProvider)
	{
		_dataProtectionProvider = dataProtectionProvider;
	}

	public string Decrpyt(string cipherText, string purpose) => _dataProtectionProvider.CreateProtector(purpose).Unprotect(cipherText);

	public string Encrpyt(string plainText, string purpose) => _dataProtectionProvider.CreateProtector(purpose).Protect(plainText);
}
