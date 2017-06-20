using System.Security.Cryptography;
using System.Text;

public static class Hash
{



    public static byte[] SHA256(byte[] tBytes)
    {
        SHA256Managed sha = new SHA256Managed();
        return sha.ComputeHash(tBytes);
    }

    public static byte[] SHA256(byte[] tBytes, int lIndex, int lLength)
    {
        SHA256Managed sha = new SHA256Managed();
        return sha.ComputeHash(tBytes, lIndex, lLength);
    }

    public static string HexStringFromBytes(byte[] bytes)
    {
        var sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            var hex = b.ToString("x2");
            sb.Append(hex);
        }
        return sb.ToString();
    }

    public static string GetDropboxContentHash(byte[] tBytes)
    {
        long l4MB = 1024 * 1024 * 4;
        long lByteLen = tBytes.LongLength;
        int iNumChunks = (int)(lByteLen  / l4MB);
        if (iNumChunks * l4MB < lByteLen)
            ++iNumChunks;

        int iSHABytes = 32;
        byte[] tHashBytes = new byte[iNumChunks * iSHABytes];

        int iHashByteIndex = 0;
        for (long lByteIndex = 0; lByteIndex < lByteLen; lByteIndex += l4MB)
        {
            long lBytesRemaining = lByteLen - lByteIndex;
            long lChunkSize = lBytesRemaining > l4MB ? l4MB : lBytesRemaining;

            SHA256(tBytes, (int)lByteIndex, (int)lChunkSize).CopyTo(tHashBytes, iHashByteIndex);
            iHashByteIndex = tHashBytes.Length;
        }

        return HexStringFromBytes(tHashBytes);
    }

	
}
