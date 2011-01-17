/* This file is part of libWiiSharp
 * Copyright (C) 2009 Leathl
 * 
 * libWiiSharp is free software: you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * libWiiSharp is distributed in the hope that it will be
 * useful, but WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace libWiiSharp
{
    public class CommonKey
    {
        private static string standardKey = "ebe42a225e8593e448d9c5457381aaf7";
        private static string koreanKey = "63b82bb4f4614e2e13f2fefbba4c9b7e";
        private static string dsiKey = "af1bf516a807d21aea45984f04742861";

        private static string currentDir = System.IO.Directory.GetCurrentDirectory();

        private static string standardKeyName = "key.bin";
        private static string koreanKeyName = "kkey.bin";
        private static string dsiKeyName = "dsikey.bin";

        public static byte[] GetStandardKey()
        {
            if (System.IO.File.Exists(System.IO.Path.Combine(currentDir, standardKeyName)))
                return System.IO.File.ReadAllBytes(System.IO.Path.Combine(currentDir, standardKeyName));
            else
                return Shared.HexStringToByteArray(standardKey);
        }

        public static byte[] GetKoreanKey()
        {
            if (System.IO.File.Exists(System.IO.Path.Combine(currentDir, koreanKeyName)))
                return System.IO.File.ReadAllBytes(System.IO.Path.Combine(currentDir, koreanKeyName));
            else
                return Shared.HexStringToByteArray(koreanKey);
        }

        public static byte[] GetDSiKey()
        {
            if (System.IO.File.Exists(System.IO.Path.Combine(currentDir, dsiKeyName)))
                return System.IO.File.ReadAllBytes(System.IO.Path.Combine(currentDir, dsiKeyName));
            else
                return Shared.HexStringToByteArray(dsiKey);
        }
    }
}
