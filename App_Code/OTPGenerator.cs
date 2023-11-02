using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for OTPGenerator
/// </summary>
public class OTPGenerator
{
    private static readonly Random random = new Random();

	public OTPGenerator()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public static string GenerateOTP(int length)
    {
        const string characters = "0123456789"; // You can include more characters if needed
        char[] otp = new char[length];

        for (int i = 0; i < length; i++)
        {
            otp[i] = characters[random.Next(0, characters.Length)];
        }

        return new string(otp);
    }
}